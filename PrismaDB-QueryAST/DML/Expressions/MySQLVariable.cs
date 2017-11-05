using System.Collections.Generic;
using System.Data;

namespace PrismaDB.QueryAST.DML
{
    public class MySQLVariable : Expression
    {
        public MySQLVariable(string columnName)
        {
            setValue(columnName);
        }

        public MySQLVariable(Identifier column)
            : this(column.id)
        { }

        public override void setValue(params object[] value)
        {
            Parent = null;
            ColumnName = new Identifier((string)value[0]);
        }

        public override Expression Clone()
        {
            var clone = new MySQLVariable(ColumnName);

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
            return DialectResolver.Dialect.MySQLVariableToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is MySQLVariable otherVar)) return false;

            return ColumnName.Equals(otherVar.ColumnName);
        }

        public override int GetHashCode()
        {
            return unchecked(ColumnName.GetHashCode());
        }
    }
}
