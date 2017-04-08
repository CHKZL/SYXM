namespace 检波器项目01
{
    partial class STX
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
            this.BT = new System.Windows.Forms.Label();
            this.STB = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // BT
            // 
            this.BT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BT.AutoSize = true;
            this.BT.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.BT.Location = new System.Drawing.Point(211, 9);
            this.BT.Name = "BT";
            this.BT.Size = new System.Drawing.Size(89, 20);
            this.BT.TabIndex = 0;
            this.BT.Text = "选择通道";
            // 
            // STB
            // 
            this.STB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.STB.Location = new System.Drawing.Point(9, 9);
            this.STB.Margin = new System.Windows.Forms.Padding(0);
            this.STB.Name = "STB";
            this.STB.ScrollGrace = 0D;
            this.STB.ScrollMaxX = 0D;
            this.STB.ScrollMaxY = 0D;
            this.STB.ScrollMaxY2 = 0D;
            this.STB.ScrollMinX = 0D;
            this.STB.ScrollMinY = 0D;
            this.STB.ScrollMinY2 = 0D;
            this.STB.Size = new System.Drawing.Size(516, 143);
            this.STB.TabIndex = 1;
            this.STB.UseExtendedPrintDialog = true;
            // 
            // STX
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 161);
            this.Controls.Add(this.STB);
            this.Controls.Add(this.BT);
            this.Name = "STX";
            this.Text = "STX";
            this.Load += new System.EventHandler(this.STX_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BT;
        private ZedGraph.ZedGraphControl STB;
    }
}