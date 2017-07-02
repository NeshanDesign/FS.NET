using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FsNet.Data.Contracts.Model;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FsNet.Data.Domain.User
{
    public class ApplicationRole : IdentityRole, IEntity<string>
    {
        public virtual DateTime? CreationDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }

        [StringLength(35)]
        public virtual string CreatedBy { get; set; }

        [StringLength(35)]       
        public virtual string ModifiedBy { get; set; }
    }
}
