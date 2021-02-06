using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dynamic_Screen_Capture
{
    public partial class Form1 : Form
    {
        bool isCapturing = false;   //是否正在截图
        string filePath = "D:\\ImageTemp\\";     //默认文件保存路径。
        int timeInterval = 1000;    //单位ms
            

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = filePath;
            textBox2.Text = timeInterval.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        //截取屏幕图像
        public Bitmap GetScreen()
        {
            Rectangle ScreenArea = Screen.GetWorkingArea(this);
            Bitmap bmp = new Bitmap(ScreenArea.Width,ScreenArea.Height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, new Size(ScreenArea.Width, ScreenArea.Height));
            }

            return bmp;
        }

        //保存屏幕图像为文件
        public void SaveImage()
        {
            string fileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            string path = filePath + fileName + ".bmp";
            Bitmap bmp = GetScreen();
            bmp.Save(path);
            //bmp.Dispose();
        }


        //开始截图（录制）
        private void button1_Click(object sender, EventArgs e)
        {
            if(!isCapturing)
            {
                isCapturing = true;

                //时间间隔
                timeInterval = Int32.Parse(textBox2.Text);
                timer1.Interval = timeInterval;
                textBox2.Enabled = false;

                //button外观
                button1.BackColor = ColorTranslator.FromHtml("0xDB0909");
                button1.ForeColor = ColorTranslator.FromHtml("0xFFFFFF");
                button1.Text = "正在截图...";               
                timer1.Enabled = true;
            }
            else
            {
                isCapturing = false;

                textBox2.Enabled = true;

                //button外观
                button1.BackColor = ColorTranslator.FromHtml("0xE1E1E1");
                button1.ForeColor = ColorTranslator.FromHtml("0x000000");
                button1.Text = "开始";
                timer1.Enabled = false;
            }
            
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            SaveImage();
        }

        //修改文件路径
        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
            {
                filePath = folderBrowserDialog.SelectedPath + "\\"; //记得在后面加上斜杠，表示这是一个目录
                textBox1.Text = filePath;
            }

        }
    }
    
}
