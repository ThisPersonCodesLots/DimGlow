namespace DimGlow
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            trackBar1 = new TrackBar();
            notifyIcon1 = new NotifyIcon(components);
            textBox1 = new TextBox();
            checkBox1 = new CheckBox();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(93, 233);
            button1.Margin = new Padding(2);
            button1.Name = "button1";
            button1.Size = new Size(80, 33);
            button1.TabIndex = 0;
            button1.Text = "Reset";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(213, 233);
            button2.Margin = new Padding(2);
            button2.Name = "button2";
            button2.Size = new Size(80, 33);
            button2.TabIndex = 1;
            button2.Text = "Apply";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(333, 233);
            button3.Margin = new Padding(2);
            button3.Name = "button3";
            button3.Size = new Size(80, 33);
            button3.TabIndex = 2;
            button3.Text = "Icon Tray";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(163, 135);
            trackBar1.Margin = new Padding(2);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(199, 56);
            trackBar1.TabIndex = 3;
            trackBar1.Scroll += trackBar1_Scroll;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Text = "notifyIcon1";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(208, 193);
            textBox1.Margin = new Padding(2);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(118, 27);
            textBox1.TabIndex = 4;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(396, 138);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(105, 24);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Dark Mode";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(533, 300);
            Controls.Add(checkBox1);
            Controls.Add(textBox1);
            Controls.Add(trackBar1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Margin = new Padding(2);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DimGlow";
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        private Button button1;
        private Button button2;
        private Button button3;
        private TrackBar trackBar1;
        private NotifyIcon notifyIcon1;
        private TextBox textBox1;
        private CheckBox checkBox1;
    }
}