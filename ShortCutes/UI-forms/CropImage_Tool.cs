using ShortCutes.src.Utils;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ShortCutes
{
    public partial class CropImage_Tool : Form
    {
        int cropX;
        int cropY;
        int cropWidth;
        int cropHeight;
        int croptempX;
        int croptempY;
        int oCropX;
        int oCropY;
        bool canpaintcrop = false;
        readonly Pen cropPen;
        Bitmap OriginalImageT;
        Bitmap ActualImage;
        public DialogResult DialogResult1 = DialogResult.No;
        readonly private string temppath = Path.GetTempPath() + @"\ShortCutes\";

        public CropImage_Tool()
        {
            InitializeComponent();
            DialogResult1 = DialogResult.No;

            pictureBox1.Image = ActualImage = OriginalImageT = new Bitmap(temppath + "tempORIGINAL.png");
            pictureBox1.BackColor = Color.FromArgb(22, 28, 38);

            cropPen = new Pen(Color.Black, 1);
            cropPen.DashStyle = DashStyle.DashDotDot;
            cropPen.Color = Color.FromArgb(0, 255, 0);
            cropPen.Width = 2;

            ChangeSize(OriginalImageT);
        }

        private void ChangeSize(Bitmap image)
        {
            if (image.Height > OriginalImageT.Width)
            {
                pictureBox1.Height = panel1.Height;
                pictureBox1.Width = (int)Math.Round((float)image.Width / image.Height * panel1.Width);
                pictureBox1.Location = new Point((int)Math.Round((float)(panel1.Width - pictureBox1.Width) / 2), 0);
            }
            else
            {
                pictureBox1.Width = panel1.Width;
                pictureBox1.Height = (int)Math.Round((float)image.Height / image.Width * panel1.Height);
                pictureBox1.Location = new Point(0, (int)Math.Round((float)(panel1.Height - pictureBox1.Height) / 2));
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Cursor = Cursors.Cross;
                croptempX = e.X;
                croptempY = e.Y;
                canpaintcrop = true;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Default;
            canpaintcrop = false;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (canpaintcrop)
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control)
                {
                    if (e.X > pictureBox1.Width)
                        Cursor.Position = pictureBox1.PointToScreen(new Point(pictureBox1.Width, e.Y));
                    else if (e.X < 0)
                        Cursor.Position = pictureBox1.PointToScreen(new Point(0, e.Y));
                    if (e.Y > pictureBox1.Height)
                        Cursor.Position = pictureBox1.PointToScreen(new Point(e.X, pictureBox1.Height));
                    else if (e.Y < 0)
                        Cursor.Position = pictureBox1.PointToScreen(new Point(e.X, 0));
                }

                cropX = croptempX;
                cropY = croptempY;
                int tempcropWidth = e.X - cropX;
                int tempcropHeight = e.Y - cropY;
                if (tempcropWidth > tempcropHeight)
                {

                    cropWidth = tempcropWidth;
                    cropHeight = tempcropWidth;
                }
                else
                {
                    cropWidth = tempcropHeight;
                    cropHeight = tempcropHeight;
                }

                oCropX = cropX;
                oCropY = cropY;
                if (cropWidth < 0)
                {
                    oCropX = e.X;
                    oCropY = e.Y;
                }
                cancrop = true;
                pictureBox1.Refresh();
            }
        }


        private void OriginalImage_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = ActualImage = OriginalImageT;
            SaveBTN.Enabled = cancrop = false;
            ChangeSize(OriginalImageT);
        }

        bool cancrop = false;
        private void CropBTN_Click(object sender, EventArgs e)
        {
            if (cancrop)
            {
                cancrop = false;
                SaveBTN.Enabled = true;
                Crop();
            }
            else
            {
                using (var info = new MessageForm("Keep the click and move the mouse to determine the area to crop", 0))
                    info.ShowDialog();
                return;
            }
        }

        private void SaveBTN_Click(object sender, EventArgs e)
        {
            if (Math.Abs(cropWidth) < 1 || ActualImage == OriginalImageT)
            {
                using (var info = new MessageForm("First crop the image to save it.\nIf don't wanna crop just close croper tool", 0))
                    info.ShowDialog();
                return;
            }

            ActualImage.Save(temppath + "temp.png");
            DialogResult1 = DialogResult.Yes;
            closeBtn.PerformClick();
        }


        private void SaveOriBTN_Click(object sender, EventArgs e)
        {
            OriginalImageT.Save(temppath + "temp.png");
            DialogResult1 = DialogResult.Yes;
            closeBtn.PerformClick();
        }

        private void Crop()
        {
            if (cropWidth > 0)
            {
                oCropX = cropX;
                oCropY = cropY;
            }
            Rectangle rect = new Rectangle((int)Math.Round(((float)oCropX / (float)pictureBox1.Width) * (float)ActualImage.Width),
                (int)Math.Round(((float)oCropY / (float)pictureBox1.Height) * (float)ActualImage.Height),
                (int)Math.Round(((float)Math.Abs(cropWidth) / (float)pictureBox1.Width) * (float)ActualImage.Width),
                (int)Math.Round(((float)Math.Abs(cropHeight) / (float)pictureBox1.Height) * (float)ActualImage.Height));

            Bitmap BoxImage = new Bitmap(ActualImage, ActualImage.Width, ActualImage.Height);

            Bitmap _img = new Bitmap((int)Math.Round(((float)Math.Abs(cropWidth) / (float)pictureBox1.Width) * (float)ActualImage.Width),
                (int)Math.Round(((float)Math.Abs(cropHeight) / (float)pictureBox1.Height) * (float)ActualImage.Height));

            Graphics g = Graphics.FromImage(_img);

            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.CompositingQuality = CompositingQuality.HighQuality;

            g.DrawImage(BoxImage, 0, 0, rect, GraphicsUnit.Pixel);
            pictureBox1.Image = ActualImage = _img;
            ChangeSize(_img);
        }

        protected override void OnPaint(PaintEventArgs pea)
        {
            Pen pen = new Pen(Color.FromArgb(80, 116, 128), 4);

            PointF pt1 = new PointF(0, 0);
            PointF pt2 = new PointF(0, Height);
            PointF pt3 = new PointF(Width, 0);
            PointF pt4 = new PointF(Width, Height);

            pea.Graphics.DrawLine(pen, pt1, pt2);
            pea.Graphics.DrawLine(pen, pt1, pt3);
            pea.Graphics.DrawLine(pen, pt2, pt4);
            pea.Graphics.DrawLine(pen, pt3, pt4);
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            ActualImage.Dispose();
            OriginalImageT.Dispose();
            pictureBox1.Image.Dispose();
            Dispose();
        }

        SolidBrush SBBlack = new SolidBrush(Color.FromArgb(175, 0, 0, 0));
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            if (canpaintcrop)
            {
                e.Graphics.FillRectangle(SBBlack, 0, 0, pictureBox1.Width, oCropY);
                e.Graphics.FillRectangle(SBBlack, 0, oCropY, oCropX, Math.Abs(cropHeight));
                e.Graphics.FillRectangle(SBBlack, 0, Math.Abs(cropWidth) + oCropY, pictureBox1.Width, pictureBox1.Height - (Math.Abs(cropWidth) + oCropY));
                e.Graphics.FillRectangle(SBBlack, Math.Abs(cropWidth) + oCropX, oCropY, pictureBox1.Width - (Math.Abs(cropWidth) + oCropX), Math.Abs(cropHeight));
                e.Graphics.DrawRectangle(cropPen, oCropX, oCropY, Math.Abs(cropWidth), Math.Abs(cropHeight));
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control && e.Button == MouseButtons.Left)
            {
                var X = e.X;
                var Y = e.Y;

                if (pictureBox1.Width > pictureBox1.Height)
                    if (e.Y < pictureBox1.Height)
                        Y = 0;
                    else
                        Y = pictureBox1.Height - 1;
                else
                    if (e.X < pictureBox1.Width)
                    X = 0;
                else
                    X = pictureBox1.Width - 1;

                mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
                Cursor.Position = pictureBox1.PointToScreen(new Point(X, Y));
                mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            }
        }

        private const int MOUSEEVENTF_MOVE = 0x0001; /* mouse move */
        private const int MOUSEEVENTF_LEFTDOWN = 0x0002; /* left button down */
        private const int MOUSEEVENTF_LEFTUP = 0x0004; /* left button up */
        private const int MOUSEEVENTF_RIGHTDOWN = 0x0008; /* right button down */

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        private void CropImage_Tool_MouseDown(object sender, MouseEventArgs e)
        {
            this.MoveForm();
        }
    }
}
