using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PrismaDB.QueryAST;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;
using PrismaDB.QueryAST.Result;
using Xunit;

namespace QueryTests
{
    public class ResultTest
    {
        ColumnDefinition aColDef = new ColumnDefinition(new Identifier("a"), SqlDataType.MSSQL_INT, null, true, false, ColumnEncryptionFlags.Addition);
        ColumnDefinition bColDef = new ColumnDefinition(new Identifier("b"), SqlDataType.MSSQL_VARCHAR, 10, false);
        ColumnDefinition cColDef = new ColumnDefinition(new Identifier("b"), SqlDataType.MSSQL_DATETIME);

        [Fact(DisplayName = "ResultReader to ResultTable")]
        public void TestReaderToTable()
        {
            var reader = new ResultReader();
            reader.Columns.Add(new ColumnRef("a"), aColDef);
            reader.Columns.Add(new ColumnRef("b"), bColDef);
            reader.Columns.Add(new ColumnRef("c"), cColDef);

            new Task(() =>
            {
                for (var i = 0; i < 5; i++)
                {
                    Thread.Sleep(500);
                    var row = reader.NewRow();
                    row.Add(new object[] { (i + 1) * 1, (i + 1) * 2, (i + 1) * 3 });
                    reader.Write(row);
                }
                reader.Close();
            }).Start();

            var table = new ResultTable(reader);

            var expectedRow = table.Rows[2] as ResultRow;
            Assert.Equal(3, expectedRow[new ColumnRef("a")]);
            Assert.Equal(6, expectedRow[bColDef]);
            Assert.Equal(9, expectedRow["c"]);
            Assert.Equal(5, table.Rows.Count);
        }

        [Fact(DisplayName = "ResultTable to ResultReader")]
        public void TestTableToReader()
        {
            var table = new ResultTable();
            table.Columns.Add(new ColumnRef("a"), aColDef);
            table.Columns.Add(new ColumnRef("b"), bColDef);
            table.Columns.Add(new ColumnRef("c"), cColDef);

            for (var i = 0; i < 5; i++)
            {
                var row = table.NewRow();
                row.Add(new object[] { (i + 1) * 1, (i + 1) * 2, (i + 1) * 3 });
                table.Rows.Add(row);
            }

            var results = new List<int[]>();
            using (var reader = new ResultReader(table))
            {
                while (reader.Read())
                {
                    var row = new int[3];
                    row[0] = (int)reader[new ColumnRef("a")];
                    row[1] = (int)reader[bColDef];
                    row[2] = (int)reader["c"];
                    results.Add(row);
                }
            }

            Assert.Equal(3, results[2][0]);
            Assert.Equal(6, results[2][1]);
            Assert.Equal(9, results[2][2]);
            Assert.Equal(5, results.Count);
        }
    }
}
