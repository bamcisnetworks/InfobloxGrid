using BAMCIS.Infoblox.Errors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;

namespace BAMCIS.Infoblox.PowerShell
{
    /// <summary>
    /// Provides common utilies used by a number of the PowerShell cmdlets
    /// </summary>
    internal class PSCommon
    {
        /// <summary>
        /// Recurses through inner exceptions in an exception and adds them to a list
        /// </summary>
        /// <param name="list">The list maintaining all of the exceptions</param>
        /// <param name="e">The exception to recurse through</param>
        /// <returns>An IEnumerable of exceptions</returns>
        private static IEnumerable<Exception> RecurseExceptions(List<Exception> list, Exception e)
        {
            list.Add(e);

            if (e.InnerException != null)
            {
                list.AddRange(RecurseExceptions(list, e.InnerException));
            }

            return list;
        }

        /// <summary>
        /// Flattens an aggregate exception into an IEnumerable of exceptions
        /// </summary>
        /// <param name="ae">The aggregate exception to flatten</param>
        /// <returns>The list of exceptions</returns>
        private static IEnumerable<Exception> IterateAggregate(AggregateException ae)
        {
            List<Exception> List = new List<Exception>();
            foreach (Exception e in ae.InnerExceptions)
            {
                List.AddRange(RecurseExceptions(new List<Exception>(), e));
            }

            return List;
        }

        /// <summary>
        /// Writes an IEnumerable of Exceptions to the provided PSHost console. This 
        /// function allows us to print in both PowerShell ISE and the regular PowerShell
        /// console. It tries to get a PSHost even if one isn't provided to
        /// </summary>
        /// <param name="exceptions">The exceptions to print</param>
        /// <param name="host">The PSHost to write the content out to.</param>
        private static void WriteExceptions(IEnumerable<Exception> exceptions, PSHost host = null)
        {
            if (host == null)
            {
                Runspace Runspace = Runspace.DefaultRunspace;

                //Attempt to get the default host value
                if (Runspace != null)
                {
                    Pipeline NewPipeline = Runspace.CreateNestedPipeline("Get-Variable host -ValueOnly", false);

                    /*   The returned object from the Cmdlet:
                     *   Name             : ConsoleHost
                     * 
                     *   Name             : Windows PowerShell ISE Host
                     *   Version          : 5.1.15063.138
                     *   InstanceId       : 71ed23e7-7b80-4273-a3d7-a0ab70dfb21b
                     *   UI               : System.Management.Automation.Internal.Host.InternalHostUserInterface
                     *   CurrentCulture   : en-US
                     *   CurrentUICulture : en-US
                     *   PrivateData      : Microsoft.PowerShell.Host.ISE.ISEOptions
                     *   DebuggerEnabled  : True
                     *   IsRunspacePushed : False
                     *   Runspace         : System.Management.Automation.Runspaces.LocalRunspace 
                     */

                    Collection<PSObject> Results = NewPipeline.Invoke();

                    if (Results.Count > 0)
                    {
                        host = Results[0].BaseObject as PSHost;
                    }
                }
            }

            //If a default host was found or one was provided
            if (host != null)
            {
                PSHostUserInterface UserInterface = host.UI;

                foreach (Exception Ex in exceptions)
                {
                    UserInterface.WriteLine();
                    UserInterface.WriteLine(ConsoleColor.Red, ConsoleColor.Black, "ERROR:            " + Ex.GetType().FullName);
                    UserInterface.WriteLine(ConsoleColor.Red, ConsoleColor.Black, "MESSAGE:          " + Ex.Message);

                    if (Ex.GetType() == typeof(InfobloxCustomException))
                    {
                        InfobloxCustomException ce = (InfobloxCustomException)Ex;
                        UserInterface.WriteLine(ConsoleColor.Red, ConsoleColor.Black, "HTTP ERROR:       " + ce.Error);
                        UserInterface.WriteLine(ConsoleColor.Red, ConsoleColor.Black, "HTTP CODE:        " + ce.HttpCode);
                        UserInterface.WriteLine(ConsoleColor.Red, ConsoleColor.Black, "HTTP STATUS CODE: " + ce.HttpStatusCode);
                        UserInterface.WriteLine(ConsoleColor.Red, ConsoleColor.Black, "HTTP MESSAGE:     " + ce.HttpMessage);
                    }

                    if (!String.IsNullOrEmpty(Ex.StackTrace))
                    {
                        UserInterface.WriteLine(ConsoleColor.Red, ConsoleColor.Black, "STACK TRACE:      " + Ex.StackTrace);
                    }

                    UserInterface.WriteLine();
                }
            }
            else //Otherwise, just use console write
            {
                foreach (Exception Ex in exceptions)
                {
                    ConsoleColor originalBackground = Console.BackgroundColor;
                    ConsoleColor originalForeground = Console.ForegroundColor;

                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine("ERROR:            " + Ex.GetType().FullName);
                    Console.WriteLine("MESSAGE:          " + Ex.Message);

                    if (Ex.GetType() == typeof(InfobloxCustomException))
                    {
                        InfobloxCustomException ce = (InfobloxCustomException)Ex;
                        Console.WriteLine("HTTP ERROR:       " + ce.Error);
                        Console.WriteLine("HTTP CODE:        " + ce.HttpCode);
                        Console.WriteLine("HTTP STATUS CODE: " + ce.HttpStatusCode);
                        Console.WriteLine("HTTP MESSAGE:     " + ce.HttpMessage);
                    }

                    if (!String.IsNullOrEmpty(Ex.StackTrace))
                    {
                        Console.WriteLine("STACK TRACE:      " + Ex.StackTrace);
                    }

                    //Reset console colors
                    Console.BackgroundColor = originalBackground;
                    Console.ForegroundColor = originalForeground;
                    Console.WriteLine();
                }
            }
        }

        /// <summary>
        /// Writes out a flattened AggregateException to the PS Console
        /// </summary>
        /// <param name="exception">The AggregateException to print</param>
        /// <param name="host">The host to print the exception information to, typically this.Host from a PSCmdlet.</param>
        internal static void WriteExceptions(AggregateException exception, PSHost host = null)
        {
            PSCommon.WriteExceptions(PSCommon.IterateAggregate(exception), host);
        }

        /// <summary>
        /// Writes out a flattened Exception to the PS Console
        /// </summary>
        /// <param name="exception">The exception to print</param>
        /// <param name="host">The host to print the exception information to, typically this.Host from a PSCmdlet.</param>
        internal static void WriteExceptions(Exception exception, PSHost host = null)
        {
            PSCommon.WriteExceptions(PSCommon.RecurseExceptions(new List<Exception>(), exception), host);
        }
    }
}
