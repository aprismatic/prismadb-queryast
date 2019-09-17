using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PrismaDB.QueryAST;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;
using PrismaDB.QueryAST.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace QueryTests
{
    public class QueryTest
    {
        [Fact(DisplayName = "ScalarFunction")]
        public void FunctionEquals()
        {
            var func1 = new ScalarFunction("func");
            func1.AddChild(new ConstantContainer(1));
            func1.AddChild(new ConstantContainer("abc"));

            var func2 = new ScalarFunction("func");
            func2.AddChild(new ConstantContainer(1));
            func2.AddChild(new ConstantContainer("abc"));
            Assert.Equal(func1, func2);

            var func3 = new ScalarFunction("func");
            func3.AddChild(new ConstantContainer("abc"));
            func3.AddChild(new ConstantContainer(1));
            Assert.NotEqual(func1, func3);

            var func4 = (ScalarFunction)func2.Clone();
            func4.SetChild(0, new ConstantContainer("123"));

            Assert.NotEqual(func2, func4);

            var func5 = new ScalarFunction("func");
            Assert.NotEqual(func1, func5);

            var func6 = new ScalarFunction("func");
            func6.AddChild(new ConstantContainer(1));
            Assert.NotEqual(func1, func6);

            var func7 = new ScalarFunction("func");
            func7.AddChild(new ColumnRef("tt", "col"));
            Assert.Single(func7.GetColumns());
            Assert.Equal(new ColumnRef("tt", "col"), func7.GetColumns()[0]);
        }

        [Fact(DisplayName = "Functions' Equals")]
        public void FunctionsEquals()
        {
            {
                var func = new SumAggregationFunction("fn");
                func.AddChild(new ColumnRef("a"));

                var func2 = new SumAggregationFunction("fn");
                Assert.NotEqual(func, func2);
                func2.AddChild(new ColumnRef("a"));
                Assert.Equal(func, func2);

                var func3 = func.Clone() as SumAggregationFunction;
                Assert.Equal(func, func3);
            }
            {
                var func = new CountAggregationFunction("fn");
                func.AddChild(new ColumnRef("a"));

                var func2 = new CountAggregationFunction("fn");
                Assert.NotEqual(func, func2);
                func2.AddChild(new ColumnRef("a"));
                Assert.Equal(func, func2);

                var func3 = func.Clone() as CountAggregationFunction;
                Assert.Equal(func, func3);
            }
            {
                var func = new AvgAggregationFunction("fn");
                func.AddChild(new ColumnRef("a"));

                var func2 = new AvgAggregationFunction("fn");
                Assert.NotEqual(func, func2);
                func2.AddChild(new ColumnRef("a"));
                Assert.Equal(func, func2);

                var func3 = func.Clone() as AvgAggregationFunction;
                Assert.Equal(func, func3);
            }
            {
                var func = new PaillierAdditionFunction("fn", new ColumnRef("a"), new ColumnRef("b"),
                    new ConstantContainer(new byte[] { 0x00 }));

                var func2 = new PaillierAdditionFunction("fn", new ColumnRef("a"), new ColumnRef("b"),
                    new ConstantContainer(new byte[] { 0x00 }), "alias");
                Assert.NotEqual(func, func2);
                func.Alias.id = "alias";
                Assert.Equal(func, func2);

                var func3 = func.Clone() as PaillierAdditionFunction;
                Assert.Equal(func, func3);
            }
            {
                var func = new ElGamalMultiplicationFunction("fn", new ColumnRef("a"), new ColumnRef("b"),
                    new ConstantContainer(new byte[] { 0x00 }));

                var func2 = new ElGamalMultiplicationFunction("fn", new ColumnRef("a"), new ColumnRef("b"),
                    new ConstantContainer(new byte[] { 0x00 }), "alias");
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

        [Fact(DisplayName = "Constant Containers")]
        public void Constants()
        {
            var c1 = new ConstantContainer();
            var c2 = new ConstantContainer(123);
            Assert.NotEqual(c1, c2);
            c1.constant = new IntConstant(123);
            Assert.Equal(c1, c2);
            c1.Alias = new Identifier("abc");
            Assert.NotEqual(c1, c2);
            c2.Alias = new Identifier("abc");
            Assert.Equal(c1, c2);

            c2.constant = c1.constant;
            ((IntConstant)c1.constant).intvalue = 10;
            Assert.Equal(10, ((IntConstant)c2.constant).intvalue);

            var q1 = new InsertQuery();
            q1.Values.Add(new List<Expression> { new ConstantContainer(), new ConstantContainer(), new ConstantContainer() });
            q1.Values.Add(new List<Expression> { new ConstantContainer(), new ConstantContainer(), new ConstantContainer() });
            Assert.Equal(6, q1.GetConstants().Count());
            q1.SetConstant(1);
            q1.SetConstant("abc");
            q1.SetConstant(2);
            q1.SetConstant(10);
            q1.SetConstant("def");
            q1.SetConstant(20);

            Assert.Equal(q1.Values[0][0], new ConstantContainer(1));
            Assert.Equal(q1.Values[0][1], new ConstantContainer("abc"));
            Assert.Equal(q1.Values[0][2], new ConstantContainer(2));
            Assert.Equal(q1.Values[1][0], new ConstantContainer(10));
            Assert.Equal(q1.Values[1][1], new ConstantContainer("def"));
            Assert.Equal(q1.Values[1][2], new ConstantContainer(20));

            var q2 = new SelectQuery();
            q2.SelectExpressions.Add(new Addition(new ColumnRef("a"), new ConstantContainer()));
            q2.SelectExpressions.Add(new ConstantContainer());
            q2.SelectExpressions.Add(new ColumnRef("b"));
            q2.SelectExpressions.Add(new Multiplication(new Addition(new ConstantContainer(), new ColumnRef("c")), new ConstantContainer()));
            var and = new Disjunction();
            and.OR.Add(new BooleanEquals(new ColumnRef("a"), new ConstantContainer()));
            and.OR.Add(new BooleanLike(new ColumnRef("b"), new ConstantContainer()));
            q2.Where.CNF.AND.Add(and);
            var and2 = new Disjunction();
            and2.OR.Add(new BooleanGreaterThan(new ConstantContainer(), new ConstantContainer()));
            q2.Where.CNF.AND.Add(and2);

            Assert.Equal(8, q2.GetConstants().Count());
            q2.SetConstant(10);
            q2.SetConstant(20);
            q2.SetConstant(30);
            q2.SetConstant(40);
            q2.SetConstant(50);
            q2.SetConstant("%abc%");
            q2.SetConstant(60);
            q2.SetConstant(70);

            Assert.Equal(((Addition)q2.SelectExpressions[0]).right, new ConstantContainer(10));
            Assert.Equal(q2.SelectExpressions[1], new ConstantContainer(20));
            Assert.Equal(((Addition)((Multiplication)q2.SelectExpressions[3]).left).left, new ConstantContainer(30));
            Assert.Equal(((Multiplication)q2.SelectExpressions[3]).right, new ConstantContainer(40));
            Assert.Equal(((BooleanEquals)q2.Where.CNF.AND[0].OR[0]).right, new ConstantContainer(50));
            Assert.Equal(((BooleanLike)q2.Where.CNF.AND[0].OR[1]).SearchValue, new ConstantContainer("%abc%"));
            Assert.Equal(((BooleanGreaterThan)q2.Where.CNF.AND[1].OR[0]).left, new ConstantContainer(60));
            Assert.Equal(((BooleanGreaterThan)q2.Where.CNF.AND[1].OR[0]).right, new ConstantContainer(70));

            var q3 = new InsertQuery();
            q3.Values.Add(new List<Expression> { new ConstantContainer(index: 1), new ConstantContainer(index: 2), new ConstantContainer(index: 3) });
            q3.Values.Add(new List<Expression> { new ConstantContainer(index: 4), new ConstantContainer(index: 5), new ConstantContainer(index: 6) });
            Assert.Equal(6, q3.GetConstants().Count());
            q3.SetConstant(1, 1);
            q3.SetConstant("def", 5);
            q3.SetConstant(10, 4);
            q3.SetConstant(2, 3);
            q3.SetConstant(20, 6);
            q3.SetConstant("abc", 2);

            Assert.Equal(q3.Values[0][0], new ConstantContainer(1));
            Assert.Equal(q3.Values[0][1], new ConstantContainer("abc"));
            Assert.Equal(q3.Values[0][2], new ConstantContainer(2));
            Assert.Equal(q3.Values[1][0], new ConstantContainer(10));
            Assert.Equal(q3.Values[1][1], new ConstantContainer("def"));
            Assert.Equal(q3.Values[1][2], new ConstantContainer(20));
        }

        [Fact(DisplayName = "NULLs")]
        public void TestNulls()
        {
            var a = new ConstantContainer(new NullConstant());
            var b = new ConstantContainer(new NullConstant());

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
                new Tuple<ColumnRef, OrderDirection>(new ColumnRef(new TableRef("t1"), "col1"), OrderDirection.ASC));

            {
                var clonedSel = new SelectQuery(selQuery);

                var allColumns = new List<ColumnRef>();
                allColumns.AddRange(selQuery.SelectExpressions.SelectMany(x => x.GetColumns()));
                allColumns.AddRange(selQuery.From.GetColumns());
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
                updatedColumns.AddRange(clonedSel.SelectExpressions.SelectMany(x => x.GetColumns()));
                updatedColumns.AddRange(clonedSel.From.GetColumns());
                updatedColumns.AddRange(clonedSel.Where.GetColumns());
                updatedColumns.AddRange(clonedSel.GroupBy.GetColumns());
                updatedColumns.AddRange(clonedSel.OrderBy.GetColumns());

                Assert.Contains(new ColumnRef(new TableRef("t1"), "col1"), updatedColumns);
                Assert.Contains(new ColumnRef(new TableRef("t2"), "col2"), updatedColumns);

                Assert.DoesNotContain(new ColumnRef(new TableRef("a"), "col1"), updatedColumns);
                Assert.DoesNotContain(new ColumnRef(new TableRef("b"), "col2"), updatedColumns);
            }
            {
                var allColumns = new List<ColumnRef>();
                allColumns.AddRange(selQuery.SelectExpressions.SelectMany(x => x.GetColumns()));
                allColumns.AddRange(selQuery.From.GetColumns());
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
                updatedColumns.AddRange(selQuery.From.GetColumns());
                updatedColumns.AddRange(selQuery.Where.GetColumns());
                updatedColumns.AddRange(selQuery.GroupBy.GetColumns());
                updatedColumns.AddRange(selQuery.OrderBy.GetColumns());

                Assert.DoesNotContain(new ColumnRef(new TableRef("t1"), "col1"), updatedColumns);
                Assert.DoesNotContain(new ColumnRef(new TableRef("t2"), "col2"), updatedColumns);

                Assert.Contains(new ColumnRef(new TableRef("a"), "col1"), updatedColumns);
                Assert.Contains(new ColumnRef(new TableRef("b"), "col2"), updatedColumns);
            }
        }

        [Fact(DisplayName = "Operations")]
        public void TestOperations()
        {
            var table = new ResultTable();
            table.Columns.Add(new ColumnRef("a"));
            table.Columns.Add(new ColumnRef("b"));
            table.Columns.Add(new ColumnRef("c"));
            table.Columns.Add(new ColumnRef("d"));
            var row = table.NewRow();
            row.Add(new object[] { 1, 432, 24.42d, 0.256m });

            var computeAddInt = new Addition(new ColumnRef("a"), new ColumnRef("b"));
            Assert.Equal(433, computeAddInt.Eval(row));

            var computeAddDouble = new Addition(new ColumnRef("c"), new ColumnRef("c"));
            Assert.Equal(48.84, computeAddDouble.Eval(row));

            var computeAddDecimal = new Addition(new ColumnRef("d"), new ColumnRef("d"));
            Assert.Equal(0.512m, computeAddDecimal.Eval(row));

            var computeAddMixed = new Addition(new ColumnRef("c"), new ColumnRef("d"));
            Assert.Equal(24.676m, computeAddMixed.Eval(row));

            var computeSubInt = new Subtraction(new ColumnRef("a"), new ColumnRef("b"));
            Assert.Equal(-431, computeSubInt.Eval(row));

            var computeSubDouble = new Subtraction(new ColumnRef("c"), new ColumnRef("c"));
            Assert.Equal(0d, computeSubDouble.Eval(row));

            var computeSubDecimal = new Subtraction(new ColumnRef("d"), new ColumnRef("d"));
            Assert.Equal(0m, computeSubDecimal.Eval(row));

            var computeSubMixed = new Subtraction(new ColumnRef("c"), new ColumnRef("d"));
            Assert.Equal(24.164m, computeSubMixed.Eval(row));

            var computeMulInt = new Multiplication(new ColumnRef("a"), new ColumnRef("b"));
            Assert.Equal(432, computeMulInt.Eval(row));

            var computeMulDouble = new Multiplication(new ColumnRef("c"), new ColumnRef("c"));
            Assert.Equal(24.42d * 24.42d, computeMulDouble.Eval(row));

            var computeMulDecimal = new Multiplication(new ColumnRef("d"), new ColumnRef("d"));
            Assert.Equal(0.065536m, computeMulDecimal.Eval(row));

            var computeMulMixed = new Multiplication(new ColumnRef("c"), new ColumnRef("d"));
            Assert.Equal(6.25152m, computeMulMixed.Eval(row));

            var computeDivInt = new Division(new ColumnRef("a"), new ColumnRef("b"));
            Assert.Equal(0, computeDivInt.Eval(row));

            var computeDivDouble = new Division(new ColumnRef("c"), new ColumnRef("c"));
            Assert.Equal(24.42d / 24.42d, computeDivDouble.Eval(row));

            var computeDivDecimal = new Division(new ColumnRef("d"), new ColumnRef("d"));
            Assert.Equal(1m, computeDivDecimal.Eval(row));

            var computeDivMixed = new Division(new ColumnRef("c"), new ColumnRef("d"));
            Assert.Equal(95.390625m, computeDivMixed.Eval(row));
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

            Assert.Equal(3, table.Columns.Count);

            var jsonRes = JsonConvert.SerializeObject(table);

            Assert.NotNull(jsonRes);
        }

        [Fact(DisplayName = "BooleanExpression Like")]
        public void TestBooleanExpressionLike()
        {
            //Initialize
            var table = new ResultTable();
            var col = new ResultColumnHeader("TextColumn", typeof(string));
            col.Expression = new ColumnRef("TextColumn");
            table.Columns.Add(col);
            var like = new BooleanLike();

            var row1 = table.NewRow();
            row1.Add(new object[] { "%%ABCDEFG" });

            //Escape character test
            like = new BooleanLike(new ColumnRef("TextColumn"), new ConstantContainer("!%!%%"), '!');
            Assert.Equal(true, like.Eval(row1));

            //Leading percent test
            like = new BooleanLike(new ColumnRef("TextColumn"), new ConstantContainer("%EFG"));
            Assert.Equal(true, like.Eval(row1));

            //Trailing percent test
            like = new BooleanLike(new ColumnRef("TextColumn"), new ConstantContainer("!%!%ABC%"), '!');
            Assert.Equal(true, like.Eval(row1));

            //Middle percent test
            like = new BooleanLike(new ColumnRef("TextColumn"), new ConstantContainer("!%!%ABC%EFG"), '!');
            Assert.Equal(true, like.Eval(row1));

            //Leading underscore test
            like = new BooleanLike(new ColumnRef("TextColumn"), new ConstantContainer("_!%ABCDEFG"), '!');
            Assert.Equal(true, like.Eval(row1));

            //Trailing underscore test
            like = new BooleanLike(new ColumnRef("TextColumn"), new ConstantContainer("!%!%ABCDEF_"), '!');
            Assert.Equal(true, like.Eval(row1));

            //Middle underscore test
            like = new BooleanLike(new ColumnRef("TextColumn"), new ConstantContainer("!%!%ABC_EFG"), '!');
            Assert.Equal(true, like.Eval(row1));

            //Mixed wild card test
            like = new BooleanLike(new ColumnRef("TextColumn"), new ConstantContainer("__ABCD%_"));
            Assert.Equal(true, like.Eval(row1));

            //Case insensitive test
            like = new BooleanLike(new ColumnRef("TextColumn"), new ConstantContainer("___bcd%_"));
            Assert.Equal(true, like.Eval(row1));

            //Invalid escape character test
            like = new BooleanLike(new ColumnRef("TextColumn"), new ConstantContainer("!%!%!ABCDEF!G!"));
            Assert.Equal(false, like.Eval(row1));

            //Null escape character test
            like = new BooleanLike(new ColumnRef("TextColumn"), new ConstantContainer("\\%\\%ABCDEFG"), null);
            Assert.Equal(true, like.Eval(row1));
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
