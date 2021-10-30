using API.Models;
using API.Models.DTOs;

namespace API.Services.Interfaces;

public interface IUserService
{
    User? GetByIdentityID(Guid id);
    User? GetByName(string name);

    Task<User> Create(UserDTO userDTO, Guid identityID);
    Task Update(UserDTO userDTO, Guid identityID);
    bool ExistsName(string name);
}
