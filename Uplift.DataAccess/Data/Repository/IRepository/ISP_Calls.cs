using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Uplift.DataAccess.Data.Repository.IRepository
{
    public interface ISP_Calls : IDisposable
    {
        IEnumerable<T> RetunList<T>(string procedureName, DynamicParameters param = null);
        void ExecuteWithOutReturn(string procedureName, DynamicParameters param = null);
        T ExecuteReturnScaler<T>(string procedureName, DynamicParameters param = null);
    }
}
