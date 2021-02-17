using System;
using System.Collections.Generic;
using System.Drawing;

using System.Windows.Forms;

namespace Dynamic_Screen_Capture
{
    class DynamicScan
    {
        Bitmap bmpBuffer1;  //上一张的截图
        Bitmap bmpBuffer2;  //当前的截图
        Bitmap bmpBufferTemp;   //对当前截图画出变化区域后的图片

        int iRow = 0;   //行扫描变量
        int i = 0;  //列扫描变量
        int step = 2;   //隔行扫描的步长


        //随机数生成器
        Random random = new Random();
        public string SavePath { get; set; } = "D:\\ImageTemp\\";


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
            string path = SavePath + fileName + ".bmp";
            bmp.Save(path);
        }




        //动态分块隔行扫描算法
        //三个bmp，一个代表上一张截图，一个代表当前截图，一个代表在当前截图画出了变化区域的图片
        public void RecordScreen()
        {
            //对于第一张截图（第一帧）
            if (bmpBuffer1 == null)
            {
                bmpBuffer1 = GetScreen();
                SaveImage(bmpBuffer1);
                return;
            }


            bmpBuffer2 = GetScreen();
            bmpBufferTemp = new Bitmap(bmpBuffer2); //复制当前图像，准备用于画矩形。

            iRow = 0;

            List<Rectangle> rectList = new List<Rectangle>();


            while (iRow < bmpBuffer2.Height)
            {
                Rectangle rect = new Rectangle();
                if(GetChangedRegion(bmpBuffer1, bmpBuffer2, ref rect))
                {
                    rectList.Add(rect);
                }               
            }


            using (Graphics g = Graphics.FromImage(bmpBufferTemp))
            {
                Brush brush = new SolidBrush(Color.Red);
                Pen pen = new Pen(brush, 1);               

                for(int i=0;i<rectList.Count;i++)
                {
                    g.DrawRectangle(pen, rectList[i]);
                }
                
                SaveImage(bmpBufferTemp);
            }

            bmpBuffer1.Dispose();   //销毁上一张图片，防止内存占用过多
            bmpBufferTemp.Dispose();
            bmpBuffer1 = bmpBuffer2;
        }



        

        //获取变化区域
        private bool GetChangedRegion(Bitmap bmpPre, Bitmap bmpNow, ref Rectangle rect)
        {
            //int x = random.Next(1600);
            //int y = random.Next(800);

            //Rectangle changedRegion = new Rectangle(x, y, 800, 600);

            //由于截图总是截全屏，所以前后两张bmp的长宽都是一样的。
            int width = bmpNow.Width;
            int height = bmpNow.Height;

            Point p1 = new Point();  //矩形左上角的坐标
            Point p2 = new Point();  //矩形右下角的坐标
            bool isFirstUnequal = true;    //是否第一次扫描到了不同的像素
            bool isLineChanged = false;     //本行是否有不同的像素点


            //比较像素是否不同
            for(;iRow<height;)
            {
                for(i=0;i<width;i++)
                {                  
                    if (bmpNow.GetPixel(i,iRow)!=bmpPre.GetPixel(i,iRow))  //扫描到不同像素点
                    {
                        isLineChanged = true;
                        if (isFirstUnequal)  //第一次扫描到不同的像素点
                        {
                            p1.X = i;
                            p1.Y = iRow;
                            p2.X = i;
                            p2.Y = iRow;
                            isFirstUnequal = false;
                            

                        }
                        else   //再次扫描到不同的像素点
                        {
                            if (i < p1.X)
                                p1.X = i;
                            if (i > p2.X)
                                p2.X = i;
                            if(iRow > p2.Y)
                                p2.Y = iRow;

                            
                        }
                    }
                }

                iRow += step;

                if (isLineChanged)
                {
                    isLineChanged = false;
                }
                else
                {                   
                    break;
                }
                    
            }

            if(p1==p2)
            {
                return false;
            }
            else
            {
                rect =  new Rectangle(p1.X, p1.Y, p2.X - p1.X+1, p2.Y - p1.Y+1);
                return true;
            }
            
        }

    }
}
