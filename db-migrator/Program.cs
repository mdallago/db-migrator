using db_migrator;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;

AppDomain.CurrentDomain.ProcessExit += (s, e) => ExecuteShutdownTasks();
Console.CancelKeyPress += (s, e) => ExecuteShutdownTasks();

static void ExecuteShutdownTasks()
{
    AnsiConsole.MarkupLine("Shutting down...");
}

var rootCommand = new RootCommand("Database migrator runner.");
rootCommand.AddCommand(new RunCommand());
rootCommand.AddCommand(new ShowCommand());

var parser = new CommandLineBuilder(rootCommand)
.UseDefaults()
.Build();

return await parser.InvokeAsync(args);