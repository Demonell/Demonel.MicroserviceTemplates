namespace WebApi.Options
{
    public class AppOptions
    {
        public string ApplicationName { get; set; }
        public int SwaggerVersion { get; set; }
        public string IdentityUrl { get; set; }
        public string[] AllowedCorsOrigin { get; set; }
    }
}
