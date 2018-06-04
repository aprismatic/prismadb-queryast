using System;
using System.Collections.Generic;
using System.Data;
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

        public void RemoveMetadata()
        {
            foreach (var col in Columns.Headers)
                col.RemoveMetadata();
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
                        _rows = _rows.OrderBy(x => x[orderPair.First]).ToList();
                        break;
                    case OrderDirection.DESC:
                        _rows = _rows.OrderByDescending(x => x[orderPair.First]).ToList();
                        break;
                }
            }
        }

        public void Load(IDataReader reader)
        {
            if (Rows.Count > 0 || Columns.Count > 0)
                throw new ApplicationException("ResultTable is not empty.");

            var schemaTable = reader.GetSchemaTable();

            foreach (DataRow dataRow in schemaTable.Rows)
            {
                var resColumn = new ResultColumnHeader
                {
                    ColumnName = dataRow["ColumnName"].ToString(),
                    DataType = Type.GetType(dataRow["DataType"].ToString())
                };

                Columns.Add(resColumn);
            }

            while (reader.Read())
            {
                var resRow = NewRow();

                for (var i = 0; i < Columns.Count; i++)
                    resRow[i] = reader[i];

                Rows.Add(resRow);
            }
        }
    }
}
