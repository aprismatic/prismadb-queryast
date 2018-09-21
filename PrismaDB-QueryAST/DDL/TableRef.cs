using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;

namespace PrismaDB.QueryAST
{
    [TypeConverter(typeof(TableRefTypeConverter))]
    public class TableRef
    {
        public Identifier Table;
        public bool IsTempTable;
        public Identifier Alias;

        public TableRef(string TableName, bool IsTemp = false, string AliasName = "")
        {
            Table = new Identifier(TableName);
            IsTempTable = IsTemp;
            Alias = new Identifier(AliasName);
        }

        public TableRef Clone()
        {
            return new TableRef(Table.id, IsTempTable, Alias.id);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.TableRefToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is TableRef otherTR)) return false;

            return String.Equals(Table.id, otherTR.Table.id, StringComparison.InvariantCultureIgnoreCase)
                && String.Equals(Alias.id, otherTR.Alias.id, StringComparison.InvariantCultureIgnoreCase)
                && IsTempTable == otherTR.IsTempTable;
        }

        public override int GetHashCode()
        {
            return unchecked(
                Table.GetHashCode() *
                Alias.GetHashCode() *
                (IsTempTable.GetHashCode() + 1));
        }

        public class TableRefTypeConverter : TypeConverter
        {
            private const string TableIdKey = "TableId";
            private const string IsTempTableKey = "IsTempTable";

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string s)
                {
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(s);
                    return new TableRef(dict[TableIdKey], Convert.ToBoolean(dict[IsTempTableKey]));
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    var tr = (TableRef)value;
                    var dict = new Dictionary<string, string>
                    {
                        [TableIdKey] = tr.Table.id,
                        [IsTempTableKey] = tr.IsTempTable.ToString()
                    };
                    return JsonConvert.SerializeObject(dict);
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
