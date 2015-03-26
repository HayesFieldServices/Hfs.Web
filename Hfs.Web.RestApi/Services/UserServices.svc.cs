using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using Twilio;
using System.Web;
using System.IO;
using Hfs.Web.RestApi.Models;

namespace Hfs.Web.RestApi
{  
    public class UserServices : IUserServices
    {
        public const string UsernameFilter = @"[^a-zA-Z0-9\-'._@]";
        public const string PasswordFilter = @"[ -~]";
        public const string TwilioVoiceCallerId = "+14055624350";
        public const string TwilioSmsNumber = "+14056738015";
        public const string TwilioVoiceResponseService = GlobalVar.TwilioRoot + "/user/verify/voice/answer";
        public const string TwilioVoiceStatusService = GlobalVar.TwilioRoot + "/user/verify/voice/status";
        public const string TwilioSmsResponseService = GlobalVar.TwilioRoot + "/user/verify/sms/receive";
        public const string TwilioSmsStatusService = GlobalVar.TwilioRoot + "/user/verify/sms/status";
        public const string TwilioAssetPath = "~/Assets/Twilio/";

        public string CreateUser(string firstName, string middleName, string lastName, string nameSuffix, string nickName, string emailAddress, string requestedUsername, string requestedPassword)
        {
            //using (var context = new Entities())
            //{
            //    var account = context.Accounts.Where(a => a.username == requestedUsername).SingleOrDefault();
                
            //}

            //return string.Join("", Regex.Matches("© M51714jake ©", passwordFilter).Cast<Match>().Select(m => m.Value));

            return "SUCCESS";
        }

        /// <summary>
        /// Sends a verification code to the given number via SMS
        /// </summary>
        /// <param name="phoneNumber">The user's phone number</param>
        /// <returns>The status of the request</returns>
        public string SendSms(string phoneNumber)
        {
            using (var context = new HFSDBEntities())
            {
                var isBlacklistedNumber = context.PhoneBlacklists
                    .Where(n => n.phone_number == phoneNumber)
                    .Any();

                if (isBlacklistedNumber)
                {
                    return "BLACKLIST";
                }
                else
                {
                    var code = NewVerificationCode(phoneNumber);

                    var twilio = new TwilioRestClient(
                        GlobalVar.TwilioAccountSid, 
                        GlobalVar.TwilioAuthToken
                    );
                    
                    var sms = twilio.SendMessage(
                        TwilioSmsNumber, 
                        phoneNumber,
                        GetFileAsString(TwilioAssetPath + "sms-verification-message.txt").Replace("%CODE%", code), 
                        TwilioSmsStatusService
                    );

                    if (sms.RestException != null)
                    {
                        return sms.RestException.Message.ToUpper();
                    }
                    else
                    {
                        return "SUCCESS";
                    }
                }
            }
        }

        /// <summary>
        /// Responds to an incoming SMS message
        /// </summary>
        /// <param name="stream">The incoming POST request</param>
        public void ReceiveSms(System.IO.Stream stream)
        {
            using (var requestBody = new StreamReader(stream, Encoding.UTF8))
            {              
                var message = HttpUtility.ParseQueryString(requestBody.ReadToEnd());

                if (message["Body"] == "STOP")
                {
                    using (var context = new HFSDBEntities())
                    {
                        var blacklistedNumber = new PhoneBlacklist()
                        {
                            phone_number = message["From"],
                            method = "SMS"
                        };
                        context.PhoneBlacklists.Add(blacklistedNumber);
                        context.SaveChanges();
                    }

                    return;
                }
                
                if (message["Body"] == "START")
                {
                    using (var context = new HFSDBEntities())
                    {
                        var phoneNumber = message["From"];
                        
                        var blacklistedNumber = context.PhoneBlacklists
                            .Where(p => p.phone_number == phoneNumber)
                            .SingleOrDefault();

                        context.PhoneBlacklists.Remove(blacklistedNumber);
                        context.SaveChanges();
                    }

                    return;
                }
                
                // All other messages receive a standard response describing the verification service
                WriteTwimlResponse(GetFileAsString(TwilioAssetPath + "sms-response-text.xml"));
            }
        }

        /// <summary>
        /// Handle SMS status notifications from Twilio
        /// </summary>
        /// <param name="stream">The incoming POST request</param>
        public void HandleSmsStatus(System.IO.Stream stream)
        {
            using (var requestBody = new StreamReader(stream, Encoding.UTF8))
            {
                var notification = HttpUtility.ParseQueryString(requestBody.ReadToEnd());

                if (new[] { "undelivered", "failed" }.Contains(notification["MessageStatus"]))
                {

                }
            }
        }

        /// <summary>
        /// Provides a scripted response when someone voice dials the Twilio SMS number
        /// </summary>
        public void HandleCallToTwilioSms()
        {
            WriteTwimlResponse(GetFileAsString(TwilioAssetPath + "sms-response-voice.xml"));
        }

        /// <summary>
        /// Initiates a voice call for 2nd-factor authentication
        /// </summary>
        /// <param name="phoneNumber">The user's phone number</param>
        /// <returns>The status of the request</returns>
        public string InitiateVoiceCall(string phoneNumber)
        {
            using (var context = new HFSDBEntities())
            {
                var isBlacklistedNumber = context.PhoneBlacklists
                    .Where(n => n.phone_number == phoneNumber)
                    .Any();

                if (isBlacklistedNumber)
                {
                    return "BLACKLIST";
                }
                else
                {
                    var code = NewVerificationCode(phoneNumber);

                    var twilio = new TwilioRestClient(
                        GlobalVar.TwilioAccountSid,
                        GlobalVar.TwilioAuthToken
                    );

                    var call = twilio.InitiateOutboundCall(
                        TwilioVoiceCallerId, 
                        phoneNumber, 
                        TwilioVoiceResponseService, 
                        TwilioVoiceStatusService
                    );

                    if (call.RestException != null)
                    {
                        return call.RestException.Message.ToUpper();
                    }
                    else
                    {
                        return "SUCCESS";
                    }
                }
            }
        }

        /// <summary>
        /// Provides script and handles user response during 2nd-factor authentication call
        /// </summary>
        /// <param name="stream">The incoming POST request</param>
        public void ConductVoiceCall(System.IO.Stream stream)
        {
            using (var requestBody = new StreamReader(stream, Encoding.UTF8))
            {
                var call = HttpUtility.ParseQueryString(requestBody.ReadToEnd());

                if (call["Digits"] != "9") // The opt out digit
                {
                    using (var context = new HFSDBEntities())
                    {
                        var phoneNumber = call["Called"];
                        
                        var code = context.VerificationCodes
                            .Where(c => c.phone_number == phoneNumber)
                            .SingleOrDefault().code.ToString();

                        var codeWithPauses = Regex.Replace(code, ".", @"$0. ");
                        
                        WriteTwimlResponse(
                            GetFileAsString(TwilioAssetPath + "voice-verification-script.xml")
                                .Replace("%HANDLER%", TwilioVoiceResponseService)
                                .Replace("%CODE%", codeWithPauses)
                        );
                    }

                    return;
                }
                else
                {
                    using (var context = new HFSDBEntities())
                    {
                        var blacklistedNumber = new PhoneBlacklist()
                        {
                            phone_number = call["Called"],
                            method = "Voice"
                        };
                        context.PhoneBlacklists.Add(blacklistedNumber);
                        context.SaveChanges();
                    }

                    WriteTwimlResponse(GetFileAsString(TwilioAssetPath + "voice-blacklist-script.xml"));
                }
            }
        }

        /// <summary>
        /// Handle call status notifications from Twilio
        /// </summary>
        /// <param name="stream">The incoming POST request</param>
        public void HandleVoiceCallStatus(System.IO.Stream stream)
        {
            using (var requestBody = new StreamReader(stream, Encoding.UTF8))
            {
                var call = HttpUtility.ParseQueryString(requestBody.ReadToEnd());

                if (new[] { "busy", "no-answer", "canceled", "failed" }.Contains(call["CallStatus"]))
                {

                }
            }
        }

        /// <summary>
        /// Generates a new 2nd-factor verification code associated with the given number
        /// </summary>
        /// <param name="phoneNumber">The user's phone number</param>
        /// <returns>The 8-digit verification code</returns>
        private string NewVerificationCode(string phoneNumber)
        {
            using (var context = new HFSDBEntities())
            {
                var existingCode = context.VerificationCodes
                    .Where(c => c.phone_number == phoneNumber)
                    .SingleOrDefault();

                if (existingCode != null) context.VerificationCodes.Remove(existingCode);

                var newCode = new VerificationCode
                {
                    phone_number = phoneNumber,
                    expiration_dt = DateTime.Now.AddMinutes(5)
                };

                context.VerificationCodes.Add(newCode);
                context.SaveChanges();

                return newCode.code.ToString();
            }
        }

        /// <summary>
        /// Returns the given file as a string
        /// </summary>
        private string GetFileAsString(string relativePath)
        {
            var absolutePath = HttpContext.Current.Server.MapPath(relativePath);
            return File.ReadAllText(absolutePath);
        }

        /// <summary>
        /// Writes the twiML to the HTTP Response
        /// </summary>
        /// <param name="twiml">The twiML to send</param>
        private void WriteTwimlResponse(string twiml)
        {
            var http = HttpContext.Current;

            http.Response.Clear();
            http.Response.ContentType = "text/xml";
            http.Response.ContentEncoding = System.Text.Encoding.UTF8;
            http.Response.Write(twiml);
        }
    }
}
