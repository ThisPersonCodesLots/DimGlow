using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

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
            // User_Setting (value = 0 by default) is the value saved in user settings from the previous session before closing the app
            trackBar1.Value = Properties.Settings.Default.User_Setting;
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

            // Load settings from the file if it exists
            if (System.IO.File.Exists("config.xml"))
            {
                LoadConfiguration();
            }
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
            SaveConfiguration();
        }

        private void SaveConfiguration()
        {
            // Save the trackbar value to user settings
            Properties.Settings.Default.User_Setting = trackBar1.Value;
            Properties.Settings.Default.Save();

            // Save the settings to a file
            SaveConfigurationToFile();
        }

        private void SaveConfigurationToFile()
        {
            var serializer = new XmlSerializer(typeof(UserSettings));

            using (var writer = new StreamWriter("config.xml"))
            {
                var userSettings = new UserSettings
                {
                    UserSetting = trackBar1.Value
                };

                serializer.Serialize(writer, userSettings);
            }
        }

        private void LoadConfiguration()
        {
            var serializer = new XmlSerializer(typeof(UserSettings));

            using (var reader = new StreamReader("config.xml"))
            {
                var userSettings = (UserSettings)serializer.Deserialize(reader);
                trackBar1.Value = userSettings.UserSetting;
                textBox1.Text = trackBar1.Value.ToString();
                ApplyDarkOverlay(trackBar1.Value);
            }
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

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                // Enable dark mode
                EnableDarkMode();
            }
            else
            {
                // Disable dark mode
                DisableDarkMode();
            }
        }

        private void EnableDarkMode()
        {
            // Change the color properties of your form and controls to reflect the dark mode
            this.BackColor = Color.FromArgb(64, 64, 64);
            // Modify other controls as needed
        }

        private void DisableDarkMode()
        {
            // Restore the original color properties of your form and controls
            this.BackColor = SystemColors.Control;
            // Restore other controls as needed
        }
    }

    public class UserSettings
    {
        public int UserSetting { get; set; }
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
