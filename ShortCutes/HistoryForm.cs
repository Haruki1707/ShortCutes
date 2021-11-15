using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ShortCutes
{
    public partial class HistoryForm : Form
    {
        int Ypos = 0;
        int Namenum = 0;
        bool alreadyvisible = false;
        int VScrollWidth = SystemInformation.VerticalScrollBarWidth;
        public int ShortCuteIndex = -1;
        public HistoryForm()
        {
            InitializeComponent();

            panel1.Size = new Size(panel1.Width - VScrollWidth, panel1.Height);
            panel1.Location = new Point(panel1.Location.X + (VScrollWidth / 2), panel1.Location.Y);

            button1.Size = new Size(button1.Width - VScrollWidth, button1.Height);
            label2.Size = new Size(label2.Width - VScrollWidth, label2.Height);

            var Copylist = XmlDocSC.ShortCutes.ToList();
            Copylist.Reverse();
            Namenum = Copylist.Count - 1;

            foreach (var ShortCute in Copylist)
                DrawShortCute(ShortCute);
        }

        private void DrawShortCute(ShortCute SC)
        {
            var btn = new Button()
            {
                Name = "BTN" + Namenum.ToString(),
                Size = button1.Size,
                BackColor = button1.BackColor,
                ForeColor = button1.ForeColor,
                Font = button1.Font,
                FlatStyle = button1.FlatStyle,
                FlatAppearance =
                {
                    BorderColor = button1.FlatAppearance.BorderColor,
                    BorderSize = button1.FlatAppearance.BorderSize,
                    MouseOverBackColor = button1.FlatAppearance.MouseOverBackColor
                },
                Location = new Point(0, Ypos),
            };
            btn.Click += new EventHandler(Button_Click);

            var picbox = new PictureBox()
            {
                Name = "PTB" + Namenum.ToString(),
                Size = pictureBox1.Size,
                Cursor = pictureBox1.Cursor,
                BackColor = pictureBox1.BackColor,
                BorderStyle = pictureBox1.BorderStyle,
                SizeMode = pictureBox1.SizeMode,
                Location = new Point(btn.FlatAppearance.BorderSize, btn.FlatAppearance.BorderSize),
            };
            if(File.Exists(SC.Image))
                picbox.Image = Image.FromFile(SC.Image);
            picbox.Click += new EventHandler(Control_Click);
            picbox.MouseEnter += new EventHandler(Control_MouseEnter);
            picbox.MouseLeave += new EventHandler(Control_MouseLeave);
            btn.Controls.Add(picbox);

            string emuname = "";
            foreach (var emu in Emulators.EmulatorsList)
            {
                if (emu.Exe.ToLower() == Path.GetFileName(SC.EmuPath).ToLower())
                {
                    emuname = emu.Name;
                    break;
                }
            }

            var label = new Label()
            {
                Text = emuname + ": " + SC.Name,
                Name = "LBL" + Namenum--.ToString(),
                TextAlign = label2.TextAlign,
                Font = label2.Font,
                BackColor = label2.BackColor,
                BorderStyle = label2.BorderStyle,
                FlatStyle = label2.FlatStyle,
                ForeColor = label2.ForeColor,
                Size = label2.Size,
                Location = new Point(picbox.Width, 0),
            };
            label.Click += new EventHandler(Control_Click);
            label.MouseEnter += new EventHandler(Control_MouseEnter);
            label.MouseLeave += new EventHandler(Control_MouseLeave);
            btn.Controls.Add(label);


            Ypos += button1.Height;

            panel1.Controls.Add(btn);

            if (panel1.VerticalScroll.Visible && !alreadyvisible)
            {
                panel1.Size = new Size(panel1.Width + VScrollWidth, panel1.Height);
                panel1.Location = new Point(panel1.Location.X - (VScrollWidth / 2), panel1.Location.Y);
                alreadyvisible = true;
            }
        }

        private void Button_Click(Object sender, EventArgs e)
        {
            var button = ((Button)sender);
            ShortCuteIndex = int.Parse(button.Name.Replace("BTN", ""));

            foreach (var btn in panel1.Controls.OfType<Button>())
                foreach (var pB in btn.Controls.OfType<PictureBox>())
                    if(pB.Image != null)
                        pB.Image.Dispose();

            Close();
        }
        private void Control_Click(Object sender, EventArgs e)
        {
            string number = "";
            if (sender is PictureBox)
                number = ((PictureBox)sender).Name.Replace("PTB", "");
            else
                number = ((Label)sender).Name.Replace("LBL", "");

            CurrentButton(number).PerformClick();
        }

        protected override void OnPaint(PaintEventArgs pea)
        {
            Pen pen = new Pen(Color.FromArgb(45, 56, 74), 10);

            PointF pt1 = new PointF(0, 0);
            PointF pt2 = new PointF(0, Height);
            PointF pt3 = new PointF(Width, 0);
            PointF pt4 = new PointF(Width, Height);

            // Draws the line 
            pea.Graphics.DrawLine(pen, pt1, pt2);
            pea.Graphics.DrawLine(pen, pt1, pt3);
            pea.Graphics.DrawLine(pen, pt3, pt4);
            pea.Graphics.DrawLine(pen, pt2, pt4);
        }

        private Button CurrentButton(string number) => panel1.Controls.OfType<Button>().FirstOrDefault(b => b.Name == "BTN" + number);
        private void Control_MouseEnter(object sender, EventArgs e)
        {
            string number = "";
            if (sender is PictureBox)
                number = ((PictureBox)sender).Name.Replace("PTB", "");
            else
                number = ((Label)sender).Name.Replace("LBL", "");

            CurrentButton(number).BackColor = button1.FlatAppearance.MouseOverBackColor;
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            string number = "";
            if (sender is PictureBox)
                number = ((PictureBox)sender).Name.Replace("PTB", "");
            else
                number = ((Label)sender).Name.Replace("LBL", "");

            CurrentButton(number).BackColor = button1.BackColor;

        }

        private void closeBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
