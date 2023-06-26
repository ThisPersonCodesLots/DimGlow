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
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(94, 349);
            button1.Name = "button1";
            button1.Size = new Size(131, 40);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(323, 349);
            button2.Name = "button2";
            button2.Size = new Size(131, 40);
            button2.TabIndex = 1;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Location = new Point(600, 349);
            button3.Name = "button3";
            button3.Size = new Size(131, 40);
            button3.TabIndex = 2;
            button3.Text = "button3";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(244, 133);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(299, 80);
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
            textBox1.Location = new Point(297, 206);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(175, 35);
            textBox1.TabIndex = 4;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(textBox1);
            Controls.Add(trackBar1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
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
    }
}