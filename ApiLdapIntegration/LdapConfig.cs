namespace ApiLdapIntegration
{
    public class LdapConfig
    {
        public string ServerHost { get; set; }
        public int ServerPort { get; set; }
        public string BaseDN { get; set; }
        public string BindDN { get; set; }
        public string BindPassword { get; set; }
        public string UserFilter { get; set; }
        public bool SecureSocketLayer { get; set; }
    }
}
