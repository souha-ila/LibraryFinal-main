using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace library.Models
{
    public class Livre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Titre { get; set; }

        [Required]
        public string Auteur { get; set; }

        public string Resume { get; set; }

        public bool EstEmprunte { get; set; }

        public string ImageUrl { get; set; }
    }
}
