using Messenger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DbSet<ChatEntity> Chats { get; set; }
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<UserChatEntity> UserChats { get; set; }
    public DbSet<UserAuthEntity> UserAuth { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserChatEntity>()
            .HasKey(uce => uce.Id);
        modelBuilder.Entity<UserChatEntity>()
            .HasOne(uce => uce.User)
            .WithMany(u => u.UserChats)
            .HasForeignKey(uce => uce.UserId);
        modelBuilder.Entity<UserChatEntity>()
            .HasOne(uce => uce.Chat)
            .WithMany(c => c.UserChats)
            .HasForeignKey(uce => uce.ChatId);
        
    }
}