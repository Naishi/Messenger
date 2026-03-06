namespace Messenger.Service.Settings;

public class AttachmentSettings
{
    public int MaxAttachmentSize { get; set; }
    public long MaxFileSizeBytes { get; set; } = 20 * 1024 * 1024; //20 MB
    public List<string> AllowedExtension { get; set; } = [".jpg", ".png", ".pdf", ".mp3", ".mp4", ".docx", ".doc", ".txt"];
}