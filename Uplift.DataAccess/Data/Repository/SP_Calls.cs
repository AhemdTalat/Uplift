using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Uplift.DataAccess.Data.Repository.IRepository;

namespace Uplift.DataAccess.Data.Repository
{
    public class SP_Calls : ISP_Calls
    {
        private readonly ApplicationDbContext _db;
        private static string ConnectionString;

        public SP_Calls(ApplicationDbContext db)
        {
            _db = db;
            ConnectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public T ExecuteReturnScaler<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sql = new SqlConnection(ConnectionString))
            {
                sql.Open();
                return (T)Convert.ChangeType(sql.ExecuteScalar<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure), typeof(T));
            }
        }

        public void ExecuteWithOutReturn(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sql = new SqlConnection(ConnectionString))
            {
                sql.Open();
                sql.Execute(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> RetunList<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection sql = new SqlConnection(ConnectionString))
            {
                sql.Open();
                return sql.Query<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
