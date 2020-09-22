namespace GetVolCurrUtil
{
	// Token: 0x02000002 RID: 2
	public partial class Form1 : global::System.Windows.Forms.Form
	{
		// Token: 0x06000007 RID: 7 RVA: 0x00002738 File Offset: 0x00000938
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002758 File Offset: 0x00000958
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.startBtn = new global::System.Windows.Forms.Button();
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.voltageTB = new global::System.Windows.Forms.TextBox();
			this.currentTB = new global::System.Windows.Forms.TextBox();
			this.intervalTB = new global::System.Windows.Forms.TextBox();
			this.testTimeTB = new global::System.Windows.Forms.TextBox();
			this.remainTime = new global::System.Windows.Forms.Label();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.timer2 = new global::System.Windows.Forms.Timer(this.components);
			base.SuspendLayout();
			this.startBtn.Location = new global::System.Drawing.Point(12, 160);
			this.startBtn.Name = "startBtn";
			this.startBtn.Size = new global::System.Drawing.Size(174, 23);
			this.startBtn.TabIndex = 0;
			this.startBtn.Text = "Start";
			this.startBtn.UseVisualStyleBackColor = true;
			this.startBtn.Click += new global::System.EventHandler(this.startBtn_Click);
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(12, 13);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(69, 12);
			this.label1.TabIndex = 2;
			this.label1.Text = "Voltage (V) : ";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(12, 41);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(75, 12);
			this.label2.TabIndex = 3;
			this.label2.Text = "Current (mA) :";
			this.label3.AutoSize = true;
			this.label3.Location = new global::System.Drawing.Point(12, 78);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(91, 12);
			this.label3.TabIndex = 4;
			this.label3.Text = "Inteval Time (ms):";
			this.label4.AutoSize = true;
			this.label4.Location = new global::System.Drawing.Point(12, 109);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(79, 12);
			this.label4.TabIndex = 5;
			this.label4.Text = "Test Time (sec):";
			this.voltageTB.Location = new global::System.Drawing.Point(111, 10);
			this.voltageTB.Name = "voltageTB";
			this.voltageTB.Size = new global::System.Drawing.Size(75, 22);
			this.voltageTB.TabIndex = 6;
			this.currentTB.Location = new global::System.Drawing.Point(111, 38);
			this.currentTB.Name = "currentTB";
			this.currentTB.Size = new global::System.Drawing.Size(75, 22);
			this.currentTB.TabIndex = 7;
			this.intervalTB.Location = new global::System.Drawing.Point(111, 78);
			this.intervalTB.Name = "intervalTB";
			this.intervalTB.Size = new global::System.Drawing.Size(75, 22);
			this.intervalTB.TabIndex = 8;
			this.testTimeTB.Location = new global::System.Drawing.Point(111, 109);
			this.testTimeTB.Name = "testTimeTB";
			this.testTimeTB.Size = new global::System.Drawing.Size(75, 22);
			this.testTimeTB.TabIndex = 9;
			this.remainTime.AutoSize = true;
			this.remainTime.Location = new global::System.Drawing.Point(12, 135);
			this.remainTime.Name = "remainTime";
			this.remainTime.Size = new global::System.Drawing.Size(8, 12);
			this.remainTime.TabIndex = 10;
			this.remainTime.Text = ".";
			this.timer1.Interval = 1000;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			this.timer2.Interval = 1000;
			this.timer2.Tick += new global::System.EventHandler(this.timer2_Tick);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(198, 196);
			base.Controls.Add(this.remainTime);
			base.Controls.Add(this.testTimeTB);
			base.Controls.Add(this.intervalTB);
			base.Controls.Add(this.currentTB);
			base.Controls.Add(this.voltageTB);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.startBtn);
			base.Name = "Form1";
			this.Text = "Get Data";
			base.Load += new global::System.EventHandler(this.Form1_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0400000D RID: 13
		private global::System.ComponentModel.IContainer components;

		// Token: 0x0400000E RID: 14
		private global::System.Windows.Forms.Button startBtn;

		// Token: 0x0400000F RID: 15
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000010 RID: 16
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000011 RID: 17
		private global::System.Windows.Forms.Label label3;

		// Token: 0x04000012 RID: 18
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000013 RID: 19
		private global::System.Windows.Forms.TextBox voltageTB;

		// Token: 0x04000014 RID: 20
		private global::System.Windows.Forms.TextBox currentTB;

		// Token: 0x04000015 RID: 21
		private global::System.Windows.Forms.TextBox intervalTB;

		// Token: 0x04000016 RID: 22
		private global::System.Windows.Forms.TextBox testTimeTB;

		// Token: 0x04000017 RID: 23
		private global::System.Windows.Forms.Label remainTime;

		// Token: 0x04000018 RID: 24
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04000019 RID: 25
		private global::System.Windows.Forms.Timer timer2;
	}
}
