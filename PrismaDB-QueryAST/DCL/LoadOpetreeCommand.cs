using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public class LoadOpetreeCommand : Command
    {
        public LoadOpetreeCommand()
        {
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.LoadOpetreeCommandToString(this);

        public override object Clone() => new LoadSchemaCommand();
    }
}