namespace StundetInformation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Enrolls
    {
        public int studentID { get; set; }

        public int courseID { get; set; }

        [StringLength(100)]
        public string status { get; set; }

        public int id { get; set; }

        public virtual Course Course { get; set; }

        public virtual Student Student { get; set; }
    }
}
