using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BLL.Shared;
using DAL;

namespace BLL
{
    public class Dashboard
    {
        public string[] GetDBConfig()
        {
            ConfigModifier objConfigModifier = new ConfigModifier();
            return objConfigModifier.GetDBConfig();
        }

        public bool ModifyConnectionString(DashboardConfigModel objConfigBO)
        {
            ConfigModifier objConfigModifier = new ConfigModifier();
            return objConfigModifier.ModifyConnectionString(objConfigBO.Server, objConfigBO.Database);
        }
    }
}
