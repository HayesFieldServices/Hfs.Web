using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Web;

namespace Hfs.Web.RestApi
{
    [ServiceContract]
    public interface IUserServices
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "create/")]
        string CreateUser(string firstName, string middleName, string lastName, string nameSuffix, string nickName, string emailAddress, string requestedUsername, string requestedPassword);

        [OperationContract]
        [WebInvoke(UriTemplate = "verify/sms/send")]
        string SendSms(string phoneNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "verify/sms/receive")]
        void ReceiveSms(System.IO.Stream stream);

        [OperationContract]
        [WebInvoke(UriTemplate = "verify/sms/status")]
        void HandleSmsStatus(System.IO.Stream stream);

        [OperationContract]
        [WebInvoke(UriTemplate = "verify/sms/handle-call")]
        void HandleCallToTwilioSms();

        [OperationContract]
        [WebInvoke(UriTemplate = "verify/voice/call")]
        string InitiateVoiceCall(string phoneNumber);

        [OperationContract]
        [WebInvoke(UriTemplate = "verify/voice/answer")]
        void ConductVoiceCall(System.IO.Stream stream);

        [OperationContract]
        [WebInvoke(UriTemplate = "verify/voice/status")]
        void HandleVoiceCallStatus(System.IO.Stream stream);
    }
}
