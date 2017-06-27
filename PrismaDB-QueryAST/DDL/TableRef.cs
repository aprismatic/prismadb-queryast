using System;

namespace PrismaDB.QueryAST
{
    public class TableRef
    {
        public Identifier Table;
        public bool IsTempTable;

        public TableRef(string TableName, bool IsTemp = false)
        {
            Table = new Identifier(TableName);
            IsTempTable = IsTemp;
        }

        public TableRef Clone()
        {
            return new TableRef(Table.id, IsTempTable);
        }

        public override string ToString()
        {
            return Table.id.Length > 0
                ? "[" + (IsTempTable ? "#" : "") + Table.id + "]"
                : "";
        }

        public override bool Equals(object other)
        {
            var otherTR = other as TableRef;
            if (otherTR == null) return false;

            return String.Equals(Table.id, otherTR.Table.id, StringComparison.InvariantCultureIgnoreCase)
                && IsTempTable == otherTR.IsTempTable;
        }

        public override int GetHashCode()
        {
            return unchecked(
                Table.GetHashCode() *
                (IsTempTable.GetHashCode() + 1));
        }
    }
}
