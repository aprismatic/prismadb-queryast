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
        

        MySQL_INT = 1000,
        MySQL_TINYINT = 1001,
        MySQL_SMALLINT = 1002,
        MySQL_BIGINT = 1003,
        MySQL_DOUBLE = 1004,

        MySQL_DATE = 1100,
        MySQL_DATETIME = 1101,
        MySQL_TIMESTAMP = 1102,
        
        MySQL_CHAR = 1200,
        MySQL_VARCHAR = 1201,
        MySQL_TEXT = 1202,
        
        MySQL_BINARY = 1300,
        MySQL_VARBINARY = 1301,
        MySQL_BLOB = 1302,

        MySQL_ENUM = 1400,
    }

    [Flags]
    public enum ColumnEncryptionFlags
    {
        None = 0,
        Store = 1,
        Addition = 2,
        Multiplication = 4,
        Search = 8,
        Range = 16
    }

    public class ColumnDefinition : ICloneable
    {
        public Identifier ColumnName;
        public SqlDataType DataType;
        public ColumnEncryptionFlags EncryptionFlags;
        public int? KeyVersion;
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
                                int? keyVersion = null,
                                Expression defaultValue = null,
                                bool autoIncrement = false)
            : this(new Identifier(columnName),
                   dataType,
                   length,
                   nullable,
                   isRowId,
                   encryptionFlags,
                   keyVersion,
                   defaultValue,
                   autoIncrement)
        { }

        public ColumnDefinition(Identifier column,
                                SqlDataType dataType,
                                int? length = null,
                                bool nullable = true,
                                bool isRowId = false,
                                ColumnEncryptionFlags encryptionFlags = ColumnEncryptionFlags.None,
                                int? keyVersion = null,
                                Expression defaultValue = null,
                                bool autoIncrement = false)
        {
            this.ColumnName = column == null ? new Identifier("") : column.Clone();
            this.DataType = dataType;
            this.EncryptionFlags = encryptionFlags;
            this.KeyVersion = keyVersion;
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
            KeyVersion = other.KeyVersion;
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
                && (KeyVersion == otherCD.KeyVersion)
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
                (KeyVersion == null ? 1 : KeyVersion.GetHashCode()) *
                (Length == null ? 1 : Length.GetHashCode()) *
                EnumValues.Select(x => x.GetHashCode()).Aggregate((a, b) => a * b) *
                (1 + Nullable.GetHashCode()) *
                (1 + IsRowId.GetHashCode()) *
                DefaultValue.GetHashCode() *
                (1 + AutoIncrement.GetHashCode()));
        }
    }
}
