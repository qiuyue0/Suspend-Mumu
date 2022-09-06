using System.Diagnostics;

namespace SuspendMuMu
{
    // 全局变量，用来存放当前选择的pid、进程名、vkCode
    public class Common
    {
        public static int Pid { get; set; }
        public static string ProessName { get; set; }
        public static int VkCode { get; set; }
        public static string KeyName { get; set; }
    }
    public enum Status
    {
        SUSPENDED,
        RESUMED,
        NOT_RUNNING
    }
    public static class GetStatus
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

                        return Status.SUSPENDED;
                    }
                    else
                    {
                        return Status.RESUMED;
                    }
                }
                else
                {
                    return Status.NOT_RUNNING;
                }
            }
            catch
            {
                return Status.NOT_RUNNING;
            }
        }
    }
}