namespace PrismaDB.QueryAST.DCL
{
    public abstract class Command : Query
    {
    }

    public abstract class AsyncCommand : Command
    {
        public bool StatusCheck;
    }
}