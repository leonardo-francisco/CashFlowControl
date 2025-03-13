using CashFlowControl.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowControl.Application.Contracts
{
    public interface IConsolidationService
    {
        Task<ConsolidationDTO> GetDailyConsolidationAsync(DateTime date);
    }
}
