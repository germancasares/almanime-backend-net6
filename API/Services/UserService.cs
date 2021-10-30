using API.Models;
using API.Models.DTOs;
using API.Models.Enums;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using API.Utils;

namespace API.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserService> _logger;

    public UserService(
        IUnitOfWork unitOfWork,
        ILogger<UserService> logger
        )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public User? GetByIdentityID(Guid id) => _unitOfWork.Users.GetByID(id);
    public User? GetByName(string name) => _unitOfWork.Users.GetByName(name);
    public bool ExistsName(string name) => GetByName(name) != null;

    public Task<User> Create(UserDTO userDTO, Guid identityID)
    {
        if (_unitOfWork.Users.GetByID(identityID) != null)
        {
            _logger.Emit(ELoggingEvent.UserAlreadyExists, new { IdentityID = identityID });
            throw new ArgumentException(nameof(identityID));
        }

        return CreateInternal(userDTO, identityID);
    }
    private async Task<User> CreateInternal(UserDTO userDTO, Guid identityID)
    {
        var userEntity = new User
        {
            Id = identityID,
            UserName = userDTO.Name
        };

        var user = _unitOfWork.Users.Create(userEntity);

        // If this fails, we don't upload an avatar for nothing.
        _unitOfWork.Save();

        if (userDTO.Avatar != null)
        {
            try
            {
                user.AvatarUrl = await _unitOfWork.Storage.UploadAvatar(userDTO.Avatar, user.Id);
                _unitOfWork.Users.Update(user);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                _logger.Emit(ELoggingEvent.CantUploadAvatar, new { IdentityID = identityID, Exception = ex });
                return user;
            }
        }

        return user;
    }

    public Task Update(UserDTO userDTO, Guid identityID)
    {
        var user = _unitOfWork.Users.GetByID(identityID);
        if (user == null)
        {
            _logger.Emit(ELoggingEvent.UserDoesntExist, new { UserIdentityID = identityID });
            throw new ArgumentException(nameof(identityID));
        }

        return UpdateInternal(userDTO, user);
    }
    public async Task UpdateInternal(UserDTO userDTO, User user)
    {
        if (!string.IsNullOrWhiteSpace(userDTO.Name))
        {
            user.UserName = userDTO.Name;
            _unitOfWork.Users.Update(user);
        }

        if (userDTO.Avatar != null)
        {
            user.AvatarUrl = await _unitOfWork.Storage.UploadAvatar(userDTO.Avatar, user.Id);
            _unitOfWork.Users.Update(user);
        }

        // We do not want to Update if UserDTO is empty.

        _unitOfWork.Save();
    }
}
