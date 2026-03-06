using Messenger.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace Messenger.Domain.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<ChatEntity> Chats { get; set; }

    public DbSet<UserEntity> Users { get; set; }

    public DbSet<UserChatEntity> UserChats { get; set; }

    public DbSet<UserAuthEntity> UserAuth { get; set; }

    public DbSet<ContactEntity> Contacts { get; set; }

    public DbSet<MessageEntity> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContactEntity>()
            .HasOne(c => c.OwnerUser)
            .WithMany(u => u.Contacts)
            .HasForeignKey(c => c.OwnerUserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<ContactEntity>()
            .HasOne(c => c.ContactUser)
            .WithMany(u => u.AddedBy)
            .HasForeignKey(c => c.ContactUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MessageEntity>()
            .HasKey(m => m.Id);
        modelBuilder.Entity<MessageEntity>()
            .HasOne(m => m.User)
            .WithMany()
            .HasForeignKey(m => m.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<MessageEntity>()
            .HasOne(m => m.Chat)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserChatEntity>()
            .HasKey(uce => uce.Id);
        modelBuilder.Entity<UserChatEntity>()
            .HasOne(uce => uce.User)
            .WithMany(u => u.UserChats)
            .HasForeignKey(uce => uce.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<UserChatEntity>()
            .HasOne(uce => uce.Chat)
            .WithMany(c => c.UserChats)
            .HasForeignKey(uce => uce.ChatId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<UserAuthEntity>()
            .HasIndex(userAuth => userAuth.Email)
            .IsUnique();
        modelBuilder.Entity<UserEntity>()
            .HasIndex(user => user.NickName)
            .IsUnique();
            
    }
}