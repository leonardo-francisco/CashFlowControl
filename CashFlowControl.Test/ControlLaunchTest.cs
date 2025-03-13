using AutoMapper;
using CashFlowControl.Application.DTOs;
using CashFlowControl.Application.Services;
using CashFlowControl.Domain.Entities;
using CashFlowControl.Domain.Enum;
using CashFlowControl.Domain.Interfaces;
using Moq;
using Polly;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowControl.Test
{
    public class ControlLaunchTest
    {
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;
        private readonly ConsolidationService _consolidationService;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMockForTransactionService;
        private readonly TransactionService _transactionService;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public ControlLaunchTest()
        {
            // Mock para os repositórios de transações
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _transactionRepositoryMockForTransactionService = new Mock<ITransactionRepository>();

            // Criamos um Circuit Breaker real, mas controlado para os testes
            _circuitBreakerPolicy = Policy
                .Handle<Exception>()
                .CircuitBreakerAsync(2, TimeSpan.FromSeconds(10)); // Abre após 2 falhas, fecha após 10s

            // Instância do ConsolidationService com o Circuit Breaker real
            _consolidationService = new ConsolidationService(_transactionRepositoryMock.Object, _circuitBreakerPolicy);

            // Instância do TransactionService
            var mapperMock = new Mock<IMapper>();
            _transactionService = new TransactionService(_transactionRepositoryMockForTransactionService.Object, mapperMock.Object);
        }

        [Fact]
        public async Task GetDailyConsolidationAsync_ShouldHandleCircuitBreakerFailure()
        {
            // Arrange: Simula falha no repositório (2 vezes para abrir o Circuit Breaker)
            _transactionRepositoryMock
                .Setup(repo => repo.GetByDateAsync(It.IsAny<DateTime>()))
                .ThrowsAsync(new Exception("Erro de transação"));

            // Act: Chama duas vezes para acionar o Circuit Breaker
            await Assert.ThrowsAsync<Exception>(() => _consolidationService.GetDailyConsolidationAsync(DateTime.Now));
            await Assert.ThrowsAsync<Exception>(() => _consolidationService.GetDailyConsolidationAsync(DateTime.Now));

            // Terceira chamada deve falhar diretamente com BrokenCircuitException
            await Assert.ThrowsAsync<BrokenCircuitException>(() => _consolidationService.GetDailyConsolidationAsync(DateTime.Now));
        }

        [Fact]
        public async Task GetDailyConsolidationAsync_ShouldReturnCorrectData_WhenServiceIsAvailable()
        {
            // Arrange: Simula transações normais
            var date = DateTime.UtcNow;
            var transactions = new List<Transaction>()
            {
                new Transaction { Type = TransactionType.Credit, Amount = 100 },
                new Transaction { Type = TransactionType.Debit, Amount = 50 }
            };

            _transactionRepositoryMock
                .Setup(repo => repo.GetByDateAsync(date))
                .ReturnsAsync(transactions);

            // Act: Chama o método de consolidação
            var result = await _consolidationService.GetDailyConsolidationAsync(date);

            // Assert: Verifica se os valores retornados são corretos
            Assert.Equal(date, result.Date);
            Assert.Equal(100, result.TotalCredits);
            Assert.Equal(50, result.TotalDebits);
            Assert.Equal(50, result.Balance);
        }


        [Fact]
        public async Task AddTransactionAsync_ShouldAddTransactionSuccessfully()
        {
            // Arrange
            var transactionDto = new TransactionDTO
            {
                Amount = 100,
                Type = "Credit",
                Date = DateTime.Now
            };

            _transactionRepositoryMockForTransactionService
                .Setup(repo => repo.AddAsync(It.IsAny<Transaction>()))
                .Returns(Task.CompletedTask);

            // Act
            await _transactionService.AddTransactionAsync(transactionDto);

            // Assert
            _transactionRepositoryMockForTransactionService.Verify(
                repo => repo.AddAsync(It.IsAny<Transaction>()),
                Times.Once
            );
        }
    }
}
