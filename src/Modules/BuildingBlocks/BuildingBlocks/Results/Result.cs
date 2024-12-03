using System.Diagnostics.CodeAnalysis;

namespace BuildingBlocks.Results
{
    public class Result
    {
        public Result(bool isSuccess, Error.Error error)
        {
            if ((isSuccess && error != BuildingBlocks.Error.Error.None) || (!isSuccess && error == BuildingBlocks.Error.Error.None))
                throw new ArgumentException("Invalid error", nameof(error));

            IsSuccess = isSuccess;
            Error = error;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public Error.Error Error { get; }

        public static Result Success()
        {
            return new Result(true, BuildingBlocks.Error.Error.None);
        }

        public static Result<TValue> Success<TValue>(TValue value)
        {
            return new Result<TValue>(value, true, BuildingBlocks.Error.Error.None);
        }

        public static Result Failure(Error.Error error)
        {
            return new Result(false, error);
        }

        public static Result<TValue> Failure<TValue>(Error.Error error)
        {
            return new Result<TValue>(default, false, error);
        }
    }

    public class Result<TValue> : Result
    {
        private readonly TValue? _value;

        public Result(TValue? value, bool isSuccess, Error.Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        [NotNull]
        public TValue Value => IsSuccess ? _value! : throw new InvalidOperationException("The value of a failure result can't be accessed.");

        public static implicit operator Result<TValue>(TValue? value)
        {
            return value is not null ? Success(value) : Failure<TValue>(BuildingBlocks.Error.Error.NullValue);
        }

        public static Result<TValue> ValidationFailure(Error.Error error)
        {
            return new Result<TValue>(default, false, error);
        }
    }
}