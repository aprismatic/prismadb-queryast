using System.Collections.Generic;
using System.Linq;
using PrismaDB.Commons;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.Result
{
    public class DataTable
    {
        private List<DataColumn> _columns;
        private List<DataRow> _rows;

        public List<DataColumn> Columns => _columns;
        public List<DataRow> Rows => _rows;

        public string TableName { get; set; }


        public DataTable()
        {
            _columns = new List<DataColumn>();
            _rows = new List<DataRow>();
        }

        public DataTable(string tableName) : this()
        {
            TableName = tableName;
        }

        public DataRow NewRow()
        {
            return new DataRow(this);
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
