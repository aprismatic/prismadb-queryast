using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class OrderByClause : ICloneable
    {
        public List<Tuple<Expression, OrderDirection>> OrderColumns;

        public OrderByClause()
        {
            OrderColumns = new List<Tuple<Expression, OrderDirection>>();
        }

        public OrderByClause(OrderByClause other)
        {
            OrderColumns = new List<Tuple<Expression, OrderDirection>>(other.OrderColumns.Capacity);
            OrderColumns.AddRange(other.OrderColumns.Select(
                x => new Tuple<Expression, OrderDirection>(x.Item1.Clone() as Expression, x.Item2)));
        }

        public object Clone()
        {
            return new OrderByClause(this);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.OrderByClauseToString(this);
        }

        public bool CheckDataRow(DataRow r)
        {
            return OrderColumns.Any(c => (bool)c.Item1.Eval(r));
        }

        public List<ColumnRef> GetOrderByColumns()
        {
            var orderByCols = OrderColumns.SelectMany(x => x.Item1.GetColumns());
            return orderByCols.Distinct().ToList();
        }
    }

    public enum OrderDirection
    {
        ASC,
        DESC
    }
}