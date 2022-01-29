using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using TwitchAuthDotnetCoreSPA.Models;
using TwitchAuthDotnetCoreSPA.Services;

namespace TwitchAuthDotnetCoreSPA.Controllers {
	[Route("twitch")]
	public class TwitchController : Controller {
		private readonly TwitchService _twitchService;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public TwitchController(TwitchService twitchService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) {
			_twitchService = twitchService;
			_userManager = userManager;
			_signInManager = signInManager;
		}
		[HttpGet("auth")]
		public async Task<IActionResult> Index(string code, string scope) {
			TwitchUser twitchUser = await _twitchService.GetTwitchUser(code, scope);
			if(twitchUser != null && twitchUser.data[0] != null && twitchUser.data[0].id > 0) {
				ApplicationUser user = await _userManager.FindByEmailAsync(twitchUser.data[0].email);
				if(user == null) {
					var appUserNew = new ApplicationUser() {
						Email = twitchUser.data[0].email,
						TwitchId = twitchUser.data[0].id,
						UserName = twitchUser.data[0].login
					};
					var result = await _userManager.CreateAsync(appUserNew);
					if(result.Succeeded) {
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
			return LocalRedirect("/Error");
		}
	}
}
