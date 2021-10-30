namespace API.Utils;

public static class ExceptionMessage
{
    public const string ParameterNotValid = "The parameter provided to this method is invalid.";
    public const string ESeasonOutOfRange = "Month out of ESeason range.";
    public const string ESubtitleFormatOutOfRange = "Extension out of ESubtitleFormat range.";
    public const string OnlyFounderCanDeleteFansub = "Only the founder of the fansub can delete it.";
    public const string UserDoesntBelongOnFansub = "User does not belong on the fansub.";
}
