namespace GetProcessorInformation
{
    partial class FormProcessorInfo
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormProcessorInfo));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.plotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateRateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusNoProcessors = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusNoCores = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLogicalProcessors = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.txtCPUUsage = new System.Windows.Forms.TextBox();
            this.toolStripStatusLabelMemory = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Blue Highway", 8.999999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMemory,
            this.toolStripDropDownButton1,
            this.toolStripStatusLabel1,
            this.toolStripStatusNoProcessors,
            this.toolStripStatusLabel2,
            this.toolStripStatusNoCores,
            this.toolStripStatusLabel3,
            this.toolStripStatusLogicalProcessors});
            this.statusStrip1.Location = new System.Drawing.Point(0, 217);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 7, 0);
            this.statusStrip1.Size = new System.Drawing.Size(256, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.plotToolStripMenuItem,
            this.updateRateToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(46, 20);
            this.toolStripDropDownButton1.Text = "Format";
            this.toolStripDropDownButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
            // 
            // plotToolStripMenuItem
            // 
            this.plotToolStripMenuItem.Name = "plotToolStripMenuItem";
            this.plotToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.plotToolStripMenuItem.Text = "Plot";
            this.plotToolStripMenuItem.Click += new System.EventHandler(this.plotToolStripMenuItem_Click);
            // 
            // updateRateToolStripMenuItem
            // 
            this.updateRateToolStripMenuItem.Name = "updateRateToolStripMenuItem";
            this.updateRateToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.updateRateToolStripMenuItem.Text = "Set Update Rate";
            this.updateRateToolStripMenuItem.Click += new System.EventHandler(this.updateRateToolStripMenuItem_Click);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Left;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(79, 17);
            this.toolStripStatusLabel1.Text = "No. Processors:  ";
            // 
            // toolStripStatusNoProcessors
            // 
            this.toolStripStatusNoProcessors.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusNoProcessors.Name = "toolStripStatusNoProcessors";
            this.toolStripStatusNoProcessors.Size = new System.Drawing.Size(12, 17);
            this.toolStripStatusNoProcessors.Text = "1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(54, 17);
            this.toolStripStatusLabel2.Text = "No. Cores:  ";
            // 
            // toolStripStatusNoCores
            // 
            this.toolStripStatusNoCores.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusNoCores.Name = "toolStripStatusNoCores";
            this.toolStripStatusNoCores.Size = new System.Drawing.Size(4, 17);
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(90, 12);
            this.toolStripStatusLabel3.Text = "Logical Processors:  ";
            // 
            // toolStripStatusLogicalProcessors
            // 
            this.toolStripStatusLogicalProcessors.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLogicalProcessors.Name = "toolStripStatusLogicalProcessors";
            this.toolStripStatusLogicalProcessors.Size = new System.Drawing.Size(4, 4);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // txtCPUUsage
            // 
            this.txtCPUUsage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCPUUsage.Location = new System.Drawing.Point(0, 0);
            this.txtCPUUsage.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.txtCPUUsage.Multiline = true;
            this.txtCPUUsage.Name = "txtCPUUsage";
            this.txtCPUUsage.Size = new System.Drawing.Size(256, 217);
            this.txtCPUUsage.TabIndex = 1;
            // 
            // toolStripStatusLabelMemory
            // 
            this.toolStripStatusLabelMemory.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabelMemory.Name = "toolStripStatusLabelMemory";
            this.toolStripStatusLabelMemory.Size = new System.Drawing.Size(4, 17);
            // 
            // FormProcessorInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(3F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 239);
            this.Controls.Add(this.txtCPUUsage);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Blue Highway Condensed", 8.249999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(1, 3, 1, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormProcessorInfo";
            this.Text = "Processor Information";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormProcessorInfo_FormClosing);
            this.Shown += new System.EventHandler(this.FormProcessorInfo_Shown);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusNoProcessors;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusNoCores;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLogicalProcessors;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TextBox txtCPUUsage;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem plotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateRateToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMemory;
    }
}

