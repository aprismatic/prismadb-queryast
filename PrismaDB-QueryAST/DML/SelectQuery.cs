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

        public SelectQuery()
        {
            SelectExpressions = new List<Expression>();
            FromTables = new List<TableRef>();
            Where = new WhereClause();
            Limit = null;
        }

        public SelectQuery(SelectQuery other)
        {
            SelectExpressions = new List<Expression>(other.SelectExpressions.Capacity);
            SelectExpressions.AddRange(other.SelectExpressions.Select(x => x.Clone() as Expression));

            FromTables = new List<TableRef>(other.FromTables.Capacity);
            FromTables.AddRange(other.FromTables.Select(x => x.Clone()));

            Where = new WhereClause(other.Where);

            Limit = other.Limit;
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
