namespace MotionDetection
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.motionImageBox = new Emgu.CV.UI.ImageBox();
            this.label5 = new System.Windows.Forms.Label();
            this.FeatureComboBox = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.surfResultLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.motionImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // motionImageBox
            // 
            this.motionImageBox.Location = new System.Drawing.Point(147, 12);
            this.motionImageBox.Name = "motionImageBox";
            this.motionImageBox.Size = new System.Drawing.Size(1011, 595);
            this.motionImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.motionImageBox.TabIndex = 2;
            this.motionImageBox.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(240, 641);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 25);
            this.label5.TabIndex = 7;
            this.label5.Text = "Orb result";
            // 
            // FeatureComboBox
            // 
            this.FeatureComboBox.AccessibleName = "";
            this.FeatureComboBox.FormattingEnabled = true;
            this.FeatureComboBox.Location = new System.Drawing.Point(243, 691);
            this.FeatureComboBox.Name = "FeatureComboBox";
            this.FeatureComboBox.Size = new System.Drawing.Size(121, 21);
            this.FeatureComboBox.TabIndex = 8;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(888, 691);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // surfResultLabel
            // 
            this.surfResultLabel.AutoSize = true;
            this.surfResultLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.surfResultLabel.Location = new System.Drawing.Point(885, 641);
            this.surfResultLabel.Name = "surfResultLabel";
            this.surfResultLabel.Size = new System.Drawing.Size(100, 25);
            this.surfResultLabel.TabIndex = 10;
            this.surfResultLabel.Text = "Surf result";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1255, 744);
            this.Controls.Add(this.surfResultLabel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.FeatureComboBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.motionImageBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.motionImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Emgu.CV.UI.ImageBox motionImageBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox FeatureComboBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label surfResultLabel;
    }
}

