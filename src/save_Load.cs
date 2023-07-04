using System;
using System.IO;
using System.Xml.Serialization;

namespace DimGlow
{
    public partial class Form1 : Form
    {
        // Same code
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
            var serializer = new XmlSerializer(typeof(UserSettings));
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

                    // File saved successfully, break the retry
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
                var serializer = new XmlSerializer(typeof(UserSettings));

                using (var fileStream = new FileStream("config.xml", FileMode.Open, FileAccess.Read))
                {
                    var userSettings = (UserSettings)serializer.Deserialize(fileStream);
                    trackBar1.Value = userSettings.UserSetting;
                    textBox1.Text = trackBar1.Value.ToString();
                    ApplyDarkOverlay(trackBar1.Value);
                    checkBox1.Checked = userSettings.DarkMode;
                }
            }
        }
    }
}
    