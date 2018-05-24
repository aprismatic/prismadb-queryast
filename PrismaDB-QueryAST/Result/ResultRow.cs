using System;
using System.Collections.Generic;
using System.Linq;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.Result
{
    public class ResultRow
    {
        private readonly ResultTable _table;

        internal List<object> Items { get; }

        internal ResultRow(ResultTable table)
        {
            _table = table;
            Items = new List<object>(_table.Columns.Headers.Count);
        }

        public int Count()
        {
            return Items.Count();
        }

        public void Add(object value)
        {
            if (Items.Count >= _table.Columns.Headers.Count)
                throw new ApplicationException("Items in row has reached the number of columns in table.");
            Items.Add(value);
        }

        public void Set(int index, object value)
        {
            if (index > _table.Columns.Headers.Count - 1)
                throw new ApplicationException("Index is out of the range of columns in table.");
            Items[index] = value;
        }

        public void Set(string columnName, object value)
        {
            Items[_table.Columns.Headers.IndexOf(
                    _table.Columns.Headers.Single(
                        x => x.ColumnName.Equals(columnName)))] = value;
        }

        public void Set(Expression exp, object value)
        {
            Items[_table.Columns.Headers.IndexOf(
                _table.Columns.Headers.Single(
                    x => x.Expression.Equals(exp)))] = value;
        }

        public void Set(ColumnDefinition columnDef, object value)
        {
            Items[_table.Columns.Headers.IndexOf(
                _table.Columns.Headers.Single(
                    x => x.ColumnDefinition.Equals(columnDef)))] = value;
        }

        public object Get(int index)
        {
            return Items[index];
        }

        public object Get(string columnName)
        {
            return Items[_table.Columns.Headers.IndexOf(
                _table.Columns.Headers.Single(
                    x => x.ColumnName.Equals(columnName)))];
        }

        public object Get(Expression exp)
        {
            return Items[_table.Columns.Headers.IndexOf(
                _table.Columns.Headers.Single(
                    x => x.Expression.Equals(exp)))];
        }

        public object Get(ColumnDefinition columnDef)
        {
            return Items[_table.Columns.Headers.IndexOf(
                _table.Columns.Headers.Single(
                    x => x.ColumnDefinition.Equals(columnDef)))];
        }
    }
}
