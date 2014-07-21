using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using SEModAPIExtensions.API;
using SEModAPIExtensions.API.Plugin;
using SEModAPIExtensions.API.Plugin.Events;
using SEModAPIInternal.API.Common;
using SEModAPIInternal.Support;
using SEModAPIInternal.API.Server;

namespace Log_Scraper_Plugin
{
    public class Core : PluginBase, IChatEventHandler
    {
        public static string ArchivePath;
        public static string CurrentLogPath;
        public static string ParentPath;
        public Core()
        {
        }
        public override void Init()
        {
            ParentPath =
    Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            do
            {
                ParentPath = Directory.GetParent(ParentPath).ToString();
            } while (ParentPath.Contains("Mods"));
            ArchivePath =
                ParentPath
                + @"\Archived Logs"
                + @"\" + "Internal_Chat"
                + @"\" + DateTime.Now.ToString("yyyy")
                + @"\" + DateTime.Now.ToString("MMMM");
            CurrentLogPath =
                ArchivePath
                + @"\"
                + DateTime.Now.ToString("dd-MMM")
                + ".txt";
            GenerateNewLog();
        }
        public override void Update()
        {
        }
        public override void Shutdown()
        {
        }
        public void OnChatReceived(ChatManager.ChatEvent chatEvent)
        {
            DateTimeCheck();
            try
            {
                DateTime timestamp = chatEvent.timestamp;
                ulong sourceUserId = chatEvent.sourceUserId;
                string message = chatEvent.message;
                string type = chatEvent.type.ToString();
                System.IO.File.AppendAllText(CurrentLogPath, timestamp.ToString("[yyyy-MM-dd HH:MM:ss.fff]") + "   " + "<" + sourceUserId + ">" + "   "+ "Message:  " + message + "\r\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void DateTimeCheck()
        {
            if (DateTime.Now.ToString("dd-MMM") != Path.GetFileNameWithoutExtension(CurrentLogPath))
            {
                ArchivePath =
                    ParentPath +
                    @"\Archived Logs"
                    + @"\" + "Internal_Chat"
                    + @"\" + DateTime.Now.ToString("yyyy")
                    + @"\" + DateTime.Now.ToString("MMMM");
                CurrentLogPath =
                    ArchivePath
                    + @"\"
                    + DateTime.Now.ToString("dd-MMM")
                    + ".txt";
                GenerateNewLog();
            }
        }
        public void GenerateNewLog()
        {
            try
            {
                Directory.CreateDirectory(ArchivePath);
                File.Open(CurrentLogPath, FileMode.Append, FileAccess.Write).Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public void OnChatSent(ChatManager.ChatEvent chatEvent)
        {

            DateTimeCheck();
            try
            {
                DateTime timestamp = chatEvent.timestamp;
                ulong sourceUserId = chatEvent.sourceUserId;
                ulong remoteUserId = chatEvent.remoteUserId;
                string message = chatEvent.message;
                ushort priority = chatEvent.priority;
                string type = chatEvent.type.ToString();
                System.IO.File.AppendAllText(CurrentLogPath, timestamp.ToString("[yyyy-MM-dd HH:MM:ss.fff]") + "   " + "<--------SERVER-------->" + "   "+ "Message:  " + message + "\r\n");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}  
