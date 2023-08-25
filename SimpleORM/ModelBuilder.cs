using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace SimpleORM;

public class ModelBuilder<T>
{
    private readonly IDataReader _dataReader;
    
    public ModelBuilder(IDataReader dataReader)
    {
        _dataReader = dataReader;
    }

    public IEnumerable<T> Build()
    {
        var list = new List<T>();
        var func = BuildFunction();
        while (_dataReader.Read())
        {
            list.Add(func(_dataReader));
        }

        return list;
    }
    
    private Func<IDataReader, T> BuildFunction()
    {
        var dataReaderParam = Expression.Parameter(typeof(IDataReader));
        var newExpression = Expression.New(typeof(T));
        var memberInitExpression = Expression.MemberInit(newExpression, typeof(T)
            .GetProperties()
            .Select(prop => Expression.Bind(prop, BuildReadColumnExpression(dataReaderParam, prop))));

        return Expression.Lambda<Func<IDataReader, T>>(memberInitExpression, dataReaderParam).Compile();
    }
    
    private Expression BuildReadColumnExpression(Expression dataReader, PropertyInfo propInfo)
    {
        var propNameParam = Expression.Constant(propInfo.Name);
        var interfaceConversion = Expression.Convert(dataReader, typeof(IDataRecord));
        var dataReaderValueExpression = Expression.Property(interfaceConversion, 
            "Item", propNameParam);

        return Expression.Convert(dataReaderValueExpression, propInfo.PropertyType);
    }
}