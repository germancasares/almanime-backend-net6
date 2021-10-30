using API.Models;
using API.Models.DTOs;

namespace API.Services.Interfaces;

public interface ISubtitleService
{
    Task<Subtitle> Create(SubtitleDTO subtitleDTO, Guid identityID);
    Subtitle? GetByID(Guid ID);
}
