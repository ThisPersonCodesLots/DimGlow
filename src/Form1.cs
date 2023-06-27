using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DimGlow
{
    public partial class Form1 : Form
    {
        private OverlayForm[] overlayForms;
        private NotifyIcon? notifyIcon;
        private Dictionary<CheckBox, int> monitorDictionary;

        public Form1()
        {
            InitializeComponent();

            int monitorCount = Screen.AllScreens.Length;
            overlayForms = new OverlayForm[monitorCount];
            monitorDictionary = new Dictionary<CheckBox, int>();

            for (int i = 0; i < monitorCount; i++)
            {
                var checkBox = new CheckBox
                {
                    Text = $"Monitor {i + 1}",
                    AutoSize = true,
                    Location = new Point(10, 50 + (i * 30))
                };
                checkBox.CheckedChanged += MonitorCheckBox_CheckedChanged;
                monitorDictionary.Add(checkBox, i);
                panel1.Controls.Add(checkBox);

                overlayForms[i] = new OverlayForm
                {
                    Visible = false,
                    Bounds = Screen.AllScreens[i].Bounds
                };
            }

            Text = "DimGlow";

            trackBar1.Minimum = 0;
            trackBar1.Maximum = 95;
            trackBar1.Value = Properties.Settings.Default.User_Setting;
            trackBar1.TickFrequency = 10;
            trackBar1.LargeChange = 10;
            trackBar1.SmallChange = 1;
            trackBar1.Scroll += trackBar1_Scroll;

            textBox1.Text = trackBar1.Value.ToString();
            textBox1.TextChanged += textBox1_TextChanged;

            this.ShowInTaskbar = true;
            this.Icon = new Icon("images\\icon.ico");

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

        private void MonitorCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            int monitorIndex = monitorDictionary[checkBox];

            if (checkBox.Checked)
            {
                overlayForms[monitorIndex].Visible = true;
            }
            else
            {
                overlayForms[monitorIndex].Visible = false;
            }
        }

        private void ApplyDarkOverlay(int transparency)
        {
            foreach (OverlayForm overlayForm in overlayForms)
            {
                int monitorIndex = Array.IndexOf(overlayForms, overlayForm);
                CheckBox checkBox = monitorDictionary.FirstOrDefault(x => x.Value == monitorIndex).Key;

                if (checkBox != null && checkBox.Checked)
                {
                    overlayForm.Opacity = transparency / 100.0;
                    overlayForm.Visible = true;
                }
                else
                {
                    overlayForm.Visible = false;
                }
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (sender == null) return;
            trackBar1.Value = trackBar1.Minimum;
            textBox1.Text = trackBar1.Value.ToString();
            foreach (OverlayForm overlayForm in overlayForms)
            {
                overlayForm.Visible = false;
            }
            ApplyDarkOverlay(trackBar1.Value);
        }

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

        private void button3_Click(object sender, EventArgs e)
        {
            notifyIcon = new NotifyIcon();
            notifyIcon.Text = "DimGlow";
            notifyIcon.Icon = new Icon("images\\icon.ico");
            notifyIcon.Visible = true;
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
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

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (sender == null) return;
            int transparency = trackBar1.Value;
            ApplyDarkOverlay(transparency);
            textBox1.Text = trackBar1.Value.ToString();
        }

        void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            notifyIcon = new NotifyIcon();
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.ShowInTaskbar = true;
            notifyIcon.Visible = false;
        }

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
            this.BackColor = Color.FromArgb(64, 64, 64);
        }

        private void DisableDarkMode()
        {
            this.BackColor = SystemColors.Control;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

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
            this.TopMost = true;
            this.DoubleBuffered = true;
            this.BackColor = Color.Black;
            this.ShowInTaskbar = false;
            this.Opacity = 0.1;
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
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(284, 261);
            this.Name = "OverlayForm";
            this.Text = "OverlayForm";
            this.ResumeLayout(false);
        }
    }
}
