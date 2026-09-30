using ECommerce.API.Constants;
using ECommerce.API.Contracts.Requests.Auth;
using ECommerce.API.Contracts.Responses;
using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.UseCases.Common.Options;
using ECommerce.UseCases.Features.Auth.Commands.ConfirmEmail;
using ECommerce.UseCases.Features.Auth.Commands.Login;
using ECommerce.UseCases.Features.Auth.Commands.RefreshToken;
using ECommerce.UseCases.Features.Auth.Commands.Register;
using ECommerce.UseCases.Features.Auth.Commands.ResendOtp;
using ECommerce.UseCases.Features.Auth.Commands.RevokeToken;
using ECommerce.UseCases.Features.Auth.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ECommerce.API.Controllers;

/// <summary>
/// Manages authentication, registration, and session control endpoints.
/// </summary>
public class AuthController(ISender sender, IOptions<JwtSettings> options) : ApiControllerBase
{
    private readonly JwtSettings _jwtSettings = options.Value;


    /// <summary>
    /// Initiates the user registration process by creating a pending user record and sending an OTP to the provided email.
    /// </summary>
    /// <param name="request">The payload containing the registration details including email, password, and name.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response indicating the registration was initiated successfully.</returns>
    /// <response code="200">The registration was initiated successfully and an OTP was sent.</response>
    /// <response code="400">The request payload is invalid or malformed.</response>
    /// <response code="409">A user with the specified email already exists.</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Register(
        [FromBody] RegisterCommand request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(request, ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(AuthMessages.RegistrationInitiatedSuccessfully);
    }

    /// <summary>
    /// Confirms a user's email and activates the account using the OTP sent during registration.
    /// </summary>
    /// <param name="request">The payload containing the email and the verification code.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response indicating the email was confirmed successfully.</returns>
    /// <response code="200">The email was confirmed and the account is activated.</response>
    /// <response code="400">The verification code is invalid, or the request payload is malformed.</response>
    /// <response code="404">No pending registration was found for the specified email.</response>
    /// <response code="409">The email has already been confirmed.</response>
    [HttpPost("confirm-email")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ConfirmEmail(
        [FromBody] ConfirmEmailCommand request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(request, ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(AuthMessages.EmailConfirmedSuccessfully);
    }

    /// <summary>
    /// Resends the verification OTP to the user's email with time-based rate limiting to prevent spam.
    /// </summary>
    /// <param name="request">The payload containing the email address of the pending registration.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response indicating the new OTP was resent successfully.</returns>
    /// <response code="200">A new OTP was successfully sent to the email.</response>
    /// <response code="400">The request payload is invalid or the OTP request limit was exceeded.</response>
    /// <response code="404">No pending registration was found for the specified email.</response>
    /// <response code="409">The email has already been confirmed.</response>
    [HttpPost("resend-otp")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ResendOtp(
        [FromBody] ResendOtpCommand request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(request, ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(AuthMessages.OtpResentSuccessfully);
    }

    /// <summary>
    /// Authenticates a user, establishes a new session, and sets secure HttpOnly cookies for web clients.
    /// </summary>
    /// <param name="request">The payload containing the user's email and password.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The authentication tokens and expiration details.</returns>
    /// <response code="200">The user was successfully authenticated and tokens were generated.</response>
    /// <response code="400">The credentials are invalid, or the email is not confirmed.</response>
    /// <response code="404">The user account was not found.</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Login(
        [FromBody] LoginCommand request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(request, ct);

        if (result.IsFailure)
            return Problem(result);

        AddRefreshTokenCookie(result.Value.RefreshToken);
        AddAccessTokenCookie(result.Value.AccessToken, result.Value.AccessTokenExpiration);

        return Success(result.Value, AuthMessages.LoginSuccessful);
    }

    /// <summary>
    /// Issues a new Access Token using a valid Refresh Token (reads from body first, then fallback to secure cookie).
    /// </summary>
    /// <param name="request">The payload containing the refresh token (optional, mainly for mobile clients).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>New authentication tokens and expiration details.</returns>
    /// <response code="200">The session was successfully refreshed and new tokens were issued.</response>
    /// <response code="400">The request payload is invalid or the refresh token is missing.</response>
    /// <response code="401">The refresh token is invalid, expired, or revoked.</response>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<AuthResponse>>> Refresh(
            [FromBody] RefreshTokenRequest request,
            CancellationToken ct = default)
    {
        var token = request.Token ?? Request.Cookies["X-Refresh-Token"];

        if (string.IsNullOrWhiteSpace(token))
            return Problem(Result.Failure(AuthErrors.TokenMissing));

        var result = await sender.Send(new RefreshTokenCommand(token), ct);

        if (result.IsFailure)
        {
            DeleteRefreshTokenCookie();
            DeleteAccessTokenCookie();
            return Problem(result);
        }

        AddRefreshTokenCookie(result.Value.RefreshToken);
        AddAccessTokenCookie(result.Value.AccessToken, result.Value.AccessTokenExpiration);

        return Success(result.Value, AuthMessages.TokenRefreshedSuccessfully);
    }


    /// <summary>
    /// Revokes the current session, signs the user out, and clears the authentication cookies.
    /// </summary>
    /// <param name="request">The payload containing the refresh token to revoke (optional).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response indicating successful logout.</returns>
    /// <response code="200">The session was successfully revoked and cookies were cleared.</response>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Logout(
            [FromBody] RefreshTokenRequest request,
            CancellationToken ct = default)
    {
        var token = request.Token ?? Request.Cookies["X-Refresh-Token"];

        if (!string.IsNullOrWhiteSpace(token))
        {
            await sender.Send(new RevokeTokenCommand(token), ct);
        }

        DeleteRefreshTokenCookie();
        DeleteAccessTokenCookie();

        return Success(AuthMessages.LogoutSuccessful);
    }

    private void AddAccessTokenCookie(string accessToken, DateTimeOffset accessTokenExpiration)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Path = "/",
            Expires = accessTokenExpiration
        };

        Response.Cookies.Append("X-Access-Token", accessToken, cookieOptions);
    }
    private void AddRefreshTokenCookie(string refreshToken)
    {
        var version = HttpContext.GetRouteValue("version")?.ToString() ?? "1";

        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,

            Path = $"/api/v{version}/auth/refresh",

            Expires = DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays)
        };

        Response.Cookies.Append("X-Refresh-Token", refreshToken, cookieOptions);
    }
    private void DeleteAccessTokenCookie()
    {
        Response.Cookies.Delete("X-Access-Token", new CookieOptions { Path = "/" });
    }
    private void DeleteRefreshTokenCookie()
    {
        var version = HttpContext.GetRouteValue("version")?.ToString() ?? "1";

        Response.Cookies.Delete("X-Refresh-Token", new CookieOptions
        {
            Path = $"/api/v{version}/auth/refresh",
            Secure = true,
            HttpOnly = true,
            SameSite = SameSiteMode.Strict
        });
    }
}