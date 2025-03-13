using CashFlowControl.Application.DTOs;
using CashFlowControl.Domain.Entities;
using CashFlowControl.Domain.Enum;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowControl.Application.Validation
{
    public class TransactionValidator : AbstractValidator<TransactionDTO>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("O valor da transação deve ser positivo.");
            RuleFor(x => x.Type)
                .Must(type => type == "Credit" || type == "Debit")
                .WithMessage("O tipo de transação deve ser 'Credit' ou 'Debit'.");
        }
    }
}
