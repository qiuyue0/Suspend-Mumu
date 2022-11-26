using System;
using System.ComponentModel;
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

    [DllImport("ntdll.dll")]
    private static extern int NtQueryInformationProcess(IntPtr processHandle, int processInformationClass, ref ParentProcessUtilities processInformation, int processInformationLength, out int returnLength);
    
    [StructLayout(LayoutKind.Sequential)]
    public struct ParentProcessUtilities
    {
        // These members must match PROCESS_BASIC_INFORMATION
        internal IntPtr Reserved1;
        internal IntPtr PebBaseAddress;
        internal IntPtr Reserved2_0;
        internal IntPtr Reserved2_1;
        internal IntPtr UniqueProcessId;
        internal IntPtr InheritedFromUniqueProcessId;
    }
    public static void Suspend(this Process process)
    {
        foreach (ProcessThread thread in process.Threads)
        {
            IntPtr pOpenThread = OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);
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
    public static Process Parent(this Process process)
    {
        IntPtr handle = process.Handle;
        ParentProcessUtilities pbi = new ParentProcessUtilities();
        int returnLength;
        int status = NtQueryInformationProcess(handle, 0, ref pbi, Marshal.SizeOf(pbi), out returnLength);
        if (status != 0)
            throw new Win32Exception(status);

        try
        {
            return Process.GetProcessById(pbi.InheritedFromUniqueProcessId.ToInt32());
        }
        catch (ArgumentException)
        {
            // not found
            return null;
        }
    }
}