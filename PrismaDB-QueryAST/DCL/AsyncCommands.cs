using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public abstract class AsyncCommand : Command
    {
        public bool StatusCheck;
    }

    public class UpdateKeysCommand : AsyncCommand
    {
        public UpdateKeysCommand(bool statusCheck = false) { StatusCheck = statusCheck; }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.UpdateKeysCommandToString(this);

        public override object Clone() => new UpdateKeysCommand(StatusCheck);
    }

    public class EncryptColumnCommand : AsyncCommand
    {
        public ColumnRef Column;
        public ColumnEncryptionFlags EncryptionFlags;

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

    public class DecryptColumnCommand : AsyncCommand
    {
        public ColumnRef Column;

        public DecryptColumnCommand(bool statusCheck = false)
            : this(new ColumnRef(""), statusCheck) { }

        public DecryptColumnCommand(string columnName, bool statusCheck = false)
            : this(new ColumnRef(columnName), statusCheck) { }

        public DecryptColumnCommand(ColumnRef column, bool statusCheck = false)
        {
            Column = column;
            StatusCheck = statusCheck;
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.DecryptColumnCommandToString(this);

        public override object Clone() => new DecryptColumnCommand((ColumnRef)Column.Clone(), StatusCheck);
    }

    public class RebalanceOpetreeCommand : AsyncCommand
    {
        public List<Constant> WithValues;

        public RebalanceOpetreeCommand(bool statusCheck = false)
        {
            WithValues = new List<Constant>();
            StatusCheck = statusCheck;
        }
        public RebalanceOpetreeCommand(List<Constant> withValues, bool statusCheck = false)
        {
            WithValues = withValues;
            StatusCheck = statusCheck;
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.RebalanceOpetreeCommandToString(this);

        public override object Clone()
        {
            var res = new RebalanceOpetreeCommand(StatusCheck);
            foreach (var value in WithValues)
                res.WithValues.Add((Constant)value.Clone());
            return res;
        }
    }
}
