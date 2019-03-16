﻿using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace PrismaDB.QueryAST.Result
{
    public class ResultReader : PrismaDB.Result.ResultReader
    {
        public new ResultColumnList Columns => (ResultColumnList)base.Columns;

        public ResultReader() : this("") { }

        public ResultReader(string tableName)
        {
            base.Columns = new ResultColumnList(this);
            _rows = new BlockingCollection<PrismaDB.Result.ResultRow>();
            TableName = tableName;
        }

        public ResultReader(ResultTable table)
        {
            base.Columns = new ResultColumnList(this);
            _rows = new BlockingCollection<PrismaDB.Result.ResultRow>();

            foreach (var column in table.Columns)
                Columns.Add(column);

            new Task(() =>
            {
                foreach (var row in table.Rows)
                    Write(NewRow(row as ResultRow));
                _rows.CompleteAdding();
            }).Start();
        }

        public new ResultRow NewRow()
        {
            return new ResultRow(this);
        }

        public ResultRow NewRow(ResultRow other)
        {
            return new ResultRow(this, other);
        }

        public object this[ResultColumnHeader header]
        {
            get => CurrentRow[Columns.Headers.IndexOf(header)];
        }

        public object this[Expression exp]
        {
            get => CurrentRow[Columns.Headers.IndexOf(
                ((ResultColumnList)Columns).Headers.Single(
                    x => ((ResultColumnHeader)x).Expression.Equals(exp)))];
        }

        public object this[ColumnDefinition columnDef]
        {
            get => CurrentRow[Columns.Headers.IndexOf(
                ((ResultColumnList)Columns).Headers.Single(
                    x => ((ResultColumnHeader)x).ColumnDefinition.Equals(columnDef)))];
        }
    }
}
