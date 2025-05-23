using BuildingBlocks.Error;

namespace CustomerService.Domain.Errors
{
    public static class CustomerError
    {
        public static readonly Error CustomerNotFound = Error.NotFound("Customer.NotFound", "Customer not found.");

        public static readonly Error CustomerNameEmpty =
            Error.Validation("Customer.UsernameEmpty", "CustomerName cannot be empty.");

        public static readonly Error EmailEmpty = Error.Validation("Customer.EmailEmpty", "Email cannot be empty.");

        public static readonly Error EmailInvalidFormat =
            Error.Validation("Customer.EmailInvalidFormat", "Email format is invalid.");

        public static readonly Error CustomerAlreadyExists =
            Error.Validation("Customer.UserAlreadyExists", "Email is already in use.");

        public static readonly Error PasswordEmpty =
            Error.Validation("Customer.PasswordEmpty", "Password cannot be empty.");

        public static readonly Error PasswordWeak = Error.Validation("Customer.PasswordWeak",
            "Password must be at least 8 characters long and contain numbers and special characters.");

        public static readonly Error CustomerInactive = Error.Conflict("Customer.Inactive", "Customer is inactive.");

        public static readonly Error CustomerAlreadyActive =
            Error.Conflict("Customer.AlreadyActive", "Customer is already active.");

        public static readonly Error PhoneNumberEmpty =
            Error.Validation("Customer.PhoneNumberEmpty", "Phone number cannot be empty.");

        public static readonly Error PhoneNumberInvalid =
            Error.Validation("Customer.PhoneNumberInvalid", "Phone number is invalid.");

        public static readonly Error InvalidCredentials =
            Error.Validation("Customer.InvalidCredentials", "Customer is invalid credentials.");

        public static readonly Error IncorrectOldPassword =
            Error.Validation("Customer.IncorrectOldPassword", "Old password is incorrect.");

        public static readonly Error NoChangeDetected =
            Error.Validation("Customer.NoChangeDetected", "No change detected.");

        public static readonly Error AddressAlreadyExists =
            Error.Validation("Customer.AddressAlreadyExists", "Address already exists.");

        public static readonly Error AddressNotFound =
            Error.Validation("Customer.AddressNotFound", "Address not found.");
    }
}