using System.CommandLine;
using System.IO;

namespace GitHubApp.CLI
{
    public static class GlobalOptions
    {
        public static Option<int> AppId = new Option<int>(
            aliases: new string[] { "--app_id", "-a" },
            description: "An App ID of the application"
            )
        {
            Required = true
        };

        public static Option Pem = new Option(
            new string[] { "--pem", "-p" },
            "A path to the PEM file containing private key. For more info https://developer.github.com/apps/building-github-apps/authenticating-with-github-apps/")
        {
            Required = true,
            Argument = new Argument<FileInfo>().ExistingOnly()
        };

    }
}
