using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using FsNet.Data.Contracts.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FsNet.Data.Domain.User
{
    public class ApplicationUser : IdentityUser, IEntity<string>
    {
        [StringLength(20)]
        public virtual string MobileNumber { get; set; }
        
        [StringLength(4)]
        public virtual string CountryPrefix { get; set; }
        public virtual DateTime? CreationDate { get; set; }
        public virtual DateTime? ModifiedDate { get; set; }

        [StringLength(35)]
        public virtual string CreatedBy { get; set; }

        [StringLength(35)]       
        public virtual string ModifiedBy { get; set; }

        [StringLength(100)]
        public string TelegramUrl { get; set; }

        [StringLength(100)]
        public string InstagramId { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager,
            string authenticationType)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            return userIdentity;
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(
            UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }
}
