using CashFlowControl.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowControl.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);
        Task<IEnumerable<Transaction>> GetByDateAsync(DateTime date);
        Task<decimal> GetDailyConsolidatedAsync(DateTime date);
    }
}
