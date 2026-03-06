using System.ComponentModel.DataAnnotations;

namespace Messenger.Domain.Entities;

public class AttachmentEntity
{
    public int Id { get; set; }

    public int MessageId { get; set; }

    [MaxLength(128)]
    public required string FileName { get; set; }
    [MaxLength(16)]
    public required string ContentType { get; set; }

    public long FileSize { get; set; }

    public AttachmentType Type { get; set; }

    public DateTime UploadDate { get; set; } = DateTime.UtcNow;

    [MaxLength(512)]
    public required string LocalFilePath { get; set; }
    
    [MaxLength(512)]
    public string? RemoteFileUrl { get; set; }

    public void DetermineType()
    {
        if (ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
            Type = AttachmentType.IsImage;
        else if (ContentType.StartsWith("video/", StringComparison.OrdinalIgnoreCase))
            Type = AttachmentType.IsVideo;
        else if (ContentType.StartsWith("audio/", StringComparison.OrdinalIgnoreCase))
            Type = AttachmentType.IsAudio;
        else if (ContentType == "application/pdf"
            || ContentType.StartsWith("application/msword")
            || ContentType.StartsWith("application/vnd"))
            Type = AttachmentType.IsDocument;
        else
            Type = AttachmentType.IsOtherFile;
    }
}