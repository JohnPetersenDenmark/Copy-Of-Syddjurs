using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Syddjurs.Utilities
{
    internal static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> SendWithTokenAsync(this HttpClient client, HttpRequestMessage request)
        {
            var token = await SecureStorage.GetAsync("auth_token");

            if (!string.IsNullOrWhiteSpace(token))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                SecureStorage.Remove("auth_token");

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Shell.Current.GoToAsync("LoginPage");
                    await Application.Current.MainPage.DisplayAlert("Session expired", "Please log in again.", "OK");
                });
            }

            return response;
        }
    }
}
