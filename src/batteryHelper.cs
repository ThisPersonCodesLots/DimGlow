using System;
using System.Drawing;
using System.Windows.Forms;

public class BatteryHelper
{
    private System.Windows.Forms.Timer batteryTimer;
    private NotifyIcon notifyIcon1;

    public BatteryHelper(NotifyIcon notifyIcon)
    {
        notifyIcon1 = notifyIcon;
        InitializeBatteryTimer();
    }

    private void InitializeBatteryTimer()
    {
        // Create and start the battery timer
        batteryTimer = new System.Windows.Forms.Timer();
        batteryTimer.Interval = 5000; // Update every 5 seconds
        batteryTimer.Tick += BatteryTimer_Tick;
        batteryTimer.Start();
    }

    private void UpdateBatteryStatus()
    {
        PowerStatus powerStatus = SystemInformation.PowerStatus;
        // Check if the laptop is currently running on battery power
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
}
