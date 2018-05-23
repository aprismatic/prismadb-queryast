using System.Linq;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.Result
{
    public class DataRow
    {
        private readonly DataTable _table;

        public object[] Items { get; }

        internal DataRow(DataTable table)
        {
            _table = table;
            Items = new object[_table.Columns.Count];
        }

        public object Get(int index)
        {
            return Items[index];
        }

        public object Get(string columnName)
        {
            return Items[_table.Columns.IndexOf(_table.Columns.First(x => x.ColumnName.Equals(columnName)))];
        }

        public object Get(Expression exp)
        {
            return Items[_table.Columns.IndexOf(_table.Columns.First(x => x.Expression.Equals(exp)))];
        }

        public object Get(ColumnDefinition columnDef)
        {
            return Items[_table.Columns.IndexOf(_table.Columns.First(x => x.ColumnDefinition.Equals(columnDef)))];
        }
    }
}
