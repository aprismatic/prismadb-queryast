using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.Result
{
    public class ResultRow : IEnumerable<object>
    {
        private readonly ResultTable _table;

        internal List<object> Items { get; }

        internal ResultRow(ResultTable table)
        {
            _table = table;
            Items = new List<object>(_table.Columns.Headers.Count);
        }

        public int Count => this.Count();

        public object this[int index]
        {
            get => Items[index];

            set
            {
                if (index > _table.Columns.Headers.Count - 1)
                    throw new ApplicationException("Index is out of the range of columns in table.");
                Items[index] = value;
            }
        }

        public object this[string columnName]
        {
            get => Items[_table.Columns.Headers.IndexOf(
                _table.Columns.Headers.Single(
                    x => x.ColumnName.Equals(columnName)))];
            set
            {
                Items[_table.Columns.Headers.IndexOf(
                    _table.Columns.Headers.Single(
                        x => x.ColumnName.Equals(columnName)))] = value;
            }
        }

        public object this[Expression exp]
        {
            get => Items[_table.Columns.Headers.IndexOf(
                _table.Columns.Headers.Single(
                    x => x.Expression.Equals(exp)))];

            set
            {
                Items[_table.Columns.Headers.IndexOf(
                    _table.Columns.Headers.Single(
                        x => x.Expression.Equals(exp)))] = value;
            }
        }

        public object this[ColumnDefinition columnDef]
        {
            get => Items[_table.Columns.Headers.IndexOf(
                _table.Columns.Headers.Single(
                    x => x.ColumnDefinition.Equals(columnDef)))];

            set
            {
                Items[_table.Columns.Headers.IndexOf(
                    _table.Columns.Headers.Single(
                        x => x.ColumnDefinition.Equals(columnDef)))] = value;
            }
        }

        public object this[ResultColumnHeader header]
        {
            get => Items[_table.Columns.Headers.IndexOf(header)];

            set
            {
                Items[_table.Columns.Headers.IndexOf(header)] = value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<object>)Items).GetEnumerator();
        }

        public IEnumerator<object> GetEnumerator()
        {
            return ((IEnumerable<object>)Items).GetEnumerator();
        }

        public void Add(object value)
        {
            if (Items.Count >= _table.Columns.Headers.Count)
                throw new ApplicationException("Items in row has reached the number of columns in table.");
            Items.Add(value);
        }

        public void Add(IEnumerable<object> valList)
        {
            foreach (var val in valList)
                Add(val);
        }
    }
}