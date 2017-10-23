namespace ExtractHtml
{
    partial class Form1
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.OpenFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.combChildren = new System.Windows.Forms.ComboBox();
            this.gotoNode = new System.Windows.Forms.Button();
            this.textXPath = new System.Windows.Forms.TextBox();
            this.htmlPath = new System.Windows.Forms.TextBox();
            this.getChild = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.getChild);
            this.splitContainer1.Panel1.Controls.Add(this.OpenFile);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.combChildren);
            this.splitContainer1.Panel1.Controls.Add(this.gotoNode);
            this.splitContainer1.Panel1.Controls.Add(this.textXPath);
            this.splitContainer1.Panel1.Controls.Add(this.htmlPath);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox4);
            this.splitContainer1.Size = new System.Drawing.Size(786, 483);
            this.splitContainer1.SplitterDistance = 342;
            this.splitContainer1.TabIndex = 0;
            // 
            // OpenFile
            // 
            this.OpenFile.Location = new System.Drawing.Point(252, 80);
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(75, 23);
            this.OpenFile.TabIndex = 13;
            this.OpenFile.Text = "选择Html";
            this.OpenFile.UseVisualStyleBackColor = true;
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "子节点集：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "当前节点的XPath:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "本地Html文件路径：";
            // 
            // textBox4
            // 
            this.textBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox4.Location = new System.Drawing.Point(0, 0);
            this.textBox4.Multiline = true;
            this.textBox4.Name = "textBox4";
            this.textBox4.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox4.Size = new System.Drawing.Size(440, 483);
            this.textBox4.TabIndex = 6;
            // 
            // combChildren
            // 
            this.combChildren.FormattingEnabled = true;
            this.combChildren.Location = new System.Drawing.Point(116, 250);
            this.combChildren.Name = "combChildren";
            this.combChildren.Size = new System.Drawing.Size(99, 20);
            this.combChildren.TabIndex = 4;
            this.combChildren.SelectedIndexChanged += new System.EventHandler(this.combChildren_SelectedIndexChanged);
            // 
            // gotoNode
            // 
            this.gotoNode.Location = new System.Drawing.Point(252, 247);
            this.gotoNode.Name = "gotoNode";
            this.gotoNode.Size = new System.Drawing.Size(75, 23);
            this.gotoNode.TabIndex = 3;
            this.gotoNode.Text = "转到节点";
            this.gotoNode.UseVisualStyleBackColor = true;
            this.gotoNode.Click += new System.EventHandler(this.gotoNode_Click);
            // 
            // textXPath
            // 
            this.textXPath.Location = new System.Drawing.Point(50, 141);
            this.textXPath.Name = "textXPath";
            this.textXPath.Size = new System.Drawing.Size(277, 21);
            this.textXPath.TabIndex = 2;
            // 
            // htmlPath
            // 
            this.htmlPath.Location = new System.Drawing.Point(50, 53);
            this.htmlPath.Name = "htmlPath";
            this.htmlPath.Size = new System.Drawing.Size(277, 21);
            this.htmlPath.TabIndex = 1;
            // 
            // getChild
            // 
            this.getChild.Location = new System.Drawing.Point(252, 169);
            this.getChild.Name = "getChild";
            this.getChild.Size = new System.Drawing.Size(75, 23);
            this.getChild.TabIndex = 14;
            this.getChild.Text = "获取子节点";
            this.getChild.UseVisualStyleBackColor = true;
            this.getChild.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 483);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.ComboBox combChildren;
        private System.Windows.Forms.Button gotoNode;
        private System.Windows.Forms.TextBox textXPath;
        private System.Windows.Forms.TextBox htmlPath;
        private System.Windows.Forms.Button OpenFile;
        private System.Windows.Forms.Button getChild;
    }
}

