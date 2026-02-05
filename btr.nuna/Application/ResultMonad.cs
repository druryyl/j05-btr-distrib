using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.nuna.Application
{
    public readonly struct Result<T>
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }
        public T Value { get; }

        private Result(bool isSuccess, T value, string error)
        {
            IsSuccess = isSuccess;
            Value = value;
            Error = error;
        }

        public static Result<T> Success(T value)
            => new Result<T>(true, value, string.Empty);

        public static Result<T> Failure(string error)
            => new Result<T>(false, default, error);

        public Result<TU> Map<TU>(Func<T, TU> func)
            => IsSuccess
                ? Result<TU>.Success(func(Value))
                : Result<TU>.Failure(Error);

        public Result<TU> Bind<TU>(Func<T, Result<TU>> func)
            => IsSuccess ? func(Value) : Result<TU>.Failure(Error);
    }

    public readonly struct Result
    {
        public bool IsSuccess { get; }
        public bool IsFailure => !IsSuccess;
        public string Error { get; }

        private Result(bool isSuccess, string error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, string.Empty);
        public static Result Failure(string error) => new Result(false, error);
    }

    public static class ResultExtensions
    {
        public static Result<T> OnSuccess<T>(this Result<T> result, Action<T> action)
        {
            if (result.IsSuccess)
                action(result.Value);
            return result;
        }

        public static Result<T> OnFailure<T>(this Result<T> result, Action<string> action)
        {
            if (result.IsFailure)
                action(result.Error);
            return result;
        }
    }
}
