//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CrmMini.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class GROUP
    {
        public int GROUP_CODE { get; set; }
        public int ROW_ORDER_NO { get; set; }
        public Nullable<int> UPPER_GROUP_CODE { get; set; }
        public Nullable<int> UPPER_GROUP_ROW_NO { get; set; }
        public string EXP_TR { get; set; }
        public string EXP_EN { get; set; }
        public string EXP_GR { get; set; }
        public Nullable<System.DateTime> LAST_UPDATE { get; set; }
        public string LAST_UPDATE_USER { get; set; }
        public Nullable<System.DateTime> CREATE_DATE { get; set; }
        public string CREATE_USER { get; set; }
        public Nullable<int> ORDER_BY { get; set; }
    }
}