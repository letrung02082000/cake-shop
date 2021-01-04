using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLib
{
    public class DatabaseAccess
    {
        // Cake CRUD
        public static List<CakeModel> LoadCake()
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                var output = connection.Query<CakeModel>($"", new DynamicParameters());
                return output.ToList();
            }
        }

        public static int SaveCake(CakeModel cake)
        {
            using (IDbConnection connection = new SQLiteConnection(LoadConnectionString()))
            {
                int cakeId = connection.ExecuteScalar<int>("INSERT INTO cake(cakeId, cakeCode, cakeName, cakeName2, cakeCat, cakePrice, cakeDesc) VALUES(@CakeId, @CakeCode, @CakeName, @CakeName2, @CakeCat, @CakePrice, @CakeDesc)", cake);
                return cakeId;
            }
        }

        private static string LoadConnectionString(string id = "default")
        {
            return ConfigurationManager.ConnectionStrings[id].ConnectionString;
        }
    }
}
