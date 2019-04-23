using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class SelectQuery : DmlQuery
    {
        public List<Expression> SelectExpressions;
        public FromClause From;
        public WhereClause Where;
        public uint? Limit;
        public OrderByClause OrderBy;
        public GroupByClause GroupBy;
        public bool LockForUpdate;

        public SelectQuery()
        {
            SelectExpressions = new List<Expression>();
            From = new FromClause();
            Where = new WhereClause();
            Limit = null;
            OrderBy = new OrderByClause();
            GroupBy = new GroupByClause();
            LockForUpdate = false;
        }

        public SelectQuery(SelectQuery other)
        {
            SelectExpressions = new List<Expression>(other.SelectExpressions.Capacity);
            SelectExpressions.AddRange(other.SelectExpressions.Select(x => x.Clone() as Expression));

            From = new FromClause(other.From);

            Where = new WhereClause(other.Where);

            Limit = other.Limit;

            OrderBy = new OrderByClause(other.OrderBy);

            GroupBy = new GroupByClause(other.GroupBy);

            LockForUpdate = other.LockForUpdate;
        }

        public override List<TableRef> GetTables()
        {
            return From.GetTables();
        }

        public override string ToString() => DialectResolver.Dialect.SelectQueryToString(this);

        public override object Clone() => new SelectQuery(this);
    }
}