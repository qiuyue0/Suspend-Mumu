using System;
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
            List<int> result = new List<int>();
            string wmiQuery = string.Format("select ThreadCount,Handle from Win32_Process where Name='{0}'", ProcessName);
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(wmiQuery))
            {
                using (ManagementObjectCollection retObjectCollection = searcher.Get())
                {
                    foreach (ManagementObject retObject in retObjectCollection)
                    {
                        uint ThreadCount = (uint)retObject["ThreadCount"];
                        int Handle = int.Parse((string)retObject["Handle"]);
                        if (ThreadCount > 100) result.Add(Handle);

                    }
                    try
                    {
                        if (whichone == "主窗口")
                        {
                            return result[0];
                        }
                        else if (whichone == "多开窗口1")
                        {
                            return result[1];
                        }
                        else if (whichone == "多开窗口2")
                        {
                            return result[2];
                        }
                    }
                    catch
                    {
                        return -1;
                    }
                }
                return -1;
            }

        }
    }
}