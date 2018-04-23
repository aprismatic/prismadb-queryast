using PrismaDB.QueryAST.DML;

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

        public override string ToString()
        {
            return DialectResolver.Dialect.ExportSettingsCommandToString(this);
        }

        public override object Clone()
        {
            var clone = new ExportSettingsCommand(FileUri.strvalue);

            return clone;
        }
    }
}