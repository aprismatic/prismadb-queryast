using Newtonsoft.Json;
using PrismaDB.Commons;
using PrismaDB.QueryAST.DML;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace PrismaDB.QueryAST.Result
{
    public class ResultTable : PrismaDB.Result.ResultTable
    {
        public new ResultColumnList Columns => (ResultColumnList)base.Columns;

        [JsonIgnore]
        [XmlIgnore]
        public ReadOnlyCollection<ResultRow> ReadOnlyRows => base.Rows.Cast<ResultRow>().ToList().AsReadOnly();

        public ResultTable()
        {
            base.Columns = new ResultColumnList(this);
            _rows = new List<PrismaDB.Result.ResultRow>();
        }

        public ResultTable(string tableName) : this()
        {
            TableName = tableName;
        }

        public new ResultRow NewRow()
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
