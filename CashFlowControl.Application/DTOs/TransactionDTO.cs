using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowControl.Application.DTOs
{
    public class TransactionDTO
    {
        public string? Id { get; set; }
        public string Type { get; set; } 
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
