using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortCutes
{
    public partial class MessageForm : Form
    {
        public MessageForm(string Message, int Type)
        {
            InitializeComponent();

            DialogResult = DialogResult.No;
            Messagelbl.Text = Message;

            switch (Type)
            {
                //Info message
                case 0:
                    YESbtn.Hide();
                    NObtn.Hide();
                    iconPB.Image = System.Drawing.SystemIcons.Exclamation.ToBitmap();
                    break;
                //Error message
                case 1:
                    YESbtn.Hide();
                    NObtn.Hide();
                    iconPB.Image = System.Drawing.SystemIcons.Error.ToBitmap();
                    break;
                //Succes message
                case 2:
                    OKbtn.Hide();
                    iconPB.Image = System.Drawing.SystemIcons.Information.ToBitmap();
                    break;
                case 3:
                    this.Size = new Size(440, 336);
                    OKbtn.Hide();
                    iconPB.Hide();
                    Messagelbl.Size = new Size(382, 20);
                    Messagelbl.Location = new Point(18, 10);
                    Messagelbl.TextAlign = ContentAlignment.TopCenter;
                    Messagelbl.Text = "ShortCute Design";
                    YESbtn.Text = "Square";
                    NObtn.Text = "Rectangular";
                    YESbtn.Location = new Point(YESbtn.Location.X - 35, YESbtn.Location.Y);
                    NObtn.Location = new Point(NObtn.Location.X + 65, NObtn.Location.Y);

                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();

                    PictureBox image;
                    image = new PictureBox()
                    {
                        Size = new Size(200, 200),
                        Location = new Point(15, 60),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BorderStyle = BorderStyle.None,
                        Image = global::ShortCutes.Properties.Resources.square,
                    };
                    this.Controls.Add(image);

                    PictureBox image2;
                    image2 = new PictureBox()
                    {
                        Size = new Size(200, 252),
                        Location = new Point(225, 35),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BorderStyle = BorderStyle.None,
                        Image = global::ShortCutes.Properties.Resources.rectangular,
                    };
                    this.Controls.Add(image2);

                    Size tempsize = image.Size;
                    Point temppoint = image.Location;

                    if (Message == "True")
                    {
                        tempsize = image2.Size;
                        temppoint = image2.Location;
                    }

                    Label borderPB;
                    borderPB = new Label()
                    {
                        Size = new Size(tempsize.Width + 6, tempsize.Height + 6),
                        Location = new Point(temppoint.X - 3, temppoint.Y - 3),
                        BorderStyle = BorderStyle.None,
                        BackColor = Color.Red
                    };
                    this.Controls.Add(borderPB);
                    break;
                case 4:
                    progressBar1.Visible = true;
                    OKbtn.Hide();
                    YESbtn.Hide();
                    NObtn.Hide();
                    iconPB.Hide();

                    Messagelbl.Size = new Size(382, 75);
                    Messagelbl.Location = new Point(18, 10);
                    Messagelbl.Font = new Font(Messagelbl.Font.FontFamily, Messagelbl.Font.Size + 5);
                    Messagelbl.Text = "ShortCutes Update Download Progress";
                    EZ_Updater.Update(DownloadProgress, RestartProgram);
                    break;
                default:
                    break;
            }
        }

        private void DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = bytesIn / totalBytes * 100;

            progressBar1.Value = int.Parse(Math.Truncate(percentage).ToString());
        }

        private void RestartProgram(object sender, AsyncCompletedEventArgs e)
        {
            Timer TimerSC = new Timer
            {
                Interval = 700,
                Enabled = true,
            };
            TimerSC.Tick += Execute_Tick;
            TimerSC.Start();
        }

        private void Execute_Tick(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void OKbtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NObtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            OKbtn_Click(sender, e);
        }

        private void YESbtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            OKbtn_Click(sender, e);
        }

        protected override void OnPaint(PaintEventArgs pea)
        {
            Pen pen = new Pen(Color.FromArgb(15, 19, 26) , 10);

            PointF pt1 = new PointF(0, 0);
            PointF pt2 = new PointF(0, Height);
            PointF pt3 = new PointF(Width, 0);
            PointF pt4 = new PointF(Width, Height);

            // Draws the line 
            pea.Graphics.DrawLine(pen, pt1, pt2);
            pea.Graphics.DrawLine(pen, pt1, pt3);
            pea.Graphics.DrawLine(pen, pt3, pt4);
        }
    }
}
