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
            k_hook.Start();//安装键盘钩子
            hookstart = true;
            //最小化主窗口
            this.WindowState = FormWindowState.Minimized;
            //任务栏取消图标
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
                //还原窗体
                this.WindowState = FormWindowState.Normal;
                //任务显示
                this.ShowInTaskbar = true;
            }
            //激活窗体
            this.Activate();
        }

        private void RestoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //还原窗体
                this.WindowState = FormWindowState.Normal;
                //任务显示
                this.ShowInTaskbar = true;
            }
            //激活窗体
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
                k_hook.Start();//安装键盘钩子
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
            //取消关闭窗口
            e.Cancel = true;
            //最小化主窗口
            this.WindowState = FormWindowState.Minimized;
            //任务栏取消图标
            this.ShowInTaskbar = false;
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                //任务栏取消图标
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
