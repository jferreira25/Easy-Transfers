using System;

namespace Easy.Transfers.CrossCutting.Configuration.Extensions.Models
{
    public class ObjectEnum<T> where T : struct, IConvertible
    {
        public T Id { get; set; }
        public string Descricao { get; set; }

        public ObjectEnum(T id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }
    }
}
