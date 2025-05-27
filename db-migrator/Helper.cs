using DbUp;
using DbUp.Builder;
using DbUp.ScriptProviders;
using Spectre.Console;

namespace db_migrator
{
    public static class Helper
    {
        public const string MIGRATIONS_FOLDER_NAME = "Migrations";

        public static bool ValidateFolder()
        {
            if (!Directory.Exists(MIGRATIONS_FOLDER_NAME))
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                AnsiConsole.MarkupLine(Emoji.Known.CrossMark + " [bold red]ERROR[/] :cross_mark:");
                AnsiConsole.MarkupLineInterpolated($"[red]Migrations folder '{MIGRATIONS_FOLDER_NAME}' does not exist![/]");
                return false;
            }
            return true;
        }

        public static void Header()
        {
            AnsiConsole.Write(new FigletText("DB MIgrator").Centered().Color(Color.SteelBlue));
        }

        public static UpgradeEngineBuilder BuildMigrator(string connString)
        {
            return
            DeployChanges.To
                .SqlDatabase(connString)
                .WithScriptsFromFileSystem(MIGRATIONS_FOLDER_NAME, new FileSystemScriptOptions()
                {
                    IncludeSubDirectories = true,

                }, new DbUp.Engine.SqlScriptOptions()
                {
                    ScriptType = DbUp.Support.ScriptType.RunOnce
                })
                .LogToConsole();
        }
    }
}
