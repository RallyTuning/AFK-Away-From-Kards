namespace AFK_Away_From_Kards_Master
{
    partial class FormLogin
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
            Txt_Url = new TextBox();
            WV2_Login = new Microsoft.Web.WebView2.WinForms.WebView2();
            Lbl_LoginStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)WV2_Login).BeginInit();
            SuspendLayout();
            // 
            // Txt_Url
            // 
            Txt_Url.Dock = DockStyle.Top;
            Txt_Url.ForeColor = Color.DimGray;
            Txt_Url.Location = new Point(0, 0);
            Txt_Url.Name = "Txt_Url";
            Txt_Url.ReadOnly = true;
            Txt_Url.Size = new Size(778, 31);
            Txt_Url.TabIndex = 0;
            // 
            // WV2_Login
            // 
            WV2_Login.AllowExternalDrop = true;
            WV2_Login.CreationProperties = null;
            WV2_Login.DefaultBackgroundColor = Color.White;
            WV2_Login.Dock = DockStyle.Fill;
            WV2_Login.Location = new Point(0, 31);
            WV2_Login.Name = "WV2_Login";
            WV2_Login.Size = new Size(778, 782);
            WV2_Login.TabIndex = 1;
            WV2_Login.ZoomFactor = 1D;
            // 
            // Lbl_LoginStatus
            // 
            Lbl_LoginStatus.BorderStyle = BorderStyle.FixedSingle;
            Lbl_LoginStatus.Dock = DockStyle.Bottom;
            Lbl_LoginStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            Lbl_LoginStatus.ForeColor = Color.Firebrick;
            Lbl_LoginStatus.Location = new Point(0, 813);
            Lbl_LoginStatus.Name = "Lbl_LoginStatus";
            Lbl_LoginStatus.Size = new Size(778, 31);
            Lbl_LoginStatus.TabIndex = 2;
            Lbl_LoginStatus.Text = "Status...";
            Lbl_LoginStatus.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FormLogin
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(778, 844);
            Controls.Add(WV2_Login);
            Controls.Add(Lbl_LoginStatus);
            Controls.Add(Txt_Url);
            DoubleBuffered = true;
            MinimizeBox = false;
            Name = "FormLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "AFK: Login";
            Load += FormLogin_Load;
            ((System.ComponentModel.ISupportInitialize)WV2_Login).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox Txt_Url;
        private Microsoft.Web.WebView2.WinForms.WebView2 WV2_Login;
        private Label Lbl_LoginStatus;
    }
}