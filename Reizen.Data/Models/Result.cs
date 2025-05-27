using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reizen.Data.Models
{
    public class Result<T>
    {
        public bool IsSuccessful
        {
            get;
        }
        public T? Value
        {
            get;
        }
        public string? Error
        {
            get;
        }
        private Result (bool isSuccess, T? value, string? error = null)
        {
            IsSuccessful = isSuccess;
            Value = value;
            Error = error;
        }
        public static Result<T> Success (T value) => new(true, value);
        public static Result<T> Failure (string error) => new(false, default, error);
    }

}
