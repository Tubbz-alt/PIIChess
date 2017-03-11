using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAsync.Chess.UI
{
    public class ChessConsoleCommander
    {
        private static object commandLocker = new object();
        private static ChessCommandProcessor Commander;
        private static IChessRenderEngine Render;

        static ChessConsoleCommander()
        {
            Render = new ConsoleRenderEngine();
            Commander = new ChessCommandProcessor(Render);
            Render.OnAsyncMessageWasShown += Render_OnAsyncMessageWasShown;
        }
        public static void StartCommandLine()
        {
            Render.RenderWelcomeMessage();
            Render.RenderHelp();
            bool finishGame = false;
            do
            {
                finishGame = GetAndProcessCommand();
            } while (!finishGame);
            Render.ShowMessage("bye.");
        }

        private static void Render_OnAsyncMessageWasShown(object sender, EventArgs e)
        {           
            GetAndProcessCommand();            
        }

        private static bool  GetAndProcessCommand()
        {
            Commander.ProcessCommand("PROMPT", true);
            string currentCommand = Console.ReadLine();
            lock (commandLocker) //if a command is in progress I wait for it.
            {
                //Check if the command is not an internal command.
                return Commander.ProcessCommand(currentCommand, false);
            }
        }
    }
}
