using BuildingBlocks.Error;

namespace UserService.Domain.Errors
{
    public static class UserError
    {
        public static readonly Error UserNotFound = Error.NotFound("User.NotFound", "User not found.");

        public static readonly Error UsernameEmpty = Error.Validation("User.UsernameEmpty", "Username cannot be empty.");

        public static readonly Error UsernameDuplicate = Error.Validation("User.UsernameDuplicate", "Username already exists.");

        public static readonly Error EmailEmpty = Error.Validation("User.EmailEmpty", "Email cannot be empty.");

        public static readonly Error EmailInvalidFormat = Error.Validation("User.EmailInvalidFormat", "Email format is invalid.");

        public static readonly Error UserAlreadyExists = Error.Validation("User.UserAlreadyExists", "Email is already in use.");

        public static readonly Error PasswordEmpty = Error.Validation("User.PasswordEmpty", "Password cannot be empty.");

        public static readonly Error PasswordWeak = Error.Validation("User.PasswordWeak",
            "Password must be at least 8 characters long and contain numbers and special characters.");

        public static readonly Error UserInactive = Error.Conflict("User.Inactive", "User is inactive.");

        public static readonly Error UserAlreadyActive = Error.Conflict("User.AlreadyActive", "User is already active.");

        public static readonly Error UserAddressNotFound = Error.NotFound("User.AddressNotFound", "User address not found.");

        public static readonly Error UserAddressDuplicate = Error.Validation("User.AddressDuplicate", "User address already exists.");

        public static readonly Error PhoneNumberEmpty = Error.Validation("User.PhoneNumberEmpty", "Phone number cannot be empty.");

        public static readonly Error PhoneNumberInvalid = Error.Validation("User.PhoneNumberInvalid", "Phone number is invalid.");

        public static readonly Error InvalidCredentials = Error.Validation("User.InvalidCredentials", "User is invalid credentials.");

        public static readonly Error IncorrectOldPassword = Error.Validation("User.IncorrectOldPassword", "Old password is incorrect.");
    }
}