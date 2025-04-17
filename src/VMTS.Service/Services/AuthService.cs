﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using VMTS.Core.Entities.Identity;
using VMTS.Core.ServicesContract;

namespace VMTS.Service.Services;

public class AuthService : IAuthService
{
    private readonly IConfiguration _configuration;
    private static readonly Random _random = new Random();

    public AuthService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    #region Email

    public async Task<string> GenerateUniqueEmailAsync(string firstName, string lastName)
    {
        
        var domain = "veemanage.com";
        
        var cleanFirstName = firstName.Replace(" ", "").ToLower();
        var cleanLastName = lastName.Replace(" ", "").ToLower();
    
        char[] symbols = { '_', '.', '=' , '*' , '-', '~' , '^', '$' , '#' };
        
        var randomSymbol = symbols[_random.Next(symbols.Length)];
        var randomChars = GenerateRandomCharacters(2); 
        
        string email = $"{cleanFirstName}{randomSymbol}{randomChars}{cleanLastName}@{domain}";

        return email;
        
    }
    private static string GenerateRandomCharacters(int length)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyz";
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < length; i++)
        {
            sb.Append(chars[_random.Next(chars.Length)]);
        }
        return sb.ToString();
    }

    #endregion

    
    

    public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
    {
       
        #region Claims

        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), 
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.UserName)
        };
    
        var userRoles = await userManager.GetRolesAsync(user);
        foreach (var role in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        #endregion

        
    

        #region SecretKey

        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:secretkey"]));

        #endregion

        #region Generate Token

        var token = new JwtSecurityToken(
            audience: _configuration["JWT:ValidAudience"],
            issuer: _configuration["JWT:ValidIssuer"],
            expires: DateTime.UtcNow.AddDays(double.Parse(_configuration["JWT:Expires"])),
            claims: authClaims,
            signingCredentials: new SigningCredentials(secretKey , SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

        #endregion
    }
}