using System;
using System.Collections.Generic;
using System.Text;

namespace PrismaDB.QueryAST.DML
{
    public class SelectQuery : DMLQuery
    {
        public List<Expression> SelectExpressions;
        public List<TableRef> FromTables;
        public WhereClause Where;

        public SelectQuery()
        {
            SelectExpressions = new List<Expression>();
            FromTables = new List<TableRef>();
            Where = new WhereClause();
        }

        public SelectQuery(SelectQuery other)
        {
            SelectExpressions = new List<Expression>(other.SelectExpressions.Capacity);
            foreach (var expr in other.SelectExpressions)
                SelectExpressions.Add(expr.Clone());

            FromTables = new List<TableRef>(other.FromTables.Capacity);
            foreach (var tref in other.FromTables)
                FromTables.Add(tref.Clone());

            Where = new WhereClause(other.Where);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.SelectQueryToString(this);
        }
    }
}
