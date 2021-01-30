using System;
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
                    propertyInfo.SetValue(yotaEntity, (byte) newValue);
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