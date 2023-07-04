using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace DimGlow
{
    public partial class Form1 : Form
    {
        private List<OverlayForm> overlayForms;
        private System.Windows.Forms.Timer batteryTimer;

        public Form1()
        {
            InitializeComponent();
            InitializeOverlayForms();
            InitializeForm();
            UpdateBatteryStatus();

// Create and start the battery timer
            batteryTimer = new System.Windows.Forms.Timer();
            batteryTimer.Interval = 5000; // Update every 5 seconds
            batteryTimer.Tick += BatteryTimer_Tick;
            batteryTimer.Start();
        }

        private void InitializeForm()
        {
            Text = "DimGlow";
            textBox1.TextChanged += textBox1_TextChanged;
            ShowInTaskbar = true;
            notifyIcon1.Icon = new Icon("images\\icon.ico");

            LoadConfiguration();
            checkBox1.Checked = Properties.Settings.Default.DarkMode;
            UpdateDarkMode();
        }

        private void InitializeOverlayForms()
        {
            overlayForms = Screen.AllScreens.Select(screen => new OverlayForm { Bounds = screen.Bounds }).ToList();
        }

        private void ApplyDarkOverlay(int transparency)
        {
            foreach (var overlayForm in overlayForms)
            {
                overlayForm.Opacity = transparency / 100.0;
                overlayForm.Visible = true;
            }
        }

        private void UpdateBatteryStatus()
        {
            PowerStatus powerStatus = SystemInformation.PowerStatus;
            // Check if the laptop is currenty running on battery power
            bool isBatteryPower = powerStatus.PowerLineStatus == PowerLineStatus.Offline;

            if (isBatteryPower)
            {
                float batteryPercentage = powerStatus.BatteryLifePercent * 100;
                DrawPictureTextOnTaskbar($"{batteryPercentage:0}%", Color.Green);
            }
            else
            {
                DrawPictureTextOnTaskbar(":d", Color.Yellow);
            }
        }

        private void BatteryTimer_Tick(object sender, EventArgs e)
        {
            UpdateBatteryStatus();
        }

        private void DrawPictureTextOnTaskbar(string text, Color textColor)
        {
            // font & size
            string fontName = "Arial";
            float fontSize = 10;

            using (Font font = new Font(fontName, fontSize, FontStyle.Bold))
            using (Image image = new Bitmap(16, 16))
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.Transparent);
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                graphics.DrawString(text, font, new SolidBrush(textColor), new PointF(0, 0));
                IntPtr hIcon = ((Bitmap)image).GetHicon();
                Icon icon = Icon.FromHandle(hIcon);
                notifyIcon1.Icon = icon;
            }
        }

        private void ResetForm()
        {
            trackBar1.Value = trackBar1.Minimum;
            textBox1.Text = trackBar1.Value.ToString();
            ApplyDarkOverlay(trackBar1.Value);
        }

        private void SaveConfiguration()
        {
            Properties.Settings.Default.User_Setting = trackBar1.Value;
            Properties.Settings.Default.DarkMode = checkBox1.Checked;
            Properties.Settings.Default.Save();
            SaveConfigurationToFile();

            UpdateBatteryStatus();
        }

        private void SaveConfigurationToFile()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(UserSettings));
            const int maxRetryAttempts = 5;
            const int retryDelayMs = 100;

            for (int retry = 0; retry < maxRetryAttempts; retry++)
            {
                try
                {
                    using (var fileStream = new FileStream("config.xml", FileMode.Create, FileAccess.Write))
                    {
                        serializer.Serialize(fileStream,
                            new UserSettings { UserSetting = trackBar1.Value, DarkMode = checkBox1.Checked });
                    }

                    // File saved successfully, break the retry loop
                    break;
                }
                catch (IOException)
                {
                    // File is being used by another process, retry after delay
                    System.Threading.Thread.Sleep(retryDelayMs);
                }
            }
        }

        private void LoadConfiguration()
        {
            if (File.Exists("config.xml"))
            {
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(UserSettings));

                using (var fileStream = new FileStream("config.xml", FileMode.Open, FileAccess.Read))
                using (var reader = new StreamReader(fileStream))
                {
                    var userSettings = (UserSettings)serializer.Deserialize(reader)!;
                    trackBar1.Value = userSettings.UserSetting;
                    textBox1.Text = trackBar1.Value.ToString();
                    ApplyDarkOverlay(trackBar1.Value);
                    checkBox1.Checked = userSettings.DarkMode;
                }
            }
        }

        private void MinimizeToTray()
        {
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += notifyIcon_DoubleClick;
            ShowInTaskbar = false;
            Hide();
        }

        private void RestoreFromTray()
        {
            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            notifyIcon1.Visible = false;
        }

        private void UpdateDarkMode()
        {
            BackColor = checkBox1.Checked ? Color.FromArgb(64, 64, 64) : SystemColors.Control;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //            if (sender == null) return;
            int transparency = trackBar1.Value;
            ApplyDarkOverlay(transparency);
            textBox1.Text = trackBar1.Value.ToString();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox1.Text, out int value) && value >= trackBar1.Minimum && value <= trackBar1.Maximum)
            {
                trackBar1.Value = value;
                ApplyDarkOverlay(value);
            }
        }

        private void button1_Click(object sender, EventArgs e) => ResetForm();

        private void button2_Click(object sender, EventArgs e) => SaveConfiguration();

        private void button3_Click(object sender, EventArgs e) => MinimizeToTray();

        void notifyIcon_DoubleClick(object? sender, EventArgs e) => RestoreFromTray();

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDarkMode();
            SaveConfiguration();
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
            InitializeOverlayForm();
        }

        private void InitializeOverlayForm()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            TopMost = true;
            DoubleBuffered = true;
            BackColor = Color.Black;
            ShowInTaskbar = false;
            Opacity = 0.1;
            StartPosition = FormStartPosition.CenterScreen;
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
            SuspendLayout();
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 261);
            Name = "OverlayForm";
            Text = "OverlayForm";
            ResumeLayout(false);
        }
    }
}