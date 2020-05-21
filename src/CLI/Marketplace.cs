using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace GitHubApp.CLI
{
    public class Marketplace
    {
        static public Command Command
        {
            get
            {
                var command = new Command("marketplace", "Get list of labels for reository")
                {
                    new Command("plans", "Lists all plans that are part of your GitHub Marketplace listing")
                    {
                        Handler = CommandHandler.Create<int, FileInfo>(GetPlansCommandAsync)
                    }
                };

                command.Handler = CommandHandler.Create<int, FileInfo>(GetListingCommandAsync);

                return command;
            }
        }


        static private async System.Threading.Tasks.Task GetListingCommandAsync(int app_id, FileInfo pem)
        {
            var github = GitHubApi.GetGitHubClient(app_id, pem);
            using var response = (await github.GetAsync("/marketplace_listing/plans/5289/accounts"))
                                              .EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(Json.Prettify(json));
        }

        static private async System.Threading.Tasks.Task GetPlansCommandAsync(int app_id, FileInfo pem)
        {
            var github = GitHubApi.GetGitHubClient(app_id, pem);
            using var response = (await github.GetAsync("/marketplace_listing/plans"))
                                              .EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            Console.WriteLine(Json.Prettify(json));
        }
    }
}
