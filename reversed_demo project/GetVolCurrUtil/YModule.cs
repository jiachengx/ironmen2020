using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

// Token: 0x0200001E RID: 30
public class YModule : YFunction
{
	// Token: 0x060000C7 RID: 199 RVA: 0x00005B84 File Offset: 0x00003D84
	public YModule(string func) : base("Module", func)
	{
		this._productName = "!INVALID!";
		this._serialNumber = "!INVALID!";
		this._logicalName = "!INVALID!";
		this._productId = -1L;
		this._productRelease = -1L;
		this._firmwareRelease = "!INVALID!";
		this._persistentSettings = -1L;
		this._luminosity = -1L;
		this._beacon = -1L;
		this._upTime = -9223372036854775807L;
		this._usbCurrent = -9223372036854775807L;
		this._rebootCountdown = -2147483648L;
		this._usbBandwidth = -1L;
	}

	// Token: 0x060000C8 RID: 200 RVA: 0x00005C24 File Offset: 0x00003E24
	protected override int _parse(YAPI.TJSONRECORD j)
	{
		YAPI.TJSONRECORD member = default(YAPI.TJSONRECORD);
		if (j.recordtype == YAPI.TJSONRECORDTYPE.JSON_STRUCT)
		{
			for (int i = 0; i <= j.membercount - 1; i++)
			{
				member = j.members[i];
				if (member.name == "productName")
				{
					this._productName = member.svalue;
				}
				else if (member.name == "serialNumber")
				{
					this._serialNumber = member.svalue;
				}
				else if (member.name == "logicalName")
				{
					this._logicalName = member.svalue;
				}
				else if (member.name == "productId")
				{
					this._productId = member.ivalue;
				}
				else if (member.name == "productRelease")
				{
					this._productRelease = member.ivalue;
				}
				else if (member.name == "firmwareRelease")
				{
					this._firmwareRelease = member.svalue;
				}
				else if (member.name == "persistentSettings")
				{
					this._persistentSettings = member.ivalue;
				}
				else if (member.name == "luminosity")
				{
					this._luminosity = member.ivalue;
				}
				else if (member.name == "beacon")
				{
					this._beacon = ((member.ivalue > 0L) ? 1L : 0L);
				}
				else if (member.name == "upTime")
				{
					this._upTime = member.ivalue;
				}
				else if (member.name == "usbCurrent")
				{
					this._usbCurrent = member.ivalue;
				}
				else if (member.name == "rebootCountdown")
				{
					this._rebootCountdown = member.ivalue;
				}
				else if (member.name == "usbBandwidth")
				{
					this._usbBandwidth = member.ivalue;
				}
			}
			return 0;
		}
		return -1;
	}

	// Token: 0x060000C9 RID: 201 RVA: 0x00005E54 File Offset: 0x00004054
	public string get_productName()
	{
		if (this._productName == "!INVALID!" && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._productName;
	}

	// Token: 0x060000CA RID: 202 RVA: 0x00005E82 File Offset: 0x00004082
	public string get_serialNumber()
	{
		if (this._serialNumber == "!INVALID!" && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._serialNumber;
	}

	// Token: 0x060000CB RID: 203 RVA: 0x00005EB0 File Offset: 0x000040B0
	public string get_logicalName()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._logicalName;
	}

	// Token: 0x060000CC RID: 204 RVA: 0x00005EDC File Offset: 0x000040DC
	public int set_logicalName(string newval)
	{
		return base._setAttr("logicalName", newval);
	}

	// Token: 0x060000CD RID: 205 RVA: 0x00005EF7 File Offset: 0x000040F7
	public int get_productId()
	{
		if (this._productId == -1L && YAPI.YISERR(base.load(5)))
		{
			return -1;
		}
		return (int)this._productId;
	}

	// Token: 0x060000CE RID: 206 RVA: 0x00005F1A File Offset: 0x0000411A
	public int get_productRelease()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1;
		}
		return (int)this._productRelease;
	}

	// Token: 0x060000CF RID: 207 RVA: 0x00005F40 File Offset: 0x00004140
	public string get_firmwareRelease()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._firmwareRelease;
	}

	// Token: 0x060000D0 RID: 208 RVA: 0x00005F69 File Offset: 0x00004169
	public int get_persistentSettings()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1;
		}
		return (int)this._persistentSettings;
	}

	// Token: 0x060000D1 RID: 209 RVA: 0x00005F90 File Offset: 0x00004190
	public int set_persistentSettings(int newval)
	{
		string rest_val = newval.ToString();
		return base._setAttr("persistentSettings", rest_val);
	}

	// Token: 0x060000D2 RID: 210 RVA: 0x00005FB4 File Offset: 0x000041B4
	public int saveToFlash()
	{
		string rest_val = "1";
		return base._setAttr("persistentSettings", rest_val);
	}

	// Token: 0x060000D3 RID: 211 RVA: 0x00005FD4 File Offset: 0x000041D4
	public int revertFromFlash()
	{
		string rest_val = "0";
		return base._setAttr("persistentSettings", rest_val);
	}

	// Token: 0x060000D4 RID: 212 RVA: 0x00005FF3 File Offset: 0x000041F3
	public int get_luminosity()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1;
		}
		return (int)this._luminosity;
	}

	// Token: 0x060000D5 RID: 213 RVA: 0x0000601C File Offset: 0x0000421C
	public int set_luminosity(int newval)
	{
		string rest_val = newval.ToString();
		return base._setAttr("luminosity", rest_val);
	}

	// Token: 0x060000D6 RID: 214 RVA: 0x0000603D File Offset: 0x0000423D
	public int get_beacon()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1;
		}
		return (int)this._beacon;
	}

	// Token: 0x060000D7 RID: 215 RVA: 0x00006064 File Offset: 0x00004264
	public int set_beacon(int newval)
	{
		string rest_val = (newval > 0) ? "1" : "0";
		return base._setAttr("beacon", rest_val);
	}

	// Token: 0x060000D8 RID: 216 RVA: 0x0000608E File Offset: 0x0000428E
	public long get_upTime()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -9223372036854775807L;
		}
		return this._upTime;
	}

	// Token: 0x060000D9 RID: 217 RVA: 0x000060BB File Offset: 0x000042BB
	public long get_usbCurrent()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -9223372036854775807L;
		}
		return this._usbCurrent;
	}

	// Token: 0x060000DA RID: 218 RVA: 0x000060E8 File Offset: 0x000042E8
	public int get_rebootCountdown()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return int.MinValue;
		}
		return (int)this._rebootCountdown;
	}

	// Token: 0x060000DB RID: 219 RVA: 0x00006114 File Offset: 0x00004314
	public int set_rebootCountdown(int newval)
	{
		string rest_val = newval.ToString();
		return base._setAttr("rebootCountdown", rest_val);
	}

	// Token: 0x060000DC RID: 220 RVA: 0x00006138 File Offset: 0x00004338
	public int reboot(int secBeforeReboot)
	{
		string rest_val = secBeforeReboot.ToString();
		return base._setAttr("rebootCountdown", rest_val);
	}

	// Token: 0x060000DD RID: 221 RVA: 0x0000615C File Offset: 0x0000435C
	public int triggerFirmwareUpdate(int secBeforeReboot)
	{
		string rest_val = (-secBeforeReboot).ToString();
		return base._setAttr("rebootCountdown", rest_val);
	}

	// Token: 0x060000DE RID: 222 RVA: 0x00006180 File Offset: 0x00004380
	public int get_usbBandwidth()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1;
		}
		return (int)this._usbBandwidth;
	}

	// Token: 0x060000DF RID: 223 RVA: 0x000061A8 File Offset: 0x000043A8
	public int set_usbBandwidth(int newval)
	{
		string rest_val = newval.ToString();
		return base._setAttr("usbBandwidth", rest_val);
	}

	// Token: 0x060000E0 RID: 224 RVA: 0x000061CC File Offset: 0x000043CC
	public YModule nextModule()
	{
		string hwid = "";
		if (YAPI.YISERR(base._nextFunction(ref hwid)))
		{
			return null;
		}
		if (hwid == "")
		{
			return null;
		}
		return YModule.FindModule(hwid);
	}

	// Token: 0x060000E1 RID: 225 RVA: 0x00006205 File Offset: 0x00004405
	public void registerValueCallback(YModule.UpdateCallback callback)
	{
		if (callback != null)
		{
			base._registerFuncCallback(this);
		}
		else
		{
			base._unregisterFuncCallback(this);
		}
		this._callback = new YModule.UpdateCallback(callback.Invoke);
	}

	// Token: 0x060000E2 RID: 226 RVA: 0x0000622C File Offset: 0x0000442C
	public void set_callback(YModule.UpdateCallback callback)
	{
		this.registerValueCallback(callback);
	}

	// Token: 0x060000E3 RID: 227 RVA: 0x00006235 File Offset: 0x00004435
	public void setCallback(YModule.UpdateCallback callback)
	{
		this.registerValueCallback(callback);
	}

	// Token: 0x060000E4 RID: 228 RVA: 0x0000623E File Offset: 0x0000443E
	public override void advertiseValue(string value)
	{
		if (this._callback != null)
		{
			this._callback(this, value);
		}
	}

	// Token: 0x060000E5 RID: 229 RVA: 0x00006258 File Offset: 0x00004458
	public override string get_friendlyName()
	{
		int fundesc = 0;
		int devdesc = 0;
		string funcName = "";
		string dummy = "";
		string errmsg = "";
		string snum = "";
		string funcid = "";
		int retcode = base._getDescriptor(ref fundesc, ref errmsg);
		if (YAPI.YISERR(retcode))
		{
			base._throw(retcode, errmsg);
			return "!INVALID!";
		}
		retcode = YAPI.yapiGetFunctionInfo(fundesc, ref devdesc, ref snum, ref funcid, ref funcName, ref dummy, ref errmsg);
		if (YAPI.YISERR(retcode))
		{
			base._throw(retcode, errmsg);
			return "!INVALID!";
		}
		if (funcName != "")
		{
			return funcName;
		}
		return snum;
	}

	// Token: 0x060000E6 RID: 230 RVA: 0x000062E8 File Offset: 0x000044E8
	public void setImmutableAttributes(ref YAPI.yDeviceSt infos)
	{
		this._serialNumber = infos.serial;
		this._productName = infos.productname;
		this._productId = (long)((ulong)infos.deviceid);
	}

	// Token: 0x060000E7 RID: 231 RVA: 0x00006310 File Offset: 0x00004510
	private int _getFunction(int idx, ref string serial, ref string funcId, ref string funcName, ref string funcVal, ref string errmsg)
	{
		List<uint> functions = null;
		YAPI.YDevice dev = null;
		int devdescr = 0;
		int res = base._getDevice(ref dev, ref errmsg);
		if (YAPI.YISERR(res))
		{
			base._throw(res, errmsg);
			return res;
		}
		res = dev.getFunctions(ref functions, ref errmsg);
		if (YAPI.YISERR(res))
		{
			return res;
		}
		int fundescr = Convert.ToInt32(functions[idx]);
		res = YAPI.yapiGetFunctionInfo(fundescr, ref devdescr, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg);
		if (YAPI.YISERR(res))
		{
			return res;
		}
		return 0;
	}

	// Token: 0x060000E8 RID: 232 RVA: 0x00006394 File Offset: 0x00004594
	public int functionCount()
	{
		List<uint> functions = null;
		YAPI.YDevice dev = null;
		string errmsg = "";
		int res = base._getDevice(ref dev, ref errmsg);
		if (YAPI.YISERR(res))
		{
			base._throw(res, errmsg);
			return res;
		}
		res = dev.getFunctions(ref functions, ref errmsg);
		if (YAPI.YISERR(res))
		{
			functions = null;
			base._throw(res, errmsg);
			return res;
		}
		return functions.Count;
	}

	// Token: 0x060000E9 RID: 233 RVA: 0x00006400 File Offset: 0x00004600
	public string functionId(int functionIndex)
	{
		string serial = "";
		string funcId = "";
		string funcName = "";
		string funcVal = "";
		string errmsg = "";
		int res = this._getFunction(functionIndex, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg);
		if (YAPI.YISERR(res))
		{
			base._throw(res, errmsg);
			return "!INVALID!";
		}
		return funcId;
	}

	// Token: 0x060000EA RID: 234 RVA: 0x00006464 File Offset: 0x00004664
	public string functionName(int functionIndex)
	{
		string serial = "";
		string funcId = "";
		string funcName = "";
		string funcVal = "";
		string errmsg = "";
		int res = this._getFunction(functionIndex, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg);
		if (YAPI.YISERR(res))
		{
			base._throw(res, errmsg);
			return "!INVALID!";
		}
		return funcName;
	}

	// Token: 0x060000EB RID: 235 RVA: 0x000064C8 File Offset: 0x000046C8
	public string functionValue(int functionIndex)
	{
		string serial = "";
		string funcId = "";
		string funcName = "";
		string funcVal = "";
		string errmsg = "";
		int res = this._getFunction(functionIndex, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg);
		if (YAPI.YISERR(res))
		{
			base._throw(res, errmsg);
			return "!INVALID!";
		}
		return funcVal;
	}

	// Token: 0x060000EC RID: 236 RVA: 0x0000652C File Offset: 0x0000472C
	public static YModule FindModule(string func)
	{
		if (YModule._ModuleCache.ContainsKey(func))
		{
			return (YModule)YModule._ModuleCache[func];
		}
		YModule res = new YModule(func);
		YModule._ModuleCache.Add(func, res);
		return res;
	}

	// Token: 0x060000ED RID: 237 RVA: 0x0000656C File Offset: 0x0000476C
	public static YModule FirstModule()
	{
		int[] v_fundescr = new int[1];
		int dev = 0;
		int neededsize = 0;
		string serial = null;
		string funcId = null;
		string funcName = null;
		string funcVal = null;
		string errmsg = "";
		int size = Marshal.SizeOf(v_fundescr[0]);
		IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(v_fundescr[0]));
		int err = YAPI.apiGetFunctionsByClass("Module", 0, p, size, ref neededsize, ref errmsg);
		Marshal.Copy(p, v_fundescr, 0, 1);
		Marshal.FreeHGlobal(p);
		if (YAPI.YISERR(err) | neededsize == 0)
		{
			return null;
		}
		serial = "";
		funcId = "";
		funcName = "";
		funcVal = "";
		errmsg = "";
		if (YAPI.YISERR(YAPI.yapiGetFunctionInfo(v_fundescr[0], ref dev, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg)))
		{
			return null;
		}
		return YModule.FindModule(serial + "." + funcId);
	}

	// Token: 0x060000EE RID: 238 RVA: 0x00006643 File Offset: 0x00004843
	private static void _ModuleCleanup()
	{
	}

	// Token: 0x040000BE RID: 190
	public const string PRODUCTNAME_INVALID = "!INVALID!";

	// Token: 0x040000BF RID: 191
	public const string SERIALNUMBER_INVALID = "!INVALID!";

	// Token: 0x040000C0 RID: 192
	public const string LOGICALNAME_INVALID = "!INVALID!";

	// Token: 0x040000C1 RID: 193
	public const int PRODUCTID_INVALID = -1;

	// Token: 0x040000C2 RID: 194
	public const int PRODUCTRELEASE_INVALID = -1;

	// Token: 0x040000C3 RID: 195
	public const string FIRMWARERELEASE_INVALID = "!INVALID!";

	// Token: 0x040000C4 RID: 196
	public const int PERSISTENTSETTINGS_LOADED = 0;

	// Token: 0x040000C5 RID: 197
	public const int PERSISTENTSETTINGS_SAVED = 1;

	// Token: 0x040000C6 RID: 198
	public const int PERSISTENTSETTINGS_MODIFIED = 2;

	// Token: 0x040000C7 RID: 199
	public const int PERSISTENTSETTINGS_INVALID = -1;

	// Token: 0x040000C8 RID: 200
	public const int LUMINOSITY_INVALID = -1;

	// Token: 0x040000C9 RID: 201
	public const int BEACON_OFF = 0;

	// Token: 0x040000CA RID: 202
	public const int BEACON_ON = 1;

	// Token: 0x040000CB RID: 203
	public const int BEACON_INVALID = -1;

	// Token: 0x040000CC RID: 204
	public const long UPTIME_INVALID = -9223372036854775807L;

	// Token: 0x040000CD RID: 205
	public const long USBCURRENT_INVALID = -9223372036854775807L;

	// Token: 0x040000CE RID: 206
	public const int REBOOTCOUNTDOWN_INVALID = -2147483648;

	// Token: 0x040000CF RID: 207
	public const int USBBANDWIDTH_SIMPLE = 0;

	// Token: 0x040000D0 RID: 208
	public const int USBBANDWIDTH_DOUBLE = 1;

	// Token: 0x040000D1 RID: 209
	public const int USBBANDWIDTH_INVALID = -1;

	// Token: 0x040000D2 RID: 210
	private static Hashtable _ModuleCache = new Hashtable();

	// Token: 0x040000D3 RID: 211
	private YModule.UpdateCallback _callback;

	// Token: 0x040000D4 RID: 212
	protected string _productName;

	// Token: 0x040000D5 RID: 213
	protected string _serialNumber;

	// Token: 0x040000D6 RID: 214
	protected string _logicalName;

	// Token: 0x040000D7 RID: 215
	protected long _productId;

	// Token: 0x040000D8 RID: 216
	protected long _productRelease;

	// Token: 0x040000D9 RID: 217
	protected string _firmwareRelease;

	// Token: 0x040000DA RID: 218
	protected long _persistentSettings;

	// Token: 0x040000DB RID: 219
	protected long _luminosity;

	// Token: 0x040000DC RID: 220
	protected long _beacon;

	// Token: 0x040000DD RID: 221
	protected long _upTime;

	// Token: 0x040000DE RID: 222
	protected long _usbCurrent;

	// Token: 0x040000DF RID: 223
	protected long _rebootCountdown;

	// Token: 0x040000E0 RID: 224
	protected long _usbBandwidth;

	// Token: 0x0200001F RID: 31
	// (Invoke) Token: 0x060000F1 RID: 241
	public delegate void UpdateCallback(YModule func, string value);
}
