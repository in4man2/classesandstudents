using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ClassesAndStudents.Models
{
    public class ClassEntity
    {
        public ClassEntity()
        {
            StudentEntities = new HashSet<StudentEntity>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid ID { get; set; }
        public string ClassName { get; set; }
        public string Location { get; set; }
        public string Teacher { get; set; }  
        public virtual ICollection<StudentEntity> StudentEntities { get; set; }
    }
}