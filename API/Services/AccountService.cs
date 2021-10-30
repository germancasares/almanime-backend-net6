using API.Models;
using API.Models.DTOs.Account;
using API.Services.Interfaces;
using API.Settings;
using API.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services;

public class AccountService : IAccountService
{
    //private readonly IUserService _userService;
    private readonly TokenSettings _tokenSettings;
    private readonly ILogger<AccountService> _logger;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    public AccountService(
        //IUserService userService,
        IOptions<TokenSettings> tokenSettings,
        ILogger<AccountService> logger,
        UserManager<User> userManager,
        SignInManager<User> signInManager
    )
    {
        //_userService = userService;
        _tokenSettings = tokenSettings.Value;
        _logger = logger;

        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<bool> ExistsUsername(string username) => await _userManager.FindByNameAsync(username) != null;

    public async Task<bool> ExistsEmail(string email) => await _userManager.FindByEmailAsync(email) != null;

    //public async Task<(JwtSecurityToken token, IEnumerable<IdentityError> errors)> CreateAccount(RegisterDTO registerDTO)
    //{
    //    var identityUser = _mapper.Map<IdentityUser>(registerDTO);

    //    var result = await _userManager.CreateAsync(identityUser, registerDTO.Password);

    //    if (!result.Succeeded)
    //    {
    //        _logger.Emit(ELoggingEvent.CantCreateAccount, new { result.Errors });
    //        return (null, result.Errors);
    //    }

    //    await _userService.Create(new UserDTO { Avatar = registerDTO.Avatar, Name = identityUser.UserName }, new Guid(identityUser.Id));

    //    await _signInManager.SignInAsync(identityUser, false);

    //    return (GenerateJwtToken(identityUser), new List<IdentityError>());
    //}

    public async Task<JwtSecurityToken?> Login(LoginDTO loginDTO)
    {
        // The user is identified either by Email or by Username
        var user = await _userManager.FindByEmailAsync(loginDTO.Identifier) ?? await _userManager.FindByNameAsync(loginDTO.Identifier);

        if (user == null) return null;

        var signInResult = await _signInManager.PasswordSignInAsync(user, loginDTO.Password, loginDTO.IsPersistent, false);

        if (!signInResult.Succeeded) return null;

        return GenerateJwtToken(user);
    }

    private JwtSecurityToken GenerateJwtToken(User user)
    {
        var claims = new List<Claim>
            {
                new Claim("username", user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                // Jwt ID
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                // Issued at
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret ?? throw new ArgumentException(ExceptionMessage.ParameterNotValid)));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(Convert.ToDouble(_tokenSettings.AccessExpirationDays));
        var notBefore = DateTime.Now;

        return new JwtSecurityToken(
            _tokenSettings.Issuer,
            _tokenSettings.Audience,
            claims,
            notBefore,
            expires,
            creds
        );
    }
}
