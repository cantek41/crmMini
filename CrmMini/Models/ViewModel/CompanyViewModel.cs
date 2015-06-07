using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrmMini.Models
{
    public class CompanyViewModel
    {
        public int COMPANY_CODE { get; set; }
        public string COMPANY_NAME { get; set; }
        public string COMPANY_COMMERCIAL_CODE { get; set; }        
        public string ADDRESS { get; set; }       
        public Nullable<int> PHONE { get; set; }
        public Nullable<int> FAX { get; set; }
        public string MAIL { get; set; }
        public string WEBADDRESS { get; set; }       
        public Nullable<int> COMPANY_REGION { get; set; }
    }
}