using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dynamic_Screen_Capture
{
    public partial class Form1 : Form
    {
        bool isCapturing = false;   //是否正在截图
        int timeInterval = 1000;    //单位ms

        DynamicScan dynamicScanner = new DynamicScan();
        

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = dynamicScanner.SavePath;
            numericUpDown1.Value = timeInterval;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //开始录制
            if(!isCapturing)
            {
                isCapturing = true;

                //时间间隔
                timeInterval = (int)numericUpDown1.Value;
                timer1.Interval = timeInterval;
                numericUpDown1.Enabled = false;

                //button外观
                button1.BackColor = ColorTranslator.FromHtml("0xDB0909");
                button1.ForeColor = ColorTranslator.FromHtml("0xFFFFFF");
                button1.Text = "正在录屏...";

                //form外观
                this.Text = "正在录屏...";

                timer1.Enabled = true;
            }

            //结束录制
            else
            {
                isCapturing = false;

                numericUpDown1.Enabled = true;

                //button外观
                button1.BackColor = ColorTranslator.FromHtml("0xE1E1E1");
                button1.ForeColor = ColorTranslator.FromHtml("0x000000");
                button1.Text = "开始";

                //form外观
                this.Text = "Dynamic Screen Capturer";

                timer1.Enabled = false;
            }
            
        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            dynamicScanner.RecordScreen();
        }

        //修改文件路径
        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
            {
                string filePath = folderBrowserDialog.SelectedPath + "\\"; //记得在后面加上斜杠，表示这是一个目录
                dynamicScanner.SavePath = filePath;
                textBox1.Text = filePath;
            }
        }


    }

}
