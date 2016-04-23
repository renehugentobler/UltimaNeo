using System;
using System.Reflection;
using System.Text;

/// <summary>
///  websocket-sharp -  C# implementation of the WebSocket protocol client and server
///  http://sta.github.io/websocket-sharp 
///  sta.blockhead 4/21/2016  The MIT License
/// </summary>
using WebSocketSharp;
using WebSocketSharp.Net;
using WebSocketSharp.Server;

/// <summary>
///  cmdline - Command Line Parser
///  https://code.msdn.microsoft.com/Command-Line-Parser-Library-a8ba828a/sourcecode?fileId=24875&pathId=661435652
///  Ron Jacobs 4/9/2012  MS-LPL 
/// </summary>
using CmdLine;

using System.Threading.Tasks;

namespace UltimaNeo.WebSocket.Server
{

    [CommandLineArguments(Program = "UltimaNeo", Title = "", Description = "")]
    public class Arguments
    {
        [CommandLineParameter(Name = "cmd", ParameterIndex = 1, Required = true, Description = "Specifies the command to be executed.")]
        public string cmd { get; set; }

        [CommandLineParameter(Command = "?", Default = false, IsHelp = true, Description = "Show Help")]
        public bool ishelp { get; set; }

    }

    public class Ultimaneo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
            string msg = string.Empty;

            if (e.Data.Length == 0)
            {
            }
            else
            {
            }
        }
    }


    class Program
    {

        /*
                        if (e.Data.Length <= 4)
                        {
                            if (e.Data[0] != '>')
                            {
                            }
                            else
                            {
                                switch (e.Data.Remove(0, 1).ToUpper())
                                {
                                    case "EX":
                                        {
                                            msg = String.Format("{0:u} {1}", DateTime.Now, e.Data);
                                            Send(msg);
                                            wssv.Stop();
                                            Environment.Exit(-1);
                                        }
                                        break;
                                    default:
                                        { }
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (e.Data[3] !='!')
                            {
                            }
                            else
                            {
                                switch (e.Data.Substring(0, 3).ToUpper())
                                { 
                                    case "TST":
                                        {
                                            msg = String.Format("{0:u} {1}", DateTime.Now, e.Data.Remove(0, 4));
                                        }
                                        break;
                                    default:
                                        { }
                                        break;
                                }
                            }
                            Send(msg);
                        }
                    }
        */

        static void WebSocketServicSstatus()
        {
            Console.WriteLine("{0,-22} {1}", "Address", wssv.Address);
            Console.WriteLine("{0,-22} {1}", "AuthenticationSchemes", wssv.AuthenticationSchemes);
            Console.WriteLine("{0,-22} {1}", "IsListening", wssv.IsListening);
            Console.WriteLine("{0,-22} {1}", "IsSecure", wssv.IsSecure);
            Console.WriteLine("{0,-22} {1}", "KeepClean", wssv.KeepClean);
            Console.WriteLine("{0,-22} {1}", "Log", wssv.Log);
            Console.WriteLine("{0,-22} {1}", "Port", wssv.Port);
            Console.WriteLine("{0,-22} {1}", "Realm", wssv.Realm);
            Console.WriteLine("{0,-22} {1}", "ReuseAddress", wssv.ReuseAddress);
            Console.WriteLine("{0,-22} {1}", "SslConfiguration", wssv.SslConfiguration);
            Console.WriteLine("{0,-22} {1}", "UserCredentialsFinder", wssv.UserCredentialsFinder);
            Console.WriteLine("{0,-22} {1}", "WaitTime", wssv.WaitTime);
            Console.WriteLine("{0,-22} {1}", "WebSocketServices", wssv.WebSocketServices);
        }

        public static WebSocketServer wssv;
        public static Arguments arguments;

        static void Main(string[] args)
        {

            Console.WriteLine("Ultima Neo Server {0}", String.Format("{0}", String.Format("{0}", AssemblyName.GetAssemblyName(Assembly.GetAssembly(typeof(Program)).Location).Version.ToString())));
            Console.WriteLine();

            try
            {
                arguments = CommandLine.Parse<Arguments>();
            }
            catch (CommandLineHelpException helpException)
            {
                // User asked for help
                CommandLine.WriteLineColor(ConsoleColor.White, helpException.ArgumentHelp.GetHelpText(System.Console.BufferWidth));
                Environment.Exit(1);
            }
            catch (CommandLineException exception)
            {
                // Some other kind of command line error
                CommandLine.WriteLineColor(ConsoleColor.Red, exception.ArgumentHelp.Message);
                CommandLine.WriteLineColor(ConsoleColor.White, exception.ArgumentHelp.GetHelpText(System.Console.BufferWidth));

                Environment.Exit(1);
            }

            switch (arguments.cmd.ToUpper())
            {
                case "START":
                    {
                        wssv = new WebSocketServer("ws://localhost:8081");

                        wssv.AddWebSocketService<Ultimaneo>("/ultimaneo");
                        wssv.Start();
                        WebSocketServicSstatus();
                        Console.ReadKey(true);
                        wssv.Stop();
                    }
                    break;
                default: { } break;
            }
        }
    }
}

