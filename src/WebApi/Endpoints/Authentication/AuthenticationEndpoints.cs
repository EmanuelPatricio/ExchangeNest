using Application.Shared.Queries.GetNewId;
using Application.Users.ChangePassword;
using Application.Users.Create;
using Application.Users.LogIn;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace WebApi.Endpoints.Authentication;

public class AuthenticationEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/auth").WithTags("Authentication").AllowAnonymous();

        group.MapPost("/signup", SignUp);
        group.MapPost("/signin", SignIn);
        group.MapPost("/forgot-password", SendForgotPasswordMail);
        group.MapPut("/change-password", ChangePassword);
    }

    public static async Task<Results<Created, UnprocessableEntity<string>, BadRequest<string>>> SignUp(SignUpRequest request, ISender sender)
    {
        try
        {
            var query = new GetNewIdQuery("Id", "Users");

            var id = await sender.Send(query);

            if (id is null)
            {
                return TypedResults.UnprocessableEntity("Couldn't get id for the new user");
            }

            var command = new RegisterUserCommand(
                id.Value,
                request.FirstName,
                request.LastName,
                request.Nic,
                request.Email,
                request.Password,
                request.BirthDate,
                request.ImageUrl,
                request.RoleId,
                request.StatusId,
                request.OrganizationId,
                request.CountryId,
                request.Token);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return TypedResults.UnprocessableEntity(result.Error.Name);
            }

            return TypedResults.Created();
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static async Task<Results<Ok<LogInUserResponse>, UnauthorizedHttpResult, BadRequest<string>>> SignIn(SignInRequest request, ISender sender, CancellationToken cancellationToken)
    {
        try
        {
            var command = new LogInUserCommand(request.Email, request.Password);

            var result = await sender.Send(command, cancellationToken);

            if (result.IsFailure)
            {
                return TypedResults.Unauthorized();
            }

            return TypedResults.Ok(result.Value);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static async Task<Results<Ok, UnprocessableEntity<string>, BadRequest<string>>> SendForgotPasswordMail(ForgotPasswordRequest request, ISender sender)
    {
        try
        {
            var command = new ForgotForgotPasswordCommand(request.Email, request.Url);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return TypedResults.UnprocessableEntity(result.Error.Name);
            }

            return TypedResults.Ok();
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static async Task<Results<Ok, BadRequest<string>, UnprocessableEntity<string>>> ChangePassword(ChangeUserPasswordRequest request, ISender sender)
    {
        try
        {
            var command = new ChangePasswordUserCommand(request.Token, request.NewPassword);

            var result = await sender.Send(command);

            if (result.IsFailure)
            {
                return TypedResults.UnprocessableEntity(result.Error.Name);
            }

            return TypedResults.Ok();
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }
}
