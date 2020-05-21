using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GitHubApp.CLI
{
    public static class GitHubApi
    {
        #region Name Constants

        public const string BEARER     = "Bearer";
        public const string USER_AGENT = "GitHubAppCLI";
        public static Uri   GITHUB_API = new Uri("https://api.github.com/");

        #endregion


        #region GitHub Client

        public static HttpClient GetGitHubClient(int id, FileInfo file)
        {
            var generator = new GitHubJwt.GitHubJwtFactory(
                new GitHubJwt.FilePrivateKeySource(file.FullName),
                new GitHubJwt.GitHubJwtFactoryOptions
                {
                    AppIntegrationId = id,
                    ExpirationSeconds = 555
                }
            );

            var jwtToken = generator.CreateEncodedJwtToken();
            var client = new HttpClient { BaseAddress = GITHUB_API };

            client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github.machine-man-preview+json");
            client.DefaultRequestHeaders.UserAgent.ParseAdd(USER_AGENT);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(BEARER, jwtToken);

            return client;
        }

        public static async Task<string> GraphQL(this HttpClient github, string query)
        {
            var content = new StringContent(query, Encoding.UTF8);
            using var response = (await github.PostAsync("graphql", content)
                                              .ConfigureAwait(false))
                                              .EnsureSuccessStatusCode();

            var payload = await response.Content.ReadAsStringAsync()
                                                .ConfigureAwait(false);
            return payload;
        }

        #endregion


        #region Authentication

        //public static async Task<HttpClient> AuthenticateAsInstallation(this HttpClient github, LabelEventPayload payload)
        //{
        //    using (var content = new StringContent(string.Empty))
        //    {
        //        string method = $"/installations/{payload.InstallationId}/access_tokens";
        //        using var response = (await github.PostAsync(method, content))
        //                                          .EnsureSuccessStatusCode();
        //        var json = await response.Content.ReadAsStringAsync();
        //        var obj = JObject.Parse(json);
        //        var token = obj[TOKEN]?.Value<string>();
        //        github.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(BEARER, token);

        //        return github;
        //    }
        //}

        #endregion
    }
}
