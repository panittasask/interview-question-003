namespace ApproveDocument.Entities;

public enum DocumentStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}

public class Document
{
    public int Id { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string DocumentDetail { get; set; } = string.Empty;
    public DocumentStatus Status { get; set; } = DocumentStatus.Pending;
    public string? StatusReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
