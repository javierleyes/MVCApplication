using System.ComponentModel.DataAnnotations;

namespace MyMVCApplication.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Name")]
        public string StudentName { get; set; }

        [Range(5, 50)]
        public int Age { get; set; }
    }
}