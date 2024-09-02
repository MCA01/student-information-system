namespace StundetInformation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Course()
        {
            Enrolls = new HashSet<Enrolls>();
            StudentGrade = new HashSet<StudentGrade>();
        }

        public int id { get; set; }

        public int? courseCode { get; set; }

        public int? instructorId { get; set; }

        [StringLength(100)]
        public string courseName { get; set; }

        public int semesterID { get; set; }

        public int departmentID { get; set; }

        [StringLength(3000)]
        public string description { get; set; }

        public int? ects { get; set; }

        public bool? isActive { get; set; }

        public int? weeklyHours { get; set; }

        public virtual Department Department { get; set; }

        public virtual User User { get; set; }

        public virtual Semester Semester { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enrolls> Enrolls { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentGrade> StudentGrade { get; set; }
    }
}
