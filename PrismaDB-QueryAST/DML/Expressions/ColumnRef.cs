using System.Collections.Generic;
using System.Data;
using System.Text;

namespace PrismaDB.QueryAST.DML
{
    public class ColumnRef : Expression
    {
        public TableRef Table;

        public ColumnRef(string ColumnName)
        {
            setValue("", ColumnName);
        }

        public ColumnRef(string TableName, string ColumnName)
        {
            setValue(TableName, ColumnName);
        }

        public ColumnRef(TableRef Table, string ColumnName)
        {
            setValue(Table.TableName, ColumnName);
        }

        public override void setValue(params object[] value)
        {
            Parent = null;
            Table = new TableRef((string)value[0]);
            ColumnName = (string)value[1];
        }

        public override Expression Clone()
        {
            var clone = new ColumnRef(Table, ColumnName);

            return clone;
        }

        public override object Eval(DataRow r)
        {
            return r[ToString()];
        }

        public override List<ColumnRef> GetColumns()
        {
            return new List<ColumnRef>()
            {
                (ColumnRef)Clone()
            };
        }

        public override string ToString()
        {
            var tblnm = Table.ToString();

            var sb = new StringBuilder(tblnm);

            if (tblnm.Length > 0)
                sb.Append(".");

            sb.Append("[");
            sb.Append(ColumnName);
            sb.Append("]");

            return sb.ToString();
        }
    }
}
