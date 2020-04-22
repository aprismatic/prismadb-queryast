using System;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DML
{
    public class UpdateQuery : DmlQuery
    {
        public TableRef UpdateTable;
        public List<Tuple<ColumnRef, ConstantContainer>> UpdateExpressions;
        public WhereClause Where;

        public UpdateQuery()
        {
            UpdateTable = new TableRef("");
            UpdateExpressions = new List<Tuple<ColumnRef, ConstantContainer>>();
            Where = new WhereClause();
        }

        public UpdateQuery(UpdateQuery other)
        {
            UpdateTable = other.UpdateTable.Clone();

            UpdateExpressions = new List<Tuple<ColumnRef, ConstantContainer>>(other.UpdateExpressions.Count);
            foreach (var pr in other.UpdateExpressions)
            {
                var newpr = new Tuple<ColumnRef, ConstantContainer>((ColumnRef)pr.Item1.Clone(), (ConstantContainer)pr.Item2.Clone());
                UpdateExpressions.Add(newpr);
            }

            Where = other.Where;
        }

        public override List<TableRef> GetTables() => new List<TableRef> { UpdateTable };

        public override List<ConstantContainer> GetConstants()
        {
            var res = new List<ConstantContainer>();

            foreach (var exp in UpdateExpressions)
                if (exp.Item2 is ConstantContainer cc)
                    res.Add(cc);

            res.AddRange(Where.GetConstants());

            return res;
        }

        public override string ToString() => DialectResolver.Dialect.UpdateQueryToString(this);

        public override object Clone() => new UpdateQuery(this);
    }
}
