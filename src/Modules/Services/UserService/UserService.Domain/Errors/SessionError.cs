using BuildingBlocks.Error;

namespace UserService.Domain.Errors
{
    public static class SessionError
    {
        public static readonly Error SessionNotFound =
            Error.NotFound("Session.NotFound", "Session does not exist.");

        public static Error SessionRetrievalFailed(string message)
        {
            return Error.Failure("Session.RetrievalFailed",
                $"Failed to retrieve session: {message}");
        }

        public static Error SessionDeletionFailed(string message)
        {
            return Error.Failure("Session.DeletionFailed",
                $"Failed to delete session: {message}");
        }

        public static Error SessionSaveFailed(string message)
        {
            return Error.Failure("Session.SaveFailed",
                $"Failed to save session: {message}");
        }
    }
}