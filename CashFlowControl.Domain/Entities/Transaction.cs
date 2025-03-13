using CashFlowControl.Domain.Enum;
using CashFlowControl.Domain.Exceptions;
using MongoDB.Bson.Serialization.Attributes;

namespace CashFlowControl.Domain.Entities
{
    public class Transaction : BaseEntity
    {
        [BsonElement("amount")]
        public decimal Amount { get; set; }
        [BsonElement("description")]
        public string Description { get; set; }
        [BsonElement("date")]
        public DateTime Date { get; set; }
        [BsonElement("type")]
        public TransactionType Type { get; set; }

        public Transaction() { }

        public Transaction(decimal amount, string description, DateTime date, TransactionType type)
        {
            if (amount <= 0)
                throw new DomainException("O valor da transação deve ser positivo.");

            Amount = amount;
            Description = description;
            Date = date;
            Type = type;
        }
    }
}
