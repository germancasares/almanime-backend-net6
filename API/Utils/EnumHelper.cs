﻿using API.Models.Enums;

namespace API.Utils;

public static class EnumHelper
{
    public static T? GetEnumFromString<T>(string? value) where T : struct
    {
        if (value == null) return null;

        var isEnum = Enum.TryParse<T>(value, out var type);

        return isEnum ? type : null;
    }

    public static ESeason GetSeason(DateTime date) => GetSeason(date.Month);

    public static ESeason GetSeason(int month)
    {
        return month switch
        {
            int n when n >= 1 && n <= 2 => ESeason.Winter,
            int n when n >= 3 && n <= 5 => ESeason.Spring,
            int n when n >= 6 && n <= 8 => ESeason.Summer,
            int n when n >= 9 && n <= 11 => ESeason.Fall,
            int n when n == 12 => ESeason.Winter,
            _ => throw new ArgumentException(ExceptionMessage.ESeasonOutOfRange),
        };
    }

    public static ESubtitleFormat GetSubtitleFormat(this IFormFile file)
    {
        return file.GetExtension() switch
        {
            ".ass" => ESubtitleFormat.ASS,
            ".srt" => ESubtitleFormat.SRT,
            _ => throw new ArgumentException(ExceptionMessage.ESubtitleFormatOutOfRange),
        };
    }
}
