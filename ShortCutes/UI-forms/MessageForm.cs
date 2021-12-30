using EZ_Updater;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ShortCutes
{
    public partial class MessageForm : Form
    {
        public static void Info(string message, string clipboard = null)
        {
            using (var info = new MessageForm(message, 0, clipboard))
                info.ShowDialog();
        }
        public static void Error(string message)
        {
            using (var error = new MessageForm(message, 1))
                error.ShowDialog();
        }
        public static bool Success(string message)
        {
            using (var success = new MessageForm(message, 2))
            {
                success.ShowDialog();
                return success.DialogResult == DialogResult.Yes;
            }
        }

        string cliptext = null;
        public MessageForm(string Message, int Type, string texttoCB = null)
        {
            InitializeComponent();

            cliptext = texttoCB;
            DialogResult = DialogResult.No;
            Messagelbl.Text = Message;

            switch (Type)
            {
                //Info message
                case 0:
                    YESbtn.Hide();
                    NObtn.Hide();
                    iconPB.Image = SystemIcons.Exclamation.ToBitmap();
                    break;
                //Error message
                case 1:
                    YESbtn.Hide();
                    NObtn.Hide();
                    iconPB.Image = SystemIcons.Error.ToBitmap();
                    break;
                //Success message
                case 2:
                    OKbtn.Hide();
                    iconPB.Image = SystemIcons.Information.ToBitmap();
                    break;
                case 3:
                    this.Size = new Size(440, 336);
                    OKbtn.Hide();
                    iconPB.Hide();
                    Messagelbl.Size = new Size(382, 25);
                    Messagelbl.Location = new Point(18, 7);
                    Messagelbl.TextAlign = ContentAlignment.TopCenter;
                    Messagelbl.Text = "ShortCute Design";
                    Messagelbl.Font = new Font("Bahnschrift SemiBold SemiConden", 14F);
                    YESbtn.Text = "Square";
                    NObtn.Text = "Rectangular";
                    YESbtn.Location = new Point(YESbtn.Location.X - 35, YESbtn.Location.Y);
                    NObtn.Location = new Point(NObtn.Location.X + 65, NObtn.Location.Y);

                    PictureBox image;
                    image = new PictureBox()
                    {
                        Size = new Size(200, 200),
                        Location = new Point(15, 65),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BorderStyle = BorderStyle.None,
                        Image = Properties.Resources.square,
                    };
                    image.Click += YESbtn_Click;
                    this.Controls.Add(image);

                    PictureBox image2;
                    image2 = new PictureBox()
                    {
                        Size = new Size(200, 252),
                        Location = new Point(225, 40),
                        SizeMode = PictureBoxSizeMode.Zoom,
                        BorderStyle = BorderStyle.None,
                        Image = Properties.Resources.rectangular,
                    };
                    image2.Click += NObtn_Click;
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
                    closeBtn.Hide();

                    Messagelbl.Size = new Size(382, 75);
                    Messagelbl.Location = new Point(18, 10);
                    Messagelbl.Font = new Font(Messagelbl.Font.FontFamily, Messagelbl.Font.Size + 5);
                    Messagelbl.Text = Updater.Message;
                    Updater.Update(UIChange);
                    break;
                //Info2 message
                case 5:
                    YESbtn.Hide();
                    NObtn.Hide();
                    iconPB.Image = SystemIcons.Information.ToBitmap();
                    break;
                default:
                    break;
            }

            closeBtn.Location = new Point(this.Size.Width - closeBtn.Width, 0);
        }

        private void UIChange(object sender, EventArgs e)
        {
            Messagelbl.Text = Updater.Message;
            progressBar1.Value = Updater.ProgressPercentage;

            switch (Updater.State)
            {
                case UpdaterState.Canceled:
                case UpdaterState.InstallFailed:
                    OKbtn.Visible = true;
                    break;
                case UpdaterState.Installed:
                    Application.Restart();
                    break;
            }
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
            Pen pen = new Pen(panel1.BackColor, 10);

            PointF pt1 = new PointF(0, 0);
            PointF pt2 = new PointF(0, Height);
            PointF pt3 = new PointF(Width, 0);
            PointF pt4 = new PointF(Width, Height);

            // Draws the line 
            pea.Graphics.DrawLine(pen, pt1, pt2);
            pea.Graphics.DrawLine(pen, pt1, pt3);
            pea.Graphics.DrawLine(pen, pt3, pt4);
        }

        private void Messagelbl_DoubleClick(object sender, EventArgs e)
        {
            if (cliptext != null)
            {
                try
                {
                    Clipboard.SetText(cliptext);
                    Messagelbl.Text = Messagelbl.Text.Replace("Double click on this text to copy path folder to clipboard", "Path copied to clipboard");
                }
                catch { }
            }
        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}
