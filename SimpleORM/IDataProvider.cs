using System.Data;
using System.Data.Common;

namespace SimpleORM;

public interface IDataProvider
{
    void OpenConnection();

    void CloseConnection();
    
    IDbCommand Execute(string sql);
}