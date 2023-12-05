using System.Linq.Expressions;

namespace FeatureExamples;

public class ParameterReplacer : ExpressionVisitor
{
    private readonly ParameterExpression _oldParameter;
    private readonly Expression _replacement;

    public ParameterReplacer(ParameterExpression oldParameter, Expression replacement)
    {
        _oldParameter = oldParameter;
        _replacement = replacement;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        if (node == _oldParameter)
            return _replacement;
        return base.VisitParameter(node);
    }

    public static void Example()
    {
        // Usage:
        Expression<Func<int, int>> expr = x => x + 1;
        var replacer = new ParameterReplacer(expr.Parameters[0], Expression.Constant(68));
        var newExpr = replacer.Visit(expr.Body) as BinaryExpression;

        Console.WriteLine(newExpr);  // Outputs: (68 + 1)
    }
}
