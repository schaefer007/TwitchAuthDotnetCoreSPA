using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TwitchAuthDotnetCoreSPA.Models;

namespace TwitchAuthDotnetCoreSPA.Services {
	public class TwitchService {
		private readonly TwitchSettings _settings;
		private readonly IHttpContextAccessor _context;
		private readonly IHttpClientFactory _httpClientFactory;

		public TwitchService(TwitchSettings settings, IHttpContextAccessor context, IHttpClientFactory httpClientFactory) {
			_settings = settings;
			_context = context;
			_httpClientFactory = httpClientFactory;
		}

		internal string GetClientId() {
			return _settings.ClientId;
		}

		internal string GetClientSecret() {
			return _settings.ClientSecret;
		}

		internal string GetTwitchLoginUrl() {
			var request = _context.HttpContext.Request;
			var _baseURL = $"{request.Scheme}://{request.Host}";
			var redirectURL = $"{_baseURL}/twitch/auth";
			return $"https://id.twitch.tv/oauth2/authorize?client_id={GetClientId()}&redirect_uri={redirectURL}&response_type=code&scope=user:read:email&force_verify=true";
		}

		internal async Task<TwitchUser> GetTwitchUser(string code, string scope) {
			var request = _context.HttpContext.Request;
			var _baseURL = $"{request.Scheme}://{request.Host}";
			var redirectURL = $"{_baseURL}/twitch/auth";
			string tokenRequestUrl = $"https://id.twitch.tv/oauth2/token?client_id={GetClientId()}&client_secret={GetClientSecret()}&code={code}&grant_type=authorization_code&redirect_uri={redirectURL}";
			var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, tokenRequestUrl);

			var httpClient = _httpClientFactory.CreateClient();
			var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

			if(httpResponseMessage.IsSuccessStatusCode) {
				var contentStream = await httpResponseMessage.Content.ReadAsStringAsync();
				TwitchAuth twitchAuth = JsonConvert.DeserializeObject<TwitchAuth>(contentStream);
				TwitchUser twitchUser = await GetUser(twitchAuth.AccessToken);
				return twitchUser;
			}

			return null;
		}

		internal async Task<TwitchUser> GetUser(string accessToken) {
			var httpRequestMessage = new HttpRequestMessage(
				HttpMethod.Get,
				"https://api.twitch.tv/helix/users") {
				Headers =
					{
						{ "Client-ID", GetClientId() },
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
