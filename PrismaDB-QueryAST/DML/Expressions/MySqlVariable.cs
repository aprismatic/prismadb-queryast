using PrismaDB.QueryAST.Result;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DML
{
    public class MySqlVariable : Expression
    {
        public Identifier VariableName;

        public MySqlVariable(string variableName)
          : this(variableName, "")
        { }

        public MySqlVariable(Identifier variable)
            : this(variable.id)
        { }

        public MySqlVariable(string variableName, string aliasName)
        {
            VariableName = new Identifier(variableName);
            Alias = new Identifier(aliasName);
        }

        public MySqlVariable(Identifier variable, Identifier alias)
            : this(variable.id, alias.id)
        { }

        public override object Clone() => new MySqlVariable(VariableName, Alias);

        public override object Eval(ResultRow r) => r[this];

        public override List<ColumnRef> GetColumns() => new List<ColumnRef>();

        public override List<ColumnRef> GetNoCopyColumns() => new List<ColumnRef>();

        public override bool UpdateChild(Expression child, Expression newChild) => false;

        public override string ToString() => DialectResolver.Dialect.MySqlVariableToString(this);

        public override bool Equals(object other)
        {
            if (!(other is MySqlVariable otherVar)) return false;

            return VariableName.Equals(otherVar.VariableName)
                && Alias.Equals(otherVar.Alias);
        }

        public override int GetHashCode() =>
            unchecked(VariableName.GetHashCode() *
                      Alias.GetHashCode());
    }
}
