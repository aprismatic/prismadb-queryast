using System;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DML
{
    public class UpdateQuery : DmlQuery
    {
        public TableRef UpdateTable;
        public List<Tuple<ColumnRef, Constant>> UpdateExpressions;
        public WhereClause Where;

        public UpdateQuery()
        {
            UpdateTable = new TableRef("");
            UpdateExpressions = new List<Tuple<ColumnRef, Constant>>();
            Where = new WhereClause();
        }

        public UpdateQuery(UpdateQuery other)
        {
            UpdateTable = other.UpdateTable.Clone();

            UpdateExpressions = new List<Tuple<ColumnRef, Constant>>(other.UpdateExpressions.Count);
            foreach (var pr in other.UpdateExpressions)
            {
                var newpr = new Tuple<ColumnRef, Constant>((ColumnRef)pr.Item1.Clone(), (Constant)pr.Item2.Clone());
                UpdateExpressions.Add(newpr);
            }

            Where = other.Where;
        }

        public override List<TableRef> GetTables() => new List<TableRef> { UpdateTable.Clone() };

        public override List<PlaceholderConstant> GetPlaceholders()
        {
            var res = new List<PlaceholderConstant>();

            foreach (var exp in UpdateExpressions)
                if (exp.Item2 is PlaceholderConstant phc)
                    res.Add(phc);

            res.AddRange(Where.GetPlaceholders());

            return res;
        }

        public override string ToString() => DialectResolver.Dialect.UpdateQueryToString(this);

        public override object Clone() => new UpdateQuery(this);
    }
}
