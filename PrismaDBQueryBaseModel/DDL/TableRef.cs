namespace PrismaDBQueryBaseModel
{
    public class TableRef
    {
        public string TableName;

        public TableRef(string TableName)
        {
            this.TableName = TableName;
        }

        public TableRef Clone()
        {
            return new TableRef(TableName);
        }

        public override string ToString()
        {
            return TableName.Length > 0 ? "[" + TableName + "]" : "";
        }
    }
}
