namespace Yes.Domain.Logs
{
    [Table("Log")]
    public class LogEntity
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; }

        public LogTypeEnum Type { get; set; }

        public int RelatedId { get; set; }

        public RelatedTypeEnum RelatedType { get; set; }

        public string Content { get; set; }

        public bool Success { get; set; }

        public string Ip { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
