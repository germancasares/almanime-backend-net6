using API.Models;
using API.Models.DTOs;
using API.Models.Enums;
using API.Repositories.Interfaces;
using API.Services.Interfaces;
using API.Utils;

namespace API.Services;

public class SubtitleService : ISubtitleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SubtitleService> _logger;

    public SubtitleService(
        IUnitOfWork unitOfWork,
        ILogger<SubtitleService> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public Subtitle? GetByID(Guid ID) => _unitOfWork.Subtitles.GetByID(ID);

    public Task<Subtitle> Create(SubtitleDTO subtitleDTO, Guid identityID)
    {
        var user = _unitOfWork.Users.GetByID(identityID);
        if (user == null)
        {
            _logger.Emit(ELoggingEvent.UserDoesntExist, new { UserIdentityID = identityID });
            throw new ArgumentException(nameof(identityID));
        }

        var episode = _unitOfWork.Episodes.GetByAnimeSlugAndNumber(subtitleDTO.AnimeSlug, subtitleDTO.EpisodeNumber);
        if (episode == null)
        {
            _logger.Emit(ELoggingEvent.EpisodeDoesntExist, new { subtitleDTO.AnimeSlug, subtitleDTO.EpisodeNumber });
            throw new ArgumentException(nameof(subtitleDTO.EpisodeNumber));
        }

        var fansub = _unitOfWork.Fansubs.GetByAcronym(subtitleDTO.FansubAcronym);
        if (fansub == null)
        {
            _logger.Emit(ELoggingEvent.FansubDoesNotExist, new { subtitleDTO.FansubAcronym });
            throw new ArgumentException(nameof(subtitleDTO.FansubAcronym));
        }

        if (!fansub.Memberships.Any(m => m.UserID == user.Id))
        {
            _logger.Emit(ELoggingEvent.UserDoesntBelongOnFansub, new { subtitleDTO.FansubAcronym });
            throw new ArgumentException(ExceptionMessage.UserDoesntBelongOnFansub);
        }

        return CreateInternal(subtitleDTO, identityID, episode, fansub, user);
    }

    private async Task<Subtitle> CreateInternal(SubtitleDTO subtitleDTO, Guid identityID, Episode episode, Fansub fansub, User user)
    {
        var subtitle = _unitOfWork.Subtitles.Create(new Subtitle
        (
            status: subtitleDTO.Status,
            episodeID: episode.ID,
            fansubID: fansub.ID,
            format: subtitleDTO.Subtitle.GetSubtitleFormat()
        ));;

        var subtitlePartial = _unitOfWork.SubtitlePartials.Create(new SubtitlePartial(
            userID: user.Id,
            subtitleID: subtitle.ID
        ));

        _unitOfWork.Save();

        string subtitleUrl;
        try
        {
            subtitleUrl = await _unitOfWork.Storage.UploadSubtitle(subtitleDTO.Subtitle, fansub.ID, subtitle.ID);
        }
        catch (Exception ex)
        {
            // TODO: AZ to do this cleaning instead of on the call
            _logger.Emit(
                ELoggingEvent.CantUploadSubtitle,
                new
                {
                    subtitleDTO.AnimeSlug,
                    subtitleDTO.EpisodeNumber,
                    UserIdentityID = identityID,
                    Exception = ex,
                }
            );
            _unitOfWork.Subtitles.Delete(subtitle);
            _unitOfWork.SubtitlePartials.Delete(subtitlePartial);
            _unitOfWork.Save();
            throw;
        }

        string subtitlePartialUrl;
        try
        {
            subtitlePartialUrl = await _unitOfWork.Storage.UploadSubtitlePartial(subtitleDTO.Subtitle, fansub.ID, subtitle.ID, subtitlePartial.ID);
        }
        catch (Exception ex)
        {
            // TODO: AZ to do this cleaning instead of on the call
            _logger.Emit(
                ELoggingEvent.CantUploadSubtitlePartial,
                new
                {
                    subtitleDTO.AnimeSlug,
                    subtitleDTO.EpisodeNumber,
                    UserIdentityID = identityID,
                    Exception = ex,
                }
            );
            _unitOfWork.Subtitles.Delete(subtitle);
            _unitOfWork.SubtitlePartials.Delete(subtitlePartial);
            _unitOfWork.Save();

            _unitOfWork.Storage.DeleteSubtitle(fansub.ID, subtitle.ID);
            throw;
        }

        try
        {
            subtitle.Url = subtitleUrl;
            subtitlePartial.Url = subtitlePartialUrl;
            _unitOfWork.Save();
        }
        catch (Exception ex)
        {
            // TODO: AZ to do this cleaning instead of on the call
            _logger.Emit(
                ELoggingEvent.CantLinkSubtitleUrl,
                new
                {
                    subtitleDTO.AnimeSlug,
                    subtitleDTO.EpisodeNumber,
                    UserIdentityID = identityID,
                    Exception = ex,
                }
            );
            _unitOfWork.Subtitles.Delete(subtitle);
            _unitOfWork.SubtitlePartials.Delete(subtitlePartial);
            _unitOfWork.Save();

            _unitOfWork.Storage.DeleteSubtitle(fansub.ID, subtitle.ID);
            _unitOfWork.Storage.DeleteSubtitlePartial(fansub.ID, subtitle.ID, subtitlePartial.ID);

            throw;
        }

        return subtitle;
    }
}
