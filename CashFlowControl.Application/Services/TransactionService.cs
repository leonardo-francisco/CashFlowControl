using AutoMapper;
using CashFlowControl.Application.Contracts;
using CashFlowControl.Application.DTOs;
using CashFlowControl.Domain.Entities;
using CashFlowControl.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowControl.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;      
        private readonly IMapper _mapper;

        public TransactionService(ITransactionRepository transactionRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;           
            _mapper = mapper;
        }

        public async Task AddTransactionAsync(TransactionDTO transactionDto)
        {
            var entity = _mapper.Map<Transaction>(transactionDto);
            await _transactionRepository.AddAsync(entity);
        }

        public async Task<List<TransactionDTO>> GetTransactionsByDateAsync(DateTime date)
        {
            var transactions = await _transactionRepository.GetByDateAsync(date);
            return _mapper.Map<List<TransactionDTO>>(transactions);
        }
    }
}
