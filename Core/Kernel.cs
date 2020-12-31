using System;
using System.IO;
using Sys = Cosmos.System;
using Gizmo.ErrorHandler;
using Cosmos.HAL;
using Cosmos.Core;
using System.Timers;
using System.Threading;
using Gizmo.Utilities;
using Console = System.Console;
using Cosmos.System.FileSystem;
using Util = Gizmo.Utilities;
using System.Collections.Generic;
using Cosmos.System.FileSystem.Listing;
using System.Linq.Expressions;


namespace Gizmo.Core
{
    public class Kernel : Sys.Kernel
    {
        //Inits Filesys
        Sys.FileSystem.CosmosVFS fs;
        string current_directory = "0:\\";
        internal static string file;

        private string[] GetDirFadr(string adr) // Get Directories From Address
        {
            var dirs = Directory.GetDirectories(adr);
            return dirs;
        }

        protected override void BeforeRun()
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            //Displays file info
            fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);


        E:
            Console.WriteLine("Gizmo OS booted successfully. Enter password: ");
            string password = "1234";
            string input = Console.ReadLine();
            if (input == password)
            {
                Console.Clear();
                Run();
            }
            else
            {
                Console.WriteLine("WRONG PASSWORD. TRY AGAIN!");
                goto E;
            }

        }

        protected override void Run()
        {

            Console.Write(current_directory + "> ");
            string input = Console.ReadLine();

            //FILE SYS DIR
            string[] dirs = GetDirFadr(current_directory);

            if (input.StartsWith("DIR"))
            {
                foreach (var item in dirs)
                {
                    Console.WriteLine(item);
                }
            }

            //FILE SYS TYPE
            if (input.StartsWith("TYPE"))
            {
                string fs_type = Sys.FileSystem.VFS.VFSManager.GetFileSystemType("0:/");
                Console.WriteLine("File System Type: " + fs_type);
            }

            var directory_list = Sys.FileSystem.VFS.VFSManager.GetDirectoryListing("0:/");

            //FILE SYS READ
            if (input.StartsWith("READ"))
            {
                try
                {
                    foreach (var directoryEntry in directory_list)
                    {
                        var file_stream = directoryEntry.GetFileStream();
                        var entry_type = directoryEntry.mEntryType;
                        if (entry_type == Sys.FileSystem.Listing.DirectoryEntryTypeEnum.File)
                        {
                            byte[] content = new byte[file_stream.Length];
                            file_stream.Read(content, 0, (int)file_stream.Length);
                            Console.WriteLine("File name: " + directoryEntry.mName);
                            Console.WriteLine("File size: " + directoryEntry.mSize);
                            Console.WriteLine("Content: ");
                            foreach (char ch in content)
                            {
                                Console.Write(ch.ToString());
                            }
                            Console.WriteLine();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }

            //FILE SYS RDFILE


            //echo command
            if (input.StartsWith("echo ")) { Console.WriteLine(input.Remove(0, 5)); }

            //shows commands
            if (input.StartsWith("HELP"))
            {
                Console.WriteLine("********COMMANDS**************************");
                Console.WriteLine("* HELP- this menu                        *");
                Console.WriteLine("* CLR- clears screen                     *");
                Console.WriteLine("* REBOOT- reboots the system             *");
                Console.WriteLine("* SHTDWN- shutdowns system               *");
                Console.WriteLine("*                                        *");
                Console.WriteLine("********READ******************************");
                Console.WriteLine("* Cosmos OS- https://github.com/CosmosOS *");
                Console.WriteLine("* ABT- about this operating system       *");
                Console.WriteLine("******************************************");
                Console.WriteLine("*                                        *");
                Console.WriteLine("********SYSTEM INFO***********************");
                Console.WriteLine("* VER- displays version info             *");
                Console.WriteLine("******************************************");
                Console.WriteLine("*                                        *");
                Console.WriteLine("********FILE SYSTEM***********************");
                Console.WriteLine("* DIR- displays files in directory       *");
                Console.WriteLine("* TYPE- File System Type                 *");
                Console.WriteLine("* READ- Reads all files (Non-Executables)*");
                Console.WriteLine("******************************************");

            }

            if (input.StartsWith("CLR"))
            {
                Console.Clear();
            }

            if (input.StartsWith("REBOOT"))
            {
                Console.WriteLine("Would you like to Reboot Gizmo OS? Yes/No");
                string ans = Console.ReadLine();
                if (ans.ToLower() == "y" || ans.ToLower() == "yes" || ans.ToLower() == "Yes")
                {
                    Sys.Power.Reboot();
                }
                else
                {
                    Console.Clear();
                }
            }

            if (input.StartsWith("SHTDWN"))
            {
                Console.WriteLine("Would you like to Shutdown Gizmo OS? Yes/No");
                string ans = Console.ReadLine();
                if (ans.ToLower() == "y" || ans.ToLower() == "yes" || ans.ToLower() == "Yes")
                {
                    Sys.Power.Shutdown();
                }
                else
                {
                    Console.Clear();
                }
            }

            if (input.StartsWith("VER"))
            {
                Console.WriteLine("Gizmo OS 0.0.1");
            }

            if (input.StartsWith("ABT"))
            {
                Console.WriteLine("Cosmos OS User Kit with VS19");
            }

            if (input.StartsWith("MIV"))
            {
                Util.MIV.StartMIV();
            }

            if (input.StartsWith("BEEP"))
            {
                Util.BeepDemo.startBeepDemo();
            }

        }
    }
}