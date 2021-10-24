using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using SuspendMuMu;
namespace BOWKeyBoardHook
{
    
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            k_hook.showText += showtext;
            k_hook.Start();//��װ���̹���
            hookstart = true;
            //��С��������
            this.WindowState = FormWindowState.Minimized;
            //������ȡ��ͼ��
            this.ShowInTaskbar = false;
        }

        private void showtext(string text1, string text2, string text3)
        {
            if (text1 != null)
                lb1.Text = text1;
            if (text2 != null)
                lb2.Text = text2;
            if (text3 != null)
                lb3.Text = text3;
        }

        private void NotifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //��ԭ����
                this.WindowState = FormWindowState.Normal;
                //������ʾ
                this.ShowInTaskbar = true;
            }
            //�����
            this.Activate();
        }

        private void RestoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //��ԭ����
                this.WindowState = FormWindowState.Normal;
                //������ʾ
                this.ShowInTaskbar = true;
            }
            //�����
            this.Activate();
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (hookstart)
            {
                k_hook.Stop();
            }
            this.Dispose();
            this.Close();
        }

        KeyboardHook k_hook = new KeyboardHook();
        bool hookstart = true;
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
                MessageBox.Show(ex.ToString());
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
                MessageBox.Show(ex.ToString());
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //ȡ���رմ���
            e.Cancel = true;
            //��С��������
            this.WindowState = FormWindowState.Minimized;
            //������ȡ��ͼ��
            this.ShowInTaskbar = false;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //������ȡ��ͼ��
                this.ShowInTaskbar = false;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lb1_Click(object sender, EventArgs e)
        {

        }
    }
}
