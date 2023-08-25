using System.Linq.Expressions;
using System.Text;

namespace SimpleORM;

public class SqlQueryBuilder : ExpressionVisitor
{
    private readonly StringBuilder _sqlQueryBuilder = new();
    
    private bool _isFirstConst = true;

    public string SqlQuery => _sqlQueryBuilder.ToString();

    protected override Expression VisitBinary(BinaryExpression node)
    {
        Visit(node.Left);

        switch (node.NodeType)
        {
            case ExpressionType.AndAlso:
                _sqlQueryBuilder.Append(" AND ");
                break;
            case ExpressionType.OrElse:
                _sqlQueryBuilder.Append(" OR ");
                break;
            case ExpressionType.Equal:
                _sqlQueryBuilder.Append(" = ");
                break;
            case ExpressionType.NotEqual:
                _sqlQueryBuilder.Append(" != ");
                break;
            case ExpressionType.GreaterThan:
                _sqlQueryBuilder.Append(" > ");
                break;
            case ExpressionType.LessThan:
                _sqlQueryBuilder.Append(" < ");
                break;
            case ExpressionType.GreaterThanOrEqual:
                _sqlQueryBuilder.Append(" >= ");
                break;
            case ExpressionType.LessThanOrEqual:
                _sqlQueryBuilder.Append(" <= ");
                break;
        }

        Visit(node.Right);
        
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        _sqlQueryBuilder.Append(node.Member.Name);
        return node;
    }
    
    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (_isFirstConst)
        {
            _sqlQueryBuilder.Append($"SELECT * FROM {(node.Type.GetGenericArguments()[0]).Name} WHERE ");
            _isFirstConst = false;
        }
        else
        {
            if (node.Value is null)
            {
                _sqlQueryBuilder.Append($"NULL");
            }
            else
            if (IsNumber(node.Value))
            {
                _sqlQueryBuilder.Append($"{node.Value}");
            }
            else
            if (node.Value is bool boolValue)
            {
                _sqlQueryBuilder.Append($"{ (boolValue ? "1 = 1" : "1 = 0") }");
            }
            else
            {
                _sqlQueryBuilder.Append($"'{node.Value}'");
            }
        }
        return node;
    }
    private bool IsNumber(object value)
    {
        return value is sbyte
               || value is byte
               || value is short
               || value is ushort
               || value is int
               || value is uint
               || value is long
               || value is ulong
               || value is float
               || value is double
               || value is decimal;
    }
}