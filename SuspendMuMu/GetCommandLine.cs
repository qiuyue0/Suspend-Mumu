using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace SuspendMuMu
{
    class GetCommandLine
    {
        // 
        [StructLayout(LayoutKind.Sequential)]
        internal class NtProcessBasicInfo
        {
            public int ExitStatus = 0;
            public IntPtr PebBaseAddress = IntPtr.Zero;
            public IntPtr AffinityMask = IntPtr.Zero;
            public int BasePriority = 0;
            public IntPtr UniqueProcessId = IntPtr.Zero;
            public IntPtr InheritedFromUniqueProcessId = IntPtr.Zero;
        }

        [DllImport("ntdll.dll")]
        [ResourceExposure(ResourceScope.Machine)]
        public static extern int NtQueryInformationProcess(IntPtr processHandle, int query, NtProcessBasicInfo info, int size, int[] returnedSize);

        //打开一个已存在的进程对象，并返回进程的句柄
        [DllImportAttribute("kernel32.dll", EntryPoint = "OpenProcess")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        //关闭一个内核对象。其中包括文件、文件映射、进程、线程、安全和同步对象等。
        [DllImport("kernel32.dll")]
        private static extern void CloseHandle(IntPtr hObject);

        [DllImportAttribute("kernel32.dll", EntryPoint = "ReadProcessMemory")]
        public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, IntPtr lpNumberOfBytesRead);
        
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [MarshalAs(UnmanagedType.LPWStr)] string lpBuffer, IntPtr dwSize, IntPtr lpNumberOfBytesRead);
        
        public static int ReadMemoryValue(int baseAddress, int pid)
        {
            try
            {
                byte[] buffer = new byte[4];
                //获取缓冲区地址
                IntPtr byteAddress = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                //打开一个已存在的进程对象  0x1F0FFF 最高权限
                IntPtr hProcess = OpenProcess(0x1F0FFF, false, pid);
                //将制定内存中的值读入缓冲区
                ReadProcessMemory(hProcess, (IntPtr)baseAddress, byteAddress, 4, IntPtr.Zero);
                //关闭操作
                CloseHandle(hProcess);
                //从非托管内存中读取一个 32 位带符号整数。
                return Marshal.ReadInt32(byteAddress);
            }
            catch
            {
                return 0;
            }
        }
        // 获取是否为pcr窗口的同时传出父进程pid用于以后排序
        public static Tuple<bool, Int32> IsPcr(Process process)
        {
            try
            {
                IntPtr handle = process.Handle;

                // 实例化NtProcessBasicInfo用于接收NtQueryInformationProcess输出结果
                NtProcessBasicInfo info = new NtProcessBasicInfo();

                //https://docs.microsoft.com/zh-cn/windows/win32/api/winternl/nf-winternl-ntqueryinformationprocess?redirectedfrom=MSDN
                //https://referencesource.microsoft.com/#System/compmod/microsoft/win32/NativeMethods.cs,a18802cadd278e0c
                //使用PEB读取程序commandLine
                NtQueryInformationProcess(handle, 0, info, Marshal.SizeOf(info), null);

                //(Address)processParameters = PebBaseAddress + 0x10
                int processParameters = (int)info.PebBaseAddress + 0x10;

                //(Address) commandLine = processParameters->content + 0x40
                int commandLine = ReadMemoryValue(processParameters, process.Id) + 0x40;

                // length = commandLine->content 
                short length = (short)ReadMemoryValue(commandLine + 0x0, process.Id);

                // (Address) buffer = commandLine->content + 0x4
                IntPtr buffer = (IntPtr)ReadMemoryValue(commandLine + 0x4, process.Id);

                // buffer = commandLine->content
                string s = new string('\0', length / 2);
                ReadProcessMemory(handle, buffer, s, new IntPtr(length), IntPtr.Zero);


                if (s.StartsWith("com.bilibili.priconne")) return Tuple.Create(true, info.InheritedFromUniqueProcessId.ToInt32());

                return Tuple.Create(false, 0);
            }
            catch
            {
                return Tuple.Create(false, 0);
            }
        }
    }
}
