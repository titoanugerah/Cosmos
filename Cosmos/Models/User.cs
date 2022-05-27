using System.ComponentModel.DataAnnotations;

namespace Cosmos.Models
{
    public class User
    {
        [Key]
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
