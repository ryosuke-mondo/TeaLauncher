namespace CommandLauncher
{
    partial class MainWindow
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.command_box = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // command_box
            // 
            this.command_box.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.command_box.Font = new System.Drawing.Font("MS UI Gothic", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.command_box.FormattingEnabled = true;
            this.command_box.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.command_box.Location = new System.Drawing.Point(0, 0);
            this.command_box.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.command_box.Name = "command_box";
            this.command_box.Size = new System.Drawing.Size(465, 41);
            this.command_box.TabIndex = 1;
            this.command_box.SelectedIndexChanged += new System.EventHandler(this.command_box_SelectedIndexChanged);
            this.command_box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.command_box_KeyDown);
            this.command_box.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.command_box_PreviewKeyDown_1);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 42);
            this.ControlBox = false;
            this.Controls.Add(this.command_box);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainWindow";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Shown += new System.EventHandler(this.MainWindow_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox command_box;
    }
}

