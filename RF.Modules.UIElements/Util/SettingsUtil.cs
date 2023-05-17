using RF.Modules.UIElements.Models;
using System;
using System.Linq.Expressions;
using System.Reflection;

namespace RF.Modules.UIElements.Util
{
    internal static class SettingsUtil
    {
        private static PropertyInfo GetPropertyInfo<TModel, TProperty>(Expression<Func<TModel, TProperty>> propertyExpression)
        {
            var expression = (MemberExpression)propertyExpression.Body;
            var property = expression.Member as PropertyInfo;
            if (property is null)
                throw new ArgumentException("Please specify a member expression.");

            return property;
        }

        public static string GetKey<TModel, TProperty>(
            this TModel _,
            Expression<Func<TModel, TProperty>> expression
            )
            where TModel : SettingsModel
        {
            var property = GetPropertyInfo(expression);
            return $"RF.UIElements.{typeof(TModel).Name}_{property.Name}";
        }
    }
}