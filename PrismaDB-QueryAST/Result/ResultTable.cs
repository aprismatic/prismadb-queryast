using PrismaDB.Commons;
using PrismaDB.QueryAST.DML;
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

        public void Sort(IEnumerable<Pair<string, OrderDirection>> orderColumns)
        {
            foreach (var orderPair in orderColumns.Reverse())
            {
                switch (orderPair.Second)
                {
                    case OrderDirection.ASC:
                        _rows = _rows.OrderBy(x => x[orderPair.First]).ToList();
                        break;
                    case OrderDirection.DESC:
                        _rows = _rows.OrderByDescending(x => x[orderPair.First]).ToList();
                        break;
                }
            }
        }

        public void Sort(IEnumerable<Pair<Expression, OrderDirection>> orderColumns)
        {
            foreach (var orderPair in orderColumns.Reverse())
            {
                switch (orderPair.Second)
                {
                    case OrderDirection.ASC:
                        _rows = _rows.OrderBy(x => ((ResultRow)x)[orderPair.First]).ToList();
                        break;
                    case OrderDirection.DESC:
                        _rows = _rows.OrderByDescending(x => ((ResultRow)x)[orderPair.First]).ToList();
                        break;
                }
            }
        }
    }
}
