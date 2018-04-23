using System;

namespace PrismaDB.QueryAST.DCL
{
    public class ExportSetting : Command
    {
        public String Path;

        public ExportSetting()
        {
            Path = "";
        }

        public ExportSetting(String path)
        {
            Path = path;
        }

        public override object Clone()
        {
            var clone = new ExportSetting(Path);

            return clone;
        }
    }
}
