using AxioVital.Application.Interfaces;
using AxioVital.Contracts.Requests;
using AxioVital.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxioVital.Api.Controllers;

/// <summary>
/// REST API for medical document management (upload, download, list, delete, preview).
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DocumentsController : ControllerBase
{
    private readonly IDocumentService _documentService;
    private readonly ILogger<DocumentsController> _logger;

    public DocumentsController(IDocumentService documentService, ILogger<DocumentsController> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    /// <summary>
    /// Upload a medical document with metadata.
    /// </summary>
    [HttpPost("upload")]
    [RequestSizeLimit(100 * 1024 * 1024)] // 100 MB
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromForm] UploadDocumentRequest request,
        CancellationToken cancellationToken)
    {
        if (file == null || file.Length == 0)
            return BadRequest(new { error = "No file provided." });

        if (!Enum.TryParse<FileCategory>(request.Category, ignoreCase: true, out var category))
            category = FileCategory.Other;

        using var stream = file.OpenReadStream();
        var result = await _documentService.UploadAsync(
            stream,
            file.FileName,
            file.ContentType,
            file.Length,
            request.PatientId,
            category,
            request.Description,
            request.Tags,
            cancellationToken);

        return Created($"/api/documents/{result.Id}", result);
    }

    /// <summary>
    /// Download a document by ID.
    /// </summary>
    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> Download(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var (content, contentType, fileName) = await _documentService.DownloadAsync(id, cancellationToken);
            return File(content, contentType, fileName);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = $"Document {id} not found." });
        }
    }

    /// <summary>
    /// Get a presigned URL for inline preview.
    /// </summary>
    [HttpGet("{id:guid}/preview")]
    public async Task<IActionResult> Preview(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var url = await _documentService.GetPreviewUrlAsync(id, TimeSpan.FromMinutes(15), cancellationToken);
            return Ok(new { previewUrl = url });
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = $"Document {id} not found." });
        }
    }

    /// <summary>
    /// List documents with optional filters.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] Guid? patientId,
        [FromQuery] string? category,
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        FileCategory? cat = null;
        if (!string.IsNullOrEmpty(category) && Enum.TryParse<FileCategory>(category, ignoreCase: true, out var parsed))
            cat = parsed;

        var documents = await _documentService.ListAsync(patientId, cat, search, cancellationToken);
        return Ok(documents);
    }

    /// <summary>
    /// Soft-delete a document.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _documentService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { error = $"Document {id} not found." });
        }
    }
}
