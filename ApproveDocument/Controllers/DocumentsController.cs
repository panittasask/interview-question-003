using ApproveDocument.Data;
using ApproveDocument.Entities;
using ApproveDocument.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApproveDocument.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DocumentsController(AppDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetDocuments()
    {
        var documents = await dbContext.Documents
            .OrderBy(x => x.Id)
            .Select(x => new
            {
                x.Id,
                x.DocumentName,
                x.DocumentDetail,
                x.Status,
                x.StatusReason,
                x.CreatedAt,
                x.UpdatedAt
            })
            .ToListAsync();

        return Ok(documents);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDocumentById(int id)
    {
        var document = await dbContext.Documents
            .Where(x => x.Id == id)
            .Select(x => new
            {
                x.Id,
                x.DocumentName,
                x.DocumentDetail,
                x.Status,
                x.StatusReason,
                x.CreatedAt,
                x.UpdatedAt
            })
            .FirstOrDefaultAsync();

        if (document is null)
        {
            return NotFound(new { message = "Document not found" });
        }

        return Ok(document);
    }

    [HttpPost("status")]
    public async Task<IActionResult> UpdateDocumentStatus([FromBody] UpdateDocumentStatusRequest request)
    {
        if (!Enum.IsDefined(typeof(DocumentStatus), request.Status))
        {
            return BadRequest(new { message = "Invalid status. Allowed values: 1 = Pending, 2 = Approved, 3 = Rejected" });
        }

        if (request.Status is DocumentStatus.Approved or DocumentStatus.Rejected)
        {
            return BadRequest(new { message = "Cannot update document with status 2 or 3. Only status 1 (Pending) is allowed." });
        }

        var document = await dbContext.Documents.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (document is null)
        {
            return NotFound(new { message = "Document not found" });
        }

        document.Status = request.Status;
        document.StatusReason = request.Description;
        document.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync();

        return Ok(new
        {
            document.Id,
            document.DocumentName,
            document.DocumentDetail,
            document.Status,
            document.StatusReason,
            document.CreatedAt,
            document.UpdatedAt
        });
    }
}
