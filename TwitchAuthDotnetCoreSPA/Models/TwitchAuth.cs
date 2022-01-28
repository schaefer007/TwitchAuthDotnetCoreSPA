using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchAuthDotnetCoreSPA.Models {
	public class TwitchAuth {
		[JsonProperty("access_token")]
		public string AccessToken { get; set; }
		[JsonProperty("expires_in")]
		public int ExpiresIn { get; set; }
		[JsonProperty("refresh_token")]
		public string RefreshToken { get; set; }
		[JsonProperty("scope")]
		public List<string> Scope { get; set; }
		[JsonProperty("token_type")]
		public string TokenType { get; set; }
	}
}
/*{
 *	"access_token":"9tnpnnotvohetflfv3xjbeqvhu60sl",
 *	"expires_in":15804,
 *	"refresh_token":"9y0ikqyklcdgbcjb0pdhqb65kghi1r9czti3iz2q2507qwszem",
 *	"scope":["user:read:email"],
 *	"token_type":"bearer"
 *	}
 */