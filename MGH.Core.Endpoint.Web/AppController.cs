using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using MGH.Core.Infrastructure.Securities.Security.Extensions;

namespace MGH.Core.Endpoint.Web;

public abstract class AppController(ISender sender, IHostingEnvironment env) : ControllerBase
{
    protected readonly ISender Sender = sender;
    protected readonly IHostingEnvironment Env = env;

    /// <summary>
    /// get current environment
    /// </summary>
    /// <returns>get current environment</returns>
    [HttpGet("env")]
    public IActionResult GetEnv()
    {
        return Ok(Env.EnvironmentName);
    }

    /// <summary>
    /// Retrieves the client's IP address from the current HTTP request.
    /// </summary>
    /// <remarks>
    /// This method first checks for the <c>X-Forwarded-For</c> header, which is commonly used when the 
    /// application is behind a reverse proxy or load balancer. If found, it returns the first IP address 
    /// in the list (the original client IP). If the header is not present, it falls back to the remote 
    /// IP address obtained from the connection.
    /// </remarks>
    /// <returns>
    /// The client's IP address as a <see cref="string"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the client's IP address cannot be determined from either the <c>X-Forwarded-For</c> header 
    /// or the remote connection.
    /// </exception>
    protected string GetClientIpAddress()
    {
        if (Request.Headers.TryGetValue("X-Forwarded-For", out var forwardedHeader) &&
            !string.IsNullOrWhiteSpace(forwardedHeader))
        {
            return forwardedHeader.ToString().Split(',')[0].Trim();
        }

        var remoteIp = HttpContext.Connection.RemoteIpAddress?.MapToIPv4()?.ToString();
        if (string.IsNullOrWhiteSpace(remoteIp))
            throw new InvalidOperationException("Unable to determine client IP address from the request.");

        return remoteIp;
    }

    /// <summary>
    /// Retrieves the authenticated user's ID from the current HTTP request context.
    /// </summary>
    /// <remarks>
    /// This method validates that the user is authenticated and then extracts the user ID 
    /// from the claims principal using the <c>GetUserId()</c> extension method. 
    /// It throws exceptions when the user is not authenticated or the user ID is invalid.
    /// </remarks>
    /// <returns>
    /// The authenticated user's ID as an <see cref="int"/>.
    /// </returns>
    /// <exception cref="UnauthorizedAccessException">
    /// Thrown when no authenticated user is found in the current HTTP context.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the retrieved user ID is missing, invalid, or non-positive.
    /// </exception>
    protected int GetUserIdFromRequest()
    {
        if (HttpContext?.User == null || !(HttpContext.User.Identity?.IsAuthenticated ?? false))
            throw new UnauthorizedAccessException("User is not authenticated.");

        var userId = HttpContext.User.GetUserId();

        if (userId <= 0)
            throw new InvalidOperationException("Invalid user ID in authentication context.");

        return userId;
    }

    /// <summary>
    /// Sets a secure HTTP cookie in the response with the specified key, value, and expiration time.
    /// </summary>
    /// <remarks>
    /// This method creates and appends a cookie to the HTTP response using secure defaults:
    /// <list type="bullet">
    /// <item><description><see cref="CookieOptions.HttpOnly"/> is enabled to prevent client-side script access.</description></item>
    /// <item><description><see cref="CookieOptions.Secure"/> is enabled to ensure transmission only over HTTPS.</description></item>
    /// <item><description><see cref="CookieOptions.SameSite"/> is set to <c>Strict</c> to mitigate CSRF attacks.</description></item>
    /// <item><description>The cookie's path is set to the root ("/") to limit its scope appropriately.</description></item>
    /// </list>
    /// These options can be customized through parameters.
    /// </remarks>
    /// <param name="key">The unique name of the cookie.</param>
    /// <param name="value">The value to store in the cookie.</param>
    /// <param name="expires">The UTC date and time when the cookie should expire.</param>
    /// <param name="httpOnly">
    /// Optional. Indicates whether the cookie is accessible only to the server-side code. Default is <c>true</c>.
    /// </param>
    /// <param name="secure">
    /// Optional. Indicates whether the cookie should only be transmitted over HTTPS connections. Default is <c>true</c>.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the <see cref="Response"/> object is not available in the current HTTP context.
    /// </exception>
    protected void SetCookie(string key, string value, DateTime expires, bool httpOnly = true, bool secure = true)
    {
        if (Response is null)
            throw new InvalidOperationException("HTTP response is not available to set cookies.");

        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException($"Cookie '{key}' cannot have a null or empty value.", nameof(value));

        var cookieOptions = new CookieOptions
        {
            HttpOnly = httpOnly,          // Prevent client-side scripts from reading it
            Secure = secure,              // Send only over HTTPS
            SameSite = Env.IsDevelopment() ? SameSiteMode.Lax : SameSiteMode.Strict,  // Prevent cross-site request forgery (CSRF)
            Path = "/",                   // Limit cookie scope
            Expires = expires
        };

        Response.Cookies.Append(key, value, cookieOptions);
    }


    /// <summary>
    /// Retrieves the refresh token value from the request cookies using the specified cookie key.
    /// </summary>
    /// <remarks>
    /// This method attempts to extract the refresh token from the incoming HTTP request's cookies.
    /// By default, it looks for a cookie named <c>refreshToken</c>, but a custom key can be provided
    /// through the <paramref name="cookieKey"/> parameter.
    /// <para>
    /// If the cookie is missing or contains an empty value, an <see cref="InvalidOperationException"/> 
    /// is thrown to indicate that the refresh token is not available in the request context.
    /// </para>
    /// </remarks>
    /// <param name="cookieKey">
    /// Optional. The name of the cookie containing the refresh token. Defaults to <c>"refreshToken"</c>.
    /// </param>
    /// <returns>
    /// The refresh token value as a <see cref="string"/>.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the refresh token cookie is missing or its value is empty.
    /// </exception>
    protected string GetRefreshTokenFromCookies(string cookieKey = "refreshToken")
    {
        if (!Request.Cookies.TryGetValue(cookieKey, out var refreshToken) || string.IsNullOrWhiteSpace(refreshToken))
            throw new InvalidOperationException($"Refresh token cookie '{cookieKey}' is missing or empty in the request.");

        return refreshToken;
    }

}