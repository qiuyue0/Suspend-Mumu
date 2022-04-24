using System.Diagnostics;
using System.Threading;
using System.Management;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;

namespace SuspendMuMu
{
    // 全局变量，用来存放当前选择的pid
    public class common 
    {
    private static int PID;

        public static int content
        {
            get { return PID; }
            set { PID = value; }
        }
    }
    public enum Status
    {
        Suspend,
        Resume,
        NotRunning
    }
    public static class Getstatus {
        public static Status GetThreadStatus(Process process)
        {
            try
            {
                if (null != process)
                {
                    ProcessThread thread = process.Threads[0];
                    if (thread.WaitReason == ThreadWaitReason.Suspended)
                    {

                        return Status.Suspend;
                    }
                    else
                    {
                        return Status.Resume;
                    }
                }
                else
                {
                    return Status.NotRunning;
                }
            }
            catch
            {
                return Status.Resume;
            }
        }
    }
}