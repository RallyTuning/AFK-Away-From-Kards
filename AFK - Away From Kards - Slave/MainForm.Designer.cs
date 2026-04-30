using System.Xml.Linq;

namespace AFK_Away_From_Kards_Slave
{
    partial class MainForm
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
            PicApp = new PictureBox();
            ToolBarBottom = new ToolStrip();
            TsLbl_Countdown = new ToolStripLabel();
            TxtHelp = new TextBox();
            ((System.ComponentModel.ISupportInitialize)PicApp).BeginInit();
            ToolBarBottom.SuspendLayout();
            SuspendLayout();
            // 
            // PicApp
            // 
            PicApp.Location = new Point(0, 28);
            PicApp.Name = "PicApp";
            PicApp.Size = new Size(460, 215);
            PicApp.SizeMode = PictureBoxSizeMode.CenterImage;
            PicApp.TabIndex = 0;
            PicApp.TabStop = false;
            // 
            // ToolBarBottom
            // 
            ToolBarBottom.GripStyle = ToolStripGripStyle.Hidden;
            ToolBarBottom.ImageScalingSize = new Size(24, 24);
            ToolBarBottom.Items.AddRange(new ToolStripItem[] { TsLbl_Countdown });
            ToolBarBottom.Location = new Point(0, 0);
            ToolBarBottom.Name = "ToolBarBottom";
            ToolBarBottom.Size = new Size(460, 38);
            ToolBarBottom.TabIndex = 1;
            ToolBarBottom.Text = "Toobar";
            // 
            // TsLbl_Countdown
            // 
            TsLbl_Countdown.Alignment = ToolStripItemAlignment.Right;
            TsLbl_Countdown.Name = "TsLbl_Countdown";
            TsLbl_Countdown.Size = new Size(80, 33);
            TsLbl_Countdown.Text = "00:00:00";
            // 
            // TxtHelp
            // 
            TxtHelp.BackColor = SystemColors.Window;
            TxtHelp.Font = new Font("Courier New", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TxtHelp.Location = new Point(114, 91);
            TxtHelp.Multiline = true;
            TxtHelp.Name = "TxtHelp";
            TxtHelp.ReadOnly = true;
            TxtHelp.ScrollBars = ScrollBars.Both;
            TxtHelp.Size = new Size(209, 86);
            TxtHelp.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            ClientSize = new Size(460, 243);
            Controls.Add(TxtHelp);
            Controls.Add(ToolBarBottom);
            Controls.Add(PicApp);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            FormClosing += MainForm_FormClosing;
            Load += MainForm_Load;
            Shown += MainForm_Shown;
            ((System.ComponentModel.ISupportInitialize)PicApp).EndInit();
            ToolBarBottom.ResumeLayout(false);
            ToolBarBottom.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox PicApp;
        private ToolStrip ToolBarBottom;
        private TextBox TxtHelp;
        private ToolStripLabel TsLbl_Countdown;
    }
}
