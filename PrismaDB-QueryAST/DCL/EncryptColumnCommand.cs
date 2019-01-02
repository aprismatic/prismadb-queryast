using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.DCL
{
    public class EncryptColumnCommand : Command
    {
        public ColumnRef Column;
        public ColumnEncryptionFlags EncryptionFlags;
        public bool StatusCheck;

        public EncryptColumnCommand(bool statusCheck = false)
            : this(new ColumnRef(""), ColumnEncryptionFlags.None, statusCheck) { }

        public EncryptColumnCommand(string columnName, ColumnEncryptionFlags encryptionFlags, bool statusCheck = false)
            : this(new ColumnRef(columnName), encryptionFlags, statusCheck) { }

        public EncryptColumnCommand(ColumnRef column, ColumnEncryptionFlags encryptionFlags, bool statusCheck = false)
        {
            Column = column;
            EncryptionFlags = encryptionFlags;
            StatusCheck = statusCheck;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.EncryptColumnCommandToString(this);
        }

        public override object Clone()
        {
            return new EncryptColumnCommand((ColumnRef)Column.Clone(), EncryptionFlags, StatusCheck);
        }
    }
}