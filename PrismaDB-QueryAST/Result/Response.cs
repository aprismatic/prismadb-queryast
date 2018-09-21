using System;
using System.Collections.Generic;
using System.Text;

namespace PrismaDB.QueryAST.Result
{
    public abstract class Response { }

    public class QueryResponse : Response
    {
        public ResultTable TableData { get; set; }
        public int Warnings { get; set; }

        public QueryResponse()
        {
            TableData = new ResultTable();
            Warnings = 0;
        }
    }

    public class NonQueryResponse : Response
    {
        public int RowsAffected { get; set; }
        public int LastInsertId { get; set; }
        public int Warnings { get; set; }
        public string InfoMessage { get; set; }

        public NonQueryResponse()
        {
            RowsAffected = 0;
            LastInsertId = 0;
            Warnings = 0;
            InfoMessage = "";
        }
    }

    public class ErrorResponse : Response
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public ErrorResponse()
        {
            ErrorCode = 0;
            ErrorMessage = "";
        }
    }
}
