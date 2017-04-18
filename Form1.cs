using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrollApp
{
    public partial class Form1 : Form
    {

        private Boolean canClose;
        private int posIndex;
        private System.Media.SoundPlayer player;

        public Form1()
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ControlBox = false;
            posIndex = 0;
            player = new System.Media.SoundPlayer(TrollApp.Properties.Resources.trollsong);
            player.PlayLooping();
            StartOnTop();
            canClose = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }

        public void StartOnTop()
        { 
            Thread thread = new Thread(new ThreadStart(AlwaysOnTop));
            thread.Start();
        }


        public void AlwaysOnTop()
        {
            while (true)
            {
                if (posIndex == 2)
                {
                    Random random = new Random();
                    int newX = random.Next(0, Screen.PrimaryScreen.Bounds.Width - 500);
                    int newY = random.Next(0, Screen.PrimaryScreen.Bounds.Height - 300);

                    if (!InvokeRequired)
                        base.Location = new Point(newX, newY);
                    else
                        this.Invoke((MethodInvoker)(() => base.Location = new Point(newX, newY)));
                    posIndex = 0;
                }
                if (!InvokeRequired)
                    base.TopMost = true;
                else
                    this.Invoke((MethodInvoker)(() => base.TopMost = true));
                posIndex++;
                Thread.Sleep(100);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if(!canClose)
            {
                e.Cancel = true;
            } 

            base.OnFormClosing(e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.D6))
            {
                canClose = true;
                player.Stop();
                this.Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
