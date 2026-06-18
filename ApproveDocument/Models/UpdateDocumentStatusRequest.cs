using ApproveDocument.Entities;
using System.ComponentModel.DataAnnotations;

namespace ApproveDocument.Models;

public class UpdateDocumentStatusRequest
{
    [Required]
    public int Id { get; set; }

    [Required]
    public DocumentStatus Status { get; set; }

    [MaxLength(1000)]
    public string? Description { get; set; }
}
