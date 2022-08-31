namespace wangtaomaoweihuan
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.item_name = new System.Windows.Forms.TextBox();
            this.item_type = new System.Windows.Forms.TextBox();
            this.item_ctime = new System.Windows.Forms.TextBox();
            this.item_mname = new System.Windows.Forms.TextBox();
            this.item_size = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(108, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(108, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "类型：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(108, 173);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(112, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "创建文件时间：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(108, 237);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(112, 15);
            this.label4.TabIndex = 3;
            this.label4.Text = "修改文件时间：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(108, 301);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 15);
            this.label5.TabIndex = 4;
            this.label5.Text = "大小：";
            // 
            // item_name
            // 
            this.item_name.Location = new System.Drawing.Point(270, 51);
            this.item_name.Name = "item_name";
            this.item_name.ReadOnly = true;
            this.item_name.Size = new System.Drawing.Size(310, 25);
            this.item_name.TabIndex = 5;
            // 
            // item_type
            // 
            this.item_type.Location = new System.Drawing.Point(270, 119);
            this.item_type.Name = "item_type";
            this.item_type.ReadOnly = true;
            this.item_type.Size = new System.Drawing.Size(310, 25);
            this.item_type.TabIndex = 6;
            // 
            // item_ctime
            // 
            this.item_ctime.Location = new System.Drawing.Point(270, 173);
            this.item_ctime.Name = "item_ctime";
            this.item_ctime.ReadOnly = true;
            this.item_ctime.Size = new System.Drawing.Size(310, 25);
            this.item_ctime.TabIndex = 7;
            // 
            // item_mname
            // 
            this.item_mname.Location = new System.Drawing.Point(270, 237);
            this.item_mname.Name = "item_mname";
            this.item_mname.ReadOnly = true;
            this.item_mname.Size = new System.Drawing.Size(310, 25);
            this.item_mname.TabIndex = 8;
            // 
            // item_size
            // 
            this.item_size.Location = new System.Drawing.Point(270, 290);
            this.item_size.Name = "item_size";
            this.item_size.ReadOnly = true;
            this.item_size.Size = new System.Drawing.Size(310, 25);
            this.item_size.TabIndex = 9;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.item_size);
            this.Controls.Add(this.item_mname);
            this.Controls.Add(this.item_ctime);
            this.Controls.Add(this.item_type);
            this.Controls.Add(this.item_name);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox item_name;
        private System.Windows.Forms.TextBox item_type;
        private System.Windows.Forms.TextBox item_ctime;
        private System.Windows.Forms.TextBox item_mname;
        private System.Windows.Forms.TextBox item_size;
    }
}