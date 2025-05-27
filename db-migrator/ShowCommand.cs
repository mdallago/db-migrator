using Spectre.Console;
using System.CommandLine;

namespace db_migrator
{
    public class ShowCommand : Command
    {
        private readonly Option<string> connStringOption = new(["--connection", "-c"], "Connection string to the database")
        {
            IsRequired = true
        };

        public ShowCommand() : base("show", "Show migrations to execute")
        {
            AddOption(connStringOption);
            this.SetHandler(OnHandleShowCommand, connStringOption);
        }

        static Task<int> OnHandleShowCommand(string connString)
        {
            Helper.Header();

            if (!Helper.ValidateFolder())
            {
                return Task.FromResult(-1);
            }

            var upgrader = Helper.BuildMigrator(connString).Build();
            var result = upgrader.GetScriptsToExecute();

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            if (result.Count == 0)
            {    
                var m = new Markup(Emoji.Known.Warning + " [bold yellow]Warning[/] :warning:");
                m.Centered();
                AnsiConsole.Write(m);
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLineInterpolated($"[yellow]{"No migrations to show!"}[/]");
                return Task.FromResult(0);
            }

            AnsiConsole.MarkupLine(Emoji.Known.BeatingHeart + " [bold green]SUCCESS[/] :beating_heart: ");

            foreach (var script in result)
            {
                AnsiConsole.MarkupLineInterpolated($"[green]{script}[/]");
            }

            return Task.FromResult(0);
        }
    }
}
