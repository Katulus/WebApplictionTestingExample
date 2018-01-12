namespace Server.Models
{
    public class Node
    {
        public int Id { get; set; }

        public string IpOrHostname { get; set; }

        public string PollingMethod { get; set; }
    }
}