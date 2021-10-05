using System;

namespace Easy.Transfers.CrossCutting.Configuration.Extensions
{

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class NomeCollectionAttribute : Attribute
    {
        public NomeCollectionAttribute(string nomeCollection)
        {
            NomeCollection = nomeCollection;
        }
        public string NomeCollection { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class DataCollectionExtension
    {
        public static string ObterNomeCollection<T>()
        {

            NomeCollectionAttribute MyAttribute =
                (NomeCollectionAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(NomeCollectionAttribute));

            if (MyAttribute == null)
            {
                return typeof(T).Name;
            }
            else
            {
                return MyAttribute.NomeCollection;
            }
        }
    }
}
