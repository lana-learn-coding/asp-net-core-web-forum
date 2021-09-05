using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using DAL.Models.Auth;

namespace DAL.Models.Forum
{
    public class Vote : Entity
    {
        public virtual User User { get; set; }

        public virtual Post Post { get; set; }

        [Required(ErrorMessage = "Please select Up or Down vote")]
        public short Value { get; set; }

        [JsonIgnore]
        [NotMapped]
        public bool IsDownVote => Value < 0;
    }
}