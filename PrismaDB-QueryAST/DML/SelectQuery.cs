using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class SelectQuery : DmlQuery
    {
        public List<Expression> SelectExpressions;
        public List<TableRef> FromTables;
        public List<SelectSubQuery> FromSubQueries;
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
            FromSubQueries = new List<SelectSubQuery>();
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

            FromSubQueries = new List<SelectSubQuery>(other.FromSubQueries.Capacity);
            FromSubQueries.AddRange(other.FromSubQueries.Select(x => x.Clone()));

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

    public class SelectSubQuery
    {
        public SelectQuery Select { get; set; }
        public Identifier Alias { get; set; }

        public SelectSubQuery()
        {
            Select = new SelectQuery();
            Alias = new Identifier();
        }

        public SelectSubQuery(SelectSubQuery other)
        {
            Select = (SelectQuery)other.Select.Clone();
            Alias = other.Alias.Clone();
        }

        public SelectSubQuery Clone()
        {
            return new SelectSubQuery(this);
        }
    }
}