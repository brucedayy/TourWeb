//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TourWeb
{
    using System;
    using System.Collections.Generic;
    
    public partial class Note
    {
        public int Nid { get; set; }
        public string UserName { get; set; }
        public string Details { get; set; }
        public System.DateTime PubTime { get; set; }
    
        public virtual User User { get; set; }
    }
}
