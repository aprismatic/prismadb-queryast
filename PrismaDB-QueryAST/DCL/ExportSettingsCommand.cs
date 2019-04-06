using PrismaDB.QueryAST.DML;
using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public class ExportSettingsCommand : Command
    {
        public StringConstant FileUri;

        public ExportSettingsCommand()
        {
            FileUri = new StringConstant("");
        }

        public ExportSettingsCommand(string fileUri)
        {
            FileUri = new StringConstant(fileUri);
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.ExportSettingsCommandToString(this);

        public override object Clone() => new ExportSettingsCommand(FileUri.strvalue);
    }
}