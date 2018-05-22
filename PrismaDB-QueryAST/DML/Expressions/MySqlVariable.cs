using System.Collections.Generic;
using System.Data;

namespace PrismaDB.QueryAST.DML
{
    public class MySqlVariable : Expression
    {
        public Identifier VariableName;

        public MySqlVariable(string variableName)
        {
            setValue(variableName, "");
        }

        public MySqlVariable(Identifier variable)
            : this(variable.id)
        { }

        public MySqlVariable(string variableName, string aliasName)
        {
            setValue(variableName, aliasName);
        }

        public MySqlVariable(Identifier variable, string aliasName)
            : this(variable.id, aliasName)
        { }

        public MySqlVariable(string variableName, Identifier alias)
            : this(variableName, alias.id)
        { }

        public MySqlVariable(Identifier variable, Identifier alias)
            : this(variable.id, alias.id)
        { }

        public override void setValue(params object[] value)
        {
            Parent = null;
            VariableName = new Identifier((string)value[0]);
            Alias = new Identifier((string)value[1]);
        }

        public override object Clone()
        {
            var clone = new MySqlVariable(VariableName, Alias);

            return clone;
        }

        public override object Eval(DataRow r)
        {
            return r[ToString()];
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>();
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.MySqlVariableToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is MySqlVariable otherVar)) return false;

            return VariableName.Equals(otherVar.VariableName)
                && Alias.Equals(otherVar.Alias);
        }

        public override int GetHashCode()
        {
            return unchecked(
                VariableName.GetHashCode() *
                Alias.GetHashCode());
        }
    }
}
