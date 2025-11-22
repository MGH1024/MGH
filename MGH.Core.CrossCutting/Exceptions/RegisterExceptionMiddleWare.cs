using Microsoft.AspNetCore.Builder;
using MGH.Core.CrossCutting.Exceptions.MiddleWares;

namespace MGH.Core.CrossCutting.Exceptions;

public static class RegisterExceptionMiddleWare
{
    public static void UseExceptionMiddleWare(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();    
    }
}