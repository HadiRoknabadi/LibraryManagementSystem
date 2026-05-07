namespace Application.DTOs.Common
{
    public class CookieSettings
    {
        public string LoginPath { get; set; }
        public string LogoutPath { get; set; }
        public double ExpireTimeSpan { get; set; }
        public double PersisKeysLifeTimeSpan { get; set; }

    }
}
