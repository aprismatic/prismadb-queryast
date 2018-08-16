using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Newtonsoft.Json;

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
            private const string IdKey = "Id";

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string s)
                {
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(s);
                    return new Identifier(dict[IdKey]);
                }
                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            {
                if (destinationType == typeof(string))
                {
                    var identifier = (Identifier)value;
                    var dict = new Dictionary<string, string>
                    {
                        [IdKey] = identifier.id
                    };
                    return JsonConvert.SerializeObject(dict);
                }
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
