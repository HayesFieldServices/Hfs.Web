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
    
    public partial class Permission
    {
        public Permission()
        {
            this.Groups = new HashSet<Group>();
        }
    
        public int permission_id { get; set; }
        public string name { get; set; }
    
        public virtual ICollection<Group> Groups { get; set; }
    }
}