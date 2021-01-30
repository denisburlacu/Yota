using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace Yota.Helpers
{
    internal static class PropertyHelper
    {
        private static readonly Dictionary<Type, (PropertyInfo propertyInfo, int totalBits)[]> Map =
            new Dictionary<Type, (PropertyInfo propertyInfo, int totalBits)[]>();

        internal static (PropertyInfo propertyInfo, int position) GetProperty<TYota, TEnum>(TEnum tEnum)
        {
            var position = Convert.ToInt32(tEnum);

            var bits = GetProperties<TYota>();

            foreach (var (propertyInfo, bitCount) in bits)
            {
                if (position < bitCount)
                {
                    return (propertyInfo, position);
                }

                position = (position - bitCount);
            }

            throw new Exception($"Property not found for {typeof(TYota).FullName} and enum value {tEnum}");
        }

        internal static (PropertyInfo propertyInfo, int totalBits)[] GetProperties<TYota>()
        {
            var type = typeof(TYota);

            if (Map.ContainsKey(type))
            {
                return Map[type];
            }

            Monitor.Enter(Map);

            try
            {
                var newProps = GetPropertiesIntern<TYota>();
                Map.Add(type, newProps);
                return newProps;
            }
            finally
            {
                Monitor.Exit(Map);
            }
        }

        private static (PropertyInfo propertyInfo, int bits)[] GetPropertiesIntern<TYota>()
        {
            return typeof(TYota)
                .GetProperties()
                .OrderBy(it => it.Name)
                .Select(it =>
                {
                    var size = Marshal.SizeOf(it.PropertyType) * 8;
                    return (it, size);
                })
                .ToArray();
        }
    }
}