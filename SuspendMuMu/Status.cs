using System.Diagnostics;
using System.Threading;

namespace SuspendMuMu
{
    public enum Status
    {
        Suspend,
        Resume,
        NotRunning
    }
    public class Getstatus { 
        public Status GetThreadStatus()
        {
            var ProcessName = "NebulaPlayer";
            Process[] processes = Process.GetProcessesByName(ProcessName);

            if (null != processes)
            {
                ProcessThread thread = processes[0].Threads[0];
                if (thread.WaitReason == ThreadWaitReason.Suspended)
                {
                    processes[0].Resume();
                    return Status.Resume;
                }
                else
                {
                    processes[0].Suspend();
                    return Status.Suspend;
                }
            }
            else
            {
                return Status.NotRunning;
            }
        }
    }
}