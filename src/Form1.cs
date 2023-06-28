using System;
using System.IO;
using System.Windows.Forms;

namespace DimGlow
{
    public partial class Form1 : Form
    {
        private OverlayForm overlayForm;
        public Form1()
        {
            InitializeComponent();

            overlayForm = new OverlayForm
            {
                Visible = false
            };

            Text = "DimGlow";

            textBox1.Text = trackBar1.Value.ToString();
            textBox1.TextChanged += textBox1_TextChanged;

            this.ShowInTaskbar = true;
            notifyIcon1.Icon = new System.Drawing.Icon("images\\icon.ico");

            if (File.Exists("config.xml"))
            {
                LoadConfiguration();
            }

            checkBox1.Checked = Properties.Settings.Default.DarkMode;

            if (checkBox1.Checked)
            {
                EnableDarkMode();
            }
            else
            {
                DisableDarkMode();
            }
        }

        private void ApplyDarkOverlay(int transparency)
        {
            overlayForm.Opacity = transparency / 100.0;
            overlayForm.Visible = true;
        }
        //
        // Reset button
        //
        private void button1_Click(object sender, EventArgs e)
        {
            if (sender == null) return;
            trackBar1.Value = trackBar1.Minimum;
            textBox1.Text = trackBar1.Value.ToString();
            overlayForm.Visible = false;
            ApplyDarkOverlay(trackBar1.Value);
        }
        //
        // Save button
        //
        private void button2_Click(object sender, EventArgs e)
        {
            SaveConfiguration();
        }

        private void SaveConfiguration()
        {
            Properties.Settings.Default.User_Setting = trackBar1.Value;
            Properties.Settings.Default.DarkMode = checkBox1.Checked;
            Properties.Settings.Default.Save();
            SaveConfigurationToFile();
        }

        private void SaveConfigurationToFile()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(UserSettings));
            using (var writer = new StreamWriter("config.xml"))
            {
                var userSettings = new UserSettings
                {
                    UserSetting = trackBar1.Value,
                    DarkMode = checkBox1.Checked
                };
                serializer.Serialize(writer, userSettings);
            }
        }

        private void LoadConfiguration()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(UserSettings));
            using (var reader = new StreamReader("config.xml"))
            {
                var userSettings = (UserSettings)serializer.Deserialize(reader)!;
                trackBar1.Value = userSettings.UserSetting;
                textBox1.Text = trackBar1.Value.ToString();
                ApplyDarkOverlay(trackBar1.Value);
                checkBox1.Checked = userSettings.DarkMode;
            }
        }
        //
        // Icon Tray button
        //
        private void button3_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += notifyIcon_DoubleClick;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        void notifyIcon_DoubleClick(object? sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }
        //
        // Dark Mode
        //
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                EnableDarkMode();
            }
            else
            {
                DisableDarkMode();
            }
        }

        private void EnableDarkMode()
        {
            this.BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
        }

        private void DisableDarkMode()
        {
            this.BackColor = SystemColors.Control;
        }
        //
        // Trackbar
        //
        private void trackBar1_Scroll(object? sender, EventArgs e)
        {
            if (sender == null) return;
            int transparency = trackBar1.Value;
            ApplyDarkOverlay(transparency);
            textBox1.Text = trackBar1.Value.ToString();
        }

        private void textBox1_TextChanged(object? sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int value))
            {
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
    }

    public class UserSettings
    {
        public int UserSetting { get; set; }
        public bool DarkMode { get; set; }
    }

    public class OverlayForm : Form
    {
        private const int WS_EX_TRANSPARENT = 0x20;
        private const int WS_EX_LAYERED = 0x80000;

        public OverlayForm()
        {
            InitializeComponent();

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
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Name = "OverlayForm";
            this.Text = "OverlayForm";
            this.ResumeLayout(false);
        }
    }
}