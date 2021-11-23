using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

//Code source from
//http://stackoverflow.com/questions/71257/suspend-process-in-c-sharp

public static class ProcessExtension
{
    [DllImport("kernel32.dll")]
    private static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

    [DllImport("kernel32.dll")]
    private static extern uint SuspendThread(IntPtr hThread);

    [DllImport("kernel32.dll")]
    private static extern int ResumeThread(IntPtr hThread);

    public static void Suspend(this Process process)
    {
        foreach (ProcessThread thread in process.Threads)
        {
            IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
            ////if (pOpenThread == IntPtr.Zero)
            ////{
            //    break;
            //}
            _ = SuspendThread(pOpenThread);
        }
    }

    public static void Resume(this Process process)
    {
        foreach (ProcessThread thread in process.Threads)
        {
            IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
            if (pOpenThread == IntPtr.Zero)
            {
                break;
            }
            _ = ResumeThread(pOpenThread);
        }
    }
}