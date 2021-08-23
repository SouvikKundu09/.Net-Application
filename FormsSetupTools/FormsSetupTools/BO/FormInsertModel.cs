using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class FormInsertModel
    {
        public string FormNo { get; set; }
        public string Userline { get; set; }
        public string State { get; set; }
        public string FormVersion { get; set; }
        public string FormName { get; set; }
        public string FormNameAbbr { get; set; }
        public string Company { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime NewBusinessEntryDate { get; set; }
        public DateTime RenewalEntryDate { get; set; }
        public string ProductIndicator { get; set; }
        public string MdlAftrFormNo { get; set; }
        public string MdlAftrUserline { get; set; }
        public string MdlAftrState { get; set; }
        public string MdlAftrFormVersion { get; set; }
        public string MdlAftrCompany { get; set; }
        public string MdlAftrProductIndicator { get; set; }
        public string FormType { get; set; }
    }
}
