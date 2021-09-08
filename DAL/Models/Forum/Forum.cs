using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models.Topic;
using DAL.Validation;

namespace DAL.Models.Forum
{
    public class Forum : Entity, ITracked
    {
        [NotMapped]
        [JsonIgnore]
        public override string RawSlug => Title;

        [TitleCase]
        [Unique]
        [Column(TypeName = "NVARCHAR")]
        [StringLength(150)]
        [Index(IsUnique = true)]
        [Required(ErrorMessage = "Please specify a title")]
        public string Title { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(300)]
        [Required(ErrorMessage = "Please specify a sub title")]
        public string SubTitle { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(500)]
        public string Description { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(20)]
        public string Icon { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public Guid CategoryId { get; set; } = Guid.Empty;

        [Required(ErrorMessage = "Please select a language")]
        public Guid LanguageId { get; set; } = Guid.Empty;

        public Category Category { get; set; }

        public Language Language { get; set; }

        [JsonIgnore]
        public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();

        // Control order of forums
        // Top important forums are show on front page
        public short Priority { get; set; } = (short)Models.Priority.Normal;

        // Public: Anyone can create a thread
        // Protected: Anyone can create a thread, but require admin approval to display
        // Internal: For future usage. now its treated as Private 
        // Private: Only Admin can create a thread
        public AccessMode ThreadAccess { get; set; } = AccessMode.Public;

        // Public: Anyone can access the forum
        // Protected: Anyone who logged in can access the forum
        // Internal: For future usage. now its treated as Private
        // Private: Only admin (and moderators if we had that feature) can access the forum
        public AccessMode ForumAccess { get; set; } = AccessMode.Public;

        public DateTime LastActivityAt { get; set; }
    }
}