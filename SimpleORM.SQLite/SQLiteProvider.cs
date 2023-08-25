using System.Data;
using System.Data.SQLite;

namespace SimpleORM.SQLite;

public class SQLiteProvider : IDataProvider
{
    private readonly SQLiteConnection _connection;

    public SQLiteProvider(string connectionString)
    {
        _connection = new SQLiteConnection(connectionString);
    }

    public void OpenConnection()
    {
        _connection.Open();
    }

    public void CloseConnection()
    {
        _connection.Close();
    }

    public IDbCommand Execute(string sql)
    {
        return new SQLiteCommand(sql, _connection);
    }
}