using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace GetVolCurrUtil.Properties
{
	// Token: 0x02000024 RID: 36
	[CompilerGenerated]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	internal class Resources
	{
		// Token: 0x06000130 RID: 304 RVA: 0x000074A9 File Offset: 0x000056A9
		internal Resources()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000131 RID: 305 RVA: 0x000074B4 File Offset: 0x000056B4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					ResourceManager temp = new ResourceManager("GetVolCurrUtil.Properties.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = temp;
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000132 RID: 306 RVA: 0x000074ED File Offset: 0x000056ED
		// (set) Token: 0x06000133 RID: 307 RVA: 0x000074F4 File Offset: 0x000056F4
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x0400010B RID: 267
		private static ResourceManager resourceMan;

		// Token: 0x0400010C RID: 268
		private static CultureInfo resourceCulture;
	}
}
