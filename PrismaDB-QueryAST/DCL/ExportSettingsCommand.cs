using System;

namespace PrismaDB.QueryAST.DCL
{
    public class ExportSettingsCommand : Command
    {
        public string FileUri;

        public ExportSettingsCommand()
        {
            FileUri = "";
        }

        public ExportSettingsCommand(string fileUri)
        {
            FileUri = fileUri;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ExportSettingsCommandToString(this);
        }

        public override object Clone()
        {
            var clone = new ExportSettingsCommand(FileUri);

            return clone;
        }
    }
}