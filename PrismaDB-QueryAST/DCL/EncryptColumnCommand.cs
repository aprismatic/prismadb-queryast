using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.DCL
{
    public class EncryptColumnCommand : Command
    {
        public ColumnRef Column;
        public ColumnEncryptionFlags EncryptionFlags;

        public EncryptColumnCommand()
        {
            Column = new ColumnRef("");
        }

        public EncryptColumnCommand(string columnName, ColumnEncryptionFlags encryptionFlags)
        {
            Column = new ColumnRef(columnName);
            EncryptionFlags = encryptionFlags;
        }

        public EncryptColumnCommand(ColumnRef column, ColumnEncryptionFlags encryptionFlags)
        {
            Column = column;
            EncryptionFlags = encryptionFlags;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.EncryptColumnCommandToString(this);
        }

        public override object Clone()
        {
            var clone = new EncryptColumnCommand((ColumnRef)Column.Clone(), EncryptionFlags);

            return clone;
        }
    }
}