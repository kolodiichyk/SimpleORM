using System.Data;

namespace SimpleORM;

public static class IQueryableExtension
{
    public static IEnumerable<T> Execute<T>(this IQueryable<T> query, SimpleORM simpleORM)
    {
        return simpleORM.Get(query);
    }
    
    public static string BuildSQl<T>(this IQueryable<T> query)
    {
        var sqlQueryBuilder = new SqlQueryBuilder();
        sqlQueryBuilder.Visit(query.Expression);
        
        return sqlQueryBuilder.SqlQuery;
    }
}