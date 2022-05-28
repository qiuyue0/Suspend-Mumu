using SuspendMuMu;
using System;
using System.Windows.Forms;
using System.Management;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.Cryptography;

namespace BOWKeyBoardHook
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //Process[] processes = Process.GetProcessesByName("NebulaPlayer");
            //IList<int> infoList = new List<int>();
            //foreach (Process process in processes)
            //{
            //    infoList.Add(process.Id);
            //}
            //comboBox1.DataSource = infoList;
            //common.content = infoList[0];
            IList<string> infoList = new List<string>();
            infoList.Add("������");
            infoList.Add("�࿪����");
            comboBox1.DataSource = infoList;
            int PID = Emulator.GetEmulator("Nebula.exe", "������");
            common.content = PID;
            Update_lb1_text();
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
        private void comboBox1_Click(object sender,EventArgs e)
        {

        }
        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            Update_lb1_text();
            //Process[] processes = Process.GetProcessesByName("NebulaPlayer");
            //IList<int> infoList = new List<int>();
            //foreach (Process process in processes)
            //{
            //    infoList.Add(process.Id);
            //}
            //comboBox1.DataSource = null;
            //comboBox1.DataSource = infoList;
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
            k_hook.Stop();
            int PID = Emulator.GetEmulator("Nebula.exe", comboBox1.SelectedItem.ToString());
            common.content = PID;
            k_hook.Start();
            Update_lb1_text();
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
    }
}