using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Extras;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models.DTOs;

namespace API.Endpoints;

public static class SecurityEndpoints {
    public static void MapSecurityEndpoints(this WebApplication app) {
            app.MapPost("/api/Security/GetToken", GetToken).AllowAnonymous();
            app.MapPost("/api/Security/CreateUser", CreateUser).AllowAnonymous();
    }
    internal static async Task<IResult> GetToken(IOptions<JwtOptions> jwtOptions, UserManager<IdentityUser> userMgr,[FromBody] UserLoginDTO user) {
        var identityUser = await userMgr.FindByEmailAsync(user.Email);
        if (await userMgr.CheckPasswordAsync(identityUser, user.Password)) {
            var issuer = jwtOptions.Value.Issuer;
            var audience = jwtOptions.Value.Audience;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim> {
                new Claim(ClaimTypes.Email, identityUser.Email),
                new Claim(ClaimTypes.GivenName, identityUser.UserName)
            };;
            var token = new JwtSecurityToken(issuer: issuer, audience: audience, signingCredentials: credentials, claims: claims);
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return Results.Ok(stringToken);
        }
        else {
            return Results.Unauthorized();
        }
    } 
    internal static async Task<IResult> CreateUser(UserManager<IdentityUser> userMgr,[FromBody] UserRegisterDTO user) {
        var identityUser = new IdentityUser() {
            UserName = user.UserName,
            Email = user.Email
        };
        var result = await userMgr.CreateAsync(identityUser, user.Password);
        if (result.Succeeded) 
            return Results.Ok();
        else 
            return Results.BadRequest();
    }
}