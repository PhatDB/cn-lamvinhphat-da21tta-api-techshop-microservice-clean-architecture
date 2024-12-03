using BuildingBlocks.Error;
using BuildingBlocks.Results;

namespace BuildingBlocks.Extensions
{
    public sealed record ValidationError : Error.Error
    {
        public ValidationError(Error.Error[] errors) : base("Validation.General", "One or more validation errors occurred", ErrorType.Validation)
        {
            Errors = errors;
        }

        public Error.Error[] Errors { get; }

        public static ValidationError FromResults(IEnumerable<Result> results)
        {
            return new ValidationError(results.Where(r => r.IsFailure).Select(r => r.Error).ToArray());
        }
    }
}