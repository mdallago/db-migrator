using Spectre.Console;
using System.CommandLine;

namespace db_migrator
{
    public class RunCommand : Command
    {
        private readonly Option<string> connStringOption = new(["--connection", "-c"], "Connection string to the database")
        {
            IsRequired = true
        };

        public RunCommand() : base("run", "Execute migrations")
        {
            AddOption(connStringOption);
            this.SetHandler(OnHandleRunCommand, connStringOption);
        }

        static Task<int> OnHandleRunCommand(string connString)
        {
            Helper.Header();

            if (!Helper.ValidateFolder())
            {
                return Task.FromResult(-1);
            }

            var upgrader = Helper.BuildMigrator(connString).Build();
            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                AnsiConsole.MarkupLine(Emoji.Known.CrossMark + " [bold red]ERROR[/] :cross_mark:");
                AnsiConsole.MarkupLineInterpolated($"[red]{result.Error}[/]");
                return Task.FromResult(-1);
            }

            AnsiConsole.MarkupLine(Emoji.Known.BeatingHeart + " [bold green]SUCCESS[/] :beating_heart: ");

            return Task.FromResult(0);
        }
    }
}
