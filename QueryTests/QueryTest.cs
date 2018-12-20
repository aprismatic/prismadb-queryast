using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using PrismaDB.Commons;
using PrismaDB.QueryAST;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;
using PrismaDB.QueryAST.Result;
using Xunit;

namespace QueryTests
{
    public class QueryTest
    {
        [Fact(DisplayName = "ScalarFunction")]
        public void FunctionEquals()
        {
            var func1 = new ScalarFunction("func");
            func1.Parameters.Add(new IntConstant(1));
            func1.Parameters.Add(new StringConstant("abc"));

            var func2 = new ScalarFunction("func");
            func2.Parameters.Add(new IntConstant(1));
            func2.Parameters.Add(new StringConstant("abc"));
            Assert.Equal(func1, func2);

            var func3 = new ScalarFunction("func");
            func3.Parameters.Add(new StringConstant("abc"));
            func3.Parameters.Add(new IntConstant(1));
            Assert.NotEqual(func1, func3);

            var func4 = (ScalarFunction)func2.Clone();
            func4.Parameters[0] = new StringConstant("123");

            Assert.NotEqual(func2, func4);

            var func5 = new ScalarFunction("func");
            Assert.NotEqual(func1, func5);

            var func6 = new ScalarFunction("func");
            func6.Parameters.Add(new IntConstant(1));
            Assert.NotEqual(func1, func6);

            var func7 = new ScalarFunction("func");
            func7.Parameters.Add(new ColumnRef("tt", "col"));
            Assert.Single(func7.GetColumns());
            Assert.Equal(new ColumnRef("tt", "col"), func7.GetColumns()[0]);
        }

        [Fact(DisplayName = "Functions' Equals")]
        public void FunctionsEquals()
        {
            {
                var func = new SumAggregationFunction("fn");
                func.Parameters.Add(new ColumnRef("a"));

                var func2 = new SumAggregationFunction("fn");
                Assert.NotEqual(func, func2);
                func2.Parameters.Add(new ColumnRef("a"));
                Assert.Equal(func, func2);

                var func3 = func.Clone() as SumAggregationFunction;
                Assert.Equal(func, func3);
            }
            {
                var func = new CountAggregationFunction("fn");
                func.Parameters.Add(new ColumnRef("a"));

                var func2 = new CountAggregationFunction("fn");
                Assert.NotEqual(func, func2);
                func2.Parameters.Add(new ColumnRef("a"));
                Assert.Equal(func, func2);

                var func3 = func.Clone() as CountAggregationFunction;
                Assert.Equal(func, func3);
            }
            {
                var func = new AvgAggregationFunction("fn");
                func.Parameters.Add(new ColumnRef("a"));

                var func2 = new AvgAggregationFunction("fn");
                Assert.NotEqual(func, func2);
                func2.Parameters.Add(new ColumnRef("a"));
                Assert.Equal(func, func2);

                var func3 = func.Clone() as AvgAggregationFunction;
                Assert.Equal(func, func3);
            }
            {
                var func = new PaillierAdditionFunction("fn", new ColumnRef("a"), new ColumnRef("b"),
                    new BinaryConstant(new byte[] { 0x00 }));

                var func2 = new PaillierAdditionFunction("fn", new ColumnRef("a"), new ColumnRef("b"),
                    new BinaryConstant(new byte[] { 0x00 }), "alias");
                Assert.NotEqual(func, func2);
                func.Alias.id = "alias";
                Assert.Equal(func, func2);

                var func3 = func.Clone() as PaillierAdditionFunction;
                Assert.Equal(func, func3);
            }
            {
                var func = new ElGamalMultiplicationFunction("fn", new ColumnRef("a"), new ColumnRef("b"),
                    new BinaryConstant(new byte[] { 0x00 }));

                var func2 = new ElGamalMultiplicationFunction("fn", new ColumnRef("a"), new ColumnRef("b"),
                    new BinaryConstant(new byte[] { 0x00 }), "alias");
                Assert.NotEqual(func, func2);
                func.Alias.id = "alias";
                Assert.Equal(func, func2);

                var func3 = func.Clone() as ElGamalMultiplicationFunction;
                Assert.Equal(func, func3);
            }
        }

        [Fact(DisplayName = "ColumnDefinition")]
        public void ColumnDefinitionEquals()
        {
            var cd1 = new ColumnDefinition();
            cd1.ColumnName = new PrismaDB.QueryAST.Identifier("colname");
            cd1.EnumValues.Add(new StringConstant("abc"));
            cd1.EnumValues.Add(new StringConstant("def"));

            var cd2 = new ColumnDefinition();
            cd2.ColumnName = new PrismaDB.QueryAST.Identifier("colname");
            cd2.EnumValues.Add(new StringConstant("abc"));
            cd2.EnumValues.Add(new StringConstant("def"));
            Assert.True(cd1.Equals(cd2));

            var cd3 = new ColumnDefinition();
            cd3.ColumnName = new PrismaDB.QueryAST.Identifier("colname");
            cd3.EnumValues.Add(new StringConstant("def"));
            cd3.EnumValues.Add(new StringConstant("abc"));
            Assert.False(cd1.Equals(cd3));

            var cd4 = (ColumnDefinition)cd2.Clone();
            cd4.EnumValues[0] = new StringConstant("123");
            Assert.False(cd2.Equals(cd4));

            var cd5 = new ColumnDefinition();
            cd5.ColumnName = new PrismaDB.QueryAST.Identifier("colname");
            Assert.False(cd1.Equals(cd5));

            var cd6 = new ColumnDefinition();
            cd6.ColumnName = new PrismaDB.QueryAST.Identifier("colname");
            cd1.EnumValues.Add(new StringConstant("abc"));
            Assert.False(cd1.Equals(cd6));
        }

        [Fact(DisplayName = "Constant")]
        public void ConstantTest()
        {
            var str1 = new StringConstant("abc");
            var int1 = new IntConstant(1);
            var str2 = new StringConstant("abc");
            var int2 = new IntConstant(1);

            Assert.True(str1.Equals(str2));
            Assert.True(int1.Equals(int2));
        }

        [Fact(DisplayName = "Operation")]
        public void OperationTest()
        {
            var a = new Addition(new IntConstant(5), new IntConstant(10));
            var b = new Multiplication(new IntConstant(20), a);
            var c = new Subtraction(b, new IntConstant(50));
            var d = new Division(c, b);

            Assert.Equal(a, a.Clone());
            Assert.Equal(b, b.Clone());
            Assert.Equal(c, c.Clone());
            Assert.Equal(d, d.Clone());
        }

        [Fact(DisplayName = "NULLs")]
        public void TestNulls()
        {
            var a = new NullConstant();
            var b = new NullConstant();

            Assert.True(a.Equals(b));
            Assert.True(b.Equals(a));
            Assert.True(a.GetHashCode() == b.GetHashCode());
            a.Alias = new Identifier("fasdf");
            Assert.False(a.Equals(b));
            Assert.False(b.Equals(a));
            Assert.False(a.GetHashCode() == b.GetHashCode());
            b.Alias = new Identifier("fasdf");
            Assert.True(a.Equals(b));
            Assert.True(b.Equals(a));
            Assert.True(a.GetHashCode() == b.GetHashCode());

            var c = new BooleanIsNull(new ColumnRef("col1"), false);
            var d = (BooleanIsNull)c.Clone();
            Assert.True(c.left.Equals(d.left));
        }

        [Fact(DisplayName = "GetColumns")]
        public void TestGetColumns()
        {
            var selQuery = new SelectQuery();
            selQuery.SelectExpressions.Add(new ColumnRef(new TableRef("t1"), "col1"));
            selQuery.SelectExpressions.Add(new ColumnRef(new TableRef("t2"), "col2"));
            selQuery.SelectExpressions.Add(
                new Addition(new ColumnRef(new TableRef("t1"), "col1"),
                    new ColumnRef(new TableRef("t1"), "col1")));

            var dj = new Disjunction();
            dj.OR.Add(new BooleanEquals(new ColumnRef(new TableRef("t1"), "col1"),
                new ColumnRef(new TableRef("t2"), "col2")));
            selQuery.Where.CNF.AND.Add(dj);

            selQuery.OrderBy.OrderColumns.Add(
                new Pair<ColumnRef, OrderDirection>(new ColumnRef(new TableRef("t1"), "col1"), OrderDirection.ASC));

            {
                var allColumns = new List<ColumnRef>();
                allColumns.AddRange(selQuery.SelectExpressions.SelectMany(x => x.GetColumns()));
                allColumns.AddRange(selQuery.Joins.SelectMany(x => x.GetColumns()));
                allColumns.AddRange(selQuery.Where.GetColumns());
                allColumns.AddRange(selQuery.GroupBy.GetColumns());
                allColumns.AddRange(selQuery.OrderBy.GetColumns());

                foreach (var col in allColumns)
                {
                    if (col.Table.Table.id == "t1")
                        col.Table.Table.id = "a";
                    if (col.Table.Table.id == "t2")
                        col.Table.Table.id = "b";
                }

                var updatedColumns = new List<ColumnRef>();
                updatedColumns.AddRange(selQuery.SelectExpressions.SelectMany(x => x.GetColumns()));
                updatedColumns.AddRange(selQuery.Joins.SelectMany(x => x.GetColumns()));
                updatedColumns.AddRange(selQuery.Where.GetColumns());
                updatedColumns.AddRange(selQuery.GroupBy.GetColumns());
                updatedColumns.AddRange(selQuery.OrderBy.GetColumns());

                Assert.Contains(new ColumnRef(new TableRef("t1"), "col1"), updatedColumns);
                Assert.Contains(new ColumnRef(new TableRef("t2"), "col2"), updatedColumns);

                Assert.DoesNotContain(new ColumnRef(new TableRef("a"), "col1"), updatedColumns);
                Assert.DoesNotContain(new ColumnRef(new TableRef("b"), "col2"), updatedColumns);
            }
            {
                var allColumns = new List<ColumnRef>();
                allColumns.AddRange(selQuery.SelectExpressions.SelectMany(x => x.GetNoCopyColumns()));
                allColumns.AddRange(selQuery.Joins.SelectMany(x => x.GetNoCopyColumns()));
                allColumns.AddRange(selQuery.Where.GetNoCopyColumns());
                allColumns.AddRange(selQuery.GroupBy.GetNoCopyColumns());
                allColumns.AddRange(selQuery.OrderBy.GetNoCopyColumns());

                foreach (var col in allColumns)
                {
                    if (col.Table.Table.id == "t1")
                        col.Table.Table.id = "a";
                    if (col.Table.Table.id == "t2")
                        col.Table.Table.id = "b";
                }

                var updatedColumns = new List<ColumnRef>();
                updatedColumns.AddRange(selQuery.SelectExpressions.SelectMany(x => x.GetColumns()));
                updatedColumns.AddRange(selQuery.Joins.SelectMany(x => x.GetColumns()));
                updatedColumns.AddRange(selQuery.Where.GetColumns());
                updatedColumns.AddRange(selQuery.GroupBy.GetColumns());
                updatedColumns.AddRange(selQuery.OrderBy.GetColumns());

                Assert.DoesNotContain(new ColumnRef(new TableRef("t1"), "col1"), updatedColumns);
                Assert.DoesNotContain(new ColumnRef(new TableRef("t2"), "col2"), updatedColumns);

                Assert.Contains(new ColumnRef(new TableRef("a"), "col1"), updatedColumns);
                Assert.Contains(new ColumnRef(new TableRef("b"), "col2"), updatedColumns);
            }
        }

        [Fact(DisplayName = "ResultTable Serialization")]
        public void TestResultTableSerialization()
        {
            var table = new ResultTable();
            table.Columns.Add(new ResultColumnHeader("a", typeof(int)));
            table.Columns.Add(new ResultColumnHeader("b", typeof(string)));
            table.Columns.Add(new ResultColumnHeader("c", typeof(string)));
            var row1 = table.NewRow();
            row1.Add(new object[] { 1, "abc", "DEF" });
            table.Rows.Add(row1);
            var row2 = table.NewRow();
            row2.Add(new object[] { 2, null, "xyz" });
            table.Rows.Add(row2);

            string xmlRes;
            using (var stream = new StringWriter())
            {
                var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                var serializer = new XmlSerializer(table.GetType());
                var settings = new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = true
                };
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.Serialize(writer, table, namespaces);
                    xmlRes = stream.ToString();
                }
            }

            var jsonRes = JsonConvert.SerializeObject(table);

            Assert.NotNull(xmlRes);
            Assert.NotNull(jsonRes);
        }

        [Fact(DisplayName = "ColumnDefinition Serialization")]
        public void TestColumnDefinitionSerialization()
        {
            var colDefDict = new Dictionary<Identifier, ColumnDefinition>
            {
                {
                    new Identifier("col1"),
                    new ColumnDefinition("col1", SqlDataType.MSSQL_INT, null, true, false, ColumnEncryptionFlags.Store, null,
                        new NullConstant())
                },
                {
                    new Identifier("col2"),
                    new ColumnDefinition("col2", SqlDataType.MSSQL_DATETIME, null, false, false, ColumnEncryptionFlags.Store, null,
                        new ScalarFunction("CURRENT_TIMESTAMP"))
                }
            };

            var tableDict =
                new Dictionary<TableRef, Dictionary<Identifier, ColumnDefinition>> { { new TableRef("tbl1"), colDefDict } };

            var serializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects,
                TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple,
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ContractResolver = new MyContractResolver()
            };

            var json = JsonConvert.SerializeObject(tableDict, serializerSettings);

            var deserializerSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            };

            var newTableDict = JsonConvert.DeserializeObject<Dictionary<TableRef, Dictionary<Identifier, ColumnDefinition>>>(json, deserializerSettings);

            Assert.Equal(tableDict[new TableRef("tbl1")][new Identifier("col1")], newTableDict[new TableRef("tbl1")][new Identifier("col1")]);
            Assert.Equal(tableDict[new TableRef("tbl1")][new Identifier("col2")], newTableDict[new TableRef("tbl1")][new Identifier("col2")]);
        }

        internal class MyContractResolver : DefaultContractResolver
        {
            protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
            {
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                    .Select(p => base.CreateProperty(p, memberSerialization))
                    .Union(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Select(f => base.CreateProperty(f, memberSerialization)))
                    .ToList();
                props.ForEach(p => { p.Writable = true; p.Readable = true; });
                return props;
            }
        }
    }
}
