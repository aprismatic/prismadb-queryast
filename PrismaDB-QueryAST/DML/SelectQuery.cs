using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class SelectQuery : DmlQuery
    {
        public List<Expression> SelectExpressions;
        public List<TableRef> FromTables;
        public WhereClause Where;
        public uint? Limit;
        public List<JoinClause> Joins;
        public OrderByClause OrderBy;
        public GroupByClause GroupBy;
        public bool LockForUpdate;

        public SelectQuery()
        {
            SelectExpressions = new List<Expression>();
            FromTables = new List<TableRef>();
            Where = new WhereClause();
            Limit = null;
            Joins = new List<JoinClause>();
            OrderBy = new OrderByClause();
            GroupBy = new GroupByClause();
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

            Joins = new List<JoinClause>(other.Joins.Capacity);
            Joins.AddRange(other.Joins.Select(x => x.Clone() as JoinClause));

            OrderBy = new OrderByClause(other.OrderBy);

            GroupBy = new GroupByClause(other.GroupBy);

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
}
