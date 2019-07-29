namespace LolienClient
{
    partial class LolienClient
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LolienClient));
            this.replayDirectoryButton = new System.Windows.Forms.Button();
            this.leagueComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.replayDirectoryText = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.customGameReplayRadioButton = new System.Windows.Forms.RadioButton();
            this.leagueRadioButton = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // replayDirectoryButton
            // 
            resources.ApplyResources(this.replayDirectoryButton, "replayDirectoryButton");
            this.replayDirectoryButton.Name = "replayDirectoryButton";
            this.replayDirectoryButton.UseVisualStyleBackColor = true;
            this.replayDirectoryButton.Click += new System.EventHandler(this.ReplayDirectoryButton_Click);
            // 
            // leagueComboBox
            // 
            this.leagueComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.leagueComboBox, "leagueComboBox");
            this.leagueComboBox.FormattingEnabled = true;
            this.leagueComboBox.Name = "leagueComboBox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // replayDirectoryText
            // 
            resources.ApplyResources(this.replayDirectoryText, "replayDirectoryText");
            this.replayDirectoryText.Name = "replayDirectoryText";
            // 
            // customGameReplayRadioButton
            // 
            resources.ApplyResources(this.customGameReplayRadioButton, "customGameReplayRadioButton");
            this.customGameReplayRadioButton.Checked = true;
            this.customGameReplayRadioButton.Name = "customGameReplayRadioButton";
            this.customGameReplayRadioButton.TabStop = true;
            this.customGameReplayRadioButton.UseVisualStyleBackColor = true;
            this.customGameReplayRadioButton.CheckedChanged += new System.EventHandler(this.CustomGameReplayRadioButton_CheckedChanged);
            // 
            // leagueRadioButton
            // 
            resources.ApplyResources(this.leagueRadioButton, "leagueRadioButton");
            this.leagueRadioButton.Name = "leagueRadioButton";
            this.leagueRadioButton.UseVisualStyleBackColor = true;
            this.leagueRadioButton.CheckedChanged += new System.EventHandler(this.LeagueRadioButton_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LoLienClient.Properties.Resources._24658d65cdc2c9;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // LolienClient
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.leagueRadioButton);
            this.Controls.Add(this.customGameReplayRadioButton);
            this.Controls.Add(this.replayDirectoryText);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.leagueComboBox);
            this.Controls.Add(this.replayDirectoryButton);
            this.Name = "LolienClient";
            this.Load += new System.EventHandler(this.LolienClient_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button replayDirectoryButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox replayDirectoryText;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.RadioButton customGameReplayRadioButton;
        private System.Windows.Forms.RadioButton leagueRadioButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.ComboBox leagueComboBox;
    }
}

