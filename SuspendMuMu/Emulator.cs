using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SuspendMuMu
{
    public static class Emulator
    {
        public static int GetEmulator(string ProcessName, string whichone)
        {
            try
            {
                List<Process> result = new List<Process>();
                // 麻了，终于找到了可靠的判断方法
                Process[] processes = Process.GetProcessesByName(ProcessName);
                foreach (Process process in processes)
                {
                    if (GetCommandLine.IsPcr(process))
                    {
                        result.Add(process);
                    }
                }
                result.Sort(SortTime);
                int i;
                if (whichone == "主窗口" && result.Count >= 1) i = 0;
                else if (whichone == "多开窗口1" && result.Count >= 2) i = 1;
                else if (whichone == "多开窗口2" && result.Count >= 3) i = 2;
                // fallback 默认取最后开的窗口
                else i = result.Count;


                return result[i].Id;
            }
            catch
            {
                return 0;
            }
        }
        private static int SortTime(Process A, Process B)
        {
            DateTime creatTimeA = A.StartTime;
            DateTime creatTimeB = B.StartTime;
            if (creatTimeA > creatTimeB)
            {
                return 1;
            }
            else if (creatTimeA < creatTimeB)
            {
                return -1;
            }
            return 0;
        }
    }
}