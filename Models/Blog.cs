using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace library.Models
{
    public class Blog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Titre { get; set; }

        [Required]
        public string Contenu { get; set; }

        public DateTime CreatedAt { get; set; }
        public string ImageUrl { get; set; }
    }
}

