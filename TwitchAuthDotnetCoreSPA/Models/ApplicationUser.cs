using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchAuthDotnetCoreSPA.Models {
	public class ApplicationUser : IdentityUser {
		public int TwitchId { get; set; }
	}
}
