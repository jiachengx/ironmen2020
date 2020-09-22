using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace GetVolCurrUtil.Properties
{
	// Token: 0x02000025 RID: 37
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
	[CompilerGenerated]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000134 RID: 308 RVA: 0x000074FC File Offset: 0x000056FC
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x0400010D RID: 269
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
