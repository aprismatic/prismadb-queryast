using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.Result
{
    public class ResultColumnList : IEnumerable<ResultColumnHeader>
    {
        private readonly ResultTable _table;

        internal List<ResultColumnHeader> Headers { get; }

        internal ResultColumnList(ResultTable table)
        {
            _table = table;
            Headers = new List<ResultColumnHeader>();
        }

        public int Count => this.Count();

        public ResultColumnHeader this[int index] => Headers[index];

        public ResultColumnHeader this[string columnName] =>
            this[Headers.IndexOf(Headers.Single(x => x.ColumnName.Equals(columnName)))];

        public ResultColumnHeader this[Expression exp] =>
            this[Headers.IndexOf(Headers.Single(x => x.Expression.Equals(exp)))];

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ResultColumnHeader>)Headers).GetEnumerator();
        }

        public IEnumerator<ResultColumnHeader> GetEnumerator()
        {
            return ((IEnumerable<ResultColumnHeader>)Headers).GetEnumerator();
        }

        public void Add(ResultColumnHeader column)
        {
            if (_table.Rows.Count > 0)
                throw new ApplicationException("Table is not empty.");
            Headers.Add(column);
        }

        public void Add()
        {
            Add(new ResultColumnHeader());
        }

        public void Add(string columnName, Type dataType = null, int? maxLength = null)
        {
            Add(new ResultColumnHeader(columnName, dataType, maxLength));
        }

        public void Add(Expression exp, Type dataType = null, int? maxLength = null)
        {
            Add(new ResultColumnHeader(exp, dataType, maxLength));
        }

        public void Add(Expression exp, ColumnDefinition columnDef, Type dataType = null, int? maxLength = null)
        {
            Add(new ResultColumnHeader(exp, columnDef, dataType, maxLength));
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
                row.Items.RemoveAt(index);
            Headers.RemoveAt(index);
        }
    }
}