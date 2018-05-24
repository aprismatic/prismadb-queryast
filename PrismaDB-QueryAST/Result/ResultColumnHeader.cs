using System;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;

namespace PrismaDB.QueryAST.Result
{
    public class ResultColumnHeader
    {
        private Expression _expression;
        private ColumnDefinition _columnDefinition;

        public string ColumnName { get; set; }
        public Type DataType { get; set; }
        public int? MaxLength { get; set; }

        public Expression Expression
        {
            get => _expression;

            set
            {
                _expression = value;
                if (value is ColumnRef colRef)
                    ColumnName = colRef.DisplayName();
                else
                    ColumnName = value.Alias.id;
            }
        }

        public ColumnDefinition ColumnDefinition
        {
            get => _columnDefinition;

            set
            {
                _columnDefinition = value;

                switch (_columnDefinition.DataType)
                {
                    case SqlDataType.INT:
                        DataType = typeof(Int32);
                        break;
                    case SqlDataType.MSSQL_UNIQUEIDENTIFIER:
                        DataType = typeof(Guid);
                        break;
                    case SqlDataType.DATETIME:
                        DataType = typeof(DateTime);
                        break;
                    case SqlDataType.DOUBLE:
                        DataType = typeof(Double);
                        break;
                    case SqlDataType.VARCHAR:
                    case SqlDataType.TEXT:
                    case SqlDataType.TIMESTAMP:
                    case SqlDataType.ENUM:
                        DataType = typeof(String);
                        break;
                    default:
                        throw new ApplicationException("DataType not supported in DataTable.");
                }
            }
        }

        public ResultColumnHeader() : this("") { }

        public ResultColumnHeader(string columnName, Type dataType = null, int? maxLength = null)
        {
            ColumnName = columnName;
            DataType = dataType;
            MaxLength = maxLength;
        }

        public ResultColumnHeader(Expression exp, Type dataType = null, int? maxLength = null)
        {
            Expression = exp;
            MaxLength = maxLength;

            if (dataType != null)
                DataType = dataType;
        }

        public ResultColumnHeader(Expression exp, ColumnDefinition columnDef, Type dataType = null, int? maxLength = null)
        {
            Expression = exp;
            ColumnDefinition = columnDef;
            MaxLength = maxLength;

            if (dataType != null)
                DataType = dataType;
        }

        public void RemoveMetadata()
        {
            _expression = null;
            _columnDefinition = null;
        }
    }
}
