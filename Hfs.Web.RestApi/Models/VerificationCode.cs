//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hfs.Web.RestApi.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class VerificationCode
    {
        public int code_id { get; set; }
        public Nullable<int> code { get; set; }
        public string phone_number { get; set; }
        public System.DateTime expiration_dt { get; set; }
    }
}