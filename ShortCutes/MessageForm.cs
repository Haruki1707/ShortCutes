using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
                default:
                    break;
            }

            Messagelbl.Text = Message;
        }

        public DialogResult dialogResult = DialogResult.No;

        private void OKbtn_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NObtn_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.No;
            OKbtn_Click(sender, e);
        }

        private void YESbtn_Click(object sender, EventArgs e)
        {
            dialogResult = DialogResult.Yes;
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
