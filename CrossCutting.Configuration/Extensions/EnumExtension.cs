using Easy.Transfers.CrossCutting.Configuration.Extensions.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Easy.Transfers.CrossCutting.Configuration.Extensions
{
    public static class EnumExtension
    {
        public static T GetEnumByName<T>(string name) where T : struct, IConvertible
        {
            var type = typeof(T);
            foreach (var field in type.GetFields())
            {
                if (field.Name.ToLower() == name.ToLower())
                    return (T)field.GetValue(null);
            }
            throw new ArgumentException("Not found.", "name");
        }

        public static string GetDescription<T>(this T enumerador) where T : struct, IConvertible
        {
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();

            var field = enumerador.GetType().GetField(enumerador.ToString());

            var attributes = (DescriptionAttribute[])field.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            return attributes != null && attributes.Length > 0 ? attributes[0].Description : enumerador.ToString();
        }

        public static List<T> ConvertToList<T>() where T : struct, IConvertible
        {
            return Enum.GetNames(typeof(T))
                .Select(nome => GetEnumByName<T>(nome))
                .ToList();
        }

        public static List<ObjectEnum<T>> ConvertToListObject<T>() where T : struct, IConvertible
        {
            return ConvertToList<T>()
                .Select(enumerador => new ObjectEnum<T>(enumerador, enumerador.GetDescription()))
                .ToList();
        }
    }
}
