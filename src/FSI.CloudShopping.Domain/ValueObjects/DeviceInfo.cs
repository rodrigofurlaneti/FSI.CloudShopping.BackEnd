using FSI.CloudShopping.Domain.Core;

namespace FSI.CloudShopping.Domain.ValueObjects
{
    public class DeviceInfo : ValueObject
    {
        public string UserAgent { get; }
        public string Platform { get; }
        public string Language { get; }
        public string TimeZone { get; }

        private DeviceInfo() { }

        public DeviceInfo(
            string? userAgent,
            string? platform,
            string? language,
            string? timeZone)
        {
            UserAgent = NormalizeUserAgent(userAgent);
            Platform = NormalizePlatform(platform);
            Language = NormalizeLanguage(language);
            TimeZone = NormalizeTimeZone(timeZone);
        }

        private static string NormalizeUserAgent(string? value)
            => string.IsNullOrWhiteSpace(value)
                ? "unknown"
                : value.Length > 512 ? value[..512] : value;

        private static string NormalizePlatform(string? value)
            => value?.ToLower() switch
            {
                "win32" => "Windows",
                "macintel" => "MacOS",
                "linux x86_64" => "Linux",
                _ => value ?? "unknown"
            };

        private static string NormalizeLanguage(string? value)
            => string.IsNullOrWhiteSpace(value)
                ? "unknown"
                : value.ToLower();

        private static string NormalizeTimeZone(string? value)
            => string.IsNullOrWhiteSpace(value)
                ? "UTC"
                : value;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return UserAgent;
            yield return Platform;
            yield return Language;
            yield return TimeZone;
        }
    }
}
