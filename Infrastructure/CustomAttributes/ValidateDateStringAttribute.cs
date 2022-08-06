namespace Infrastructure.CustomAttributes;

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

using Common;
using Common.Constants;

public class ValidateDateStringAttribute : RequiredAttribute
{
    public override bool IsValid(object value)
    {
        var dateString = value as string;

        if (string.IsNullOrEmpty(dateString))
        {
            return false;
        }

        var parsedDate = DateTime.TryParseExact(dateString, 
            GlobalConstants.DateTimeFormats.DateFormat, 
            CultureInfo.InvariantCulture, 
            style: DateTimeStyles.AssumeUniversal, 
            out var dateTime);

        if (!parsedDate)
        {
            return false;
        }

        return dateTime >= DateTime.UtcNow.Date;
    }
}