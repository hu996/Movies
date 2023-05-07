using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Models
{
    
    public class Genre
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte ID { get;set; }
        public string Name { get;set; }
    }
}
