using CashFlowControl.Api.Model;
using CashFlowControl.Application.Contracts;
using CashFlowControl.Application.DTOs;
using FluentValidation;

namespace CashFlowControl.Api.Configuration
{
    public static class EndpointsConfiguration
    {
        public static void RegisterEndpoints(this IEndpointRouteBuilder app)
        {
            // Login para obter o Token
            app.MapPost("/login", (UserLogin user, IConfiguration config) =>
            {
                if (user.Username == "admin" && user.Password == "123456")
                {
                    var token = GenerateJwtToken.GenerateToken(user.Username);
                    return Results.Ok(new { token });
                }
                return Results.Unauthorized();
            });

            // Criar uma transação (débito ou crédito)
            app.MapPost("/transactions", async (ITransactionService transactionService, IValidator<TransactionDTO> validator, TransactionDTO transactionDto) =>
            {
                var validationResult = await validator.ValidateAsync(transactionDto);
                if (!validationResult.IsValid)
                {
                    return Results.BadRequest(validationResult.Errors);
                }

                await transactionService.AddTransactionAsync(transactionDto);
                return Results.Created($"/transactions/{transactionDto.Id}", transactionDto);
            }).RequireAuthorization();

            // Obter a transacao conforme data
            app.MapGet("/transactions/{date:datetime}", async (ITransactionService transactionService, DateTime date) =>
            {
                var transactions = await transactionService.GetTransactionsByDateAsync(date);
                return transactions.Any() ? Results.Ok(transactions) : Results.NotFound("Nenhuma transação encontrada para essa data.");
            }).RequireAuthorization();

            // Obter o consolidado diário
            app.MapGet("/consolidation/{date:datetime}", async (IConsolidationService consolidationService, DateTime date) =>
            {
                var result = await consolidationService.GetDailyConsolidationAsync(date);
                return Results.Ok(result);
            }).RequireAuthorization();
        }
    }
}
