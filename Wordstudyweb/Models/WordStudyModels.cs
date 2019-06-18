using System;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Wordstudyweb.Models
{
    public partial class Wordstudywebmodels : DbContext
    {
        public Wordstudywebmodels()
            : base("name=DefaultConnection")
        {
        }

        public virtual DbSet<Alumnus> Alumni { get; set; }
        public virtual DbSet<Anchor> Anchors { get; set; }
        public virtual DbSet<Announcement> Announcements { get; set; }
        public virtual DbSet<Bible> Bibles { get; set; }
        public virtual DbSet<Concept> Concepts { get; set; }
        public virtual DbSet<Connection> Connections { get; set; }
        public virtual DbSet<Excecutive> Excecutives { get; set; }
        public virtual DbSet<Insight> Insights { get; set; }
        public virtual DbSet<Member> Members { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Notepad> Notepads { get; set; }
        public virtual DbSet<Outline> Outlines { get; set; }
        public virtual DbSet<Reaction> Reactions { get; set; }
        public virtual DbSet<Setting> Settings { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Carousel> Carousels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Announcement>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Insight>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Message>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Outline>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Reaction>()
                .Property(e => e.RowVersion)
                .IsFixedLength();

            modelBuilder.Entity<Topic>()
                .Property(e => e.RowVersion)
                .IsFixedLength();
        }
    }

    public partial class Alumnus
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public string Address { get; set; }

        public string PostHeld { get; set; }

        public string SchoolPeriod { get; set; }
    }
    public partial class Anchor
    {
        public Guid Id { get; set; }

        public Guid TopicId { get; set; }

        [Required]
        public string Book { get; set; }

        public int Chapter { get; set; }

        public int VerseFrom { get; set; }

        public int? VersrTo { get; set; }
    }
    public partial class Announcement
    {
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Body { get; set; }

        public string ReturnUrl { get; set; }

        public DateTime? ExpiryDate { get; set; }
        public DateTime TimeStamp { get; set; }


        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
    [Table("Bible")]
    public partial class Bible
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(8)]
        public string ShortName { get; set; }

        [Required]
        public string LongName { get; set; }

        [Required]
        public string DownloadUrl { get; set; }
    }
    public partial class versesApi
    {
        public string book_id{ get; set; }
    public string book_name{ get; set; }
public string   chapter{ get; set; }
   public string    verse{ get; set; }
     public string  text{ get; set; }
    }
    public partial class BibleAPI
    {
        public string reference { get; set; }
 public List<versesApi> verses { get; set; }



        public string text{ get; set; }
    public string  translation_id { get; set; }
    public string   translation_name { get; set; }
        public string  translation_note { get; set; }
    }

    public class Carousel
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }
    }
    public partial class Concept
    {
        public Guid Id { get; set; }

        [Column("Concept")]
        [Required]
        [StringLength(20)]
        public string concept { get; set; }
    }
    public partial class Connection
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid ConnectionId { get; set; }
    }
    public partial class Excecutive
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [StringLength(10)]
        public string RoomNumber { get; set; }

        [StringLength(30)]
        public string Hall { get; set; }

        [Required]
        [StringLength(50)]
        public string Post { get; set; }

        [StringLength(10)]
        public string Level { get; set; }
    }
    public partial class Insight
    {
        public Guid Id { get; set; }


        public Guid UserId { get; set; }

        [Column("Insight")]
        [StringLength(maximumLength: 280)]
        public string insight { get; set; }

        public string Url { get; set; }
        public DateTime TimeStamp { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
    public partial class InsightData
    {
        public Guid Id { get; set; }


        public User User { get; set; }


        [StringLength(maximumLength: 280)]
        public string insight { get; set; }

        public string Url { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Status { get; set; }

        public int Reactions { get; set; }


        public byte[] RowVersion { get; set; }

    }
    public partial class Member
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [StringLength(10)]
        public string RoomNumber { get; set; }

        [StringLength(30)]
        public string Hall { get; set; }

        [StringLength(10)]
        public string Level { get; set; }
    }
    public partial class Message
    {
        public Guid Id { get; set; }

        public Guid From { get; set; }

        public Guid To { get; set; }

        [Column("Message")]
        [Required]
        public string message { get; set; }

        public DateTime TimeStamp { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
    public partial class Notepad
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        [Required]
        public string Note { get; set; }

        public DateTime TimeStamp { get; set; }
    }
    public partial class Outline
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }


        [Required]
        public string DownloadUrl { get; set; }
        public DateTime TimeStamp { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
    public partial class Reaction
    {
        public Guid Id { get; set; }

        public Guid InsightId { get; set; }

        public Guid UserId { get; set; }

        [Column("Reaction")]
        [Required]
        [StringLength(280)]
        public string reaction { get; set; }
        public DateTime TimeStamp { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
    public partial class Setting
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public bool RemindMeetings { get; set; }

        public bool AutoDownloadOutlines { get; set; }

        public bool MuteNotifications { get; set; }

        public bool MuteTopics { get; set; }

        public bool muteAnnouncments { get; set; }
    }
    public partial class Topic
    {
        public Guid Id { get; set; }

        public Guid ConceptId { get; set; }

        public Guid AnchorId { get; set; }

        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        [StringLength(125)]
        public string Description { get; set; }
        public DateTime TimeStamp { get; set; }
        [Required]
        public string Url { get; set; }

        [Column(TypeName = "timestamp")]
        [MaxLength(8)]
        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
    public class ConceptsData
    {
        public Concept concept { get; set; }
        public List<Topic> Topics { get; set; }
    }
    public class TopicData
    {
        public Guid Id { get; set; }
        [Required]
        public Guid ConceptId { get; set; }


        [Required]
        [StringLength(50)]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        [StringLength(125)]
        public string Description { get; set; }

        public DateTime TimeStamp { get; set; }

        [Required]
        public string Book { get; set; }
        [Required]
        public string Url { get; set; }
        public int Chapter { get; set; }

        public int VerseFrom { get; set; }

        public int? VersrTo { get; set; }

    }

    public class ConceptData
    {
        public Concept concept { get; set; }
        public Topic topic { get; set; }
    }
    public partial class User
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string MatriculationNumber { get; set; }

        [StringLength(20)]
        public string Gender { get; set; }

        [Required]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        [StringLength(50)]
        public string DayOfBirth { get; set; }

        public string Course { get; set; }
    }
    public class AddExternalLoginBindingModel
    {
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    public class ChangePasswordBindingModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterBindingModel

    {
        [Required]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "LastName")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Matriculation Number")]
        public string MatricNo { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterExternalBindingModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class RemoveLoginBindingModel
    {
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    public class SetPasswordBindingModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}