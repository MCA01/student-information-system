namespace StundetInformation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StudentGrade")]
    public partial class StudentGrade
    {
        public int studentID { get; set; }

        public int courseID { get; set; }

        public int? gradeValue { get; set; }

        [Column(TypeName = "date")]
        public DateTime? date { get; set; }

        public int id { get; set; }

        public int gradeWeight { get; set; }

        public virtual Course Course { get; set; }

        public virtual Student Student { get; set; }
    }
}
