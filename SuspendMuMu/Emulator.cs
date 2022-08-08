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
                // 第一个存Pcr进程PID，第二个存父进程PID
                List<ChildAndParentProcess> result = new List<ChildAndParentProcess>();
                // 麻了，终于找到了可靠的判断方法
                Process[] processes = Process.GetProcessesByName(ProcessName);
                foreach (Process process in processes)
                {
                    Tuple<bool, Int32> tuple = GetCommandLine.IsPcr(process);
                    bool isPcr = tuple.Item1;
                    if (isPcr)
                    {
                        Process parentProcess = Process.GetProcessById(tuple.Item2);
                        result.Add(new ChildAndParentProcess(process, parentProcess));
                    }
                }
                result.Sort(SortTime);
                int i;
                if (whichone == "主窗口" && result.Count >= 1) i = 0;
                else if (whichone == "多开窗口1" && result.Count >= 2) i = 1;
                else if (whichone == "多开窗口2" && result.Count >= 3) i = 2;
                // fallback 默认取最后开的窗口
                else i = result.Count;

                return result[i].child.Id;
            }
            catch
            {
                return 0;
            }
        }

        private static int SortTime(ChildAndParentProcess A, ChildAndParentProcess B)
        {
            DateTime creatTimeA = A.parent.StartTime;
            DateTime creatTimeB = B.parent.StartTime;
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

        internal class ChildAndParentProcess
        {
            public Process child;
            public Process parent;

            public ChildAndParentProcess(Process child, Process parent)
            {
                this.child = child;
                this.parent = parent;
            }
        }
    }
}