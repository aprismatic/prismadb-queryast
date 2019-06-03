using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public class SaveOpetreeCommand : Command
    {
        public SaveOpetreeCommand()
        {
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.SaveOpetreeCommandToString(this);

        public override object Clone() => new UpdateKeysCommand();
    }
}