using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowControl.Application.DTOs
{
    public class ErrorResponseDTO
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new();
    }
}
