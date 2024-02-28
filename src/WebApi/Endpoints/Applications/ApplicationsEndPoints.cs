using Application.Applications;
using Application.Applications.Cancel;
using Application.Applications.Close;
using Application.Applications.Get;
using Application.Applications.Publish;
using Application.Applications.Update;
using Application.Shared.Queries.GetNewId;
using Carter;
using Domain.Applications;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.CodeAnalysis;
using static Domain.Shared.Enums;

namespace WebApi.Endpoints.Applications;

public class ApplicationsEndPoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/applications").WithTags("Application").RequireAuthorization();

        group.MapPost("", PublishApplication);
        group.MapGet("/{applicationId:int}", GetById);
        group.MapGet("", GetAll);
        group.MapPut("", Update);
        group.MapPut("/cancel", Cancel);
        group.MapPut("/close", Close);
        group.MapDelete("/{documentId:int}", DeleteDocument);
    }

    public static async Task<Results<Created, UnprocessableEntity<string>, BadRequest<string>>> PublishApplication(PublishApplicationRequest request, ISender sender)
    {
        try
        {
            var query = new GetNewIdQuery("Id", "Applications");

            var id = await sender.Send(query);

            if (id is null)
            {
                return TypedResults.UnprocessableEntity("Couldn't get id for the new application");
            }

            query = new GetNewIdQuery("Id", "ApplicationDocuments");

            var newDocumentId = await sender.Send(query);

            if (newDocumentId is null)
            {
                return TypedResults.UnprocessableEntity("Couldn't get id for the new application document");
            }

            var applicationDocuments = new List<ApplicationDocumentValues>();
            foreach (var applicationDocument in request.ApplicationDocuments)
            {
                var applicationDocumentNewId = applicationDocuments.Count > 0 ? applicationDocuments.Last().Id : newDocumentId.Value - 1;

                applicationDocuments.Add(new ApplicationDocumentValues(++applicationDocumentNewId, applicationDocument.Category, applicationDocument.Url));
            }

            var requiredDocuments = new List<ApplicationDocumentValues>();
            foreach (var requiredDocument in request.RequiredDocuments)
            {
                var requiredDocumentNewId = applicationDocuments.Count > 0 ? applicationDocuments.Last().Id : newDocumentId.Value - 1;
                requiredDocumentNewId = requiredDocuments.Count > 0 ? requiredDocuments.Last().Id : requiredDocumentNewId;

                applicationDocuments.Add(new ApplicationDocumentValues(++requiredDocumentNewId, requiredDocument.Category, requiredDocument.Url));
            }

            var command = new PublishApplicationCommand(
                id.Value,
                request.ProgramId,
                request.StudentId,
                request.Reason,
                request.StatusId,
                applicationDocuments,
                requiredDocuments);

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

    public static async Task<Results<Ok<GetApplicationResponse>, NotFound, BadRequest<string>>> GetById(int applicationId, IApplicationRepository applicationRepository)
    {
        try
        {
            var application = await applicationRepository.GetById(new Domain.Applications.ApplicationId(applicationId));

            if (application is null)
            {
                return TypedResults.NotFound();
            }

            var response = new GetApplicationResponse(
                    application.Id.Value,
                    application.ProgramId,
                    application.StudentId,
                    application.Reason.Value,
                    application.StatusId,
                    application.Documents
                        .Where(x => x.DocumentType == (int)DocumentTypes.Application)
                        .Select(x => new { x.Id.Value, x.DocumentUrl })
                        .ToDictionary(x => x.Value, x => x.DocumentUrl),
                    application.Documents
                        .Where(x => x.DocumentType == (int)DocumentTypes.Required)
                        .Select(x => new { x.Id.Value, x.DocumentUrl })
                        .ToDictionary(x => x.Value, x => x.DocumentUrl));

            return TypedResults.Ok(response);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static async Task<Results<Ok<List<GetApplicationResponse>>, NotFound, BadRequest<string>>> GetAll(IApplicationRepository applicationRepository)
    {
        try
        {
            var applicationsResponse = new List<GetApplicationResponse>();

            var applicationsList = await applicationRepository.GetAll();

            if (applicationsList is null || applicationsList.Count == 0)
            {
                return TypedResults.NotFound();
            }

            applicationsList = applicationsList.Where(x => x.StatusId != (int)Statuses.Deleted).ToList();

            foreach (var application in applicationsList)
            {
                var response = new GetApplicationResponse(
                    application.Id.Value,
                    application.ProgramId,
                    application.StudentId,
                    application.Reason.Value,
                    application.StatusId,
                    application.Documents
                        .Where(x => x.DocumentType == (int)DocumentTypes.Application)
                        .Select(x => new { x.Id.Value, x.DocumentUrl })
                        .ToDictionary(x => x.Value, x => x.DocumentUrl),
                    application.Documents
                        .Where(x => x.DocumentType == (int)DocumentTypes.Required)
                        .Select(x => new { x.Id.Value, x.DocumentUrl })
                        .ToDictionary(x => x.Value, x => x.DocumentUrl));

                applicationsResponse.Add(response);
            }

            return TypedResults.Ok(applicationsResponse);
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }

    public static async Task<Results<Ok, UnprocessableEntity<string>, BadRequest<string>>> Update(UpdateApplicationRequest request, ISender sender)
    {
        try
        {
            var query = new GetNewIdQuery("Id", "ApplicationDocuments");

            var newDocumentId = await sender.Send(query);

            if (newDocumentId is null)
            {
                return TypedResults.UnprocessableEntity("Couldn't get id for the new application document");
            }

            var applicationDocuments = new List<ApplicationDocumentValues>();
            foreach (var applicationDocument in request.ApplicationDocuments)
            {
                if (applicationDocument.Id > 0)
                {
                    applicationDocuments.Add(applicationDocument);
                    continue;
                }

                var applicationDocumentNewId = 0;
                applicationDocumentNewId = applicationDocuments.Count > 0 ? applicationDocuments.Last().Id : applicationDocumentNewId;

                applicationDocuments.Add(new(--applicationDocumentNewId, applicationDocument.Category, applicationDocument.Url));
            }

            var requiredDocuments = new List<ApplicationDocumentValues>();
            foreach (var requiredDocument in request.RequiredDocuments)
            {
                if (requiredDocument.Id > 0)
                {
                    requiredDocuments.Add(requiredDocument);
                    continue;
                }

                var requiredDocumentNewId = 0;
                requiredDocumentNewId = applicationDocuments.Count > 0 ? applicationDocuments.Last().Id : requiredDocumentNewId;
                requiredDocumentNewId = requiredDocuments.Count > 0 ? requiredDocuments.Last().Id : requiredDocumentNewId;

                requiredDocuments.Add(new(--requiredDocumentNewId, requiredDocument.Category, requiredDocument.Url));
            }

            var command = new UpdateApplicationCommand(
                request.Id,
                request.Reason,
                request.StatusId,
                applicationDocuments,
                requiredDocuments,
                newDocumentId.Value - 1);

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

    public static async Task<Results<Ok, UnprocessableEntity<string>, BadRequest<string>>> Cancel(CancelApplicationRequest request, ISender sender)
    {
        try
        {
            var command = new CancelApplicationCommand(request.Id, request.Reason);

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

    public static async Task<Results<Ok, UnprocessableEntity<string>, BadRequest<string>>> Close(CloseApplicationRequest request, ISender sender)
    {
        try
        {
            var command = new CloseApplicationCommand(request.Id, request.Reason);

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

    public static Results<Ok, UnprocessableEntity<string>, BadRequest<string>> DeleteDocument(int documentId, IApplicationRepository applicationRepository)
    {
        try
        {
            applicationRepository.DeleteDocument(new(documentId));

            return TypedResults.Ok();
        }
        catch (Exception e)
        {
            return TypedResults.BadRequest(e.Message);
        }
    }
}
