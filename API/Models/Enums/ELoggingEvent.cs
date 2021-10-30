namespace API.Models.Enums;

public enum ELoggingEvent
{
    GetItem = 1,

    // Services
    CantCreateAccount = 2,
    AnimeSlugDoesntExist = 3,
    UserDoesntExist = 4,
    BookmarkAlreadyExists = 5,
    BookmarkDoesntExist = 6,
    UserIsNotFounder = 7,
    FansubDoesNotExist = 8,
    EpisodeDoesntExist = 9,
    UserDoesntBelongOnFansub = 10,
    CantUploadSubtitle = 11,
    CantUploadSubtitlePartial = 12,
    CantLinkSubtitleUrl = 13,
    UserAlreadyExists = 14,
    CantUploadAvatar = 15,
}
