using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreDemo.Models
{
    public class User
    {
        public int? Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        public string Title { get; set; }

        public override string ToString() => $"{Name} - {Title}";
    }
}
