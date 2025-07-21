using PAW2.Models;
using System.Linq.Expressions;

namespace PAW.Models
{
    public static class ConditionResolver
    {
        public static Expression<Func<T, bool>> ResolveCondition<T>(string searchCriteria, string propName, string value, decimal start, decimal end = 0)
        {
            var propInfo = typeof(T).GetProperty(propName, System.Reflection.BindingFlags.IgnoreCase
                | System.Reflection.BindingFlags.Public
                | System.Reflection.BindingFlags.Instance)
                ?? throw new NotSupportedException($"Condition not supported for property: {propName}");

            var param = Expression.Parameter(typeof(T), "entity");
            var prop = Expression.Property(param, propInfo);

            // Handle nullable types
            Expression propValue = prop;
            var targetType = propInfo.PropertyType;
            if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                propValue = Expression.Property(prop, "Value");
                targetType = Nullable.GetUnderlyingType(targetType)!;
            }

            // Convert string value to the property type
            object? parsedValue = null;
            if (!string.IsNullOrEmpty(value))
            {
                if (targetType == typeof(string))
                    parsedValue = value;
                else if (targetType.IsEnum)
                    parsedValue = Enum.Parse(targetType, value, true);
                else
                    parsedValue = Convert.ChangeType(value, targetType);
            }
            else if (targetType == typeof(string))
            {
                parsedValue = string.Empty;
            }

            var constantValue = Expression.Constant(parsedValue ?? Convert.ChangeType(0, targetType), targetType);
            var constantStart = Expression.Constant(Convert.ChangeType(start, targetType), targetType);
            var constantEnd = Expression.Constant(Convert.ChangeType(end, targetType), targetType);

            Expression body = searchCriteria.Replace(" ", "").ToLowerInvariant() switch
            {
                "equals" => Expression.Equal(propValue, constantValue),
                "notequals" => Expression.NotEqual(propValue, constantValue),
                "lessthan" when end != null => Expression.LessThan(propValue, constantEnd),
                "lessthanorequal" when end != null => Expression.LessThanOrEqual(propValue, constantEnd),
                "greaterthan" => Expression.GreaterThan(propValue, constantStart),
                "greaterthanorequal" when end != null => Expression.AndAlso(
                    Expression.GreaterThanOrEqual(propValue, constantStart),
                    Expression.LessThanOrEqual(propValue, constantEnd)
                ),
                "greaterthanorequal" => Expression.GreaterThanOrEqual(propValue, constantStart),
                "between" => Expression.AndAlso(
                    Expression.GreaterThanOrEqual(propValue, constantStart),
                    Expression.LessThanOrEqual(propValue, constantEnd)
                ),
                "min" => Expression.Equal(propValue, constantStart),
                "max" => Expression.Equal(propValue, constantEnd),
                "contains" when targetType == typeof(string) =>
                    Expression.Call(propValue,
                        typeof(string).GetMethod("Contains", [typeof(string)])
                        ?? throw new NotSupportedException("Contains method not found"),
                        constantValue),
                _ => throw new NotSupportedException($"Search criteria not supported: {searchCriteria}"),
            };


            // If property is nullable, check for HasValue
            if (propInfo.PropertyType.IsGenericType && propInfo.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                var hasValue = Expression.Property(prop, "HasValue");
                body = Expression.AndAlso(hasValue, body);
            }

            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
