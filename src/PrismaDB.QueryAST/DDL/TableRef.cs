using System;

namespace PrismaDB.QueryAST
{
    public class TableRef
    {
        public Identifier Table;
        public bool IsTempTable;
        public Identifier Alias;

        public TableRef(string TableName, bool IsTemp = false, string AliasName = "")
        {
            Table = new Identifier(TableName);
            IsTempTable = IsTemp;
            Alias = new Identifier(AliasName);
        }

        public TableRef Clone()
        {
            return new TableRef(Table.id, IsTempTable, Alias.id);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.TableRefToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is TableRef otherTR)) return false;

            return String.Equals(Table.id, otherTR.Table.id, StringComparison.InvariantCultureIgnoreCase)
                && String.Equals(Alias.id, otherTR.Alias.id, StringComparison.InvariantCultureIgnoreCase)
                && IsTempTable == otherTR.IsTempTable;
        }

        public override int GetHashCode()
        {
            return unchecked(
                Table.GetHashCode() *
                Alias.GetHashCode() *
                (IsTempTable.GetHashCode() + 1));
        }
    }
}
