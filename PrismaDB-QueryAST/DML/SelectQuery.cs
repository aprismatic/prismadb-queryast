using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class SelectQuery : DMLQuery
    {
        public List<Expression> SelectExpressions;
        public List<TableRef> FromTables;
        public WhereClause Where;
        public uint? Limit;
        public List<Tuple<Expression, OrderDirection>> OrderBy;
        public bool LockForUpdate;

        public SelectQuery()
        {
            SelectExpressions = new List<Expression>();
            FromTables = new List<TableRef>();
            Where = new WhereClause();
            Limit = null;
            OrderBy = new List<Tuple<Expression, OrderDirection>>();
            LockForUpdate = false;
        }

        public SelectQuery(SelectQuery other)
        {
            SelectExpressions = new List<Expression>(other.SelectExpressions.Capacity);
            SelectExpressions.AddRange(other.SelectExpressions.Select(x => x.Clone() as Expression));

            FromTables = new List<TableRef>(other.FromTables.Capacity);
            FromTables.AddRange(other.FromTables.Select(x => x.Clone()));

            Where = new WhereClause(other.Where);

            Limit = other.Limit;

            OrderBy = new List<Tuple<Expression, OrderDirection>>(other.OrderBy.Capacity);
            OrderBy.AddRange(other.OrderBy.Select(
                x => new Tuple<Expression, OrderDirection>(x.Item1.Clone() as Expression, x.Item2)));

            LockForUpdate = other.LockForUpdate;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.SelectQueryToString(this);
        }

        public override object Clone()
        {
            return new SelectQuery(this);
        }
    }

    public enum OrderDirection
    {
        ASC,
        DESC
    }
}
