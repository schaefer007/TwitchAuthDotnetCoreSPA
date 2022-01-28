using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwitchAuthDotnetCoreSPA {
	public class TwitchUser {
		public TwitchUserData[] data { get; set; }
		/*
{
	"data": [
		{
			"id": "113669524",
			"login": "schaefer007",
			"display_name": "Schaefer007",
			"type": "",
			"broadcaster_type": "",
			"description": "Battle royals and smiles all over the place",
			"profile_image_url": "https://static-cdn.jtvnw.net/jtv_user_pictures/06b5e367-a84f-40f1-8346-c3c3c6e7d501-profile_image-300x300.png",
			"offline_image_url": "",
			"view_count": 700,
			"email": "schaefer007@gmail.com",
			"created_at": "2016-01-23T20:44:12Z"
		}
	]
}*/

	}

	public class TwitchUserData {
		public int id { get; set; }
		public string login { get; set; }
		public string display_name { get; set; }
		public string profile_image_url { get; set; }
		public string email { get; set; }
	}
}
