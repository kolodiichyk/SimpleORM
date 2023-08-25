using System.Data;

namespace SimpleORM;

public abstract class SimpleORM
{
    private readonly IDataProvider _dataProvider;
    
    public SimpleORM(IDataProvider dataProvider)
    {
        _dataProvider = dataProvider;
    }

    public IEnumerable<T> Get<T>(IQueryable<T> query)
    {
        try
        {
            var sql = query.BuildSQl();
            _dataProvider.OpenConnection();
            using var command =  _dataProvider.Execute(sql);
            using IDataReader reader = command.ExecuteReader();
            var modelBuilder = new ModelBuilder<T>(reader);
            return modelBuilder.Build();
        }
        finally
        {
            _dataProvider.CloseConnection();
        }
    }
}