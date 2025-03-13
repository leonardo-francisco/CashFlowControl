using CashFlowControl.Domain.Entities;
using CashFlowControl.Domain.Enum;
using CashFlowControl.Domain.Interfaces;
using CashFlowControl.Infra.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowControl.Infra.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IMongoCollection<Transaction> _collection;

        public TransactionRepository(MongoDbContext context)
        {
            _collection = context.Transactions;
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _collection.InsertOneAsync(transaction);
        }

        public async Task<IEnumerable<Transaction>> GetByDateAsync(DateTime date)
        {           
            var startOfDay = date.Date;
            var endOfDay = date.Date.AddDays(1).AddTicks(-1);

            var filter = Builders<Transaction>.Filter.And(
                Builders<Transaction>.Filter.Gte(t => t.Date, startOfDay),
                Builders<Transaction>.Filter.Lte(t => t.Date, endOfDay)
            );
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<decimal> GetDailyConsolidatedAsync(DateTime date)
        {
            var transactions = await GetByDateAsync(date);
            return transactions.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount);
        }
    }
}
