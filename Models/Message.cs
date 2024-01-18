using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace library.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Nom { get; set; }

        [Required]
        public string Mail { get; set; }

        [Required]
        public string Contenu { get; set; }

    }
}
