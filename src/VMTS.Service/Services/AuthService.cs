using System.IdentityModel.Tokens.Jwt;
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
        
        var domain = "vtms.com";
        
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

    #region Password
    
    public async Task<string> GenerateSecurePasswordAsync(int length)
    {
        const string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        const string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        const string numbers = "0123456789";
        const string symbols = "!@#$%^&*()_+-=[]{}|;:,.<>?";
        
        string allCharacters = lowercaseLetters + uppercaseLetters + numbers + symbols;
        
        StringBuilder password = new StringBuilder();

        // Ensure at least one character from each set
        password.Append(lowercaseLetters[_random.Next(lowercaseLetters.Length)]);
        password.Append(uppercaseLetters[_random.Next(uppercaseLetters.Length)]);
        password.Append(numbers[_random.Next(numbers.Length)]);
        password.Append(symbols[_random.Next(symbols.Length)]);

        // Fill the rest of the password with random characters
        for (int i = 4; i < length; i++)
        {
            password.Append(allCharacters[_random.Next(allCharacters.Length)]);
        }

        // Shuffle the password to randomize the order of characters
        string shuffledPassword = new string(password.ToString().OrderBy(c => _random.Next()).ToArray());

        // Simulate an asynchronous operation (e.g., fetching data or logging)
        await Task.Delay(100); // Simulate a 100ms delay

        return shuffledPassword;
    }
    
    

    #endregion
    

    public async Task<string> CreateTokenAsync(AppUser user, UserManager<AppUser> userManager)
    {
        #region Claims

        var authClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email , user.Email),
            new Claim(ClaimTypes.GivenName , user.UserName)
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
            signingCredentials: new SigningCredentials(secretKey , SecurityAlgorithms.HmacSha256Signature)
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

        #endregion
    }
}