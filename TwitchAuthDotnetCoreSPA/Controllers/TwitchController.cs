using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using TwitchAuthDotnetCoreSPA.Models;

namespace TwitchAuthDotnetCoreSPA.Controllers {
	[Route("twitch")]
	public class TwitchController : Controller {
		private readonly IHttpContextAccessor _context;
		private readonly TwitchSettings _twitchSettings;
		private readonly IHttpClientFactory _httpClientFactory;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public TwitchController(IHttpContextAccessor context, TwitchSettings twitchSettings, IHttpClientFactory httpClientFactory, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
			_context = context;
			_twitchSettings = twitchSettings;
			_httpClientFactory = httpClientFactory;
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[HttpGet("auth")]
		public async Task<IActionResult> Index(string code, string scope) {
			var request = _context.HttpContext.Request;
			var _baseURL = $"{request.Scheme}://{request.Host}"; // http://localhost:5000
			var redirectURL = $"{_baseURL}/twitch/auth";
			string tokenRequestUrl = $"https://id.twitch.tv/oauth2/token?client_id={_twitchSettings.ClientId}&client_secret={_twitchSettings.ClientSecret}&code={code}&grant_type=authorization_code&redirect_uri={redirectURL}";
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, tokenRequestUrl);

			var httpClient = _httpClientFactory.CreateClient();
			var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

			if(httpResponseMessage.IsSuccessStatusCode) {
				var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
				TwitchAuth twitchAuth = JsonConvert.DeserializeObject<TwitchAuth>(contentStream);
				TwitchUser twitchUser = await GetTwitchUser(twitchAuth.AccessToken);
				if(twitchUser != null && twitchUser.data[0] != null && twitchUser.data[0].id > 0) {
					ApplicationUser user = await _userManager.FindByEmailAsync(twitchUser.data[0].email);
					if (user == null) {
						var appUserNew = new ApplicationUser() {
							Email = twitchUser.data[0].email,
							TwitchId = twitchUser.data[0].id,
							UserName = twitchUser.data[0].login
						};
						var result = await _userManager.CreateAsync(appUserNew);
						if (result.Succeeded) {
							user = await _userManager.FindByEmailAsync(appUserNew.Email);
						} else {
							System.Diagnostics.Debug.WriteLine(result);
						}
					}
					if(user != null) {
						await _signInManager.SignInAsync(user, false, "twitch");
						return LocalRedirect("/authentication/login");
					}
				}
			}
			return LocalRedirect("/Error");
			//await _signInManager.SignInAsync(new ApplicationUser(), false, "twitch");
		}

		public async Task<TwitchUser> GetTwitchUser(string accessToken) {
			var httpRequestMessage = new HttpRequestMessage(
				HttpMethod.Get,
				"https://api.twitch.tv/helix/users") {
				Headers =
					{
						{ "Client-ID", _twitchSettings.ClientId },
						{ "Authorization", $"Bearer {accessToken}" }
					}
			};

			var httpClient = _httpClientFactory.CreateClient();
			var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

			if(httpResponseMessage.IsSuccessStatusCode) {
				var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
				TwitchUser twitchUser = JsonConvert.DeserializeObject<TwitchUser>(contentStream);
				return twitchUser;
			} else {
				return null;
			}
		}
	}
}
