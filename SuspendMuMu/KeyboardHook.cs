using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
namespace SuspendMuMu
{
    class KeyboardHook
    {
        public event KeyEventHandler KeyDownEvent;
        public event KeyPressEventHandler KeyPressEvent;
        public event KeyEventHandler KeyUpEvent;
        public event MouseEventHandler OnMouseActivity;

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);
        static int hKeyboardHook = 0; //�������̹��Ӵ����ĳ�ʼֵ
        static int hMouseHook = 0;
        //ֵ��Microsoft SDK��Winuser.h���ѯ

        public event CallBack showText;
        public delegate void CallBack(string text1, string text2, string text3);


        public const int WH_KEYBOARD_LL = 13;   //�̼߳��̹��Ӽ��������Ϣ��Ϊ2��ȫ�ּ��̼��������Ϣ��Ϊ13

        public const int WH_MOUSE_LL = 14;
        HookProc KeyboardHookProcedure; //����KeyboardHookProcedure��ΪHookProc����
        //���̽ṹ
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;  //��һ��������롣�ô��������һ����ֵ�ķ�Χ1��254
            public int scanCode; // ָ����Ӳ��ɨ����Ĺؼ�
            public int flags;  // ����־
            public int time; // ָ����ʱ����ǵ����ѶϢ
            public int dwExtraInfo; // ָ��������Ϣ��ص���Ϣ
        }


        [StructLayout(LayoutKind.Sequential)]
        public class POINT
        {
            public int x;
            public int y;
        }
        //Decla

        [StructLayout(LayoutKind.Sequential)]
        public class MouseHookStruct
        {
            public POINT pt;
            public int hwnd;
            public int wHitTestCode;
            public int dwExtraInfo;
        }

        //ʹ�ô˹��ܣ���װ��һ������
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        //���ô˺���ж�ع���
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //ʹ�ô˹��ܣ�ͨ����Ϣ���Ӽ�����һ������
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);


        // ȡ�õ�ǰ�̱߳�ţ��̹߳�����Ҫ�õ���
        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();

        //ʹ��WINDOWS API���������ȡ��ǰʵ���ĺ���,��ֹ����ʧЧ
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);





        public void Start()
        {
            // ��װ���̹���
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);
                //hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);
                //************************************
                //�����̹߳���
                //SetWindowsHookEx( 2,KeyboardHookProcedure, IntPtr.Zero, GetCurrentThreadId());//ָ��Ҫ�������߳�idGetCurrentThreadId(),
                //����ȫ�ֹ���,��Ҫ���ÿռ�(using System.Reflection;)
                //SetWindowsHookEx( 13,MouseHookProcedure,Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]),0);
                //
                //����SetWindowsHookEx (int idHook, HookProc lpfn, IntPtr hInstance, int threadId)���������Ӽ��뵽���������У�˵��һ���ĸ�������
                //idHook �������ͣ���ȷ�����Ӽ���������Ϣ������Ĵ�������Ϊ2��������������Ϣ�������̹߳��ӣ������ȫ�ֹ��Ӽ���������ϢӦ��Ϊ13��
                //�̹߳��Ӽ��������Ϣ��Ϊ7��ȫ�ֹ��Ӽ��������Ϣ��Ϊ14��lpfn �����ӳ̵ĵ�ַָ�롣���dwThreadId����Ϊ0 ����һ���ɱ�Ľ��̴�����
                //�̵߳ı�ʶ��lpfn����ָ��DLL�еĹ����ӳ̡� �������⣬lpfn����ָ��ǰ���̵�һ�ι����ӳ̴��롣���Ӻ�������ڵ�ַ�������ӹ����κ�
                //��Ϣ���������������hInstanceӦ�ó���ʵ���ľ������ʶ����lpfn��ָ���ӳ̵�DLL�����threadId ��ʶ��ǰ���̴�����һ���̣߳�������
                //�̴���λ�ڵ�ǰ���̣�hInstance����ΪNULL�����Ժܼ򵥵��趨��Ϊ��Ӧ�ó����ʵ�������threaded �밲װ�Ĺ����ӳ���������̵߳ı�ʶ��
                //���Ϊ0�������ӳ������е��̹߳�������Ϊȫ�ֹ���
                //************************************
                //���SetWindowsHookExʧ��
                if (hKeyboardHook == 0)
                {
                    Stop();
                    throw new Exception("��װ���̹���ʧ��");
                }
            }

            // ��װ��깳��
            //if (hMouseHook == 0)
            //{
            //    MouseHookProcedure = new HookProc(MouseHookProc);
            //    hMouseHook = SetWindowsHookEx(WH_MOUSE_LL, MouseHookProcedure, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);

            //    if (hMouseHook == 0)
            //    {
            //        Stop();
            //        throw new Exception("��װ��깳��ʧ��");
            //    }
            //}
            
        }
        public void Stop()
        {
            bool retKeyboard = true;
            bool retMouse = true;

            if (hKeyboardHook != 0)
            {
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }

            if (hMouseHook != 0)
            {
                retMouse = UnhookWindowsHookEx(hMouseHook);
                hMouseHook = 0;
            }
            
            if (!(retKeyboard)) throw new Exception("ж�ؼ��̹���ʧ�ܣ�");
            if (!(retMouse)) throw new Exception("ж����깳��ʧ�ܣ�");
        }

        //ToAsciiְ�ܵ�ת��ָ�����������ͼ���״̬����Ӧ�ַ����ַ�
        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, //[in] ָ������ؼ�������з��롣
                                         int uScanCode, // [in] ָ����Ӳ��ɨ����Ĺؼ��뷭���Ӣ�ġ��߽�λ�����ֵ�趨�Ĺؼ�������ǣ���ѹ��
                                         byte[] lpbKeyState, // [in] ָ�룬��256�ֽ����飬������ǰ���̵�״̬��ÿ��Ԫ�أ��ֽڣ����������״̬��һ���ؼ�������߽�λ���ֽ���һ�ף��ؼ����µ������£����ڵͱ��أ�������ñ������ؼ��Ƕ��л����ڴ˹��ܣ�ֻ����λ��CAPS LOCK������صġ����л�״̬��NUM�����͹��������������ԡ�
                                         byte[] lpwTransKey, // [out] ָ��Ļ������յ������ַ����ַ���
                                         int fuState); // [in] Specifies whether a menu is active. This parameter must be 1 if a menu is active, or 0 otherwise.

        //��ȡ������״̬
        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        private const int WM_KEYDOWN = 0x100;//KEYDOWN
        private const int WM_KEYUP = 0x101;//KEYUP
        private const int WM_SYSKEYDOWN = 0x104;//SYSKEYDOWN
        private const int WM_SYSKEYUP = 0x105;//SYSKEYUP

        private static void ExecuteOption(Status status, Process process)
        {
            switch (status)
            {
                case Status.Resume:
                    process.Suspend();
                    break;
                case Status.Suspend:
                    process.Resume();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }
        Status status = Status.Resume;
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
             
            KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
            if (MyKeyboardHookStruct.vkCode == 113&(wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                try
                {
                    var process = Process.GetProcessesByName("NebulaPlayer")[0]; // ���������MuMu��������ΪNebulaPlayer����Ϊ��׼���������ģ�����������޸�
                    if (showText != null)
                    {
                        ExecuteOption(status, process);
                        switch (status)
                        {
                            case Status.Resume:
                                showText("NebulaPlayer.exe Suspended.", null, null);
                                status = Status.Suspend;
                                return 1;
                            case Status.Suspend:
                                showText("NebulaPlayer.exe Resumed.", null, null);
                                status = Status.Resume;
                                return 1;
                        }
                    }
                }
                catch
                {
                    if (showText != null)
                    {
                        var text = "MuMuģ������δ����";
                        showText(text, null, null);
                        return 1;
                    }

                }
                
            }

            if (MyKeyboardHookStruct.vkCode == 176 && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                keybd_event(116, 0, 0, 0);
                return 1;
            }

            if (MyKeyboardHookStruct.vkCode == 172 && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                keybd_event(112, 0, 0, 0);
                return 1;
            }

            if (MyKeyboardHookStruct.vkCode == 166 && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                keybd_event(27, 0, 0, 0);
                return 1;
            }

            if (MyKeyboardHookStruct.vkCode == 175 && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                keybd_event(121, 0, 0, 0);
                return 1;
            }

            if (MyKeyboardHookStruct.vkCode == 174 && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
            {
                keybd_event(122, 0, 0, 0);
                return 1;
            }

            // ���������¼�
            if ((nCode >=0) && (KeyDownEvent != null || KeyUpEvent != null || KeyPressEvent != null))
            {
                // raise KeyDown
                if (KeyDownEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyDownEvent(this, e);
                }

                //���̰���
                if (KeyPressEvent != null && wParam == WM_KEYDOWN)
                {
                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);

                    byte[] inBuffer = new byte[2];
                    if (ToAscii(MyKeyboardHookStruct.vkCode, MyKeyboardHookStruct.scanCode, keyState, inBuffer, MyKeyboardHookStruct.flags) == 1)
                    {
                        KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
                        KeyPressEvent(this, e);
                    }
                }

                // ����̧��
                if (KeyUpEvent != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyUpEvent(this, e);
                }
            }
            //�������1���������Ϣ�������Ϣ����Ϊֹ�����ٴ��ݡ�
            //�������0�����CallNextHookEx��������Ϣ����������Ӽ������´��ݣ�Ҳ���Ǵ�����Ϣ�����Ľ�����
            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }


        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDBLCLK = 0x206;










        ~KeyboardHook()
        {
            Stop();
        }
    }
}