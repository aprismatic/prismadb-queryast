namespace PrismaDB.QueryAST.DCL
{
    public class UpdateKeysCommand : Command
    {
        public bool StatusCheck;

        public UpdateKeysCommand(bool statusCheck = false)
        {
            StatusCheck = statusCheck;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.UpdateKeysCommandToString(this);
        }

        public override object Clone()
        {
            return new UpdateKeysCommand(StatusCheck);
        }
    }
}