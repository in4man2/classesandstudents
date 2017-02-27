using FluentValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClassesAndStudents.Models
{
    public class StudentValidator : AbstractValidator<StudentEntity>
    {
        private Guid _classId { get; set; }
        public StudentValidator(Guid classId)
        {
            _classId  = classId;
            RuleFor(x => x.StudentName).NotEmpty().WithMessage("Student Name is required").Length(0, 100);
            RuleFor(x => x.StudentSurname).NotEmpty().WithMessage("Student Name is required").Length(0, 100).Must(BeUniqueSurname);
            RuleFor(x => x.DOB).NotEmpty().WithMessage("Date of birth is required");
            RuleFor(x => x.GPA).NotEmpty().WithMessage("GPA is required");
        }

        private bool BeUniqueSurname(string surValue)
        {
            return new EFModel1().ClassEntities.Include("StudentEntities").FirstOrDefault(n=>n.ID== _classId ).StudentEntities.Count(x => x.StudentSurname == surValue) == 0;
        }
    }

    public class StudentEntity
    {
        public StudentEntity()
        {
            ClassEntities = new HashSet<ClassEntity>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public string StudentName { get; set; }
        public string StudentSurname { get; set; }
        public DateTime DOB { get; set; }
        [DisplayFormat(DataFormatString = "{0:n1}", ApplyFormatInEditMode = true)]
        public decimal GPA { get; set; }

        public virtual ICollection<ClassEntity> ClassEntities { get; set; }
    }

    public class StudentEntityViewModel
        {
        public Guid ID { get; set; }
        public DateTime Dob { get; set; }
                
        public string StudentFullName { get; set; }
        public int Age { get; set; }
        [DisplayFormat(DataFormatString = "{0:n1}", ApplyFormatInEditMode = true)]
        public decimal Gpa { get; set; }
    }
}