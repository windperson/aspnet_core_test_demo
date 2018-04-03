using System.ComponentModel.DataAnnotations;

namespace aspnet_core_test_demo.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        public string Title { get; set; }
        public int Age { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}