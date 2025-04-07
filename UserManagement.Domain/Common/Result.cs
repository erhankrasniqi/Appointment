

namespace UserManagement.Domain.Common
{
    public class Result
    {
        public bool IsSuccess { get; private set; }
        public string ErrorMessage { get; private set; }
        public object Data { get; private set; }

        public static Result Success(object data)
        {
            return new Result { IsSuccess = true, Data = data };
        }

        public static Result Failure(string errorMessage)
        {
            return new Result { IsSuccess = false, ErrorMessage = errorMessage };
        }
    }
}
