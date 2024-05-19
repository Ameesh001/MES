using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PointOfSale.Data.LogMaker
{
    public class Loggers
    {
        public void LogWriter(string msg)
        {
            var str = "TRUE";
            var path = "C:\\SoftLogs";

            if (str == "TRUE")
            {
                try
                {
                    string CurrentPath = path + "\\ChequeLogs";

                    if (!Directory.Exists(CurrentPath))
                    {
                        Directory.CreateDirectory(CurrentPath);
                    }

                    CurrentPath = CurrentPath + "\\" + DateTime.Now.ToString("ddMMyyyy") + "_log.txt";
                    if (!File.Exists(CurrentPath))
                    {
                        File.Create(CurrentPath).Close();
                    }
                    StreamWriter logWriter = new StreamWriter(CurrentPath, true);
                    logWriter.WriteLine(DateTime.Now.ToString("hh:mm:ss") + " : " + msg);
                    logWriter.Close();
                    logWriter.Dispose();

                }
                catch (Exception ex)
                {
                    try
                    {
                        string CurrentPath = @"C:\ChequeLogs";

                        if (!Directory.Exists(CurrentPath))
                        {
                            Directory.CreateDirectory(CurrentPath);
                        }
                        CurrentPath = CurrentPath + "\\" + DateTime.Now.ToString("ddMMyyyy") + "_log.txt";
                        if (!File.Exists(CurrentPath))
                        {
                            File.Create(CurrentPath).Close();
                        }
                        StreamWriter logWriter = new StreamWriter(CurrentPath, true);
                        logWriter.WriteLine(DateTime.Now.ToString("hh:mm:ss") + " : " + ex);
                        logWriter.Close();
                        logWriter.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }

    }
}
