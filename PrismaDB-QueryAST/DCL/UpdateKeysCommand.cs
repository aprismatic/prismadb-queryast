using System.Collections.Generic;

namespace PrismaDB.QueryAST.DCL
{
    public class UpdateKeysCommand : AsyncCommand
    {
        public UpdateKeysCommand(bool statusCheck = false)
        {
            StatusCheck = statusCheck;
        }

        public override List<TableRef> GetTables() => new List<TableRef>();

        public override string ToString() => DialectResolver.Dialect.UpdateKeysCommandToString(this);

        public override object Clone() => new UpdateKeysCommand(StatusCheck);
    }
}