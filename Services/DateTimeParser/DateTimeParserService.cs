namespace Services.DateTimeParser
{
    using System;
    using System.Globalization;

    using Common;

    public class DateTimeParserService : IDateTimeParserService
    {
        public DateTime ConvertStrings(string date, string time)
        {
            var dateString = string.Join(" ", date, time);
            var format = GlobalConstants.DateTimeFormats.DateTimeFormat;

            var dateTime = DateTime.ParseExact(dateString, format, CultureInfo.InvariantCulture);

            return dateTime;
        }
    }
}
