namespace gobang
{
    partial class gameUI
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gameUI));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.菜单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PVCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PFirstToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PVPToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chessBoard = new System.Windows.Forms.PictureBox();
            this.matchPlayerlistBox = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.nameText = new System.Windows.Forms.TextBox();
            this.NetButton = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chessBoard)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.菜单ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(718, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 菜单ToolStripMenuItem
            // 
            this.菜单ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PVCToolStripMenuItem,
            this.PVPToolStripMenuItem,
            this.toolStripMenuItem1,
            this.ExitToolStripMenuItem});
            this.菜单ToolStripMenuItem.Name = "菜单ToolStripMenuItem";
            this.菜单ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.菜单ToolStripMenuItem.Text = "游戏";
            // 
            // PVCToolStripMenuItem
            // 
            this.PVCToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.PFirstToolStripMenuItem});
            this.PVCToolStripMenuItem.Name = "PVCToolStripMenuItem";
            this.PVCToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.PVCToolStripMenuItem.Text = "人机对战";
            this.PVCToolStripMenuItem.Click += new System.EventHandler(this.PVCStartToolStripMenuItem_Click);
            // 
            // PFirstToolStripMenuItem
            // 
            this.PFirstToolStripMenuItem.Checked = true;
            this.PFirstToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.PFirstToolStripMenuItem.Name = "PFirstToolStripMenuItem";
            this.PFirstToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.PFirstToolStripMenuItem.Text = "玩家优先";
            this.PFirstToolStripMenuItem.Click += new System.EventHandler(this.PFirstToolStripMenuItem_Click);
            // 
            // PVPToolStripMenuItem
            // 
            this.PVPToolStripMenuItem.Name = "PVPToolStripMenuItem";
            this.PVPToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.PVPToolStripMenuItem.Text = "网络对战";
            this.PVPToolStripMenuItem.Click += new System.EventHandler(this.PVPToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(149, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.ExitToolStripMenuItem.Text = "退出";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ExitToolStripMenuItem_Click);
            // 
            // chessBoard
            // 
            this.chessBoard.BackgroundImage = global::gobang.Properties.Resources.bg;
            this.chessBoard.Location = new System.Drawing.Point(12, 28);
            this.chessBoard.Name = "chessBoard";
            this.chessBoard.Size = new System.Drawing.Size(450, 450);
            this.chessBoard.TabIndex = 1;
            this.chessBoard.TabStop = false;
            // 
            // matchPlayerlistBox
            // 
            this.matchPlayerlistBox.FormattingEnabled = true;
            this.matchPlayerlistBox.ItemHeight = 12;
            this.matchPlayerlistBox.Location = new System.Drawing.Point(6, 20);
            this.matchPlayerlistBox.Name = "matchPlayerlistBox";
            this.matchPlayerlistBox.Size = new System.Drawing.Size(171, 220);
            this.matchPlayerlistBox.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.matchPlayerlistBox);
            this.groupBox1.Location = new System.Drawing.Point(502, 65);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(183, 253);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "玩家列表";
            // 
            // nameLabel
            // 
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.nameLabel.Location = new System.Drawing.Point(506, 39);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(51, 20);
            this.nameLabel.TabIndex = 4;
            this.nameLabel.Text = "名字：";
            // 
            // nameText
            // 
            this.nameText.Location = new System.Drawing.Point(564, 38);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(96, 21);
            this.nameText.TabIndex = 5;
            // 
            // NetButton
            // 
            this.NetButton.Enabled = false;
            this.NetButton.Location = new System.Drawing.Point(544, 324);
            this.NetButton.Name = "NetButton";
            this.NetButton.Size = new System.Drawing.Size(89, 23);
            this.NetButton.TabIndex = 6;
            this.NetButton.Text = "网络对战开始";
            this.NetButton.UseVisualStyleBackColor = true;
            this.NetButton.Click += new System.EventHandler(this.NetButton_Click);
            // 
            // gameUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(718, 483);
            this.Controls.Add(this.NetButton);
            this.Controls.Add(this.nameText);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chessBoard);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "gameUI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "五子棋";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.gameUI_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chessBoard)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 菜单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PVCToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem ExitToolStripMenuItem;
        private System.Windows.Forms.PictureBox chessBoard;
        private System.Windows.Forms.ToolStripMenuItem PFirstToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PVPToolStripMenuItem;
        private System.Windows.Forms.ListBox matchPlayerlistBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.TextBox nameText;
        private System.Windows.Forms.Button NetButton;
    }
}

