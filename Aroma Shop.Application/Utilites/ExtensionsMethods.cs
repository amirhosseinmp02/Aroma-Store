using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Aroma_Shop.Application.Utilites
{
    public static class ExtensionsMethods
    {
        public static string ToSolar(this DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            var solarCalendar =
                $"{persianCalendar.GetYear(dateTime)}/{persianCalendar.GetMonth(dateTime)}" +
                $"/{persianCalendar.GetDayOfMonth(dateTime)}";

            return solarCalendar;
        }

        public static string ToSolarWithTime(this DateTime dateTime)
        {
            PersianCalendar persianCalendar = new PersianCalendar();

            var solarCalendar =
                $"{persianCalendar.GetHour(dateTime)}:{persianCalendar.GetMinute(dateTime)}" +
                $" - {persianCalendar.GetYear(dateTime)}/{persianCalendar.GetMonth(dateTime)}" +
                $"/{persianCalendar.GetDayOfMonth(dateTime)}";

            return solarCalendar;
        }
    }
}
