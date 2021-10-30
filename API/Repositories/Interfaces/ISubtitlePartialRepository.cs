using API.Models;

namespace API.Repositories.Interfaces;

public interface ISubtitlePartialRepository
{
    SubtitlePartial Create(SubtitlePartial entity);
    void Delete(Guid id);
    void Delete(SubtitlePartial subtitlePartial);
    SubtitlePartial? GetByID(Guid id);
}
