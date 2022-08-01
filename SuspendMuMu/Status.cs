using System.Diagnostics;

namespace SuspendMuMu
{
    // 全局变量，用来存放当前选择的pid
    public class common
    {
        private static int PID;
        private static string Name;
        private static int vk_Code;
        private static string vk_Name;
        public static int content
        {
            get { return PID; }
            set { PID = value; }
        }

        public static string ProessName
        {
            get { return Name; }
            set { Name = value; }
        }

        public static int key
        {
            get { return vk_Code; }
            set { vk_Code = value; }
        }
        public static string keyName
        {
            get { return vk_Name; }
            set { vk_Name = value; }
        }
    }
    public enum Status
    {
        Suspend,
        Resume,
        NotRunning
    }
    public static class Getstatus
    {
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
                return Status.NotRunning;
            }
        }
    }
}