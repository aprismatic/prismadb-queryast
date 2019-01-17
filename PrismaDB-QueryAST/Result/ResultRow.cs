using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;
using System.Linq;

namespace PrismaDB.QueryAST.Result
{
    public class ResultRow : PrismaDB.Result.ResultRow
    {
        protected ResultRow() { }

        internal ResultRow(ResultTable table)
            : base(table) { }

        public object this[Expression exp]
        {
            get => Items[((ResultColumnList)_table.Columns).Headers.IndexOf(
                ((ResultColumnList)_table.Columns).Headers.Single(
                    x => ((ResultColumnHeader)x).Expression.Equals(exp)))];

            set
            {
                Items[((ResultColumnList)_table.Columns).Headers.IndexOf(
                    ((ResultColumnList)_table.Columns).Headers.Single(
                        x => ((ResultColumnHeader)x).Expression.Equals(exp)))] = value;
            }
        }

        public object this[ColumnDefinition columnDef]
        {
            get => Items[((ResultColumnList)_table.Columns).Headers.IndexOf(
                ((ResultColumnList)_table.Columns).Headers.Single(
                    x => ((ResultColumnHeader)x).ColumnDefinition.Equals(columnDef)))];

            set
            {
                Items[((ResultColumnList)_table.Columns).Headers.IndexOf(
                    ((ResultColumnList)_table.Columns).Headers.Single(
                        x => ((ResultColumnHeader)x).ColumnDefinition.Equals(columnDef)))] = value;
            }
        }

        public object this[ResultColumnHeader header]
        {
            get => Items[((ResultColumnList)_table.Columns).Headers.IndexOf(header)];

            set
            {
                Items[((ResultColumnList)_table.Columns).Headers.IndexOf(header)] = value;
            }
        }
    }
}