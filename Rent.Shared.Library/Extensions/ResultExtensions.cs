using System.Net;
using Microsoft.AspNetCore.Mvc;
using Rent.Shared.Library.Results;

namespace Rent.Shared.Library.Extensions;

public static class ResultExtensions
{
    public static ObjectResult OkOrBadRequest<T>(this Result<T> result)
    {
        return result.IsFailure ? new ObjectResult(result.Errors) { StatusCode = (int)HttpStatusCode.BadRequest } : new ObjectResult(result.Data) { StatusCode = (int)HttpStatusCode.OK };
    }
    
    public static ObjectResult OkOrBadRequest<T>(this PagedResult<T> result)
    {
        return result.IsFailure ? new ObjectResult(result.Errors) { StatusCode = (int)HttpStatusCode.BadRequest } : new ObjectResult(new { result.Items, result.Meta}) { StatusCode = (int)HttpStatusCode.OK };
    }
    
    public static ObjectResult OkOrNotFound<T>(this Result<T> result)
    {
        return result.IsFailure ? new ObjectResult(result.Errors) { StatusCode = (int)HttpStatusCode.NotFound } : new ObjectResult(result.Data) { StatusCode = (int)HttpStatusCode.OK };
    }
    
    public static ObjectResult NoContentOrNotFound(this Result result)
    {
        return result.IsFailure ? new ObjectResult(result.Errors) { StatusCode = (int)HttpStatusCode.NotFound } : new ObjectResult(null) { StatusCode = (int)HttpStatusCode.NoContent };
    }

    public static ObjectResult CreatedOrBadRequest<T>(this Result<T> result)
    {
        return result.IsFailure ? new ObjectResult(result.Errors) { StatusCode = (int)HttpStatusCode.BadRequest } : new ObjectResult(result.Data) { StatusCode = (int)HttpStatusCode.Created };
    }
}