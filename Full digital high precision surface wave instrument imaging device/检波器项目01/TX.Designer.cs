namespace 检波器项目01
{
    partial class TX
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
            this.components = new System.ComponentModel.Container();
            this.TB = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // TB
            // 
            this.TB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TB.Location = new System.Drawing.Point(9, 9);
            this.TB.Margin = new System.Windows.Forms.Padding(0);
            this.TB.Name = "TB";
            this.TB.ScrollGrace = 0D;
            this.TB.ScrollMaxX = 0D;
            this.TB.ScrollMaxY = 0D;
            this.TB.ScrollMaxY2 = 0D;
            this.TB.ScrollMinX = 0D;
            this.TB.ScrollMinY = 0D;
            this.TB.ScrollMinY2 = 0D;
            this.TB.Size = new System.Drawing.Size(926, 383);
            this.TB.TabIndex = 0;
            this.TB.UseExtendedPrintDialog = true;
            // 
            // TX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 401);
            this.Controls.Add(this.TB);
            this.Name = "TX";
            this.Text = "TX";
            this.Load += new System.EventHandler(this.TX_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl TB;
    }
}