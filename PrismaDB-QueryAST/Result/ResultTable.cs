using System.Collections.Generic;
using System.Linq;
using PrismaDB.Commons;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.Result
{
    public class ResultTable
    {
        private List<ResultRow> _rows;

        public ResultColumnList Columns { get; }
        public List<ResultRow> Rows => _rows;
        public string TableName { get; set; }

        public ResultTable()
        {
            Columns = new ResultColumnList(this);
            _rows = new List<ResultRow>();
        }

        public ResultTable(string tableName) : this()
        {
            TableName = tableName;
        }

        public ResultRow NewRow()
        {
            return new ResultRow(this);
        }

        public void Sort(IEnumerable<Pair<string, OrderDirection>> orderColumns)
        {
            foreach (var orderPair in orderColumns.Reverse())
            {
                switch (orderPair.Second)
                {
                    case OrderDirection.ASC:
                        _rows = _rows.OrderBy(x => x.Get(orderPair.First)).ToList();
                        break;
                    case OrderDirection.DESC:
                        _rows = _rows.OrderByDescending(x => x.Get(orderPair.First)).ToList();
                        break;
                }
            }
        }

        public void Sort(IEnumerable<Pair<ColumnRef, OrderDirection>> orderColumns)
        {
            foreach (var orderPair in orderColumns.Reverse())
            {
                switch (orderPair.Second)
                {
                    case OrderDirection.ASC:
                        _rows = _rows.OrderBy(x => x.Get(orderPair.First)).ToList();
                        break;
                    case OrderDirection.DESC:
                        _rows = _rows.OrderByDescending(x => x.Get(orderPair.First)).ToList();
                        break;
                }
            }
        }
    }
}
