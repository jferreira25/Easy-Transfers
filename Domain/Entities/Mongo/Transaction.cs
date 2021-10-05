using Easy.Transfers.Domain.Enum;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json.Converters;
using System;
using System.Text.Json.Serialization;

namespace Easy.Transfers.Domain.Entities.Mongo
{
    public class Transaction
    {
        public Transaction()
        {
            TransactionId = Guid.NewGuid().ToString();
        }
        public ObjectId Id { get; set; }
        public string TransactionId { get; set; }

        public string AccountOrigin { get; set; }
        public string AccountDestination { get; set; }
        public decimal Value { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        [BsonRepresentation(BsonType.String)]
        public TransferStatus Status { get; set; }
        public string MessageError { get; set; }
        public DateTime CreatedTransaction { get; set; }
        public DateTime? ChangedTransaction { get; set; }
        
    }
}
