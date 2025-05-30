using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {

    }

    public DbSet<ChatEntity> Chats { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserChatEntity> UserChats { get; set; }
}