using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowControl.Domain.Entities
{
    public abstract class BaseEntity
    {
        [BsonId]
        public ObjectId Id { get; protected set; }
        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; protected set; } = DateTime.UtcNow;

        public void UpdateTimestamp() => UpdatedAt = DateTime.UtcNow;
    }
}
