﻿using System;
using System.Management;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace SuspendMuMu
{
    // 现在是不可用状态
    public static class Emulator
    {
        public static int GetEmulator(string ProcessName,string whichone)
        {
            //List<int> result = new List<int>();
            string wmiQuery = string.Format("select CommandLine,Handle from Win32_Process where Name='{0}'", ProcessName);
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery))
            {
                using (ManagementObjectCollection retObjectCollection = searcher.Get())
                {
                    foreach (ManagementObject retObject in retObjectCollection)
                    {
                        string[] commandline = Regex.Split((string)retObject["CommandLine"], "\\s", RegexOptions.IgnoreCase);
                        if (whichone == "主窗口")
                        {
                            if (commandline[5] == "0")
                            {
                                return int.Parse(retObject["Handle"].ToString());
                            }
                        }
                        else if (whichone == "多开窗口")
                        {
                            if (commandline[5] == "1")
                            {
                                return int.Parse(retObject["Handle"].ToString());
                            }
                        }
                    }
                }
            }
            return -1;
        }
        
    }
}