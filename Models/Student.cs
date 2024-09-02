namespace StundetInformation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Student")]
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            Enrolls = new HashSet<Enrolls>();
            StudentGrade = new HashSet<StudentGrade>();
        }

        public int id { get; set; }

        [StringLength(500)]
        public string firstName { get; set; }

        [StringLength(500)]
        public string lastName { get; set; }

        [StringLength(10)]
        public string gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dateOfBirth { get; set; }

        [StringLength(11)]
        public string phone { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        public double? gpa { get; set; }

        [Column(TypeName = "smalldatetime")]
        public DateTime? registrationDate { get; set; }

        [StringLength(100)]
        public string registrationStatus { get; set; }

        public bool? isActive { get; set; }

        public int? departmentID { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        public virtual Department Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Enrolls> Enrolls { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentGrade> StudentGrade { get; set; }
    }
}
