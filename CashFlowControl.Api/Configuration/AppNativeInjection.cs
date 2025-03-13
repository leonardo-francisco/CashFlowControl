using CashFlowControl.Application.Contracts;
using CashFlowControl.Application.DTOs;
using CashFlowControl.Application.Mapping;
using CashFlowControl.Application.Services;
using CashFlowControl.Application.Validation;
using CashFlowControl.Domain.Entities;
using CashFlowControl.Domain.Interfaces;
using CashFlowControl.Infra.Context;
using CashFlowControl.Infra.Repositories;
using FluentValidation;
using System;

namespace CashFlowControl.Api.Configuration
{
    public static class AppNativeInjection
    {
        public static void RegisterServices(IServiceCollection services)
        {
            #region Services
            services.AddScoped<IConsolidationService, ConsolidationService>();
            services.AddScoped<ITransactionService, TransactionService>();          
            #endregion

            #region Repositories
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            #endregion

            #region DbContext
            services.AddSingleton<MongoDbContext>(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetValue<string>("MongoDbSettings:ConnectionString");
                var databaseName = configuration.GetValue<string>("MongoDbSettings:DatabaseName");

                return new MongoDbContext(connectionString, databaseName);
            });
            #endregion

            #region Mapper
            services.AddAutoMapper(typeof(MapperConfig));
            #endregion

            #region Validator
            services.AddScoped<IValidator<TransactionDTO>, TransactionValidator>();
            #endregion
        }
    }
}
