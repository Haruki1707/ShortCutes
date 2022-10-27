using ShortCutes.src;
using ShortCutes.src.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShortCutes
{
    public partial class HistoryForm : Form
    {
        int Ypos = 0;
        int Namenum = 0;
        bool StopThread = false;
        public int ShortCuteIndex = -1;
        int VScrollWidth = SystemInformation.VerticalScrollBarWidth;

        List<ShortCute> Copylist;
        List<HistoryButton> Buttonlist = new List<HistoryButton>();
        Dictionary<string, string> Games = new Dictionary<string, string>();
        static Dictionary<string, Image> Thumbnails = new Dictionary<string, Image>();

        public HistoryForm()
        {
            InitializeComponent();

            panel1.Size = new Size(panel1.Width - VScrollWidth, panel1.Height);
            panel1.Location = new Point(panel1.Location.X + (VScrollWidth / 2), panel1.Location.Y);

            button1.Size = new Size(button1.Width - VScrollWidth, button1.Height);
            label2.Size = new Size(label2.Width - VScrollWidth, label2.Height);
        }
        internal void InvokeUI(Action a)
        {
            try
            {
                this?.Invoke(new MethodInvoker(a));
            }
            catch (Exception) { }
        }

        internal void InvokeUIAsync(Action a)
        {
            try
            {
                this?.BeginInvoke(new MethodInvoker(a));
            }
            catch (Exception) { }
        }

        private void HistoryForm_Load(object sender, EventArgs e)
        {
            Copylist = XmlDocSC.ShortCutes.ToList();
            Copylist.Reverse();
            Namenum = Copylist.Count - 1;

            Thread t = new Thread(() =>
            {
                foreach (var ShortCute in Copylist)
                {
                    DrawShortCute(ShortCute);
                    InvokeUIAsync(() => SearchBox.Focus());
                    if (StopThread)
                    {
                        StopThread = false;
                        break;
                    }
                }
            });
            t.Start();
        }

        private void DrawShortCute(ShortCute SC)
        {
            var btn = new Button()
            {
                Name = "BTN" + Namenum.ToString(),
                Text = Namenum.ToString(),
                TextAlign = ContentAlignment.TopLeft,
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
            };
            btn.Click += new EventHandler(Button_Click);
            btn.Paint += new PaintEventHandler(button_paint);

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
            if (File.Exists(SC.Image))
                picbox.Image = Thumbnail(SC, 127, 127);
            picbox.Click += new EventHandler(Control_Click);
            picbox.MouseEnter += new EventHandler(Control_MouseEnter);
            picbox.MouseLeave += new EventHandler(Control_MouseLeave);
            btn.Controls.Add(picbox);

            string emuname = "";
            foreach (var emu in Emulators.EmulatorsList)
                if (emu.CheckPath(Path.GetFileName(SC.EmuPath).ToLower(), false))
                {
                    emuname = emu.Name;
                    break;
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

            InvokeUIAsync(() =>
            {
                Buttonlist.Add(new HistoryButton(btn, SC.Name));
                panel1.Controls.Add(btn);
                btn.Location = new Point(0, Ypos - panel1.VerticalScroll.Value);
                Ypos += button1.Height;
            });
        }
        
        internal Image Thumbnail(ShortCute SC, int width, int height, bool keepAspect = true)
        {
            string id = SC.Name + SC.dateTime;
            if(Thumbnails.ContainsKey(id))
                return Thumbnails[id];
            else
            {
                Image image = Image.FromFile(SC.Image);

                if(keepAspect == true)
                {
                    width = image.Width < image.Height ? (width * image.Width) / image.Height : width;
                    height = image.Width > image.Height ? (height * image.Height) / image.Width : height;
                }

                Image thumb = image.GetThumbnailImage(width, height, () => false, IntPtr.Zero);
                image.Dispose();
                Thumbnails.Add(id, thumb);
                return Thumbnails[id];
            }
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(((Control)sender).Text))
                ShortCuteIndex = int.Parse(((Control)sender).Text);

            StopThread = true;
            while (StopThread == false)
                Thread.Sleep(1);

            /*Parallel.ForEach(panel1.Controls.OfType<Button>(), btn =>
            {
                foreach (var pB in btn.Controls.OfType<PictureBox>())
                    if (pB.Image != null)
                        pB.Image.Dispose();
            });*/

            Buttonlist = null;
            Close();
        }
        private void Control_Click(Object sender, EventArgs e)
        {
            ((Button)((Control)sender).Parent).PerformClick();
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

            Utils.DrawLine(this.Controls, pea);

            pea.Graphics.DrawLine(new Pen(Color.White, 2),
                new PointF(pictureBox2.Location.X, pictureBox2.Location.Y + pictureBox2.Height + 3),
                new PointF(pictureBox2.Location.X + pictureBox2.Width, pictureBox2.Location.Y + pictureBox2.Height + 3));
        }

        private void Control_MouseEnter(object sender, EventArgs e)
        {
            ((Button)((Control)sender).Parent).BackColor = button1.FlatAppearance.MouseOverBackColor;
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            ((Button)((Control)sender).Parent).BackColor = button1.BackColor;
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            string searchtext = SearchBox.Text.ToLower();
            if (String.IsNullOrWhiteSpace(SearchBox.Text))
                searchtext = "";
            else if (SearchBox.Text.Contains("Haruki1707"))
            {
                SearchBox.Text = "";
                using (var info = new MessageForm("", -1, ""))
                    info.ShowDialog();
                return;
            }

            var results = Buttonlist.FindAll(
            delegate (HistoryButton HB)
            {
                return HB.Name.Contains(searchtext, StringComparison.OrdinalIgnoreCase);
            });

            Ypos = 0;
            panel1.VerticalScroll.Value = 0;
            foreach (var item in results)
            {
                item.Button.Location = new Point(0, Ypos - panel1.VerticalScroll.Value);
                Ypos += item.Button.Height;
            }

            foreach (var item in Buttonlist)
                if (!results.Contains(item))
                    item.Button.Location = new Point(0, -item.Button.Height);
        }

        private void button_paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
        }

        private void panel1_SizeChanged(object sender, EventArgs e)
        {
            if (panel1.HorizontalScroll.Visible)
            {
                panel1.SizeChanged -= panel1_SizeChanged;
                panel1.Size = new Size(panel1.Width + VScrollWidth, panel1.Height);
                panel1.Location = new Point(panel1.Location.X - (VScrollWidth / 2), panel1.Location.Y);
            }
        }

        private void HistoryForm_MouseDown(object sender, MouseEventArgs e)
        {
            this.MoveForm();
        }
    }

    class HistoryButton
    {
        public Button Button;
        public string Name;

        public HistoryButton(Button btn, string Nm)
        {
            Button = btn;
            Name = Nm;
        }
    }
}
