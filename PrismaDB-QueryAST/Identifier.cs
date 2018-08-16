using System;
using System.ComponentModel;
using System.Globalization;

namespace PrismaDB.QueryAST
{
    [TypeConverter(typeof(IdentifierTypeConverter))]
    public class Identifier
    {
        public string id;

        public Identifier()
            : this("")
        { }

        public Identifier(string newId)
        {
            id = newId;
        }

        public Identifier Clone()
        {
            return new Identifier(id);
        }

        public override string ToString()
        {
            return DialectResolver.Dialect.IdentifierToString(this);
        }

        public override bool Equals(object other)
        {
            if (!(other is Identifier otherId)) return false;

            return String.Equals(id, otherId.id, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return id.ToLowerInvariant().GetHashCode();
        }

        public class IdentifierTypeConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string s)
                {
                    return new Identifier(s);
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    var tr = (Identifier)value;
                    return tr.id;
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
