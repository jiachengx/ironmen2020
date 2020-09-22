using System;
using System.Collections;
using System.Runtime.InteropServices;

// Token: 0x02000020 RID: 32
public class YCurrent : YFunction
{
	// Token: 0x060000F4 RID: 244 RVA: 0x00006654 File Offset: 0x00004854
	public YCurrent(string func) : base("Current", func)
	{
		this._logicalName = "!INVALID!";
		this._advertisedValue = "!INVALID!";
		this._unit = "!INVALID!";
		this._currentValue = -1.79769313486231E+308;
		this._lowestValue = -1.79769313486231E+308;
		this._highestValue = -1.79769313486231E+308;
		this._currentRawValue = -1.79769313486231E+308;
		this._resolution = -1.79769313486231E+308;
		this._calibrationParam = "!INVALID!";
		this._calibrationOffset = -32767L;
	}

	// Token: 0x060000F5 RID: 245 RVA: 0x000066F0 File Offset: 0x000048F0
	protected override int _parse(YAPI.TJSONRECORD j)
	{
		YAPI.TJSONRECORD member = default(YAPI.TJSONRECORD);
		if (j.recordtype == YAPI.TJSONRECORDTYPE.JSON_STRUCT)
		{
			for (int i = 0; i <= j.membercount - 1; i++)
			{
				member = j.members[i];
				if (member.name == "logicalName")
				{
					this._logicalName = member.svalue;
				}
				else if (member.name == "advertisedValue")
				{
					this._advertisedValue = member.svalue;
				}
				else if (member.name == "unit")
				{
					this._unit = member.svalue;
				}
				else if (member.name == "currentValue")
				{
					this._currentValue = Math.Round((double)member.ivalue / 65536.0);
				}
				else if (member.name == "lowestValue")
				{
					this._lowestValue = Math.Round((double)member.ivalue / 65536.0);
				}
				else if (member.name == "highestValue")
				{
					this._highestValue = Math.Round((double)member.ivalue / 65536.0);
				}
				else if (member.name == "currentRawValue")
				{
					this._currentRawValue = (double)member.ivalue / 65536.0;
				}
				else if (member.name == "resolution")
				{
					this._resolution = 1.0 / Math.Round(65536.0 / (double)member.ivalue);
				}
				else if (member.name == "calibrationParam")
				{
					this._calibrationParam = member.svalue;
				}
			}
			return 0;
		}
		return -1;
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x000068DB File Offset: 0x00004ADB
	public string get_logicalName()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._logicalName;
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x00006904 File Offset: 0x00004B04
	public int set_logicalName(string newval)
	{
		return base._setAttr("logicalName", newval);
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x0000691F File Offset: 0x00004B1F
	public string get_advertisedValue()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._advertisedValue;
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x00006948 File Offset: 0x00004B48
	public string get_unit()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._unit;
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00006974 File Offset: 0x00004B74
	public double get_currentValue()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1.79769313486231E+308;
		}
		double res = YAPI._applyCalibration(this._currentRawValue, this._calibrationParam, this._calibrationOffset, this._resolution);
		if (res != -1.79769313486231E+308)
		{
			return res;
		}
		return this._currentValue;
	}

	// Token: 0x060000FB RID: 251 RVA: 0x000069D8 File Offset: 0x00004BD8
	public int set_lowestValue(double newval)
	{
		string rest_val = Math.Round(newval * 65536.0).ToString();
		return base._setAttr("lowestValue", rest_val);
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00006A0A File Offset: 0x00004C0A
	public double get_lowestValue()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1.79769313486231E+308;
		}
		return this._lowestValue;
	}

	// Token: 0x060000FD RID: 253 RVA: 0x00006A38 File Offset: 0x00004C38
	public int set_highestValue(double newval)
	{
		string rest_val = Math.Round(newval * 65536.0).ToString();
		return base._setAttr("highestValue", rest_val);
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00006A6A File Offset: 0x00004C6A
	public double get_highestValue()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1.79769313486231E+308;
		}
		return this._highestValue;
	}

	// Token: 0x060000FF RID: 255 RVA: 0x00006A97 File Offset: 0x00004C97
	public double get_currentRawValue()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1.79769313486231E+308;
		}
		return this._currentRawValue;
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00006AC4 File Offset: 0x00004CC4
	public double get_resolution()
	{
		if (this._resolution == -1.79769313486231E+308 && YAPI.YISERR(base.load(5)))
		{
			return -1.79769313486231E+308;
		}
		return this._resolution;
	}

	// Token: 0x06000101 RID: 257 RVA: 0x00006AF5 File Offset: 0x00004CF5
	public string get_calibrationParam()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._calibrationParam;
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00006B20 File Offset: 0x00004D20
	public int set_calibrationParam(string newval)
	{
		return base._setAttr("calibrationParam", newval);
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00006B3C File Offset: 0x00004D3C
	public int calibrateFromPoints(double[] rawValues, double[] refValues)
	{
		string rest_val = YAPI._encodeCalibrationPoints(rawValues, refValues, this._resolution, this._calibrationOffset, this._calibrationParam);
		return base._setAttr("calibrationParam", rest_val);
	}

	// Token: 0x06000104 RID: 260 RVA: 0x00006B70 File Offset: 0x00004D70
	public int loadCalibrationPoints(ref double[] rawValues, ref double[] refValues)
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return this._lastErrorType;
		}
		int[] dummy = null;
		return YAPI._decodeCalibrationPoints(this._calibrationParam, ref dummy, ref rawValues, ref refValues, this._resolution, this._calibrationOffset);
	}

	// Token: 0x06000105 RID: 261 RVA: 0x00006BBC File Offset: 0x00004DBC
	public YCurrent nextCurrent()
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
		return YCurrent.FindCurrent(hwid);
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00006BF5 File Offset: 0x00004DF5
	public void registerValueCallback(YCurrent.UpdateCallback callback)
	{
		if (callback != null)
		{
			base._registerFuncCallback(this);
		}
		else
		{
			base._unregisterFuncCallback(this);
		}
		this._callback = new YCurrent.UpdateCallback(callback.Invoke);
	}

	// Token: 0x06000107 RID: 263 RVA: 0x00006C1C File Offset: 0x00004E1C
	public void set_callback(YCurrent.UpdateCallback callback)
	{
		this.registerValueCallback(callback);
	}

	// Token: 0x06000108 RID: 264 RVA: 0x00006C25 File Offset: 0x00004E25
	public void setCallback(YCurrent.UpdateCallback callback)
	{
		this.registerValueCallback(callback);
	}

	// Token: 0x06000109 RID: 265 RVA: 0x00006C2E File Offset: 0x00004E2E
	public override void advertiseValue(string value)
	{
		if (this._callback != null)
		{
			this._callback(this, value);
		}
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00006C48 File Offset: 0x00004E48
	public static YCurrent FindCurrent(string func)
	{
		if (YCurrent._CurrentCache.ContainsKey(func))
		{
			return (YCurrent)YCurrent._CurrentCache[func];
		}
		YCurrent res = new YCurrent(func);
		YCurrent._CurrentCache.Add(func, res);
		return res;
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00006C88 File Offset: 0x00004E88
	public static YCurrent FirstCurrent()
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
		int err = YAPI.apiGetFunctionsByClass("Current", 0, p, size, ref neededsize, ref errmsg);
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
		return YCurrent.FindCurrent(serial + "." + funcId);
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00006D5F File Offset: 0x00004F5F
	private static void _CurrentCleanup()
	{
	}

	// Token: 0x040000E1 RID: 225
	public const string LOGICALNAME_INVALID = "!INVALID!";

	// Token: 0x040000E2 RID: 226
	public const string ADVERTISEDVALUE_INVALID = "!INVALID!";

	// Token: 0x040000E3 RID: 227
	public const string UNIT_INVALID = "!INVALID!";

	// Token: 0x040000E4 RID: 228
	public const double CURRENTVALUE_INVALID = -1.79769313486231E+308;

	// Token: 0x040000E5 RID: 229
	public const double LOWESTVALUE_INVALID = -1.79769313486231E+308;

	// Token: 0x040000E6 RID: 230
	public const double HIGHESTVALUE_INVALID = -1.79769313486231E+308;

	// Token: 0x040000E7 RID: 231
	public const double CURRENTRAWVALUE_INVALID = -1.79769313486231E+308;

	// Token: 0x040000E8 RID: 232
	public const double RESOLUTION_INVALID = -1.79769313486231E+308;

	// Token: 0x040000E9 RID: 233
	public const string CALIBRATIONPARAM_INVALID = "!INVALID!";

	// Token: 0x040000EA RID: 234
	private static Hashtable _CurrentCache = new Hashtable();

	// Token: 0x040000EB RID: 235
	private YCurrent.UpdateCallback _callback;

	// Token: 0x040000EC RID: 236
	protected string _logicalName;

	// Token: 0x040000ED RID: 237
	protected string _advertisedValue;

	// Token: 0x040000EE RID: 238
	protected string _unit;

	// Token: 0x040000EF RID: 239
	protected double _currentValue;

	// Token: 0x040000F0 RID: 240
	protected double _lowestValue;

	// Token: 0x040000F1 RID: 241
	protected double _highestValue;

	// Token: 0x040000F2 RID: 242
	protected double _currentRawValue;

	// Token: 0x040000F3 RID: 243
	protected double _resolution;

	// Token: 0x040000F4 RID: 244
	protected string _calibrationParam;

	// Token: 0x040000F5 RID: 245
	protected long _calibrationOffset;

	// Token: 0x02000021 RID: 33
	// (Invoke) Token: 0x0600010F RID: 271
	public delegate void UpdateCallback(YCurrent func, string value);
}
