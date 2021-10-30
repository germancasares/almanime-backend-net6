using API.Models.DTOs.Account;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace API.Services.Interfaces;

public interface IAccountService
{
    Task<bool> ExistsUsername(string username);
    Task<bool> ExistsEmail(string email);
    //Task<(JwtSecurityToken token, IEnumerable<IdentityError> errors)> CreateAccount(RegisterDTO registerDTO);
    Task<JwtSecurityToken?> Login(LoginDTO loginDTO);
}
