namespace CSharp.Models
{
    public class RawTimeEntry
    {
        public required string Id { get; set; }
        public required string EmployeeName { get; set; }
        public DateTime StarTimeUtc { get; set; }
        public DateTime EndTimeUtc { get; set; }
        public string EntryNotes { get; set; } = null!;
        public DateTime? DeletedOn { get; set; }
    }
}
