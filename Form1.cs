using System;
using System.Drawing;
using System.Windows.Forms;

namespace DimGlow
{
    public partial class Form1 : Form
    {
        private OverlayForm overlayForm;
        private NotifyIcon notifyIcon;

        public Form1()
        {
            InitializeComponent();

            // Initialize the overlay form
            overlayForm = new OverlayForm
            {
                Visible = false
            };

            Text = "DimGlow";
            //
            // trackBar functionality
            //
            trackBar1.Minimum = 0;
            trackBar1.Maximum = 95;
            trackBar1.Value = Properties.Settings.Default.Transparency;
            trackBar1.TickFrequency = 10;
            trackBar1.LargeChange = 10;
            trackBar1.SmallChange = 1;
            trackBar1.Scroll += new EventHandler(trackBar1_Scroll);
            //
            // textBox functionality
            // 
            textBox1.Text = trackBar1.Value.ToString("Insert Value");
            textBox1.TextChanged += new EventHandler(textBox1_TextChanged);
            //
            // icon functionality
            //
            this.ShowInTaskbar = true;
            this.Icon = new Icon("images\\icon.ico");
            ApplyDarkOverlay(trackBar1.Value);

        }
        private void ApplyDarkOverlay(int transparency)
        {
            // Set overlay form opacity based on transparency
            overlayForm.Opacity = transparency / 100.0;
            overlayForm.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Reset button for the trackbar and the overlay
            trackBar1.Value = trackBar1.Minimum;
            textBox1.Text = trackBar1.Value.ToString();
            overlayForm.Visible = false;
            ApplyDarkOverlay(trackBar1.Value);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            Properties.Settings.Default.Transparency = trackBar1.Value;
            Properties.Settings.Default.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // minimize button - system tray
            notifyIcon = new NotifyIcon();
            notifyIcon.Text = "DimGlow";
            notifyIcon.Icon = new Icon("images\\icon.ico");
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += new EventHandler(notifyIcon_DoubleClick);
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Parse the TextBox value and update the trackbar value
            if (int.TryParse(textBox1.Text, out int value))
            {
                // Enforce the two-digit restriction
                if (textBox1.Text.Length > 2)
                {
                    textBox1.Text = textBox1.Text.Substring(0, 2);
                    textBox1.Select(2, 0);
                }

                if (value >= trackBar1.Minimum && value <= trackBar1.Maximum)
                {
                    trackBar1.Value = value;
                    ApplyDarkOverlay(value);
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int transparency = trackBar1.Value;
            ApplyDarkOverlay(transparency);
            textBox1.Text = trackBar1.Value.ToString();
        }

        void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

    }
    public class OverlayForm : Form
    {
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int WS_EX_LAYERED = 0x80000;

        public OverlayForm()
        {
            InitializeComponent();

            // Set form properties
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.BackColor = Color.Black;
            this.ShowInTaskbar = false;
            this.Opacity = 0.1;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Do not paint background to reduce flicker
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= WS_EX_TRANSPARENT | WS_EX_LAYERED;
                return cp;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // OverlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "OverlayForm";
            this.Text = "OverlayForm";
            this.ResumeLayout(false);
        }
    }
}