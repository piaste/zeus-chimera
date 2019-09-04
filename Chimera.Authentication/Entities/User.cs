using Common.Core.Identities;
using System.ComponentModel.DataAnnotations;
using ZenProgramming.Chakra.Core.Entities;

namespace Chimera.Authentication.Entities
{
    public class User: ModernEntityBase, IGenericIdentity
    {
        /// <summary>
		/// Username
		/// </summary>
		[Required]
        [StringLength(255)]
        [RegularExpression(@"^[a-zA-Z0-9_@\-\.]+$")]
        public virtual string UserName { get; set; }

        /// <summary>
        /// Password hash (SHA-384)
        /// </summary>
        [StringLength(1024)]
        public virtual string PasswordHash { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [StringLength(255)]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.'\+&=]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$")]
        public virtual string Email { get; set; }

        /// <summary>
        /// Person name
        /// </summary>
        [StringLength(255)]
        public virtual string FirstName { get; set; }

        /// <summary>
        /// Person surname
        /// </summary>
        [StringLength(255)]
        public virtual string LastName { get; set; }

        /// <summary>
        /// Flag for enable user
        /// </summary>
        [Required]
        public virtual bool IsEnabled { get; set; }

        /// <summary>
        /// Flag for administrator user
        /// </summary>
        [Required]
        public bool IsAdministrator { get; set; }
    }
}
