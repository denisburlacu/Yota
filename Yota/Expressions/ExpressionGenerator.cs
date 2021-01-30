using System;
using System.Linq.Expressions;
using Yota.Core;
using Yota.Helpers;

namespace Yota.Expressions
{
    public static class ExpressionGenerator
    {
        public static Expression<Func<TEntity, bool>> HasFlagExpression<TEntity, TYota, TEnum>(TEnum @enum)
            where TEntity : IYotaEntity<TYota, TEnum>
            where TYota : IBaseYota
        {
            var (propertyInfo, position) = PropertyHelper.GetProperty<TYota, TEnum>(@enum);

            var valueConstantExpression = GetValueConstantExpression(position, propertyInfo.PropertyType);

            var parameter = Expression.Parameter(typeof(TEntity), "x");

            var memberExpression = Expression.Property(parameter, propertyInfo.Name);

            var propertyValueExpression = Expression.Convert(memberExpression, propertyInfo.PropertyType);

            var body = Expression.NotEqual(
                left: Expression.And(propertyValueExpression, valueConstantExpression),
                right: Expression.Constant(ConvertToType(0, propertyInfo.PropertyType)));

            return Expression.Lambda<Func<TEntity, bool>>(body: body, parameters: parameter);
        }

        private static ConstantExpression GetValueConstantExpression(int position, Type propertyInfoPropertyType)
        {
            if (propertyInfoPropertyType == typeof(byte))
            {
                return Expression.Constant(Convert.ToByte(1 << position));
            }

            if (propertyInfoPropertyType == typeof(int))
            {
                return Expression.Constant(Convert.ToInt32(1 << position));
            }
            else if (propertyInfoPropertyType == typeof(long))
            {
                return Expression.Constant(Convert.ToInt64(1L << position));
            }

            throw new Exception($"Unsupported type for {propertyInfoPropertyType.FullName}");
        }

        private static object ConvertToType(object obj, Type t)
        {
            var convertedObject = Convert.ChangeType(obj, t);
            return convertedObject;
        }
    }
}