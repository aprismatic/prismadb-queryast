using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.DCL
{
    public class DecryptColumnCommand : Command
    {
        public ColumnRef Column;
        public bool StatusCheck;

        public DecryptColumnCommand(bool statusCheck = false)
            : this(new ColumnRef(""), statusCheck) { }

        public DecryptColumnCommand(string columnName, bool statusCheck = false)
            : this(new ColumnRef(columnName), statusCheck) { }

        public DecryptColumnCommand(ColumnRef column, bool statusCheck = false)
        {
            Column = column;
            StatusCheck = statusCheck;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.DecryptColumnCommandToString(this);
        }

        public override object Clone()
        {
            return new DecryptColumnCommand((ColumnRef)Column.Clone(), StatusCheck);
        }
    }
}