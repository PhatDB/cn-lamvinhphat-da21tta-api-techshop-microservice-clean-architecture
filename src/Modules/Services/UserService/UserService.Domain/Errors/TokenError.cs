using BuildingBlocks.Error;

namespace UserService.Domain.Errors
{
    public static class TokenError
    {
        public static readonly Error InvalidToken =
            Error.Validation("Token.Invalid", "The provided token is invalid.");

        public static Error TokenGenerationFailed(string message)
        {
            return Error.Failure("Token.GenerationFailed",
                $"Token generation failed: {message}");
        }

        public static Error TokenValidationFailed(string message)
        {
            return Error.Failure("Token.ValidationFailed",
                $"Token validation failed: {message}");
        }

        public static Error RefreshTokenGenerationFailed(string message)
        {
            return Error.Failure("Token.RefreshGenerationFailed",
                $"Refresh token generation failed: {message}");
        }
    }
}