namespace TwitterBackup.Services.DataModels
{
    public class LocalDBUserDetails
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public byte[] Avatar { get; set; }
    }
}
