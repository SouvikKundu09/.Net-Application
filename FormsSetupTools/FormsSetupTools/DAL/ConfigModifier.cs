using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ConfigModifier
    {
        public string[] GetDBConfig()
        {
            string[] config = new string[2];
            try
            {
                string connectString = ConfigurationManager.ConnectionStrings["sqs"].ToString();
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectString);
                string server = builder.DataSource;
                string database = builder.InitialCatalog;
                config[0] = server.Split('.')[0];
                config[1] = database;
            }
            catch { }

            return config;
        }

        public bool ModifyConnectionString(string server, string database)
        {
            try
            {
                var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
                connectionStringsSection.ConnectionStrings["sqs"].ConnectionString = "Data Source=" + server + ".aceprs.intr;Initial Catalog=" + database + ";Integrated Security=SSPI;";
                config.Save(ConfigurationSaveMode.Modified, true);
                ConfigurationManager.RefreshSection("connectionStrings");

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
