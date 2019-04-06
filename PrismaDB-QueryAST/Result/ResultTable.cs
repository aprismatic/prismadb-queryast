using PrismaDB.QueryAST.DML;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.Result
{
    public class ResultTable : PrismaDB.Result.ResultTable
    {
        public new ResultColumnList Columns => (ResultColumnList)base.Columns;

        public ResultTable() : this("") { }

        public ResultTable(string tableName)
        {
            base.Columns = new ResultColumnList(this);
            _rows = new List<PrismaDB.Result.ResultRow>();
            TableName = tableName;
        }

        public ResultTable(ResultReader reader)
        {
            base.Columns = new ResultColumnList(this);
            _rows = new List<PrismaDB.Result.ResultRow>();
            RowsAffected = reader.RowsAffected;

            foreach (var column in reader.Columns)
                Columns.Add(column);

            while (reader.Read())
                _rows.Add(NewRow(reader.CurrentRow as ResultRow));
        }

        public new ResultRow NewRow()
        {
            return new ResultRow(this);
        }

        public ResultRow NewRow(ResultRow other)
        {
            return new ResultRow(this, other);
        }

        public void Sort(IEnumerable<Tuple<string, OrderDirection>> orderColumns)
        {
            foreach (var orderTuple in orderColumns.Reverse())
            {
                switch (orderTuple.Item2)
                {
                    case OrderDirection.ASC:
                        _rows = _rows.OrderBy(x => x[orderTuple.Item1]).ToList();
                        break;
                    case OrderDirection.DESC:
                        _rows = _rows.OrderByDescending(x => x[orderTuple.Item1]).ToList();
                        break;
                }
            }
        }

        public void Sort(IEnumerable<Tuple<Expression, OrderDirection>> orderColumns)
        {
            foreach (var orderTuple in orderColumns.Reverse())
            {
                switch (orderTuple.Item2)
                {
                    case OrderDirection.ASC:
                        _rows = _rows.OrderBy(x => ((ResultRow)x)[orderTuple.Item1]).ToList();
                        break;
                    case OrderDirection.DESC:
                        _rows = _rows.OrderByDescending(x => ((ResultRow)x)[orderTuple.Item1]).ToList();
                        break;
                }
            }
        }
    }
}
