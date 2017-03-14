using Chess.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.UI
{
    public class ChessCommandProcessor
    {
        private ChessGame game;        
        private IChessRenderEngine RenderEngine;
        
        public ChessCommandProcessor(IChessRenderEngine renderEngine)
        {
            RenderEngine = renderEngine;           
        }



        public  bool ProcessCommand(string command, bool isInternal)
        {
            bool finishGame = false;
            if (!string.IsNullOrEmpty(command))
            {
                command = command.Trim().ToUpper();
                string[] parsedCommand = command.Split(' ');
                string cmd = parsedCommand[0];
                List<string> parameters = new List<string>();
                for (int paramsIndex = 1; paramsIndex < parsedCommand.Count(); paramsIndex++)
                {
                    parameters.Add(parsedCommand[paramsIndex]);
                }

                CommandCode cmdCode = CommandCode.UNKNOWN;
                try
                {
                    cmdCode = (CommandCode)Enum.Parse(typeof(CommandCode), cmd);
                } catch (Exception) {}

                switch (cmdCode)
                {
                    case CommandCode.EXIT:
                        finishGame = true;
                        break;
                    case CommandCode.CREATE:
                    #region CREATE NEW GAME COMMAND
                    if (parameters.Count() == 3)
                        {
                            string playerWhite = parsedCommand[1];
                            string playerBlack = parsedCommand[2];
                            string gameName = parsedCommand[3];
                            if (game == null)
                            {
                                game = new ChessGame();
                            }
                            try
                            {
                                game.PrepareGame(playerBlack, playerWhite, gameName);
                            }catch (ChessExceptionInvalidGameAction ex)
                            {
                                RenderEngine.ShowError(ex.ExceptionCode + ": " + ex.Message);
                            }
                            catch (Exception ex)
                            {
                                RenderEngine.ShowError("Unexpected error: " + ex.Message);
                            }

                            RenderEngine.ShowMessage("Game Created");
                        }
                        else
                        {
                            RenderEngine.ShowError("Invalid Parameter Number");
                        }
                    #endregion
                    break;
                    case CommandCode.HELP:
                        RenderEngine.RenderHelp();
                        break;
                    case CommandCode.CLEAR:
                        RenderEngine.Clear();
                        break;
                    case CommandCode.STATUS:
                    #region STATUS COMMAND
                    if (game != null)
                    {
                        try
                        {
                            RenderEngine.RenderGameStatus(game);
                            RenderEngine.RenderGameBoard(game);
                        }
                        catch (Exception ex)
                        {
                            RenderEngine.ShowError("Unexpected error: " + ex.Message);
                        }
                    }
                    else
                    {
                        RenderEngine.ShowError("No game found");
                    } 
                    #endregion
                    break;

                    case CommandCode.TIME:
                        #region TIME
                        if (game != null)
                        {
                            try
                            {
                                if (game.NextTurnPlayer != null)
                                {
                                    RenderEngine.RenderCurrentPlayerTime(game);
                                }
                                else
                                {
                                    RenderEngine.ShowError("No Active Player.");
                                }
                            }
                            catch (Exception ex)
                            {
                                RenderEngine.ShowError("Unexpected error: " + ex.Message);
                            }
                        }
                        else
                        {
                            RenderEngine.ShowError("No game found");
                        }
                        break; 
                    #endregion
                    case CommandCode.START:
                    #region START

                    if (game != null)
                    {
                        try
                        {
                            game.StartNewGame();
                            RenderEngine.ShowMessage("Game " + game.GameName + " has started at " + DateTime.Now);
                        }
                        catch (ChessExceptionInvalidGameAction ex)
                        {
                            RenderEngine.ShowMessage(ex.ExceptionCode + ": " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            RenderEngine.ShowError("Unexpected error: " + ex.Message);
                        }

                    }
                    else
                    {
                        RenderEngine.ShowError("No game found");
                    } 
                    #endregion
                    break;
                    case CommandCode.FINISH:
                        //game.NextPlayer.ResignGane();
                        break;
                    case CommandCode.PROMPT:
                        if (isInternal)
                        {
                            RenderEngine.PromptCommand(game);
                        }
                        else {
                            RenderEngine.ShowError("Unknwon Command");
                        }
                        break;
                    case CommandCode.MOVE:
                        #region MOVE
                        if (parameters.Count() == 2)
                        {
                            string from = parameters[0].ToLower();
                            string to = parameters[1].ToLower();
                            if (ChessPGN.IsValidAlgebraicPosition(from) && ChessPGN.IsValidAlgebraicPosition(to))
                            {
                                try
                                {
                                    if (game != null)
                                    {
                                        if (game.GameStatus == ChessGameStatus.ChessGameStatus_Playing)
                                        {
                                            game.NextTurnPlayer.MovePiece(from, to);
                                        }
                                        else
                                        {
                                            RenderEngine.ShowMessage("Error: Sorry, the game " + game.GameName + " has not started yet");
                                        }
                                    }
                                    else
                                    {
                                        RenderEngine.ShowMessage("No game found");
                                    }


                                }
                                catch (Exception e)
                                {
                                    RenderEngine.ShowMessage("Error: " + e.Message);
                                }

                            }
                            else
                            {
                                RenderEngine.ShowMessage("Invalid Movement");
                            }
                        } 
                        #endregion
                        break;
                    case CommandCode.DEBUG:
                        if (game != null)
                        {
                            RenderEngine.Debug(game);
                        }else
                        {
                            RenderEngine.ShowError("No game found");
                        }

                        break;

                    default:
                        RenderEngine.ShowMessage("Unknwon Command");
                        break;                    
                }               
            }
            return finishGame;
            
        }
    }

}
