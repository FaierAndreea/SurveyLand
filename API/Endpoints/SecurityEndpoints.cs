using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Models;
using Models.DTOs;

namespace API.Endpoints;

public static class SecurityEndpoints {
    public static void MapSecurityEndpoints(this WebApplication app) {
            app.MapPost("/api/security/getToken", GetToken).AllowAnonymous();
            app.MapPost("/api/security/createUser", CreateUser).AllowAnonymous();
        }
    //this needs a tryParse for complex types
    internal static async Task<IResult> GetToken(this WebApplicationBuilder builder, UserManager<IdentityUser> userMgr,[FromQuery] UserLoginDTO user) {
        var identityUser = await userMgr.FindByEmailAsync(user.Email);
        if (await userMgr.CheckPasswordAsync(identityUser, user.Password)) {
            var issuer = builder.Configuration["Jwt:Issuer"];
            var audience = builder.Configuration["Jwt:Audience"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
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
    internal static async Task<IResult> CreateUser(UserManager<IdentityUser> userMgr,[FromQuery] UserRegisterDTO user) {
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