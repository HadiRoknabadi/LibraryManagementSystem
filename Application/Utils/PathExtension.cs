public static class PathExtension
{
    public static string DomainAddress = "";

    #region User

    public static string UserAvatarOrigin= DomainAddress + "/Uploads/images/UserAvatars/Origin/";
    public static string UserAvatarOriginRelative = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/images/UserAvatars/Origin/");
    public static string UserAvatarThumb= DomainAddress + "/Uploads/images/UserAvatars/Thumb/";
    public static string UserAvatarThumbRelative = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Uploads/images/UserAvatars/Thumb/");
    #endregion

    #region Admin Theme Files

    public static string AdminThemeJsFiles = DomainAddress + "/Admin/Js/";
    public static string AdminThemeCssFiles = DomainAddress + "/Admin/Css/";
    public static string AdminThemeFontFiles = DomainAddress + "/Admin/Fonts/";
    public static string AdminThemeImageFiles = DomainAddress + "/Admin/img/";
    public static string AdminThemeMinifiedFiles = DomainAddress + "/Admin/";

    #endregion

    #region Lib Files

    public static string LibFiles = DomainAddress + "/lib/";

    #endregion

    #region Default Names

    public static string Default_Avatar_Name = "Default_Avatar.png";

    #endregion

}
