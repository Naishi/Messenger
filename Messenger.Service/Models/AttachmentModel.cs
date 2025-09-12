using System.ComponentModel.DataAnnotations;

namespace Messenger.Service.Models;

public class AttachmentModel
{
    [MaxLength(128)]public required string FileName { get; set; }
    public byte[] Attachments { get; set; } = [];
}