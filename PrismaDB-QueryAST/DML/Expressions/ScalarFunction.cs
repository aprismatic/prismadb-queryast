﻿using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PrismaDB.QueryAST.DML
{
    public class ScalarFunction : Expression
    {
        public string FunctionName;
        public List<Expression> Parameters;

        public ScalarFunction(string functionName)
        {
            setValue(functionName, new List<Expression>());
        }

        public ScalarFunction(string functionName, List<Expression> parameters)
        {
            setValue(functionName, parameters);
        }

        public override Expression Clone()
        {
            var clone = new ScalarFunction(FunctionName, new List<Expression>(Parameters));

            return clone;
        }

        public override bool Equals(object other)
        {
            var otherF = other as ScalarFunction;
            if (otherF == null) return false;

            return FunctionName.Equals(otherF.FunctionName)
                && Enumerable.SequenceEqual(Parameters.OrderBy(t => t),
                otherF.Parameters.OrderBy(t => t));
        }

        public override object Eval(DataRow r)
        {
            return r[ToString()];
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override int GetHashCode()
        {
            return unchecked(FunctionName.GetHashCode() *
                Parameters.Aggregate((a, b) =>
                new IntConstant(a.GetHashCode() * b.GetHashCode())).GetHashCode());
        }

        public override void setValue(params object[] value)
        {
            Parent = null;
            FunctionName = (string)value[0];

            if (value.Length > 1)
                Parameters = (List<Expression>)value[1];
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ScalarFunctionToString(this);
        }
    }
}
