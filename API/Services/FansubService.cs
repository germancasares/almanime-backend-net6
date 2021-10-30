using API.Models;
using API.Models.DTOs;
using API.Models.Enums;
using API.Models.Views;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using API.Utils;
using API.Utils.Mappers;

namespace API.Services;

public class FansubService : IFansubService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<FansubService> _logger;

    public FansubService(
        IUnitOfWork unitOfWork,
        ILogger<FansubService> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public bool ExistsFullName(string fullname) => _unitOfWork.Fansubs.GetByFullName(fullname) != null;

    public bool ExistsAcronym(string acronym) => _unitOfWork.Fansubs.GetByAcronym(acronym) != null;

    public Fansub? GetByID(Guid ID) => _unitOfWork.Fansubs.GetByID(ID);

    public Fansub? GetByAcronym(string acronym) => _unitOfWork.Fansubs.GetByAcronym(acronym);

    public IEnumerable<FansubAnimeView> GetCompletedAnimes(string acronym)
    {
        var animes = _unitOfWork.Animes.GetCompletedByFansub(acronym);

        return animes.Select(a => new FansubAnimeView
        {
            Slug = a.Slug,
            Name = a.Name,
            CoverImage = a.CoverImageUrl,
            FinishedDate = a.Episodes.Select(e => e.Subtitles.SingleOrDefault(s => s.Fansub.Acronym == acronym).CreationDate).OrderByDescending(d => d).First()
        });
    }

    public IEnumerable<FansubEpisodeView> GetCompletedEpisodes(string acronym)
    {
        var episodes = _unitOfWork.Episodes.GetByFansub(acronym);

        return episodes.Select(e => new FansubEpisodeView
        {
            AnimeSlug = e.Anime.Slug,
            AnimeName = e.Anime.Name,
            AnimeCoverImage = e.Anime.CoverImageUrl,
            Number = e.Number,
            Name = e.Name,
            FinishedDate = e.Subtitles.SingleOrDefault(s => s.Fansub.Acronym == acronym).CreationDate,
        });
    }

    public IEnumerable<FansubUserView> GetMembers(string acronym)
    {
        var users = _unitOfWork.Users.GetByFansub(acronym);

        return users.Select(u => new FansubUserView
        {
            Name = u.UserName,
            AvatarUrl = u.AvatarUrl,
            Role = u.Memberships.SingleOrDefault(m => m.Fansub.Acronym == acronym).Role,
        });
    }

    public Fansub Create(FansubDTO fansubDTO, Guid identityID)
    {
        var user = _unitOfWork.Users.GetByID(identityID);
        if (user == null)
        {
            _logger.Emit(ELoggingEvent.UserDoesntExist, new { UserIdentityID = identityID });
            throw new ArgumentException(null, nameof(identityID));
        }

        var fansub = _unitOfWork.Fansubs.Create(fansubDTO.MapToModel());

        _unitOfWork.Memberships.Create(new Membership
        {
            FansubID = fansub.ID,
            UserID = user.Id,
            Role = EFansubRole.Founder
        });

        _unitOfWork.Save();

        return fansub;
    }

    public void Delete(Guid fansubID, Guid identityID)
    {
        var user = _unitOfWork.Users.GetByID(identityID);
        if (user == null)
        {
            _logger.Emit(ELoggingEvent.UserDoesntExist, new { UserIdentityID = identityID });
            throw new ArgumentException(null, nameof(identityID));
        }

        var fansub = _unitOfWork.Fansubs.GetByID(fansubID);
        if (fansub == null)
        {
            _logger.Emit(ELoggingEvent.FansubDoesNotExist, new { FansubID = fansubID });
            throw new ArgumentException(null, nameof(fansubID));
        }

        if (!_unitOfWork.Memberships.IsFounder(fansubID, user.Id))
        {
            _logger.Emit(ELoggingEvent.UserIsNotFounder, new { UserIdentityID = identityID, FansubID = fansubID });
            throw new ArgumentException(ExceptionMessage.OnlyFounderCanDeleteFansub);
        }

        _unitOfWork.Fansubs.DeleteMembers(fansubID);
        _unitOfWork.Fansubs.Delete(fansubID);
        _unitOfWork.Save();
    }
}
