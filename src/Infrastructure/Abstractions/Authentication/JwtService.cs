using Application.Abstractions.Authentication;
using Domain.Abstractions;
using Domain.Shared;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Abstractions.Authentication;
internal sealed class JwtService : IJwtService
{
    private static readonly Error JwtKeyNotSpecified = new(
        "Jwt.JwtKeyNotSpecified",
        "Failed to acquire secret key for the JWT generation");

    private static readonly Error IssuerOrAudienceNotSpecified = new(
        "Jwt.AuthenticationFailed",
        "Failed to acquire issuer or audience for the JWT");

    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Result<string> GetAccessToken(int userId, int roleId)
    {
        var key = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(key))
        {
            return Result.Failure<string>(JwtKeyNotSpecified);
        }

        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        if (string.IsNullOrWhiteSpace(issuer) || string.IsNullOrWhiteSpace(audience))
        {
            return Result.Failure<string>(IssuerOrAudienceNotSpecified);
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim("userId", userId.ToString()),
            new Claim("roleId", ((Enums.Roles)roleId).ToString())
        };

        var token = new JwtSecurityToken(issuer,
                                         audience,
                                         claims,
                                         signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}