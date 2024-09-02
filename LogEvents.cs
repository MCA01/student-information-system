using System;
using System.IO;

namespace StundetInformation
{
    public static class LogEvents
    {
        public static void LogToFile(string Title, string LogMessages)
        {
            StreamWriter swLog;
            string fileName = @"C:\Users\mesut\OneDrive\Documents\IISExpress\Logs\StudentInfromationSystem\LogFile.txt";

            if (!File.Exists(fileName))
            {
                swLog = new StreamWriter(fileName);
            }
            else
            {
                swLog = File.AppendText(fileName);
            }

            swLog.WriteLine("Log Entry");
            swLog.WriteLine("{0} {1}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString());
            swLog.WriteLine("Message Title : {0}", Title);
            swLog.WriteLine("Message : {0}", LogMessages);
            swLog.WriteLine("-------------------------------------------------------------------------------------");
            swLog.WriteLine();
            swLog.Close();
        }
    }
}