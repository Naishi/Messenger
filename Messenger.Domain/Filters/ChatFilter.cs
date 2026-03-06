
namespace Messenger.Domain.Filters;

/// <summary>
/// Критерии поиска чатов
/// </summary>
public class ChatFilter
{
    /// <summary>
    /// Поиск по Id чата
    /// </summary>
    public ICollection<int>? ChatIds { get; set; }
    
    /// <summary>
    /// Поиск по Id пользователя
    /// </summary>
    public ICollection<int>? UserIds { get; set; }

    /// <summary>
    /// Поиск по имени контакта
    /// </summary>
    public string? Search { get; set; }
}