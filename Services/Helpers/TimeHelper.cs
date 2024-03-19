using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Helpers
{
    public class TimeHelper
    {
        static string GetTimeZoneIdFromCountryName(string countryName)
        {
            // Bạn có thể thêm các mã múi giờ khác vào đây tùy thuộc vào nhu cầu
            switch (countryName.ToLower())
            {
                case "vietnam":
                    return "SE Asia Standard Time";
                case "united states":
                    return "Eastern Standard Time";
                // Thêm các trường hợp cho các quốc gia khác nếu cần
                default:
                    throw new ArgumentException($"Không tìm thấy múi giờ cho quốc gia {countryName}");
            }
        }

        public static DateTime GetCurrentTimeInCountry(string country)
        {
            TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(GetTimeZoneIdFromCountryName(country));
            // Lấy thời gian hiện tại của quốc gia đó
            DateTime currentTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZone);
            return currentTime;
        }

        public static DateTime ConvertToCountryTime(DateTime timeUTC, string country)
        {
            string timezoneid = GetTimeZoneIdFromCountryName(country);
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezoneid);

            // Chuyển đổi thời gian hết hạn từ múi giờ UTC sang múi giờ Việt Nam
            DateTime expiresAccessTokenVietnamTime = TimeZoneInfo.ConvertTimeFromUtc(timeUTC, vietnamTimeZone);

            return expiresAccessTokenVietnamTime;
        }
    }
}
