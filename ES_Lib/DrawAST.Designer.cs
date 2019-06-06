namespace ES_Lib
{
    partial class DrawAST
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.ShortCutslbl = new System.Windows.Forms.Label();
            this.ShowValuesCB = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Black;
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 52);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(992, 698);
            this.panel1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Draw";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ShortCutslbl
            // 
            this.ShortCutslbl.BackColor = System.Drawing.Color.Black;
            this.ShortCutslbl.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShortCutslbl.ForeColor = System.Drawing.Color.Red;
            this.ShortCutslbl.Location = new System.Drawing.Point(464, 0);
            this.ShortCutslbl.Name = "ShortCutslbl";
            this.ShortCutslbl.Size = new System.Drawing.Size(478, 56);
            this.ShortCutslbl.TabIndex = 2;
            // 
            // ShowValuesCB
            // 
            this.ShowValuesCB.AutoSize = true;
            this.ShowValuesCB.Location = new System.Drawing.Point(370, 29);
            this.ShowValuesCB.Name = "ShowValuesCB";
            this.ShowValuesCB.Size = new System.Drawing.Size(88, 17);
            this.ShowValuesCB.TabIndex = 3;
            this.ShowValuesCB.Text = "Show Values";
            this.ShowValuesCB.UseVisualStyleBackColor = true;
            this.ShowValuesCB.CheckedChanged += new System.EventHandler(this.ShowValuesCB_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(948, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(45, 28);
            this.button2.TabIndex = 4;
            this.button2.Text = "X";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // DrawAST
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(992, 750);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.ShowValuesCB);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ShortCutslbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Name = "DrawAST";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DrawAST";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DrawAST_FormClosing);
            this.Load += new System.EventHandler(this.DrawAST_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.DrawAST_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.DrawAST_KeyUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label ShortCutslbl;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.CheckBox ShowValuesCB;
    }
}