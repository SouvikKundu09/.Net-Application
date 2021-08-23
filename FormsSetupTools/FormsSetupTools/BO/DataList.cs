using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DataList
    {
        public string[] GetServers()
        {
            string[] ServerList = new string[2];
            try
            {
                ServerList[0] = "MFRKNTACESQT04";
                ServerList[1] = "MFRKNTACESQT11";

                return ServerList;
            }
            catch
            {
                return null;
            }
        }

        public Dictionary<string, string> GetCompanyList()
        {
            Dictionary<string, string> company = new Dictionary<string, string>();
            company.Add("All", "-1");
            company.Add("Atlantic Employers Insurance Company", "06");
            company.Add("ACE Insurance Company of the Midwest", "03");
            company.Add("Bankers Standard Insurance Company", "08");
            company.Add("Pacific Employers Insurance Company", "09");

            return company;
        }

        public Dictionary<string, string> GetUserlineList()
        {
            Dictionary<string, string> userline = new Dictionary<string, string>();
            userline.Add("Select", "-1");
            userline.Add("Auto", "01");
            userline.Add("Dwelling Fire", "22");
            userline.Add("Home", "24");
            userline.Add("Umbrella", "44");
            userline.Add("Valuables", "68");
            userline.Add("Watercraft", "34");

            return userline;
        }
    }
}
