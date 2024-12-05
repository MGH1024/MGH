using MGH.Core.CrossCutting.Exceptions.MiddleWares;
using Microsoft.AspNetCore.Builder;

namespace MGH.Core.CrossCutting.Exceptions;

public static class RegisterExceptionMiddleWare
{
    public static void UseExceptionMiddleWare(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();    
    }
}