using System;
using System.Drawing;

using System.Windows.Forms;

namespace Dynamic_Screen_Capture
{
    class DynamicScan
    {
        Bitmap bmpBuffer1;  //上一张的截图
        Bitmap bmpBuffer2;  //当前的截图
        Bitmap bmpBufferTemp;   //对当前截图画矩形后的图片
        string filePath = "D:\\ImageTemp\\";    //文件保存路径。

        //随机数生成器
        Random random = new Random();

        public void setSavePath(string s) { filePath = s; }
        public string getSavePath() { return filePath; }

        //截取屏幕图像
        private Bitmap GetScreen()
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            Bitmap bmp = new Bitmap(width, height);
            using (var g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, new Size(width, height));
            }

            return bmp;
        }

        //保存屏幕图像为文件
        private void SaveImage(Bitmap bmp)
        {
            string fileName = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            string path = filePath + fileName + ".bmp";
            //Bitmap bmp = GetScreen();
            bmp.Save(path);
            //bmp.Dispose();
        }

        //动态分块隔行扫描算法
        //三个bmp，一个代表上一张截图，一个代表当前截图，一个代表在当前截图画出了变化区域的图片
        public void DrawChangedRegion()
        {
            if (bmpBuffer1 == null)
            {
                bmpBuffer1 = GetScreen();
                SaveImage(bmpBuffer1);
                return;
            }

            bmpBuffer2 = GetScreen();
            bmpBufferTemp = new Bitmap(bmpBuffer2); //复制当前图像，准备用于画矩形。


            using (Graphics g = Graphics.FromImage(bmpBufferTemp))
            {
                Brush brush = new SolidBrush(Color.Red);
                Pen pen = new Pen(brush, 1);
                g.DrawRectangle(pen, GetChangedRegion());
                SaveImage(bmpBufferTemp);
            }

            bmpBuffer1.Dispose();   //销毁上一张图片，腾出内存空间
            bmpBufferTemp.Dispose();
            bmpBuffer1 = bmpBuffer2;

        }


        //获取变化区域
        private Rectangle GetChangedRegion()
        {
            int x = random.Next(1600);
            int y = random.Next(800);

            Rectangle changedRegion = new Rectangle(x, y, 800, 600);
            return changedRegion;
        }

    }
}
