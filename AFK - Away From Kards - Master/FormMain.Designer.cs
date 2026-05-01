namespace AFK_Away_From_Kards_Master
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            TS_ToolBar = new ToolStrip();
            TsBtn_LoginForm = new ToolStripButton();
            TsLbl_StrLogged = new ToolStripLabel();
            Lbl_Logged = new ToolStripLabel();
            toolStripSeparator1 = new ToolStripSeparator();
            Pnl_Left = new Panel();
            LsvGames = new ListView();
            Col_Name = new ColumnHeader();
            Col_Cards = new ColumnHeader();
            Col_ID = new ColumnHeader();
            TS_ToolBar.SuspendLayout();
            SuspendLayout();
            // 
            // TS_ToolBar
            // 
            TS_ToolBar.GripStyle = ToolStripGripStyle.Hidden;
            TS_ToolBar.ImageScalingSize = new Size(24, 24);
            TS_ToolBar.Items.AddRange(new ToolStripItem[] { TsBtn_LoginForm, TsLbl_StrLogged, Lbl_Logged, toolStripSeparator1 });
            TS_ToolBar.Location = new Point(0, 0);
            TS_ToolBar.Name = "TS_ToolBar";
            TS_ToolBar.Size = new Size(800, 33);
            TS_ToolBar.TabIndex = 0;
            TS_ToolBar.Text = "ToolBar";
            // 
            // TsBtn_LoginForm
            // 
            TsBtn_LoginForm.DisplayStyle = ToolStripItemDisplayStyle.Image;
            TsBtn_LoginForm.Image = (Image)resources.GetObject("TsBtn_LoginForm.Image");
            TsBtn_LoginForm.ImageTransparentColor = Color.Magenta;
            TsBtn_LoginForm.Name = "TsBtn_LoginForm";
            TsBtn_LoginForm.Size = new Size(34, 28);
            TsBtn_LoginForm.Text = "Login / Logout";
            TsBtn_LoginForm.Click += TsBtn_LoginForm_Click;
            // 
            // TsLbl_StrLogged
            // 
            TsLbl_StrLogged.Name = "TsLbl_StrLogged";
            TsLbl_StrLogged.Padding = new Padding(6, 0, 0, 0);
            TsLbl_StrLogged.Size = new Size(83, 28);
            TsLbl_StrLogged.Text = "Logged:";
            // 
            // Lbl_Logged
            // 
            Lbl_Logged.AutoSize = false;
            Lbl_Logged.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Lbl_Logged.ForeColor = Color.DodgerBlue;
            Lbl_Logged.Name = "Lbl_Logged";
            Lbl_Logged.Size = new Size(124, 28);
            Lbl_Logged.Text = "Checking...";
            Lbl_Logged.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 33);
            // 
            // Pnl_Left
            // 
            Pnl_Left.BorderStyle = BorderStyle.FixedSingle;
            Pnl_Left.Dock = DockStyle.Left;
            Pnl_Left.Location = new Point(0, 33);
            Pnl_Left.Name = "Pnl_Left";
            Pnl_Left.Size = new Size(244, 417);
            Pnl_Left.TabIndex = 1;
            // 
            // LsvGames
            // 
            LsvGames.Columns.AddRange(new ColumnHeader[] { Col_Name, Col_Cards, Col_ID });
            LsvGames.Dock = DockStyle.Fill;
            LsvGames.FullRowSelect = true;
            LsvGames.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            LsvGames.Location = new Point(244, 33);
            LsvGames.Name = "LsvGames";
            LsvGames.ShowGroups = false;
            LsvGames.Size = new Size(556, 417);
            LsvGames.TabIndex = 2;
            LsvGames.TileSize = new Size(300, 90);
            LsvGames.UseCompatibleStateImageBehavior = false;
            LsvGames.View = View.Tile;
            // 
            // Col_Name
            // 
            Col_Name.Text = "Name";
            // 
            // Col_Cards
            // 
            Col_Cards.Text = "Cards";
            // 
            // Col_ID
            // 
            Col_ID.Text = "ID";
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(LsvGames);
            Controls.Add(Pnl_Left);
            Controls.Add(TS_ToolBar);
            DoubleBuffered = true;
            Name = "FormMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AFKards";
            Load += MainForm_Load;
            Shown += MainForm_Shown;
            TS_ToolBar.ResumeLayout(false);
            TS_ToolBar.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip TS_ToolBar;
        private ToolStripLabel TsLbl_StrLogged;
        private ToolStripLabel Lbl_Logged;
        private Panel Pnl_Left;
        private ListView LsvGames;
        private ToolStripButton TsBtn_LoginForm;
        private ToolStripSeparator toolStripSeparator1;
        private ColumnHeader Col_Name;
        private ColumnHeader Col_Cards;
        private ColumnHeader Col_ID;
    }
}
