using System;
using System.Linq;
using Yota.Core;
using Yota.Exceptions;
using Yota.Helpers;

namespace Yota.Guards
{
    public static class YotaGuard
    {
        public static void EnsureContainsEnoughValues<TYota, TEnum>()
            where TEnum : struct
            where TYota : IBaseYota
        {
            var maxValueYota = GetMaxValue<TYota>();
            var maxDefinedEnumValue = Enum.GetValues(typeof(TEnum))
                .Cast<int>()
                .Max();

            if (maxValueYota < maxDefinedEnumValue)
            {
                throw new YotaEnumMismatchingException();
            }
        }

        public static int GetMaxValue<TYota>()
        {
            return PropertyHelper.GetProperties<TYota>()
                .Sum(it => it.totalBits);
        }
    }
}