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

        Bitmap bmpBuffer1;  //上一张的截图
        Bitmap bmpBuffer2;  //当前的截图
        Bitmap bmpBufferTemp;   //对当前截图画矩形后的图片


        //随机数生成器
        Random random = new Random();

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
        public void SaveImage(Bitmap bmp)
        {
            string fileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            string path = filePath + fileName + ".bmp";
            //Bitmap bmp = GetScreen();
            bmp.Save(path);
            //bmp.Dispose();
        }


        //开始截图（开始录制）和结束
        private void button1_Click(object sender, EventArgs e)
        {
            //开始录制
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

            //结束录制
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
            //SaveImage();
            DynamicScan_DrawChangedRegion();
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


        //动态分块隔行扫描算法
        //三个bmp，一个代表上一张截图，一个代表当前截图，一个代表在当前截图画出了变化区域的图片
        private void DynamicScan_DrawChangedRegion()
        {
            if(bmpBuffer1 == null)
            {
                bmpBuffer1 = GetScreen();
                SaveImage(bmpBuffer1);
                return;
            }

            bmpBuffer2 = GetScreen();
            bmpBufferTemp = new Bitmap(bmpBuffer2); //复制当前图像，准备用于画矩形。


            using(Graphics g = Graphics.FromImage(bmpBufferTemp))
            {
                Brush brush = new SolidBrush(Color.Red);
                Pen pen = new Pen(brush, 1);
                g.DrawRectangle(pen, DynamicScan_GetChangedRegion());
                SaveImage(bmpBufferTemp);
            }

            bmpBuffer1.Dispose();
            bmpBufferTemp.Dispose();
            bmpBuffer1 = bmpBuffer2;
           
        }


        //获取变化区域
        private Rectangle DynamicScan_GetChangedRegion()
        {
            int x = random.Next(1600);
            int y = random.Next(800);

            Rectangle changedRegion = new Rectangle(x,y,800,600);
            return changedRegion;
        }
    }

}
