namespace PrismaDB.QueryAST.DCL
{
    public class UpdateKeysCommand : Command
    {
        public UpdateKeysCommand()
        {
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.UpdateKeysCommandToString(this);
        }

        public override object Clone()
        {
            var clone = new UpdateKeysCommand();

            return clone;
        }
    }
}