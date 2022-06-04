namespace CloudStorage
{
    public class MyMessage
    {

        public Guid ID { get; set;  } = Guid.NewGuid();       
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

    }
}