using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public class RebalanceOpetreeCommand : Command
    {
        public RebalanceOpetreeCommand() { }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.RebalanceOpetreeCommandToString(this);

        public override object Clone() => new UpdateKeysCommand();
    }
}