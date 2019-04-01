using System.Collections.Generic;
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

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.EncryptColumnCommandToString(this);

        public override object Clone() => new EncryptColumnCommand((ColumnRef)Column.Clone(), EncryptionFlags, StatusCheck);
    }
}