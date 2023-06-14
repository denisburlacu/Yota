using System;
using System.Linq;
using Yota.Core;

namespace Yota.Helpers
{
    public static class YotaHelper
    {
        public static void RemoveFlag<TYota, TEnum>(IYotaEntity<TYota, TEnum> yotaEntity, TEnum tEnum)
            where TYota : IBaseYota
            where TEnum : struct
        {
            SetFlag(yotaEntity, tEnum, false);
        }

        public static void SetFlag<TYota, TEnum>(IYotaEntity<TYota, TEnum> yotaEntity, TEnum tEnum)
            where TYota : IBaseYota
            where TEnum : struct
        {
            SetFlag(yotaEntity, tEnum, true);
        }

        public static void ClearFlags<TYota, TEnum>(IYotaEntity<TYota, TEnum> yotaEntity)
            where TYota : IBaseYota
            where TEnum : struct
        {
            var properties = PropertyHelper.GetProperties<TYota>();

            foreach (var property in properties)
            {
                var propertyInfo = property.propertyInfo;
                var currentValue = propertyInfo.GetValue(yotaEntity);

                //weird but fast
                if (currentValue is byte @byte)
                {
                    propertyInfo.SetValue(yotaEntity, (byte)0);
                }
                else if (currentValue is int @int)
                {
                    propertyInfo.SetValue(yotaEntity, 0);
                }

                else if (currentValue is long @long)
                {
                    propertyInfo.SetValue(yotaEntity, (long)0);
                }
                else
                {
                    throw new NotSupportedException(currentValue.GetType().FullName);
                }
            }
        }

        public static bool ContainsFlag<TYota, TEnum>(IYotaEntity<TYota, TEnum> yotaEntity, TEnum tEnum)
            where TYota : IBaseYota
            where TEnum : struct
        {
            var (propertyInfo, position) = PropertyHelper.GetProperty<TYota, TEnum>(tEnum);

            var currentValue = propertyInfo.GetValue(yotaEntity);

            if (currentValue != null)
            {
                //weird but fast
                if (currentValue is byte @byte)
                {
                    return (@byte & (1 << position)) != 0;
                }

                if (currentValue is int @int)
                {
                    return (@int & (1 << position)) != 0;
                }

                if (currentValue is long @long)
                {
                    return (@long & (1L << position)) != 0;
                }

                throw new NotSupportedException(currentValue.GetType().FullName);
            }

            throw new NullReferenceException($"Current value not found for property {propertyInfo.Name}");
        }

        private static void SetFlag<TYota, TEnum>(IYotaEntity<TYota, TEnum> yotaEntity, TEnum tEnum, bool set)
            where TYota : IBaseYota
            where TEnum : struct
        {
            var (propertyInfo, position) = PropertyHelper.GetProperty<TYota, TEnum>(tEnum);

            var currentValue = propertyInfo.GetValue(yotaEntity);
            if (currentValue != null)
            {
                //weird but fast
                if (currentValue is byte @byte)
                {
                    var newValue = set ? (@byte | 1 << position) : @byte & ~(1 << position);
                    propertyInfo.SetValue(yotaEntity, (byte)newValue);
                }
                else if (currentValue is int @int)
                {
                    var newValue = set ? (@int | 1 << position) : @int & ~(1 << position);
                    propertyInfo.SetValue(yotaEntity, newValue);
                }

                else if (currentValue is long @long)
                {
                    var newValue = set ? (@long | 1L << position) : @long & ~(1L << position);
                    propertyInfo.SetValue(yotaEntity, newValue);
                }
                else
                {
                    throw new NotSupportedException(currentValue.GetType().FullName);
                }
            }
        }
    }
}