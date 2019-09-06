using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class InsertQuery : DmlQuery
    {
        public TableRef Into;
        public List<ColumnRef> Columns;
        public List<List<Expression>> Values;

        public InsertQuery()
        {
            Into = new TableRef("");
            Columns = new List<ColumnRef>();
            Values = new List<List<Expression>>();
        }

        public InsertQuery(InsertQuery other)
        {
            Into = other.Into.Clone();

            Columns = new List<ColumnRef>(other.Columns.Capacity);
            Columns.AddRange(other.Columns.Select(x => x.Clone() as ColumnRef));

            Values = new List<List<Expression>>(other.Values.Capacity);
            foreach (var vallist in other.Values)
            {
                var new_vallist = new List<Expression>(vallist.Capacity);
                new_vallist.AddRange(vallist.Select(val => val.Clone() as Expression));
                Values.Add(new_vallist);
            }
        }

        public override List<TableRef> GetTables() => new List<TableRef> { Into };

        public override List<PlaceholderConstant> GetPlaceholders()
        {
            var res = new List<PlaceholderConstant>();

            foreach (var vallist in Values)
                foreach (var val in vallist)
                    res.AddRange(val.GetPlaceholders());

            return res;
        }

        public override string ToString() => DialectResolver.Dialect.InsertQueryToString(this);

        public override object Clone() => new InsertQuery(this);
    }
}
