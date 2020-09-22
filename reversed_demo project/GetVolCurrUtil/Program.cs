using System;
using System.Windows.Forms;

namespace GetVolCurrUtil
{
	// Token: 0x02000003 RID: 3
	internal static class Program
	{
		// Token: 0x06000009 RID: 9 RVA: 0x00002CD5 File Offset: 0x00000ED5
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}
