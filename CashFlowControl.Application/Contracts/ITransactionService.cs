using CashFlowControl.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowControl.Application.Contracts
{
    public interface ITransactionService
    {
        Task AddTransactionAsync(TransactionDTO transaction);
        Task<List<TransactionDTO>> GetTransactionsByDateAsync(DateTime date);
    }
}
