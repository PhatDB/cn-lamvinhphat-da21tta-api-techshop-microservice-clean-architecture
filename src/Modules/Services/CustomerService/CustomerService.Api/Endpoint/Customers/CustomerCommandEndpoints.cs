using BuildingBlocks.Abstractions.Extensions;
using BuildingBlocks.Extensions;
using BuildingBlocks.Results;
using CustomerService.Application.Commands.Customers.AddCustomerAddress;
using CustomerService.Application.Commands.Customers.AddReview;
using CustomerService.Application.Commands.Customers.DeleteCustomerAddress;
using CustomerService.Application.Commands.Customers.GoogleLogin;
using CustomerService.Application.Commands.Customers.Login;
using CustomerService.Application.Commands.Customers.Register;
using CustomerService.Application.Commands.Customers.VerifyOTP;
using CustomerService.Application.DTOs;
using MediatR;

namespace CustomerService.Api.Endpoint.Customers
{
    public class CustomerCommandEndpoints : IEndpoint
    {
        public void MapEndpoint(IEndpointRouteBuilder app)
        {
            app.MapPost("/customer/register", async (RegisterCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);
                return result.Match(success => Results.Ok(new { Id = result.Value }),
                    failure => CustomResults.Problem(failure));
            }).WithName("Register").WithTags("Customer");

            app.MapPost("/customer/login", async (LoginCommand command, ISender sender, HttpResponse response) =>
            {
                Result<LoginDto> result = await sender.Send(command);

                return result.Match(success => Results.Ok(result.Value), failure => CustomResults.Problem(failure));
            }).WithName("Login").WithTags("Customer");

            app.MapPost("/customer/google-login", async (GoogleLoginCommand command, ISender sender) =>
            {
                Result<LoginDto> result = await sender.Send(command);
                return result.Match(success => Results.Ok(result.Value), failure => CustomResults.Problem(failure));
            }).WithName("GoogleLogin").WithTags("Customer");

            app.MapPost("/customer/address", async (AddCustomerAddressCommand command, ISender sender) =>
            {
                Result<int> result = await sender.Send(command);
                return result.Match(success => Results.Ok(new { Id = result.Value }),
                    failure => CustomResults.Problem(failure));
            }).WithName("AddCustomerAddress").WithTags("Customer");

            app.MapDelete("/customer/address/{addressId:int}", async (int addressId, ISender sender) =>
            {
                Result result = await sender.Send(new DeleteCustomerAddressCommand(addressId));
                return result.IsSuccess ? Results.Ok() : CustomResults.Problem(result);
            }).WithName("DeleteCustomerAddress").WithTags("Customer");

            app.MapPost("/customer/verify-otp", async (VerifyOTPCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithName("Verify-OTP").WithTags("Customer");

            app.MapPost("/customer/review", async (AddReviewCommand command, ISender sender) =>
            {
                Result result = await sender.Send(command);
                return result.Match(Results.NoContent, CustomResults.Problem);
            }).WithName("Review").WithTags("Customer");
        }
    }
}