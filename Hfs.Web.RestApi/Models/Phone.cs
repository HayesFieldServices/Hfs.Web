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
    
    public partial class Phone
    {
        public int phone_id { get; set; }
        public int account { get; set; }
        public string number { get; set; }
        public int region { get; set; }
        public int type { get; set; }
        public bool is_primary { get; set; }
        public Nullable<System.DateTime> verify_dt { get; set; }
    
        public virtual Account Account1 { get; set; }
        public virtual PhoneRegion PhoneRegion { get; set; }
        public virtual PhoneType PhoneType { get; set; }
    }
}
