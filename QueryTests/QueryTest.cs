using PrismaDB.QueryAST;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;
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
        }
    }
}
