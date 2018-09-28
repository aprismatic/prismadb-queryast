using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.DCL
{
    public class DecryptColumnCommand : Command
    {
        public ColumnRef Column;

        public DecryptColumnCommand()
        {
            Column = new ColumnRef("");
        }

        public DecryptColumnCommand(string columnName)
        {
            Column = new ColumnRef(columnName);
        }

        public DecryptColumnCommand(ColumnRef column)
        {
            Column = column;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.DecryptColumnCommandToString(this);
        }

        public override object Clone()
        {
            var clone = new DecryptColumnCommand((ColumnRef)Column.Clone());

            return clone;
        }
    }
}