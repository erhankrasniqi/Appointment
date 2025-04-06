using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string Message { get; private set; }

        public static Result Success() => new Result { IsSuccess = true };
        public static Result Failure(string message) => new Result { IsSuccess = false, Message = message };
    }
}
