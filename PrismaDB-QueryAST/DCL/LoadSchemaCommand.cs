using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public class LoadSchemaCommand : Command
    {
        public LoadSchemaCommand()
        {
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.LoadSchemaCommandToString(this);

        public override object Clone() => new LoadSchemaCommand();
    }
}