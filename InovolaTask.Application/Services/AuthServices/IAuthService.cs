using InovolaTask.Application.Dto;
using InovolaTask.Application.Helper;

namespace InovolaTask.Application.Services.AuthServices;

public interface IAuthService
{
    Task<GeneralResponse> Register(UserRegistrationDto registrationDto);
    Task<GeneralResponse> Login(UserLoginDto loginDto);
}
