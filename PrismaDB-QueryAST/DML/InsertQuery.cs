using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PrismaDB.QueryAST.DML
{
    public class InsertQuery : DMLQuery
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
            foreach (var col in other.Columns)
                Columns.Add((ColumnRef)col.Clone());

            Values = new List<List<Expression>>(other.Values.Capacity);
            foreach (var vallist in other.Values)
            {
                var new_vallist = new List<Expression>(vallist.Capacity);
                new_vallist.AddRange(vallist.Select(val => val.Clone()));
                Values.Add(new_vallist);
            }
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.InsertQueryToString(this);
        }
    }
}
