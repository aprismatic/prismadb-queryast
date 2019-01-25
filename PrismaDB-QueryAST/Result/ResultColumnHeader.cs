using Newtonsoft.Json;
using PrismaDB.QueryAST.DDL;
using PrismaDB.QueryAST.DML;
using System;
using System.Xml.Serialization;

namespace PrismaDB.QueryAST.Result
{
    public class ResultColumnHeader : PrismaDB.Result.ResultColumnHeader
    {
        private Expression _expression;
        private ColumnDefinition _columnDefinition;

        [JsonIgnore]
        [XmlIgnore]
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

        [JsonIgnore]
        [XmlIgnore]
        public ColumnDefinition ColumnDefinition
        {
            get => _columnDefinition;

            set
            {
                _columnDefinition = value;

                switch (_columnDefinition.DataType)
                {
                    case SqlDataType.MSSQL_INT:
                    case SqlDataType.MySQL_INT:
                        DataType = typeof(Int32);
                        break;
                    case SqlDataType.MSSQL_BIGINT:
                    case SqlDataType.MySQL_BIGINT:
                        DataType = typeof(Int64);
                        break;
                    case SqlDataType.MSSQL_SMALLINT:
                    case SqlDataType.MySQL_SMALLINT:
                        DataType = typeof(Int16);
                        break;
                    case SqlDataType.MSSQL_TINYINT:
                        DataType = typeof(Byte);
                        break;
                    case SqlDataType.MySQL_TINYINT:
                        DataType = typeof(SByte);
                        break;
                    case SqlDataType.MSSQL_UNIQUEIDENTIFIER:
                        DataType = typeof(Guid);
                        break;
                    case SqlDataType.MSSQL_DATETIME:
                    case SqlDataType.MySQL_DATETIME:
                        DataType = typeof(DateTime);
                        break;
                    case SqlDataType.MSSQL_FLOAT:
                    case SqlDataType.MySQL_DOUBLE:
                        DataType = typeof(Double);
                        break;
                    case SqlDataType.MSSQL_CHAR:
                    case SqlDataType.MySQL_CHAR:
                    case SqlDataType.MSSQL_NCHAR:
                    case SqlDataType.MSSQL_VARCHAR:
                    case SqlDataType.MySQL_VARCHAR:
                    case SqlDataType.MSSQL_NVARCHAR:
                    case SqlDataType.MSSQL_TEXT:
                    case SqlDataType.MySQL_TEXT:
                    case SqlDataType.MSSQL_NTEXT:
                    case SqlDataType.MySQL_TIMESTAMP:
                    case SqlDataType.MySQL_ENUM:
                    case SqlDataType.MSSQL_DATE:
                    case SqlDataType.MySQL_DATE:
                        DataType = typeof(String);
                        break;
                    case SqlDataType.MSSQL_BINARY:
                    case SqlDataType.MSSQL_VARBINARY:
                    case SqlDataType.MySQL_BINARY:
                    case SqlDataType.MySQL_VARBINARY:
                    case SqlDataType.MySQL_BLOB:
                        DataType = typeof(byte[]);
                        break;
                    default:
                        throw new ApplicationException("DataType not supported in DataTable.");
                }
            }
        }

        public ResultColumnHeader() : this("") { }

        public ResultColumnHeader(string columnName, Type dataType = null, int? maxLength = null)
             : base(columnName, dataType, maxLength) { }

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
    }
}
