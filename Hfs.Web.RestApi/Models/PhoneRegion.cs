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
    
    public partial class PhoneRegion
    {
        public PhoneRegion()
        {
            this.Phones = new HashSet<Phone>();
        }
    
        public int phone_region_id { get; set; }
        public string region { get; set; }
        public string code { get; set; }
    
        public virtual ICollection<Phone> Phones { get; set; }
    }
}
