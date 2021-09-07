using System;
using System.ComponentModel.DataAnnotations;
using DAL.Models.Auth;

namespace DAL.Models.Forum
{
    public class Vote : Entity
    {
        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public virtual Post Post { get; set; }

        [Required(ErrorMessage = "Please select Up or Down vote")]
        public short Value { get; set; }
    }
}