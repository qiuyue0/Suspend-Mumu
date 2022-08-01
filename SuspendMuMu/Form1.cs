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
                "������",
                "�࿪����1",
                "�࿪����2"
            };
            comboBox1.DataSource = infoList;
            int PID = Emulator.GetEmulator("nebula", "������");
            common.content = PID;
            common.ProessName = "������";
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
                MessageBox.Show(string.Format("����֧�ֵİ����������¶��壬��ǰ��ݼ�Ϊ{0}", common.keyName));
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
            k_hook.Start();//��װ���̹���
            hookstart = true;
            //��С��������
            //WindowState = FormWindowState.Minimized;
            //������ȡ��ͼ��
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
                //��ԭ����
                WindowState = FormWindowState.Normal;
                //������ʾ
                ShowInTaskbar = true;
            }
            //�����
            Activate();
        }

        private void RestoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //��ԭ����
                WindowState = FormWindowState.Normal;
                //������ʾ
                ShowInTaskbar = true;
            }
            //�����
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
                k_hook.Start();//��װ���̹���
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
            //ȡ���رմ���
            //e.Cancel = true;
            //��С��������
            WindowState = FormWindowState.Minimized;
            //������ȡ��ͼ��
            ShowInTaskbar = false;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                //������ȡ��ͼ��
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
                Showtext("MuMuģ������δ����", null, null);

            }
            else
            {
                Process process = Process.GetProcessById(PID);
                string text = "MuMuģ��������";
                switch (Getstatus.GetThreadStatus(process))
                {
                    case SuspendMuMu.Status.Suspend:
                        text += "����ͣ";
                        break;
                    case SuspendMuMu.Status.Resume:
                        text += "�ѻָ�";
                        break;
                    case SuspendMuMu.Status.NotRunning:
                        text += "��δ����";
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
            MessageBox.Show("�������İ�������ͣ/�ָ�mumu���̣�����mumu����ʹ�á�\n\n" +
                "Ĭ�ϰ���ΪF2��������ѡ����ͣ��ݼ��Ҳ��ı��򰴼����Զ��壬�벻Ҫʹ��Ctrl��Alt��\n\n" +
                "�����д�Сд֮�֣���˽���ʹ��F�����������Ա����Сд���������⡣\n\n" +
                "������࿪���ڵ�ʶ���Ǹ���ģ��������ʱ���жϵģ���������������࿪���ڣ��رյ�һ���࿪���ں�ʣ��һ���Ჹλ��Ϊ�࿪����1�����ʹ��ʱ��ע���Ƿ�ѡ������ȷ�Ĵ���");
        }
    }
}