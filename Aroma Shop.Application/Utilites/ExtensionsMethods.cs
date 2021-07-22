using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Aroma_Shop.Application.Utilites
{
    public static class ExtensionsMethods
    {
        #region ToSolar

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

        #endregion

        #region IsLocal

        private const string NullIpAddress = "::1";

        public static bool IsLocal(this HttpRequest req)
        {
            var connection = req.HttpContext.Connection;
            if (connection.RemoteIpAddress.IsSet())
            {
                //We have a remote address set up
                return connection.LocalIpAddress.IsSet()
                    //Is local is same as remote, then we are local
                    ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
                    //else we are remote if the remote IP address is not a loopback address
                    : IPAddress.IsLoopback(connection.RemoteIpAddress);
            }

            return true;
        }

        private static bool IsSet(this IPAddress address)
        {
            return address != null && address.ToString() != NullIpAddress;
        }

        #endregion

        #region GetDescription

        public static string GetDescription(this Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return en.ToString();
        }

        #endregion
    }
}
