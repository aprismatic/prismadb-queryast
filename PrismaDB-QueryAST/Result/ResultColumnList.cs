using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.Result
{
    public class ResultColumnList : PrismaDB.Result.ResultColumnList
    {
        protected ResultColumnList() { }

        internal ResultColumnList(PrismaDB.Result.ResultQueryResponse table) : base(table) { }

        public new ResultColumnHeader this[int index] => (ResultColumnHeader)Headers[index];

        public new ResultColumnHeader this[string columnName] =>
            this[Headers.IndexOf(Headers.Single(x => x.ColumnName.Equals(columnName)))];

        public ResultColumnHeader this[Expression exp] =>
            this[Headers.IndexOf(Headers.Single(x => ((ResultColumnHeader)x).Expression.Equals(exp)))];

        public new IEnumerator<ResultColumnHeader> GetEnumerator()
        {
            return Headers.Cast<ResultColumnHeader>().ToList().AsReadOnly().GetEnumerator();
        }

        public new void Add()
        {
            Add(new ResultColumnHeader());
        }

        public new void Add(string columnName, Type dataType = null, int? maxLength = null)
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

        public void Remove(Expression exp)
        {
            Remove(Headers.IndexOf(Headers.Single(x => ((ResultColumnHeader)x).Expression.Equals(exp))));
        }
    }
}