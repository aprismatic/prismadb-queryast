using System;
using System.Collections.Generic;
using System.Linq;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.Result
{
    public class ResultColumnList
    {
        private readonly ResultTable _table;

        internal List<ResultColumnHeader> Headers { get; }

        internal ResultColumnList(ResultTable table)
        {
            _table = table;
            Headers = new List<ResultColumnHeader>();
        }

        public int Count()
        {
            return Headers.Count();
        }

        public ResultColumnHeader Get(int index)
        {
            return Headers[index];
        }

        public void Add(ResultColumnHeader column)
        {
            if (_table.Rows.Count > 0)
                throw new ApplicationException("Table is not empty.");
            Headers.Add(column);
        }

        public void Remove(ResultColumnHeader column)
        {
            Remove(Headers.IndexOf(column));
        }

        public void Remove(string columnName)
        {
            Remove(Headers.IndexOf(Headers.Single(x => x.ColumnName.Equals(columnName))));
        }

        public void Remove(Expression exp)
        {
            Remove(Headers.IndexOf(Headers.Single(x => x.Expression.Equals(exp))));
        }

        public void Remove(int index)
        {
            foreach (var row in _table.Rows)
            {
                row.Items.RemoveAt(index);
            }
            Headers.RemoveAt(index);
        }
    }
}
