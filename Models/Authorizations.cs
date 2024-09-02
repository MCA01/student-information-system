namespace StundetInformation.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Authorizations
    {
        public int id { get; set; }

        public int userID { get; set; }

        public int authorizationID { get; set; }

        public virtual AuthorizationType AuthorizationType { get; set; }

        public virtual User User { get; set; }
    }
}
