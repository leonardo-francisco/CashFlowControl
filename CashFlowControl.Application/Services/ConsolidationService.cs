using CashFlowControl.Application.Contracts;
using CashFlowControl.Application.DTOs;
using CashFlowControl.Domain.Enum;
using CashFlowControl.Domain.Interfaces;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowControl.Application.Services
{
    public class ConsolidationService : IConsolidationService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public ConsolidationService(ITransactionRepository transactionRepository, AsyncCircuitBreakerPolicy circuitBreakerPolicy)
        {
            _transactionRepository = transactionRepository;
            _circuitBreakerPolicy = circuitBreakerPolicy;
        }

        public async Task<ConsolidationDTO> GetDailyConsolidationAsync(DateTime date)
        {
            return await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                var transactions = await _transactionRepository.GetByDateAsync(date);
                return new ConsolidationDTO
                {
                    Date = date,
                    TotalCredits = transactions.Where(t => t.Type == TransactionType.Credit).Sum(t => t.Amount),
                    TotalDebits = transactions.Where(t => t.Type == TransactionType.Debit).Sum(t => t.Amount),
                    Balance = transactions.Sum(t => t.Type == TransactionType.Credit ? t.Amount : -t.Amount)
                };
            });
        }
    }
}
