using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace GetVolCurrUtil
{
	// Token: 0x02000002 RID: 2
	public partial class Form1 : Form
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public Form1()
		{
			this.InitializeComponent();
			if (YAPI.RegisterHub("usb", ref this.errmsg) != 0)
			{
				MessageBox.Show("RegisterHub error: " + this.errmsg, "Error");
				return;
			}
			this.logCurrAl = new List<string>();
			this.logVolAl = new List<string>();
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020B8 File Offset: 0x000002B8
		private void startBtn_Click(object sender, EventArgs e)
		{
			if (this.startBtn.Text.Equals("Start"))
			{
				this.remainTime.Text = "";
				this.logVolAl.Clear();
				this.logCurrAl.Clear();
				this.cSum = 0.0;
				this.vSum = 0.0;
				this.ccount = 0;
				if (this.intervalTB.Text.Trim().Equals(""))
				{
					MessageBox.Show("Please insert interval time.", "Warning");
					return;
				}
				if (this.testTimeTB.Text.Trim().Equals(""))
				{
					MessageBox.Show("Please insert test time.", "Warning");
					return;
				}
				try
				{
					this.timer1.Interval = Convert.ToInt32(this.intervalTB.Text.Trim());
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error(Interval Time)");
					return;
				}
				try
				{
					this.timeCount = Convert.ToInt32(this.testTimeTB.Text.Trim());
				}
				catch (Exception ex2)
				{
					MessageBox.Show(ex2.Message, "Error(Test Time)");
					return;
				}
				this.voltageSensor = YVoltage.FirstVoltage();
				if (this.voltageSensor == null || !this.voltageSensor.isOnline())
				{
					MessageBox.Show("Voltage module not connected.", "Error");
					return;
				}
				this.m = this.voltageSensor.get_module();
				this.voltageSensorDC = YVoltage.FindVoltage(this.m.get_serialNumber() + ".voltage1");
				this.m = null;
				this.currentSensor = YCurrent.FirstCurrent();
				if (this.currentSensor != null && this.currentSensor.isOnline())
				{
					this.m = this.currentSensor.get_module();
					this.currentSensorDC = YCurrent.FindCurrent(this.m.get_serialNumber() + ".current1");
					this.m = null;
					this.startBtn.Text = "Stop";
					this.timer1.Enabled = true;
					this.timer2.Enabled = true;
					this.remainTime.Visible = true;
					this.intervalTB.Enabled = false;
					this.testTimeTB.Enabled = false;
					return;
				}
				MessageBox.Show("Current module not connected.", "Error");
				return;
			}
			else
			{
				this.timer1.Enabled = false;
				this.timer2.Enabled = false;
				this.remainTime.Visible = false;
				this.intervalTB.Enabled = true;
				this.testTimeTB.Enabled = true;
				this.startBtn.Text = "Start";
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002384 File Offset: 0x00000584
		private void Form1_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002388 File Offset: 0x00000588
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.timeCount == 0)
			{
				this.timer1.Enabled = false;
				this.timer2.Enabled = false;
				this.remainTime.Visible = false;
				this.writeCSVFile(this.logVolAl, this.logCurrAl);
				return;
			}
			this.voltageTB.Text = this.voltageSensorDC.get_currentRawValue().ToString("F2");
			this.logVolAl.Add(this.voltageTB.Text);
			this.currentTB.Text = this.currentSensorDC.get_currentRawValue().ToString();
			this.logCurrAl.Add(this.currentTB.Text);
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002441 File Offset: 0x00000641
		private void timer2_Tick(object sender, EventArgs e)
		{
			this.timeCount--;
			this.remainTime.Text = this.timeCount.ToString();
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002468 File Offset: 0x00000668
		private void writeCSVFile(List<string> vol, List<string> curr)
		{
			this.Text = "Writing to Log File ....";
			DateTime now = DateTime.Now;
			string text = "";
			text += now.Year.ToString();
			if (now.Month < 10)
			{
				text += "0";
			}
			text += now.Month.ToString();
			if (now.Day < 10)
			{
				text += "0";
			}
			text = text + now.Day.ToString() + "-";
			if (now.Hour < 10)
			{
				text += "0";
			}
			text += now.Hour.ToString();
			if (now.Minute < 10)
			{
				text += "0";
			}
			text += now.Minute.ToString();
			if (now.Second < 10)
			{
				text += "0";
			}
			text += now.Second.ToString();
			Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Log");
			using (StreamWriter streamWriter = new StreamWriter(Directory.GetCurrentDirectory() + "\\Log\\Benchmark_" + text + ".csv"))
			{
				streamWriter.WriteLine("Voltage,Current");
				for (int i = 0; i < vol.Count; i++)
				{
					streamWriter.WriteLine(vol[i] + "," + curr[i]);
					if (!(vol[i] == "0") || !(curr[i] == "0"))
					{
						this.cSum += Convert.ToDouble(curr[i]);
						this.vSum += Convert.ToDouble(vol[i]);
						this.ccount++;
					}
				}
				streamWriter.WriteLine(string.Format("\nAvg. Voltage, {0:F2}", this.vSum / (double)this.ccount));
				streamWriter.WriteLine(string.Format("Avg. Current, {0:F2}", this.cSum / (double)this.ccount));
				streamWriter.WriteLine(string.Format("Avg. Watt, {0:F2}", this.vSum * this.cSum / (double)(this.ccount * this.ccount)));
			}
			this.Text = "Get Data";
			this.startBtn.Text = "Start";
			this.intervalTB.Enabled = true;
			this.testTimeTB.Enabled = true;
		}

		// Token: 0x04000001 RID: 1
		private string errmsg = "";

		// Token: 0x04000002 RID: 2
		private YVoltage voltageSensor;

		// Token: 0x04000003 RID: 3
		private YModule m;

		// Token: 0x04000004 RID: 4
		private YVoltage voltageSensorDC;

		// Token: 0x04000005 RID: 5
		private YCurrent currentSensor;

		// Token: 0x04000006 RID: 6
		private YCurrent currentSensorDC;

		// Token: 0x04000007 RID: 7
		private int ccount;

		// Token: 0x04000008 RID: 8
		private double cSum;

		// Token: 0x04000009 RID: 9
		private double vSum;

		// Token: 0x0400000A RID: 10
		private int timeCount;

		// Token: 0x0400000B RID: 11
		private List<string> logVolAl;

		// Token: 0x0400000C RID: 12
		private List<string> logCurrAl;
	}
}
