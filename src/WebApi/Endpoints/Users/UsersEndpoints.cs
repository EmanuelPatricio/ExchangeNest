using Application.Users.Delete;
using Application.Users.Get;
using Application.Users.Update;
using Carter;
using Domain.Shared;
using Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using static Domain.Shared.Enums;

namespace WebApi.Endpoints.Users;
public class UsersEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/user").WithTags("User").RequireAuthorization();

        group.MapGet("/{userId:int}", GetById);
        group.MapGet("/{userEmail}", GetByEmail);
        group.MapGet("", GetAll);
        group.MapPut("", Update);
        group.MapDelete("/{userId:int}", Delete);
    }

    public static async Task<Results<Ok<GetUserResponse>, NotFound, BadRequest<string>>> GetById(int userId, IUserRepository userRepository)
    {
        try
        {
            var user = await userRepository.GetById(new UserId(userId));

            if (user is null)
            {
                return TypedResults.NotFound();
            }

            var response = new GetUserResponse(user.Id.Value, user.FirstName.Value, user.LastName.Value, user.NIC.Value, user.Email.Value, user.BirthDate.Value, user.RoleId, user.StatusId, user.OrganizationId, user.CountryId, user.ImageUrl?.Value);

            return TypedResults.Ok(response);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static async Task<Results<Ok<GetUserResponse>, UnprocessableEntity<string>, NotFound, BadRequest<string>>> GetByEmail(string userEmail, IUserRepository userRepository)
    {
        try
        {
            var emailToValidate = Email.Create(userEmail);

            if (emailToValidate.IsFailure)
            {
                return TypedResults.UnprocessableEntity(emailToValidate.Error.Name);
            }

            var email = emailToValidate.Value;

            if (await userRepository.IsEmailUniqueAsync(email))
            {
                return TypedResults.UnprocessableEntity(UserErrors.NotFoundEmail.Name);
            }

            var user = await userRepository.GetByEmail(email);

            if (user is null)
            {
                return TypedResults.NotFound();
            }

            var response = new GetUserResponse(user.Id.Value, user.FirstName.Value, user.LastName.Value, user.NIC.Value, user.Email.Value, user.BirthDate.Value, user.RoleId, user.StatusId, user.OrganizationId, user.CountryId, user.ImageUrl?.Value);

            return TypedResults.Ok(response);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static async Task<Results<Ok<List<GetUserResponse>>, NotFound, BadRequest<string>>> GetAll(IUserRepository userRepository)
    {
        try
        {
            var usersResponse = new List<GetUserResponse>();

            var usersList = await userRepository.GetAll();

            if (usersList is null || usersList.Count == 0)
            {
                return TypedResults.NotFound();
            }

            usersList = usersList.Where(x => x.StatusId != (int)Statuses.Deleted).ToList();

            foreach (var user in usersList)
            {
                var response = new GetUserResponse(user.Id.Value, user.FirstName.Value, user.LastName.Value, user.NIC.Value, user.Email.Value, user.BirthDate.Value, user.RoleId, user.StatusId, user.OrganizationId, user.CountryId, user.ImageUrl?.Value);

                usersResponse.Add(response);
            }

            return TypedResults.Ok(usersResponse);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static async Task<Results<Ok, UnprocessableEntity<string>, BadRequest<string>>> Update(UpdateUserRequest request,
        ISender sender)
    {
        try
        {
            var command = new UpdateUserCommand(
                request.Id,
                request.FirstName,
                request.LastName,
                request.Nic,
                request.Email,
                request.BirthDate,
                request.ImageUrl,
                request.StatusId,
                request.RoleId,
                request.CountryId);

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

    public static async Task<Results<Ok, UnprocessableEntity<string>, BadRequest<string>>> Delete(int userId, ISender sender)
    {
        try
        {
            var command = new DeleteUserCommand(userId);

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
