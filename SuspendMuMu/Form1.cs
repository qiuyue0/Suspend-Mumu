using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using SuspendMuMu;

namespace BOWKeyBoardHook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Process[] processes = Process.GetProcessesByName("nebulaPlayer");
            //IList<int> infoList = new List<int>();
            //foreach (Process process in processes)
            //{
            //    infoList.Add(process.Id);
            //}
            //comboBox1.DataSource = infoList;
            //common.content = infoList[0];
            IList<string> infoList = new List<string>
            {
                "主窗口",
                "多开窗口1",
                "多开窗口2"
            };
            comboBox1.DataSource = infoList;
            int PID = Emulator.GetEmulator("nebula", "主窗口");
            common.content = PID;
            common.ProessName = "主窗口";
            common.key = 113;
            common.keyName = "F2";
            textBox1.KeyDown += new KeyEventHandler(KeyDownWork);
        }
        private void KeyDownWork(object sender, KeyEventArgs e)
        {
            k_hook.Stop();
            KeysConverter kc = new KeysConverter();
            int keyCode = (int)e.KeyCode;
            if (keyCode >= 17 & keyCode <= 18)      
            {
                MessageBox.Show(string.Format("不受支持的按键，请重新定义，当前快捷键为{0}", common.keyName));
            }
            else
            {
                textBox1.Text = kc.ConvertToString(keyCode);
                common.key = keyCode;
                common.keyName = textBox1.Text;
            }
            k_hook.Start();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            k_hook.ShowText += Showtext;
            k_hook.Start();//安装键盘钩子
            hookstart = true;
            //最小化主窗口
            //WindowState = FormWindowState.Minimized;
            //任务栏取消图标
            ShowInTaskbar = true;
        }

        private void Showtext(string text1, string text2, string text3)
        {
            if (text1 != null)
            {
                lb1.Text = text1;
            }

            if (text2 != null)
            {
                lb2.Text = text2;
            }

            if (text3 != null)
            {
                lb3.Text = text3;
            }
        }

        private void NotifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体
                WindowState = FormWindowState.Normal;
                //任务显示
                ShowInTaskbar = true;
            }
            //激活窗体
            Activate();
        }

        private void RestoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //还原窗体
                WindowState = FormWindowState.Normal;
                //任务显示
                ShowInTaskbar = true;
            }
            //激活窗体
            Activate();
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hookstart)
            {
                k_hook.Stop();
            }
            Dispose();
            Close();
        }

        private KeyboardHook k_hook = new KeyboardHook();
        private bool hookstart = true;

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                k_hook.Start();//安装键盘钩子
                hookstart = true;
                button1.Enabled = false;
                button2.Enabled = true;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.ToString());
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                k_hook.Stop();
                hookstart = false;
                button1.Enabled = true;
                button2.Enabled = false;
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.ToString());
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //取消关闭窗口
            //e.Cancel = true;
            //最小化主窗口
            WindowState = FormWindowState.Minimized;
            //任务栏取消图标
            ShowInTaskbar = false;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //任务栏取消图标
                ShowInTaskbar = false;
            }
        }

        private void Label1_Click(object sender, EventArgs e)
        {
        }

        private void Lb1_Click(object sender, EventArgs e)
        {
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void comboBox1_Click(object sender, EventArgs e)
        {

        }
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            
        }
        private void Update_lb1_text()
        {
            var PID = common.content;
            if (PID == -1)
            {
                Showtext("MuMu模拟器尚未运行", null, null);

            }
            else
            {
                Process process = Process.GetProcessById(PID);
                string text = "MuMu模拟器进程";
                switch (Getstatus.GetThreadStatus(process))
                {
                    case SuspendMuMu.Status.Suspend:
                        text += "已暂停";
                        break;
                    case SuspendMuMu.Status.Resume:
                        text += "已恢复";
                        break;
                    case SuspendMuMu.Status.NotRunning:
                        text += "尚未运行";
                        break;
                }
                lb1.Text = text;
            }


        }
        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            common.ProessName = comboBox1.SelectedItem.ToString();
        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void lb2_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            k_hook.Stop();
            int PID = Emulator.GetEmulator("nebula", comboBox1.SelectedItem.ToString());
            common.content = PID;
            common.ProessName = comboBox1.SelectedItem.ToString();
            k_hook.Start();
            Update_lb1_text();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("点击定义的按键以暂停/恢复mumu进程，黄蓝mumu均可使用。\n\n" +
                "默认按键为F2，可以在选择暂停快捷键右侧文本框按键以自定义，请不要使用Ctrl和Alt。\n\n" +
                "按键有大小写之分，因此建议使用F功能区按键以避免大小写带来的问题。\n\n" +
                "本体与多开窗口的识别是根据模拟器开启时间判断的，如果开启了两个多开窗口，关闭第一个多开窗口后，剩下一个会补位成为多开窗口1，因此使用时请注意是否选择了正确的窗口");
        }
    }
}