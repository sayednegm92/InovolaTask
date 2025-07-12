using InovolaTask.Application.BaseRepository;
using InovolaTask.Application.Dto;
using InovolaTask.Application.Helper;
using InovolaTask.Core.Entities;
using InovolaTask.Infrastructure.Helper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InovolaTask.Application.Services.AuthServices;

public class AuthService : IAuthService
{

    private readonly IResponseHandler _responseHandler;
    private readonly IRepositoryApp<User> _userRepo;
    private readonly JwtSettings _jwtSettings;
    public AuthService(IRepositoryApp<User> userRepo, IResponseHandler responseHandler, JwtSettings jwtSettings)
    {
        _userRepo = userRepo;
        _responseHandler = responseHandler;
        _jwtSettings = jwtSettings;

    }
    public async Task<GeneralResponse> Register(UserRegistrationDto registrationDto)
    {
        if (await _userRepo.AnyAsync(u => u.Username == registrationDto.Username))
            return _responseHandler.ShowMessage("Username already exists");

        if (await _userRepo.AnyAsync(u => u.Email == registrationDto.Email))
            return _responseHandler.ShowMessage("Email already exists");


        CreatePasswordHash(registrationDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User
        {
            Username = registrationDto.Username,
            Email = registrationDto.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _userRepo.AddAsync(user);
        return _responseHandler.Success(user);
    }

    public async Task<GeneralResponse> Login(UserLoginDto loginDto)
    {
        var user = await _userRepo.FirstOrDefaultAsync(u => u.Username == loginDto.Username);
        if (user == null)
            return _responseHandler.ShowMessage("User not found");

        if (!VerifyPasswordHash(loginDto.Password, user.PasswordHash, user.PasswordSalt))
            return _responseHandler.ShowMessage("User name Or Password Wrong !");
        var token = GenerateJwtToken(user);
        return _responseHandler.Success(new { Token = token });

    }
    #region Base Method
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email)
        };
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwtSettings.ExpirationInMinutes),
            signingCredentials: signingCredentials);

        var Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return Token;
    }

    #endregion
}
