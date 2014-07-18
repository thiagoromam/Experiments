using System;
using System.Linq.Expressions;

namespace ExpressionTrees
{
    static class PredicateBuilder
    {
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            return Combine(expr1, expr2, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2)
        {
            return Combine(expr1, expr2, Expression.AndAlso);
        }

        private static Expression<Func<T, bool>> Combine<T>(this Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, Func<Expression, Expression, BinaryExpression> method)
        {
            var invokedExpr = Expression.Invoke(expr2, expr1.Parameters);
            return Expression.Lambda<Func<T, bool>>(method(expr1.Body, invokedExpr), expr1.Parameters);
        }
    }

    public struct PredicateBuilder<T>
    {
        private Expression<Func<T, bool>> _expression;

        public PredicateBuilder(Expression<Func<T, bool>> expression)
        {
            _expression = expression;
        }

        public static implicit operator Expression<Func<T, bool>>(PredicateBuilder<T> predicate)
        {
            return predicate._expression;
        }

        public void And(Expression<Func<T, bool>> expression)
        {
            _expression = _expression == null ? expression : _expression.And(expression);
        }
        public void Or(Expression<Func<T, bool>> expression)
        {
            _expression = _expression == null ? expression : _expression.Or(expression);
        }

        public static PredicateBuilder<T> operator &(PredicateBuilder<T> predicate, Expression<Func<T, bool>> expression)
        {
            return new PredicateBuilder<T>(predicate._expression == null ? expression : predicate._expression.And(expression));
        }
        public static PredicateBuilder<T> operator |(PredicateBuilder<T> predicate, Expression<Func<T, bool>> expression)
        {
            return new PredicateBuilder<T>(predicate._expression == null ? expression : predicate._expression.Or(expression));
        }

    }
}
