using GitHubApp.CLI;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Threading.Tasks;

namespace GitHubApp
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            // Root command
            var cmd = new RootCommand()
            {
                // Commands
                Marketplace.Command,
            };

            cmd.Handler = CommandHandler.Create<string>(Program.ExecuteCommand);

            // Global options
            cmd.AddGlobalOption(GlobalOptions.AppId);
            cmd.AddGlobalOption(GlobalOptions.Pem);

            return await cmd.InvokeAsync(args);
        }


        static void ExecuteCommand(string command)
        {
            Console.WriteLine("ExecuteCommand");
        }
    }
}
