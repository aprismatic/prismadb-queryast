using PrismaDB.QueryAST.DML;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PrismaDB.QueryAST.DDL
{
    public enum SqlDataType
    {
        MSSQL_INT = 0,
        MSSQL_TINYINT = 1,
        MSSQL_SMALLINT = 2,
        MSSQL_BIGINT = 3,
        MSSQL_FLOAT = 4,

        MSSQL_DATE = 100,
        MSSQL_DATETIME = 101,

        MSSQL_CHAR = 200,
        MSSQL_VARCHAR = 201,
        MSSQL_TEXT = 202,
        MSSQL_NCHAR = 203,
        MSSQL_NVARCHAR = 204,
        MSSQL_NTEXT = 205,

        MSSQL_BINARY = 300,
        MSSQL_VARBINARY = 301,
        
        MSSQL_UNIQUEIDENTIFIER = 400,
        

        MYSQL_INT = 1000,
        MYSQL_TINYINT = 1001,
        MYSQL_SMALLINT = 1002,
        MYSQL_BIGINT = 1003,
        MYSQL_DOUBLE = 1004,

        MYSQL_DATE = 1100,
        MYSQL_DATETIME = 1101,
        MYSQL_TIMESTAMP = 1102,
        
        MYSQL_CHAR = 1200,
        MYSQL_VARCHAR = 1201,
        MYSQL_TEXT = 1202,
        
        MYSQL_BINARY = 1300,
        MYSQL_VARBINARY = 1301,
        MYSQL_BLOB = 1302,

        MYSQL_ENUM = 1400,
    }

    [Flags]
    public enum ColumnEncryptionFlags
    {
        None = 0,
        Store = 1,
        IntegerAddition = 2,
        IntegerMultiplication = 4,
        Search = 8,
        Range = 16
    }

    public class ColumnDefinition : ICloneable
    {
        public Identifier ColumnName;
        public SqlDataType DataType;
        public ColumnEncryptionFlags EncryptionFlags;
        public int? Length;
        public List<StringConstant> EnumValues;
        public bool Nullable;
        public bool IsRowId;
        public Expression DefaultValue;
        public bool AutoIncrement;

        public ColumnDefinition()
            : this("", SqlDataType.MSSQL_INT)
        { }

        public ColumnDefinition(string columnName,
                                SqlDataType dataType,
                                int? length = null,
                                bool nullable = true,
                                bool isRowId = false,
                                ColumnEncryptionFlags encryptionFlags = ColumnEncryptionFlags.None,
                                Expression defaultValue = null,
                                bool autoIncrement = false)
            : this(new Identifier(columnName),
                   dataType,
                   length,
                   nullable,
                   isRowId,
                   encryptionFlags,
                   defaultValue,
                   autoIncrement)
        { }

        public ColumnDefinition(Identifier column,
                                SqlDataType dataType,
                                int? length = null,
                                bool nullable = true,
                                bool isRowId = false,
                                ColumnEncryptionFlags encryptionFlags = ColumnEncryptionFlags.None,
                                Expression defaultValue = null,
                                bool autoIncrement = false)
        {
            this.ColumnName = column == null ? new Identifier("") : column.Clone();
            this.DataType = dataType;
            this.EncryptionFlags = encryptionFlags;
            this.Length = length;
            this.EnumValues = new List<StringConstant>();
            this.Nullable = nullable;
            this.IsRowId = isRowId;
            this.DefaultValue = defaultValue;
            this.AutoIncrement = autoIncrement;
        }

        public ColumnDefinition(ColumnDefinition other)
        {
            ColumnName = other.ColumnName.Clone();
            DataType = other.DataType;
            EncryptionFlags = other.EncryptionFlags;
            Length = other.Length;
            EnumValues = other.EnumValues.Select(item => (StringConstant)item.Clone()).ToList();
            Nullable = other.Nullable;
            IsRowId = other.IsRowId;
            DefaultValue = other.DefaultValue;
            AutoIncrement = other.AutoIncrement;
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.ColumnDefinitionToString(this);
        }

        public object Clone()
        {
            return new ColumnDefinition(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is ColumnDefinition otherCD)) return false;

            return (ColumnName.Equals(otherCD.ColumnName))
                && (DataType == otherCD.DataType)
                && (EncryptionFlags == otherCD.EncryptionFlags)
                && (Length == otherCD.Length)
                && (EnumValues.SequenceEqual(otherCD.EnumValues))
                && (Nullable == otherCD.Nullable)
                && (IsRowId == otherCD.IsRowId)
                && (Equals(DefaultValue, otherCD.DefaultValue)
                && (AutoIncrement == otherCD.AutoIncrement));
        }

        public override int GetHashCode()
        {
            return unchecked (
                ColumnName.GetHashCode() *
                DataType.GetHashCode() *
                EncryptionFlags.GetHashCode() *
                (Length == null ? 1 : Length.GetHashCode()) *
                EnumValues.Select(x => x.GetHashCode()).Aggregate((a, b) => a * b) *
                (1 + Nullable.GetHashCode()) *
                (1 + IsRowId.GetHashCode()) *
                DefaultValue.GetHashCode() *
                (1 + AutoIncrement.GetHashCode()));
        }
    }
}
