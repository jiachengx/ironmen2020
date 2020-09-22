using System;
using System.Collections;
using System.Runtime.InteropServices;

// Token: 0x02000022 RID: 34
public class YVoltage : YFunction
{
	// Token: 0x06000112 RID: 274 RVA: 0x00006D70 File Offset: 0x00004F70
	public YVoltage(string func) : base("Voltage", func)
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

	// Token: 0x06000113 RID: 275 RVA: 0x00006E0C File Offset: 0x0000500C
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
					this._currentValue = Math.Round((double)member.ivalue / 655.36) / 100.0;
				}
				else if (member.name == "lowestValue")
				{
					this._lowestValue = Math.Round((double)member.ivalue / 655.36) / 100.0;
				}
				else if (member.name == "highestValue")
				{
					this._highestValue = Math.Round((double)member.ivalue / 655.36) / 100.0;
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

	// Token: 0x06000114 RID: 276 RVA: 0x00007015 File Offset: 0x00005215
	public string get_logicalName()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._logicalName;
	}

	// Token: 0x06000115 RID: 277 RVA: 0x00007040 File Offset: 0x00005240
	public int set_logicalName(string newval)
	{
		return base._setAttr("logicalName", newval);
	}

	// Token: 0x06000116 RID: 278 RVA: 0x0000705B File Offset: 0x0000525B
	public string get_advertisedValue()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._advertisedValue;
	}

	// Token: 0x06000117 RID: 279 RVA: 0x00007084 File Offset: 0x00005284
	public string get_unit()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._unit;
	}

	// Token: 0x06000118 RID: 280 RVA: 0x000070B0 File Offset: 0x000052B0
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

	// Token: 0x06000119 RID: 281 RVA: 0x00007114 File Offset: 0x00005314
	public int set_lowestValue(double newval)
	{
		string rest_val = Math.Round(newval * 65536.0).ToString();
		return base._setAttr("lowestValue", rest_val);
	}

	// Token: 0x0600011A RID: 282 RVA: 0x00007146 File Offset: 0x00005346
	public double get_lowestValue()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1.79769313486231E+308;
		}
		return this._lowestValue;
	}

	// Token: 0x0600011B RID: 283 RVA: 0x00007174 File Offset: 0x00005374
	public int set_highestValue(double newval)
	{
		string rest_val = Math.Round(newval * 65536.0).ToString();
		return base._setAttr("highestValue", rest_val);
	}

	// Token: 0x0600011C RID: 284 RVA: 0x000071A6 File Offset: 0x000053A6
	public double get_highestValue()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1.79769313486231E+308;
		}
		return this._highestValue;
	}

	// Token: 0x0600011D RID: 285 RVA: 0x000071D3 File Offset: 0x000053D3
	public double get_currentRawValue()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return -1.79769313486231E+308;
		}
		return this._currentRawValue;
	}

	// Token: 0x0600011E RID: 286 RVA: 0x00007200 File Offset: 0x00005400
	public double get_resolution()
	{
		if (this._resolution == -1.79769313486231E+308 && YAPI.YISERR(base.load(5)))
		{
			return -1.79769313486231E+308;
		}
		return this._resolution;
	}

	// Token: 0x0600011F RID: 287 RVA: 0x00007231 File Offset: 0x00005431
	public string get_calibrationParam()
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return "!INVALID!";
		}
		return this._calibrationParam;
	}

	// Token: 0x06000120 RID: 288 RVA: 0x0000725C File Offset: 0x0000545C
	public int set_calibrationParam(string newval)
	{
		return base._setAttr("calibrationParam", newval);
	}

	// Token: 0x06000121 RID: 289 RVA: 0x00007278 File Offset: 0x00005478
	public int calibrateFromPoints(double[] rawValues, double[] refValues)
	{
		string rest_val = YAPI._encodeCalibrationPoints(rawValues, refValues, this._resolution, this._calibrationOffset, this._calibrationParam);
		return base._setAttr("calibrationParam", rest_val);
	}

	// Token: 0x06000122 RID: 290 RVA: 0x000072AC File Offset: 0x000054AC
	public int loadCalibrationPoints(ref double[] rawValues, ref double[] refValues)
	{
		if (this._cacheExpiration <= YAPI.GetTickCount() && YAPI.YISERR(base.load(5)))
		{
			return this._lastErrorType;
		}
		int[] dummy = null;
		return YAPI._decodeCalibrationPoints(this._calibrationParam, ref dummy, ref rawValues, ref refValues, this._resolution, this._calibrationOffset);
	}

	// Token: 0x06000123 RID: 291 RVA: 0x000072F8 File Offset: 0x000054F8
	public YVoltage nextVoltage()
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
		return YVoltage.FindVoltage(hwid);
	}

	// Token: 0x06000124 RID: 292 RVA: 0x00007331 File Offset: 0x00005531
	public void registerValueCallback(YVoltage.UpdateCallback callback)
	{
		if (callback != null)
		{
			base._registerFuncCallback(this);
		}
		else
		{
			base._unregisterFuncCallback(this);
		}
		this._callback = new YVoltage.UpdateCallback(callback.Invoke);
	}

	// Token: 0x06000125 RID: 293 RVA: 0x00007358 File Offset: 0x00005558
	public void set_callback(YVoltage.UpdateCallback callback)
	{
		this.registerValueCallback(callback);
	}

	// Token: 0x06000126 RID: 294 RVA: 0x00007361 File Offset: 0x00005561
	public void setCallback(YVoltage.UpdateCallback callback)
	{
		this.registerValueCallback(callback);
	}

	// Token: 0x06000127 RID: 295 RVA: 0x0000736A File Offset: 0x0000556A
	public override void advertiseValue(string value)
	{
		if (this._callback != null)
		{
			this._callback(this, value);
		}
	}

	// Token: 0x06000128 RID: 296 RVA: 0x00007384 File Offset: 0x00005584
	public static YVoltage FindVoltage(string func)
	{
		if (YVoltage._VoltageCache.ContainsKey(func))
		{
			return (YVoltage)YVoltage._VoltageCache[func];
		}
		YVoltage res = new YVoltage(func);
		YVoltage._VoltageCache.Add(func, res);
		return res;
	}

	// Token: 0x06000129 RID: 297 RVA: 0x000073C4 File Offset: 0x000055C4
	public static YVoltage FirstVoltage()
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
		int err = YAPI.apiGetFunctionsByClass("Voltage", 0, p, size, ref neededsize, ref errmsg);
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
		return YVoltage.FindVoltage(serial + "." + funcId);
	}

	// Token: 0x0600012A RID: 298 RVA: 0x0000749B File Offset: 0x0000569B
	private static void _VoltageCleanup()
	{
	}

	// Token: 0x040000F6 RID: 246
	public const string LOGICALNAME_INVALID = "!INVALID!";

	// Token: 0x040000F7 RID: 247
	public const string ADVERTISEDVALUE_INVALID = "!INVALID!";

	// Token: 0x040000F8 RID: 248
	public const string UNIT_INVALID = "!INVALID!";

	// Token: 0x040000F9 RID: 249
	public const double CURRENTVALUE_INVALID = -1.79769313486231E+308;

	// Token: 0x040000FA RID: 250
	public const double LOWESTVALUE_INVALID = -1.79769313486231E+308;

	// Token: 0x040000FB RID: 251
	public const double HIGHESTVALUE_INVALID = -1.79769313486231E+308;

	// Token: 0x040000FC RID: 252
	public const double CURRENTRAWVALUE_INVALID = -1.79769313486231E+308;

	// Token: 0x040000FD RID: 253
	public const double RESOLUTION_INVALID = -1.79769313486231E+308;

	// Token: 0x040000FE RID: 254
	public const string CALIBRATIONPARAM_INVALID = "!INVALID!";

	// Token: 0x040000FF RID: 255
	private static Hashtable _VoltageCache = new Hashtable();

	// Token: 0x04000100 RID: 256
	private YVoltage.UpdateCallback _callback;

	// Token: 0x04000101 RID: 257
	protected string _logicalName;

	// Token: 0x04000102 RID: 258
	protected string _advertisedValue;

	// Token: 0x04000103 RID: 259
	protected string _unit;

	// Token: 0x04000104 RID: 260
	protected double _currentValue;

	// Token: 0x04000105 RID: 261
	protected double _lowestValue;

	// Token: 0x04000106 RID: 262
	protected double _highestValue;

	// Token: 0x04000107 RID: 263
	protected double _currentRawValue;

	// Token: 0x04000108 RID: 264
	protected double _resolution;

	// Token: 0x04000109 RID: 265
	protected string _calibrationParam;

	// Token: 0x0400010A RID: 266
	protected long _calibrationOffset;

	// Token: 0x02000023 RID: 35
	// (Invoke) Token: 0x0600012D RID: 301
	public delegate void UpdateCallback(YVoltage func, string value);
}
