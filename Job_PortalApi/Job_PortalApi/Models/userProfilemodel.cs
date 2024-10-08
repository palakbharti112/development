namespace Job_PortalApi.Models
{
    public class userProfilemodel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? Resume { get; set; }

        public string? Bio { get; set; }

        public string? Location { get; set; }

        public string? Skills { get; set; }

        public string? Experience { get; set; }
    }
}
