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

        public MySqlVariable(string variableName, string columnName)
        {
            setValue(variableName, columnName);
        }

        public MySqlVariable(Identifier variable, string columnName)
            : this(variable.id, columnName)
        { }

        public MySqlVariable(string variableName, Identifier column)
            : this(variableName, column.id)
        { }

        public MySqlVariable(Identifier variable, Identifier column)
            : this(variable.id, column.id)
        { }

        public override void setValue(params object[] value)
        {
            Parent = null;
            VariableName = new Identifier((string)value[0]);
            ColumnName = new Identifier((string)value[1]);
        }

        public override object Clone()
        {
            var clone = new MySqlVariable(VariableName, ColumnName);

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
                && ColumnName.Equals(otherVar.ColumnName);
        }

        public override int GetHashCode()
        {
            return unchecked(
                VariableName.GetHashCode() *
                ColumnName.GetHashCode());
        }
    }
}
