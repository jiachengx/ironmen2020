using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x02000004 RID: 4
public class YAPI
{
	// Token: 0x0600000A RID: 10 RVA: 0x00002CEC File Offset: 0x00000EEC
	public static bool YISERR(int retcode)
	{
		return retcode < 0;
	}

	// Token: 0x0600000B RID: 11 RVA: 0x00002CF5 File Offset: 0x00000EF5
	public static void YblockingCallback(YAPI.YDevice device, ref YAPI.blockingCallbackCtx context, int returnval, string result, string errmsg)
	{
		context.res = returnval;
		context.response = result;
		context.errmsg = errmsg;
	}

	// Token: 0x0600000C RID: 12 RVA: 0x00002D10 File Offset: 0x00000F10
	public static void DisableExceptions()
	{
		YAPI.ExceptionsDisabled = true;
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002D18 File Offset: 0x00000F18
	public static void EnableExceptions()
	{
		YAPI.ExceptionsDisabled = false;
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002D20 File Offset: 0x00000F20
	private static void native_yLogFunction(IntPtr log, uint loglen)
	{
		if (YAPI.ylog != null)
		{
			YAPI.ylog(Marshal.PtrToStringAnsi(log));
		}
	}

	// Token: 0x0600000F RID: 15 RVA: 0x00002D39 File Offset: 0x00000F39
	public static void RegisterLogFunction(YAPI.yLogFunc logfun)
	{
		YAPI.ylog = logfun;
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002D44 File Offset: 0x00000F44
	private static YAPI.yDeviceSt emptyDeviceSt()
	{
		return new YAPI.yDeviceSt
		{
			vendorid = 0,
			deviceid = 0,
			devrelease = 0,
			nbinbterfaces = 0,
			manufacturer = "",
			productname = "",
			serial = "",
			logicalname = "",
			firmware = "",
			beacon = 0
		};
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002DC0 File Offset: 0x00000FC0
	private static YAPI.yapiEvent emptyApiEvent()
	{
		return new YAPI.yapiEvent
		{
			eventtype = YAPI.yapiEventType.YAPI_NOP,
			modul = null,
			fun_descr = 0,
			value = ""
		};
	}

	// Token: 0x06000012 RID: 18 RVA: 0x00002DFC File Offset: 0x00000FFC
	private static void native_yDeviceArrivalCallback(int d)
	{
		YAPI.yDeviceSt infos = YAPI.emptyDeviceSt();
		YAPI.yapiEvent ev = YAPI.emptyApiEvent();
		string errmsg = "";
		ev.eventtype = YAPI.yapiEventType.YAPI_DEV_ARRIVAL;
		if (YAPI.yapiGetDeviceInfo(d, ref infos, ref errmsg) != 0)
		{
			return;
		}
		ev.modul = YModule.FindModule(infos.serial + ".module");
		ev.modul.setImmutableAttributes(ref infos);
		if (YAPI.yArrival != null)
		{
			YAPI._PlugEvents.Add(ev);
		}
	}

	// Token: 0x06000013 RID: 19 RVA: 0x00002E70 File Offset: 0x00001070
	public static void RegisterDeviceArrivalCallback(YAPI.yDeviceUpdateFunc arrivalCallback)
	{
		YAPI.yArrival = arrivalCallback;
		if (arrivalCallback != null)
		{
			string error = "";
			for (YModule mod = YModule.FirstModule(); mod != null; mod = mod.nextModule())
			{
				if (mod.isOnline())
				{
					YAPI.yapiLockDeviceCallBack(ref error);
					YAPI.native_yDeviceArrivalCallback(mod.functionDescriptor());
					YAPI.yapiUnlockDeviceCallBack(ref error);
				}
			}
		}
	}

	// Token: 0x06000014 RID: 20 RVA: 0x00002EC4 File Offset: 0x000010C4
	private static void native_yDeviceRemovalCallback(int d)
	{
		YAPI.yapiEvent ev = YAPI.emptyApiEvent();
		YAPI.yDeviceSt infos = YAPI.emptyDeviceSt();
		string errmsg = "";
		if (YAPI.yRemoval == null)
		{
			return;
		}
		ev.fun_descr = 0;
		ev.value = "";
		ev.eventtype = YAPI.yapiEventType.YAPI_DEV_REMOVAL;
		infos.deviceid = 0;
		if (YAPI.yapiGetDeviceInfo(d, ref infos, ref errmsg) != 0)
		{
			return;
		}
		ev.modul = YModule.FindModule(infos.serial + ".module");
		YAPI._PlugEvents.Add(ev);
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002F44 File Offset: 0x00001144
	public static void RegisterDeviceRemovalCallback(YAPI.yDeviceUpdateFunc removalCallback)
	{
		YAPI.yRemoval = removalCallback;
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002F4C File Offset: 0x0000114C
	public static void native_yDeviceChangeCallback(int d)
	{
		YAPI.yapiEvent ev = YAPI.emptyApiEvent();
		YAPI.yDeviceSt infos = YAPI.emptyDeviceSt();
		string errmsg = "";
		if (YAPI.yChange == null)
		{
			return;
		}
		ev.eventtype = YAPI.yapiEventType.YAPI_DEV_CHANGE;
		if (YAPI.yapiGetDeviceInfo(d, ref infos, ref errmsg) != 0)
		{
			return;
		}
		ev.modul = YModule.FindModule(infos.serial + ".module");
		YAPI._PlugEvents.Add(ev);
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002FB0 File Offset: 0x000011B0
	public static void RegisterDeviceChangeCallback(YAPI.yDeviceUpdateFunc callback)
	{
		YAPI.yChange = callback;
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002FB8 File Offset: 0x000011B8
	private static void queuesCleanUp()
	{
		YAPI._PlugEvents.Clear();
		YAPI._PlugEvents = null;
		YAPI._DataEvents.Clear();
		YAPI._DataEvents = null;
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002FDC File Offset: 0x000011DC
	private static void native_yFunctionUpdateCallback(int f, IntPtr data)
	{
		YAPI.yapiEvent ev = YAPI.emptyApiEvent();
		ev.fun_descr = f;
		if (IntPtr.Zero.Equals(data))
		{
			ev.eventtype = YAPI.yapiEventType.YAPI_FUN_UPDATE;
		}
		else
		{
			ev.eventtype = YAPI.yapiEventType.YAPI_FUN_VALUE;
			ev.value = Marshal.PtrToStringAnsi(data);
		}
		YAPI._DataEvents.Add(ev);
	}

	// Token: 0x0600001A RID: 26 RVA: 0x0000303C File Offset: 0x0000123C
	public static void RegisterCalibrationHandler(int calibType, YAPI.yCalibrationHandler callback)
	{
		string key = calibType.ToString();
		YFunction._CalibHandlers.Add(key, callback);
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00003060 File Offset: 0x00001260
	private static double yLinearCalibrationHandler(double rawValue, int calibType, int[] parameters, double[] rawValues, double[] refValues)
	{
		int npt = calibType % 10;
		double x = rawValues[0];
		double adj = refValues[0] - x;
		int i = 0;
		if (npt > rawValues.Length)
		{
			npt = rawValues.Length;
		}
		if (npt > refValues.Length)
		{
			npt = refValues.Length + 1;
		}
		while (rawValue > rawValues[i] && i + 1 < npt)
		{
			i++;
			double x2 = x;
			double adj2 = adj;
			x = rawValues[i];
			adj = refValues[i] - x;
			if (rawValue < x && x > x2)
			{
				adj = adj2 + (adj - adj2) * (rawValue - x2) / (x - x2);
			}
		}
		return rawValue + adj;
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000030DC File Offset: 0x000012DC
	private static int yapiLockDeviceCallBack(ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		buffer.Length = 0;
		int functionReturnValue = YAPI._yapiLockDeviceCallBack(buffer);
		errmsg = buffer.ToString();
		return functionReturnValue;
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00003110 File Offset: 0x00001310
	private static int yapiUnlockDeviceCallBack(ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		buffer.Length = 0;
		int functionReturnValue = YAPI._yapiUnlockDeviceCallBack(buffer);
		errmsg = buffer.ToString();
		return functionReturnValue;
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00003144 File Offset: 0x00001344
	private static int yapiLockFunctionCallBack(ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		buffer.Length = 0;
		int functionReturnValue = YAPI._yapiLockFunctionCallBack(buffer);
		errmsg = buffer.ToString();
		return functionReturnValue;
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00003178 File Offset: 0x00001378
	private static int yapiUnlockFunctionCallBack(ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		buffer.Length = 0;
		int functionReturnValue = YAPI._yapiUnlockFunctionCallBack(buffer);
		errmsg = buffer.ToString();
		return functionReturnValue;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x000031AC File Offset: 0x000013AC
	public static YAPI.yCalibrationHandler _getCalibrationHandler(int calType)
	{
		string key = calType.ToString();
		if (YFunction._CalibHandlers.ContainsKey(key))
		{
			return YFunction._CalibHandlers[key];
		}
		return null;
	}

	// Token: 0x06000021 RID: 33 RVA: 0x000031DC File Offset: 0x000013DC
	public static double _decimalToDouble(int val)
	{
		bool negate = false;
		if (val == 0)
		{
			return 0.0;
		}
		if (val > 32767)
		{
			negate = true;
			val = 65536 - val;
		}
		else if (val < 0)
		{
			negate = true;
			val = -val;
		}
		int exp = val >> 11;
		double res = (double)(val & 2047) * YAPI.decExp[exp];
		if (!negate)
		{
			return res;
		}
		return -res;
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00003234 File Offset: 0x00001434
	private static long _doubleToDecimal(double val)
	{
		int negate = 0;
		if (val == 0.0)
		{
			return 0L;
		}
		if (val < 0.0)
		{
			negate = 1;
			val = -val;
		}
		double comp = val / 1999.0;
		int decpow = 0;
		while (comp > YAPI.decExp[decpow] && decpow < 15)
		{
			decpow++;
		}
		double mant = val / YAPI.decExp[decpow];
		long res;
		if (decpow == 15 && mant > 2047.0)
		{
			res = 32767L;
		}
		else
		{
			res = (long)((decpow << 11) + Convert.ToInt32(mant));
		}
		if (negate == 0)
		{
			return res;
		}
		return -res;
	}

	// Token: 0x06000023 RID: 35 RVA: 0x000032C4 File Offset: 0x000014C4
	public static string _encodeCalibrationPoints(double[] rawValues, double[] refValues, double resolution, long calibrationOffset, string actualCparams)
	{
		int npt = (rawValues.Length < refValues.Length) ? rawValues.Length : refValues.Length;
		int minRaw = 0;
		if (npt == 0)
		{
			return "";
		}
		int calibType;
		if (actualCparams == "")
		{
			calibType = 10 + npt;
		}
		else
		{
			int pos = actualCparams.IndexOf(',');
			calibType = Convert.ToInt32(actualCparams.Substring(0, pos));
			if (calibType <= 10)
			{
				calibType = npt;
			}
			else
			{
				calibType = 10 + npt;
			}
		}
		string res = calibType.ToString();
		if (calibType <= 10)
		{
			for (int i = 0; i < npt; i++)
			{
				int rawVal = (int)(rawValues[i] / resolution - (double)calibrationOffset + 0.5);
				if (rawVal >= minRaw && rawVal < 65536)
				{
					int refVal = (int)(refValues[i] / resolution - (double)calibrationOffset + 0.5);
					if (refVal >= 0 && refVal < 65536)
					{
						string text = res;
						res = string.Concat(new string[]
						{
							text,
							",",
							rawVal.ToString(),
							",",
							refVal.ToString()
						});
						minRaw = rawVal + 1;
					}
				}
			}
		}
		else
		{
			for (int j = 0; j < npt; j++)
			{
				int rawVal = (int)YAPI._doubleToDecimal(rawValues[j]);
				int refVal = (int)YAPI._doubleToDecimal(refValues[j]);
				string text2 = res;
				res = string.Concat(new string[]
				{
					text2,
					",",
					rawVal.ToString(),
					",",
					refVal.ToString()
				});
			}
		}
		return res;
	}

	// Token: 0x06000024 RID: 36 RVA: 0x0000344C File Offset: 0x0000164C
	public static int _decodeCalibrationPoints(string calibParams, ref int[] intPt, ref double[] rawPt, ref double[] calPt, double resolution, long calibrationOffset)
	{
		string[] valuesStr = calibParams.Split(new char[]
		{
			','
		});
		if (valuesStr.Length <= 1)
		{
			return 0;
		}
		int calibType = Convert.ToInt32(valuesStr[0]);
		int nval = 99;
		if (calibType < 20)
		{
			nval = 2 * (calibType % 10);
		}
		intPt = new int[nval];
		rawPt = new double[nval / 2];
		calPt = new double[nval / 2];
		int i = 1;
		while (i < nval && i < valuesStr.Length)
		{
			int rawval = Convert.ToInt32(valuesStr[i]);
			int calval = Convert.ToInt32(valuesStr[i + 1]);
			double rawval_d;
			double calval_d;
			if (calibType <= 10)
			{
				rawval_d = (double)((long)rawval + calibrationOffset) * resolution;
				calval_d = (double)((long)calval + calibrationOffset) * resolution;
			}
			else
			{
				rawval_d = YAPI._decimalToDouble(rawval);
				calval_d = YAPI._decimalToDouble(calval);
			}
			intPt[i - 1] = rawval;
			intPt[i] = calval;
			rawPt[i - 1 >> 1] = rawval_d;
			calPt[i - 1 >> 1] = calval_d;
			i += 2;
		}
		return calibType;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00003528 File Offset: 0x00001728
	public static double _applyCalibration(double rawValue, string calibParams, long calibOffset, double resolution)
	{
		if (rawValue == -1.79769313486231E+308 || resolution == -1.79769313486231E+308)
		{
			return -1.79769313486231E+308;
		}
		if (calibParams.IndexOf(",") <= 0)
		{
			return -1.79769313486231E+308;
		}
		int[] cur_calpar = null;
		double[] cur_calraw = null;
		double[] cur_calref = null;
		int calibType = YAPI._decodeCalibrationPoints(calibParams, ref cur_calpar, ref cur_calraw, ref cur_calref, resolution, calibOffset);
		if (calibType == 0)
		{
			return rawValue;
		}
		YAPI.yCalibrationHandler calhdl = YAPI._getCalibrationHandler(calibType);
		if (calhdl == null)
		{
			return -1.79769313486231E+308;
		}
		return calhdl(rawValue, calibType, cur_calpar, cur_calraw, cur_calref);
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000035AC File Offset: 0x000017AC
	public static string GetAPIVersion()
	{
		string version = null;
		string date = null;
		YAPI.apiGetAPIVersion(ref version, ref date);
		return "1.01.9166 (" + version + ")";
	}

	// Token: 0x06000027 RID: 39 RVA: 0x000035D8 File Offset: 0x000017D8
	public static int InitAPI(int mode, ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		if (YAPI._apiInitialized)
		{
			return 0;
		}
		string version = null;
		string date = null;
		if (YAPI.apiGetAPIVersion(ref version, ref date) != 257)
		{
			errmsg = "yapi.dll does does not match the version of the Libary (Libary=1.01.9166";
			errmsg = errmsg + " yapi.dll=" + version + ")";
			return -5;
		}
		YAPI.csmodule_initialization();
		buffer.Length = 0;
		int res = YAPI._yapiInitAPI(mode, buffer);
		errmsg = buffer.ToString();
		if (YAPI.YISERR(res))
		{
			return res;
		}
		YAPI._yapiRegisterDeviceArrivalCallback(Marshal.GetFunctionPointerForDelegate(YAPI.native_yDeviceArrivalDelegate));
		YAPI._yapiRegisterDeviceRemovalCallback(Marshal.GetFunctionPointerForDelegate(YAPI.native_yDeviceRemovalDelegate));
		YAPI._yapiRegisterDeviceChangeCallback(Marshal.GetFunctionPointerForDelegate(YAPI.native_yDeviceChangeDelegate));
		YAPI._yapiRegisterFunctionUpdateCallback(Marshal.GetFunctionPointerForDelegate(YAPI.native_yFunctionUpdateDelegate));
		YAPI._yapiRegisterLogFunction(Marshal.GetFunctionPointerForDelegate(YAPI.native_yLogFunctionDelegate));
		for (int i = 1; i <= 20; i++)
		{
			YAPI.RegisterCalibrationHandler(i, new YAPI.yCalibrationHandler(YAPI.yLinearCalibrationHandler));
		}
		YAPI._apiInitialized = true;
		return res;
	}

	// Token: 0x06000028 RID: 40 RVA: 0x000036CF File Offset: 0x000018CF
	public static void FreeAPI()
	{
		if (YAPI._apiInitialized)
		{
			YAPI._yapiFreeAPI();
			YAPI.csmodule_cleanup();
			YAPI._apiInitialized = false;
		}
	}

	// Token: 0x06000029 RID: 41 RVA: 0x000036E8 File Offset: 0x000018E8
	public static int RegisterHub(string url, ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		int res;
		if (!YAPI._apiInitialized)
		{
			res = YAPI.InitAPI(0, ref errmsg);
			if (YAPI.YISERR(res))
			{
				return res;
			}
		}
		buffer.Length = 0;
		res = YAPI._yapiRegisterHub(new StringBuilder(url), buffer);
		if (YAPI.YISERR(res))
		{
			errmsg = buffer.ToString();
		}
		return res;
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00003740 File Offset: 0x00001940
	public static int PreregisterHub(string url, ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		int res;
		if (!YAPI._apiInitialized)
		{
			res = YAPI.InitAPI(0, ref errmsg);
			if (YAPI.YISERR(res))
			{
				return res;
			}
		}
		buffer.Length = 0;
		res = YAPI._yapiPreregisterHub(new StringBuilder(url), buffer);
		if (YAPI.YISERR(res))
		{
			errmsg = buffer.ToString();
		}
		return res;
	}

	// Token: 0x0600002B RID: 43 RVA: 0x00003796 File Offset: 0x00001996
	public static void UnregisterHub(string url)
	{
		if (!YAPI._apiInitialized)
		{
			return;
		}
		YAPI._yapiUnregisterHub(new StringBuilder(url));
	}

	// Token: 0x0600002C RID: 44 RVA: 0x000037AC File Offset: 0x000019AC
	public static int UpdateDeviceList(ref string errmsg)
	{
		StringBuilder errbuffer = new StringBuilder(256);
		YAPI.yapiEvent p = default(YAPI.yapiEvent);
		int res;
		if (!YAPI._apiInitialized)
		{
			res = YAPI.InitAPI(0, ref errmsg);
			if (YAPI.YISERR(res))
			{
				return res;
			}
		}
		res = YAPI.yapiUpdateDeviceList(0U, ref errmsg);
		if (YAPI.YISERR(res))
		{
			return res;
		}
		errbuffer.Length = 0;
		res = YAPI._yapiHandleEvents(errbuffer);
		if (YAPI.YISERR(res))
		{
			errmsg = errbuffer.ToString();
			return res;
		}
		while (YAPI._PlugEvents.Count > 0)
		{
			YAPI.yapiLockDeviceCallBack(ref errmsg);
			p = YAPI._PlugEvents[0];
			YAPI._PlugEvents.RemoveAt(0);
			YAPI.yapiUnlockDeviceCallBack(ref errmsg);
			switch (p.eventtype)
			{
			case YAPI.yapiEventType.YAPI_DEV_ARRIVAL:
				if (YAPI.yArrival != null)
				{
					YAPI.yArrival(p.modul);
				}
				break;
			case YAPI.yapiEventType.YAPI_DEV_REMOVAL:
				if (YAPI.yRemoval != null)
				{
					YAPI.yRemoval(p.modul);
				}
				break;
			case YAPI.yapiEventType.YAPI_DEV_CHANGE:
				if (YAPI.yChange != null)
				{
					YAPI.yChange(p.modul);
				}
				break;
			}
		}
		return 0;
	}

	// Token: 0x0600002D RID: 45 RVA: 0x000038BC File Offset: 0x00001ABC
	public static int HandleEvents(ref string errmsg)
	{
		StringBuilder errBuffer = new StringBuilder(256);
		YAPI.yapiEvent ev = default(YAPI.yapiEvent);
		errBuffer.Length = 0;
		int res = YAPI._yapiHandleEvents(errBuffer);
		if (YAPI.YISERR(res))
		{
			errmsg = errBuffer.ToString();
			return res;
		}
		while (YAPI._DataEvents.Count > 0)
		{
			YAPI.yapiLockFunctionCallBack(ref errmsg);
			if (YAPI._DataEvents.Count == 0)
			{
				YAPI.yapiUnlockFunctionCallBack(ref errmsg);
				break;
			}
			ev = YAPI._DataEvents[0];
			YAPI._DataEvents.RemoveAt(0);
			YAPI.yapiUnlockFunctionCallBack(ref errmsg);
			if (ev.eventtype == YAPI.yapiEventType.YAPI_FUN_VALUE)
			{
				for (int i = 0; i < YFunction._FunctionCallbacks.Count; i++)
				{
					if (YFunction._FunctionCallbacks[i].get_functionDescriptor() == ev.fun_descr)
					{
						YFunction._FunctionCallbacks[i].advertiseValue(ev.value);
					}
				}
			}
		}
		return 0;
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000039AC File Offset: 0x00001BAC
	public static int Sleep(int ms_duration, ref string errmsg)
	{
		StringBuilder errBuffer = new StringBuilder(256);
		long timeout = YAPI.GetTickCount() + (long)ms_duration;
		errBuffer.Length = 0;
		int res;
		for (;;)
		{
			res = YAPI.HandleEvents(ref errmsg);
			if (YAPI.YISERR(res))
			{
				break;
			}
			if (YAPI.GetTickCount() < timeout)
			{
				res = YAPI._yapiSleep(1, errBuffer);
				if (YAPI.YISERR(res))
				{
					goto Block_3;
				}
			}
			if (YAPI.GetTickCount() >= timeout)
			{
				goto Block_4;
			}
		}
		return res;
		Block_3:
		int functionReturnValue = res;
		errmsg = errBuffer.ToString();
		return functionReturnValue;
		Block_4:
		errmsg = errBuffer.ToString();
		return res;
	}

	// Token: 0x0600002F RID: 47 RVA: 0x00003A27 File Offset: 0x00001C27
	public static long GetTickCount()
	{
		return Convert.ToInt64(YAPI._yapiGetTickCount());
	}

	// Token: 0x06000030 RID: 48 RVA: 0x00003A34 File Offset: 0x00001C34
	public static bool CheckLogicalName(string name)
	{
		return YAPI._yapiCheckLogicalName(new StringBuilder(name)) != 0;
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00003A58 File Offset: 0x00001C58
	public static int yapiGetFunctionInfo(int fundesc, ref int devdesc, ref string serial, ref string funcId, ref string funcName, ref string funcVal, ref string errmsg)
	{
		StringBuilder serialBuffer = new StringBuilder(20);
		StringBuilder funcIdBuffer = new StringBuilder(20);
		StringBuilder funcNameBuffer = new StringBuilder(20);
		StringBuilder funcValBuffer = new StringBuilder(16);
		StringBuilder errBuffer = new StringBuilder(256);
		serialBuffer.Length = 0;
		funcIdBuffer.Length = 0;
		funcNameBuffer.Length = 0;
		funcValBuffer.Length = 0;
		errBuffer.Length = 0;
		int functionReturnValue = YAPI._yapiGetFunctionInfo(fundesc, ref devdesc, serialBuffer, funcIdBuffer, funcNameBuffer, funcValBuffer, errBuffer);
		serial = serialBuffer.ToString();
		funcId = funcIdBuffer.ToString();
		funcName = funcNameBuffer.ToString();
		funcVal = funcValBuffer.ToString();
		errmsg = funcValBuffer.ToString();
		return functionReturnValue;
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00003AF8 File Offset: 0x00001CF8
	internal static int yapiGetDeviceByFunction(int fundesc, ref string errmsg)
	{
		StringBuilder errBuffer = new StringBuilder(256);
		int devdesc = 0;
		errBuffer.Length = 0;
		int res = YAPI._yapiGetFunctionInfo(fundesc, ref devdesc, null, null, null, null, errBuffer);
		errmsg = errBuffer.ToString();
		int functionReturnValue;
		if (res < 0)
		{
			functionReturnValue = res;
		}
		else
		{
			functionReturnValue = devdesc;
		}
		return functionReturnValue;
	}

	// Token: 0x06000033 RID: 51 RVA: 0x00003B40 File Offset: 0x00001D40
	public static ushort apiGetAPIVersion(ref string version, ref string date)
	{
		IntPtr pversion = 0;
		IntPtr pdate = 0;
		ushort res = YAPI._yapiGetAPIVersion(ref pversion, ref pdate);
		version = Marshal.PtrToStringAnsi(pversion);
		date = Marshal.PtrToStringAnsi(pdate);
		return res;
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00003B7C File Offset: 0x00001D7C
	internal static int yapiUpdateDeviceList(uint force, ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		buffer.Length = 0;
		int res = YAPI._yapiUpdateDeviceList(force, buffer);
		if (YAPI.YISERR(res))
		{
			errmsg = buffer.ToString();
		}
		return res;
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00003BB4 File Offset: 0x00001DB4
	protected static int yapiGetDevice(ref string device_str, string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		buffer.Length = 0;
		int functionReturnValue = YAPI._yapiGetDevice(new StringBuilder(device_str), buffer);
		errmsg = buffer.ToString();
		return functionReturnValue;
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00003BEC File Offset: 0x00001DEC
	protected static int yapiGetDeviceInfo(int d, ref YAPI.yDeviceSt infos, ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		buffer.Length = 0;
		int functionReturnValue = YAPI._yapiGetDeviceInfo(d, ref infos, buffer);
		errmsg = buffer.ToString();
		return functionReturnValue;
	}

	// Token: 0x06000037 RID: 55 RVA: 0x00003C20 File Offset: 0x00001E20
	internal static int yapiGetFunction(string class_str, string function_str, ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		buffer.Length = 0;
		int functionReturnValue = YAPI._yapiGetFunction(new StringBuilder(class_str), new StringBuilder(function_str), buffer);
		errmsg = buffer.ToString();
		return functionReturnValue;
	}

	// Token: 0x06000038 RID: 56 RVA: 0x00003C60 File Offset: 0x00001E60
	public static int apiGetFunctionsByClass(string class_str, int precFuncDesc, IntPtr dbuffer, int maxsize, ref int neededsize, ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		buffer.Length = 0;
		int functionReturnValue = YAPI._yapiGetFunctionsByClass(new StringBuilder(class_str), precFuncDesc, dbuffer, maxsize, ref neededsize, buffer);
		errmsg = buffer.ToString();
		return functionReturnValue;
	}

	// Token: 0x06000039 RID: 57 RVA: 0x00003CA0 File Offset: 0x00001EA0
	protected static int apiGetFunctionsByDevice(int devdesc, int precFuncDesc, IntPtr dbuffer, int maxsize, ref int neededsize, ref string errmsg)
	{
		StringBuilder buffer = new StringBuilder(256);
		buffer.Length = 0;
		int functionReturnValue = YAPI._yapiGetFunctionsByDevice(devdesc, precFuncDesc, dbuffer, maxsize, ref neededsize, buffer);
		errmsg = buffer.ToString();
		return functionReturnValue;
	}

	// Token: 0x0600003A RID: 58
	[DllImport("myDll.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
	private static extern void DllCallTest(ref YAPI.yDeviceSt data);

	// Token: 0x0600003B RID: 59
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiInitAPI")]
	private static extern int _yapiInitAPI(int mode, StringBuilder errmsg);

	// Token: 0x0600003C RID: 60
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiFreeAPI")]
	private static extern void _yapiFreeAPI();

	// Token: 0x0600003D RID: 61
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiRegisterLogFunction")]
	private static extern void _yapiRegisterLogFunction(IntPtr fct);

	// Token: 0x0600003E RID: 62
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiRegisterDeviceArrivalCallback")]
	private static extern void _yapiRegisterDeviceArrivalCallback(IntPtr fct);

	// Token: 0x0600003F RID: 63
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiRegisterDeviceRemovalCallback")]
	private static extern void _yapiRegisterDeviceRemovalCallback(IntPtr fct);

	// Token: 0x06000040 RID: 64
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiRegisterDeviceChangeCallback")]
	private static extern void _yapiRegisterDeviceChangeCallback(IntPtr fct);

	// Token: 0x06000041 RID: 65
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiRegisterFunctionUpdateCallback")]
	private static extern void _yapiRegisterFunctionUpdateCallback(IntPtr fct);

	// Token: 0x06000042 RID: 66
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiLockDeviceCallBack")]
	private static extern int _yapiLockDeviceCallBack(StringBuilder errmsg);

	// Token: 0x06000043 RID: 67
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiUnlockDeviceCallBack")]
	private static extern int _yapiUnlockDeviceCallBack(StringBuilder errmsg);

	// Token: 0x06000044 RID: 68
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiLockFunctionCallBack")]
	private static extern int _yapiLockFunctionCallBack(StringBuilder errmsg);

	// Token: 0x06000045 RID: 69
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiUnlockFunctionCallBack")]
	private static extern int _yapiUnlockFunctionCallBack(StringBuilder errmsg);

	// Token: 0x06000046 RID: 70
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiRegisterHub")]
	private static extern int _yapiRegisterHub(StringBuilder rootUrl, StringBuilder errmsg);

	// Token: 0x06000047 RID: 71
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiPreregisterHub")]
	private static extern int _yapiPreregisterHub(StringBuilder rootUrl, StringBuilder errmsg);

	// Token: 0x06000048 RID: 72
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiUnregisterHub")]
	private static extern void _yapiUnregisterHub(StringBuilder rootUrl);

	// Token: 0x06000049 RID: 73
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiUpdateDeviceList")]
	private static extern int _yapiUpdateDeviceList(uint force, StringBuilder errmsg);

	// Token: 0x0600004A RID: 74
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiHandleEvents")]
	private static extern int _yapiHandleEvents(StringBuilder errmsg);

	// Token: 0x0600004B RID: 75
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetTickCount")]
	private static extern ulong _yapiGetTickCount();

	// Token: 0x0600004C RID: 76
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiCheckLogicalName")]
	private static extern int _yapiCheckLogicalName(StringBuilder name);

	// Token: 0x0600004D RID: 77
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetAPIVersion")]
	private static extern ushort _yapiGetAPIVersion(ref IntPtr version, ref IntPtr date);

	// Token: 0x0600004E RID: 78
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetDevice")]
	private static extern int _yapiGetDevice(StringBuilder device_str, StringBuilder errmsg);

	// Token: 0x0600004F RID: 79
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetAllDevices")]
	private static extern int _yapiGetAllDevices(IntPtr buffer, int maxsize, ref int neededsize, StringBuilder errmsg);

	// Token: 0x06000050 RID: 80
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetDeviceInfo")]
	private static extern int _yapiGetDeviceInfo(int d, ref YAPI.yDeviceSt infos, StringBuilder errmsg);

	// Token: 0x06000051 RID: 81
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetFunction")]
	private static extern int _yapiGetFunction(StringBuilder class_str, StringBuilder function_str, StringBuilder errmsg);

	// Token: 0x06000052 RID: 82
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetFunctionsByClass")]
	private static extern int _yapiGetFunctionsByClass(StringBuilder class_str, int precFuncDesc, IntPtr buffer, int maxsize, ref int neededsize, StringBuilder errmsg);

	// Token: 0x06000053 RID: 83
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetFunctionsByDevice")]
	private static extern int _yapiGetFunctionsByDevice(int device, int precFuncDesc, IntPtr buffer, int maxsize, ref int neededsize, StringBuilder errmsg);

	// Token: 0x06000054 RID: 84
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetFunctionInfo")]
	internal static extern int _yapiGetFunctionInfo(int fundesc, ref int devdesc, StringBuilder serial, StringBuilder funcId, StringBuilder funcName, StringBuilder funcVal, StringBuilder errmsg);

	// Token: 0x06000055 RID: 85
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetErrorString")]
	private static extern int _yapiGetErrorString(int errorcode, StringBuilder buffer, int maxsize, StringBuilder errmsg);

	// Token: 0x06000056 RID: 86
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiHTTPRequestSyncStart")]
	private static extern int _yapiHTTPRequestSyncStart(ref YAPI.YIOHDL iohdl, StringBuilder device, StringBuilder url, ref IntPtr reply, ref int replysize, StringBuilder errmsg);

	// Token: 0x06000057 RID: 87
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiHTTPRequestSyncDone")]
	private static extern int _yapiHTTPRequestSyncDone(ref YAPI.YIOHDL iohdl, StringBuilder errmsg);

	// Token: 0x06000058 RID: 88
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiHTTPRequestAsync")]
	private static extern int _yapiHTTPRequestAsync(StringBuilder device, StringBuilder url, IntPtr callback, IntPtr context, StringBuilder errmsg);

	// Token: 0x06000059 RID: 89
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiHTTPRequest")]
	private static extern int _yapiHTTPRequest(StringBuilder device, StringBuilder url, StringBuilder buffer, int buffsize, ref int fullsize, StringBuilder errmsg);

	// Token: 0x0600005A RID: 90
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetBootloadersDevs")]
	private static extern int _yapiGetBootloadersDevs(StringBuilder serials, uint maxNbSerial, ref uint totalBootladers, StringBuilder errmsg);

	// Token: 0x0600005B RID: 91
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiFlashDevice")]
	private static extern int _yapiFlashDevice(ref YAPI.yFlashArg args, StringBuilder errmsg);

	// Token: 0x0600005C RID: 92
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiVerifyDevice")]
	private static extern int _yapiVerifyDevice(ref YAPI.yFlashArg args, StringBuilder errmsg);

	// Token: 0x0600005D RID: 93
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiGetDevicePath")]
	private static extern int _yapiGetDevicePath(int devdesc, StringBuilder rootdevice, StringBuilder path, int pathsize, ref int neededsize, StringBuilder errmsg);

	// Token: 0x0600005E RID: 94
	[DllImport("yapi.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, EntryPoint = "yapiSleep")]
	private static extern int _yapiSleep(int duration_ms, StringBuilder errmsg);

	// Token: 0x0600005F RID: 95 RVA: 0x00003CD8 File Offset: 0x00001ED8
	private static void csmodule_initialization()
	{
		YAPI.YDevice_devCache = new List<YAPI.YDevice>();
		YAPI._PlugEvents = new List<YAPI.yapiEvent>();
		YAPI._DataEvents = new List<YAPI.yapiEvent>();
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00003CF8 File Offset: 0x00001EF8
	private static void csmodule_cleanup()
	{
		YAPI.YDevice_devCache.Clear();
		YAPI.YDevice_devCache = null;
		YAPI._PlugEvents.Clear();
		YAPI._PlugEvents = null;
		YAPI._DataEvents.Clear();
		YAPI._DataEvents = null;
	}

	// Token: 0x0400001A RID: 26
	public const int DefaultCacheValidity = 5;

	// Token: 0x0400001B RID: 27
	public const string INVALID_STRING = "!INVALID!";

	// Token: 0x0400001C RID: 28
	public const double INVALID_DOUBLE = -1.79769313486231E+308;

	// Token: 0x0400001D RID: 29
	public const int INVALID_INT = -2147483648;

	// Token: 0x0400001E RID: 30
	public const long INVALID_LONG = -9223372036854775807L;

	// Token: 0x0400001F RID: 31
	public const string HARDWAREID_INVALID = "!INVALID!";

	// Token: 0x04000020 RID: 32
	public const string FUNCTIONID_INVALID = "!INVALID!";

	// Token: 0x04000021 RID: 33
	public const string FRIENDLYNAME_INVALID = "!INVALID!";

	// Token: 0x04000022 RID: 34
	public const int INVALID_UNSIGNED = -1;

	// Token: 0x04000023 RID: 35
	public const int Y_DETECT_NONE = 0;

	// Token: 0x04000024 RID: 36
	public const int Y_DETECT_USB = 1;

	// Token: 0x04000025 RID: 37
	public const int Y_DETECT_NET = 2;

	// Token: 0x04000026 RID: 38
	public const int Y_DETECT_ALL = 3;

	// Token: 0x04000027 RID: 39
	public const string YOCTO_API_VERSION_STR = "1.01";

	// Token: 0x04000028 RID: 40
	public const int YOCTO_API_VERSION_BCD = 257;

	// Token: 0x04000029 RID: 41
	public const string YOCTO_API_BUILD_NO = "9166";

	// Token: 0x0400002A RID: 42
	public const int YOCTO_DEFAULT_PORT = 4444;

	// Token: 0x0400002B RID: 43
	public const int YOCTO_VENDORID = 9440;

	// Token: 0x0400002C RID: 44
	public const int YOCTO_DEVID_FACTORYBOOT = 1;

	// Token: 0x0400002D RID: 45
	public const int YOCTO_DEVID_BOOTLOADER = 2;

	// Token: 0x0400002E RID: 46
	public const int YOCTO_ERRMSG_LEN = 256;

	// Token: 0x0400002F RID: 47
	public const int YOCTO_MANUFACTURER_LEN = 20;

	// Token: 0x04000030 RID: 48
	public const int YOCTO_SERIAL_LEN = 20;

	// Token: 0x04000031 RID: 49
	public const int YOCTO_BASE_SERIAL_LEN = 8;

	// Token: 0x04000032 RID: 50
	public const int YOCTO_PRODUCTNAME_LEN = 28;

	// Token: 0x04000033 RID: 51
	public const int YOCTO_FIRMWARE_LEN = 22;

	// Token: 0x04000034 RID: 52
	public const int YOCTO_LOGICAL_LEN = 20;

	// Token: 0x04000035 RID: 53
	public const int YOCTO_FUNCTION_LEN = 20;

	// Token: 0x04000036 RID: 54
	public const int YOCTO_PUBVAL_SIZE = 6;

	// Token: 0x04000037 RID: 55
	public const int YOCTO_PUBVAL_LEN = 16;

	// Token: 0x04000038 RID: 56
	public const int YOCTO_PASS_LEN = 20;

	// Token: 0x04000039 RID: 57
	public const int YOCTO_REALM_LEN = 20;

	// Token: 0x0400003A RID: 58
	public const int INVALID_YHANDLE = 0;

	// Token: 0x0400003B RID: 59
	public const int yUnknowSize = 1024;

	// Token: 0x0400003C RID: 60
	public const int SUCCESS = 0;

	// Token: 0x0400003D RID: 61
	public const int NOT_INITIALIZED = -1;

	// Token: 0x0400003E RID: 62
	public const int INVALID_ARGUMENT = -2;

	// Token: 0x0400003F RID: 63
	public const int NOT_SUPPORTED = -3;

	// Token: 0x04000040 RID: 64
	public const int DEVICE_NOT_FOUND = -4;

	// Token: 0x04000041 RID: 65
	public const int VERSION_MISMATCH = -5;

	// Token: 0x04000042 RID: 66
	public const int DEVICE_BUSY = -6;

	// Token: 0x04000043 RID: 67
	public const int TIMEOUT = -7;

	// Token: 0x04000044 RID: 68
	public const int IO_ERROR = -8;

	// Token: 0x04000045 RID: 69
	public const int NO_MORE_DATA = -9;

	// Token: 0x04000046 RID: 70
	public const int EXHAUSTED = -10;

	// Token: 0x04000047 RID: 71
	public const int DOUBLE_ACCES = -11;

	// Token: 0x04000048 RID: 72
	public const int UNAUTHORIZED = -12;

	// Token: 0x04000049 RID: 73
	public static bool ExceptionsDisabled = false;

	// Token: 0x0400004A RID: 74
	private static bool _apiInitialized = false;

	// Token: 0x0400004B RID: 75
	private static List<YAPI.YDevice> YDevice_devCache;

	// Token: 0x0400004C RID: 76
	private static YAPI.yLogFunc ylog = null;

	// Token: 0x0400004D RID: 77
	private static YAPI.yDeviceUpdateFunc yArrival = null;

	// Token: 0x0400004E RID: 78
	private static YAPI.yDeviceUpdateFunc yRemoval = null;

	// Token: 0x0400004F RID: 79
	private static YAPI.yDeviceUpdateFunc yChange = null;

	// Token: 0x04000050 RID: 80
	private static List<YAPI.yapiEvent> _PlugEvents;

	// Token: 0x04000051 RID: 81
	private static List<YAPI.yapiEvent> _DataEvents;

	// Token: 0x04000052 RID: 82
	private static double[] decExp = new double[]
	{
		1E-06,
		1E-05,
		0.0001,
		0.001,
		0.01,
		0.1,
		1.0,
		10.0,
		100.0,
		1000.0,
		10000.0,
		100000.0,
		1000000.0,
		10000000.0,
		100000000.0,
		1000000000.0
	};

	// Token: 0x04000053 RID: 83
	public static YAPI._yapiLogFunc native_yLogFunctionDelegate = new YAPI._yapiLogFunc(YAPI.native_yLogFunction);

	// Token: 0x04000054 RID: 84
	private static GCHandle native_yLogFunctionAnchor = GCHandle.Alloc(YAPI.native_yLogFunctionDelegate);

	// Token: 0x04000055 RID: 85
	public static YAPI._yapiFunctionUpdateFunc native_yFunctionUpdateDelegate = new YAPI._yapiFunctionUpdateFunc(YAPI.native_yFunctionUpdateCallback);

	// Token: 0x04000056 RID: 86
	private static GCHandle native_yFunctionUpdateAnchor = GCHandle.Alloc(YAPI.native_yFunctionUpdateDelegate);

	// Token: 0x04000057 RID: 87
	public static YAPI._yapiDeviceUpdateFunc native_yDeviceArrivalDelegate = new YAPI._yapiDeviceUpdateFunc(YAPI.native_yDeviceArrivalCallback);

	// Token: 0x04000058 RID: 88
	private static GCHandle native_yDeviceArrivalAnchor = GCHandle.Alloc(YAPI.native_yDeviceArrivalDelegate);

	// Token: 0x04000059 RID: 89
	public static YAPI._yapiDeviceUpdateFunc native_yDeviceRemovalDelegate = new YAPI._yapiDeviceUpdateFunc(YAPI.native_yDeviceRemovalCallback);

	// Token: 0x0400005A RID: 90
	private static GCHandle native_yDeviceRemovalAnchor = GCHandle.Alloc(YAPI.native_yDeviceRemovalDelegate);

	// Token: 0x0400005B RID: 91
	public static YAPI._yapiDeviceUpdateFunc native_yDeviceChangeDelegate = new YAPI._yapiDeviceUpdateFunc(YAPI.native_yDeviceChangeCallback);

	// Token: 0x0400005C RID: 92
	private static GCHandle native_yDeviceChangeAnchor = GCHandle.Alloc(YAPI.native_yDeviceChangeDelegate);

	// Token: 0x02000005 RID: 5
	public enum TJSONRECORDTYPE
	{
		// Token: 0x0400005E RID: 94
		JSON_STRING,
		// Token: 0x0400005F RID: 95
		JSON_INTEGER,
		// Token: 0x04000060 RID: 96
		JSON_BOOLEAN,
		// Token: 0x04000061 RID: 97
		JSON_STRUCT,
		// Token: 0x04000062 RID: 98
		JSON_ARRAY
	}

	// Token: 0x02000006 RID: 6
	public struct TJSONRECORD
	{
		// Token: 0x04000063 RID: 99
		public string name;

		// Token: 0x04000064 RID: 100
		public YAPI.TJSONRECORDTYPE recordtype;

		// Token: 0x04000065 RID: 101
		public string svalue;

		// Token: 0x04000066 RID: 102
		public long ivalue;

		// Token: 0x04000067 RID: 103
		public bool bvalue;

		// Token: 0x04000068 RID: 104
		public int membercount;

		// Token: 0x04000069 RID: 105
		public int memberAllocated;

		// Token: 0x0400006A RID: 106
		public YAPI.TJSONRECORD[] members;

		// Token: 0x0400006B RID: 107
		public int itemcount;

		// Token: 0x0400006C RID: 108
		public int itemAllocated;

		// Token: 0x0400006D RID: 109
		public YAPI.TJSONRECORD[] items;
	}

	// Token: 0x02000007 RID: 7
	public class TJsonParser
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00003EA0 File Offset: 0x000020A0
		public TJsonParser(string jsonData)
		{
			int p;
			if (jsonData.Substring(0, "OK\r\n".Length) == "OK\r\n")
			{
				this.httpcode = 200;
			}
			else
			{
				if (jsonData.Substring(0, "HTTP/1.1 ".Length) != "HTTP/1.1 ")
				{
					string errmsg = "data should start with HTTP/1.1 ";
					throw new Exception(errmsg);
				}
				p = jsonData.IndexOf(" ", "HTTP/1.1 ".Length - 1);
				int p2 = jsonData.IndexOf(" ", p + 1);
				this.httpcode = Convert.ToInt32(jsonData.Substring(p, p2 - p + 1));
				if (this.httpcode != 200)
				{
					return;
				}
			}
			p = jsonData.IndexOf("\r\n\r\n{");
			if (p < 0)
			{
				p = jsonData.IndexOf("\r\n\r\n[");
			}
			if (p < 0)
			{
				string errmsg = "data  does not contain JSON data";
				throw new Exception(errmsg);
			}
			jsonData = jsonData.Substring(p + 4, jsonData.Length - p - 4);
			this.data = this.Parse(jsonData).Value;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00003FB0 File Offset: 0x000021B0
		public void Dispose()
		{
			this.freestructure(ref this.data);
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00003FBE File Offset: 0x000021BE
		public YAPI.TJSONRECORD GetRootNode()
		{
			return this.data;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00003FC8 File Offset: 0x000021C8
		private YAPI.TJSONRECORD? Parse(string st)
		{
			int i = 0;
			st = "\"root\" : " + st + " ";
			return this.ParseEx(YAPI.TJsonParser.Tjstate.JWAITFORNAME, "", ref st, ref i);
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00003FFC File Offset: 0x000021FC
		private void ParseError(ref string st, int i, string errmsg)
		{
			int ststart = i - 10;
			int stend = i + 10;
			if (ststart < 0)
			{
				ststart = 0;
			}
			if (stend > st.Length)
			{
				stend = st.Length - 1;
			}
			errmsg = string.Concat(new string[]
			{
				errmsg,
				" near ",
				st.Substring(ststart, i - ststart),
				"*",
				st.Substring(i, stend - i - 1)
			});
			throw new Exception(errmsg);
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004078 File Offset: 0x00002278
		private YAPI.TJSONRECORD createStructRecord(string name)
		{
			YAPI.TJSONRECORD res = default(YAPI.TJSONRECORD);
			res.recordtype = YAPI.TJSONRECORDTYPE.JSON_STRUCT;
			res.name = name;
			res.svalue = "";
			res.ivalue = 0L;
			res.bvalue = false;
			res.membercount = 0;
			res.memberAllocated = 10;
			Array.Resize<YAPI.TJSONRECORD>(ref res.members, res.memberAllocated);
			res.itemcount = 0;
			res.itemAllocated = 0;
			res.items = null;
			return res;
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000040F8 File Offset: 0x000022F8
		private YAPI.TJSONRECORD createArrayRecord(string name)
		{
			YAPI.TJSONRECORD res = default(YAPI.TJSONRECORD);
			res.recordtype = YAPI.TJSONRECORDTYPE.JSON_ARRAY;
			res.name = name;
			res.svalue = "";
			res.ivalue = 0L;
			res.bvalue = false;
			res.itemcount = 0;
			res.itemAllocated = 10;
			Array.Resize<YAPI.TJSONRECORD>(ref res.items, res.itemAllocated);
			res.membercount = 0;
			res.memberAllocated = 0;
			res.members = null;
			return res;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00004178 File Offset: 0x00002378
		private YAPI.TJSONRECORD createStrRecord(string name, string value)
		{
			return new YAPI.TJSONRECORD
			{
				recordtype = YAPI.TJSONRECORDTYPE.JSON_STRING,
				name = name,
				svalue = value,
				ivalue = 0L,
				bvalue = false,
				itemcount = 0,
				itemAllocated = 0,
				items = null,
				membercount = 0,
				memberAllocated = 0,
				members = null
			};
		}

		// Token: 0x0600006B RID: 107 RVA: 0x000041E8 File Offset: 0x000023E8
		private YAPI.TJSONRECORD createIntRecord(string name, long value)
		{
			return new YAPI.TJSONRECORD
			{
				recordtype = YAPI.TJSONRECORDTYPE.JSON_INTEGER,
				name = name,
				svalue = "",
				ivalue = value,
				bvalue = false,
				itemcount = 0,
				itemAllocated = 0,
				items = null,
				membercount = 0,
				memberAllocated = 0,
				members = null
			};
		}

		// Token: 0x0600006C RID: 108 RVA: 0x0000425C File Offset: 0x0000245C
		private YAPI.TJSONRECORD createBoolRecord(string name, bool value)
		{
			return new YAPI.TJSONRECORD
			{
				recordtype = YAPI.TJSONRECORDTYPE.JSON_BOOLEAN,
				name = name,
				svalue = "",
				ivalue = 0L,
				bvalue = value,
				itemcount = 0,
				itemAllocated = 0,
				items = null,
				membercount = 0,
				memberAllocated = 0,
				members = null
			};
		}

		// Token: 0x0600006D RID: 109 RVA: 0x000042D0 File Offset: 0x000024D0
		private void add2StructRecord(ref YAPI.TJSONRECORD container, ref YAPI.TJSONRECORD element)
		{
			if (container.recordtype != YAPI.TJSONRECORDTYPE.JSON_STRUCT)
			{
				throw new Exception("container is not a struct type");
			}
			if (container.membercount >= container.memberAllocated)
			{
				Array.Resize<YAPI.TJSONRECORD>(ref container.members, container.memberAllocated + 10);
				container.memberAllocated += 10;
			}
			container.members[container.membercount] = element;
			container.membercount++;
		}

		// Token: 0x0600006E RID: 110 RVA: 0x0000434C File Offset: 0x0000254C
		private void add2ArrayRecord(ref YAPI.TJSONRECORD container, ref YAPI.TJSONRECORD element)
		{
			if (container.recordtype != YAPI.TJSONRECORDTYPE.JSON_ARRAY)
			{
				throw new Exception("container is not an array type");
			}
			if (container.itemcount >= container.itemAllocated)
			{
				Array.Resize<YAPI.TJSONRECORD>(ref container.items, container.itemAllocated + 10);
				container.itemAllocated += 10;
			}
			container.items[container.itemcount] = element;
			container.itemcount++;
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000043C8 File Offset: 0x000025C8
		private char Skipgarbage(ref string st, ref int i)
		{
			char sti = st[i];
			while (i < st.Length & (sti == '\n' | sti == '\r' | sti == ' '))
			{
				i++;
				if (i < st.Length)
				{
					sti = st[i];
				}
			}
			return sti;
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000441C File Offset: 0x0000261C
		private YAPI.TJSONRECORD? ParseEx(YAPI.TJsonParser.Tjstate initialstate, string defaultname, ref string st, ref int i)
		{
			YAPI.TJSONRECORD? functionReturnValue = null;
			YAPI.TJSONRECORD res = default(YAPI.TJSONRECORD);
			YAPI.TJSONRECORD value = default(YAPI.TJSONRECORD);
			string svalue = "";
			string name = defaultname;
			YAPI.TJsonParser.Tjstate state = initialstate;
			long isign = 1L;
			long ivalue = 0L;
			while (i < st.Length)
			{
				char sti = st[i];
				switch (state)
				{
				case YAPI.TJsonParser.Tjstate.JWAITFORNAME:
					if (sti == '"')
					{
						state = YAPI.TJsonParser.Tjstate.JWAITFORENDOFNAME;
					}
					else if (sti != ' ' & sti != '\n' & sti != ' ')
					{
						this.ParseError(ref st, i, "invalid char: was expecting \"");
					}
					break;
				case YAPI.TJsonParser.Tjstate.JWAITFORENDOFNAME:
					if (sti == '"')
					{
						state = YAPI.TJsonParser.Tjstate.JWAITFORCOLON;
					}
					else if (sti >= ' ')
					{
						name += sti;
					}
					else
					{
						this.ParseError(ref st, i, "invalid char: was expecting an identifier compliant char");
					}
					break;
				case YAPI.TJsonParser.Tjstate.JWAITFORCOLON:
					if (sti == ':')
					{
						state = YAPI.TJsonParser.Tjstate.JWAITFORDATA;
					}
					else if (sti != ' ' & sti != '\n' & sti != ' ')
					{
						this.ParseError(ref st, i, "invalid char: was expecting \"");
					}
					break;
				case YAPI.TJsonParser.Tjstate.JWAITFORDATA:
					if (sti == '{')
					{
						res = this.createStructRecord(name);
						state = YAPI.TJsonParser.Tjstate.JWAITFORNEXTSTRUCTMEMBER;
					}
					else if (sti == '[')
					{
						res = this.createArrayRecord(name);
						state = YAPI.TJsonParser.Tjstate.JWAITFORNEXTARRAYITEM;
					}
					else if (sti == '"')
					{
						svalue = "";
						state = YAPI.TJsonParser.Tjstate.JWAITFORSTRINGVALUE;
					}
					else if (sti >= '0' & sti <= '9')
					{
						state = YAPI.TJsonParser.Tjstate.JWAITFORINTVALUE;
						ivalue = (long)(sti - '0');
						isign = 1L;
					}
					else if (sti == '-')
					{
						state = YAPI.TJsonParser.Tjstate.JWAITFORINTVALUE;
						ivalue = 0L;
						isign = -1L;
					}
					else if (sti == 't' || sti == 'f' || sti == 'T' || sti == 'F')
					{
						svalue = sti.ToString().ToUpper();
						state = YAPI.TJsonParser.Tjstate.JWAITFORBOOLVALUE;
					}
					else if (sti != ' ' & sti != '\n' & sti != ' ')
					{
						this.ParseError(ref st, i, "invalid char: was expecting  \",0..9,t or f");
					}
					break;
				case YAPI.TJsonParser.Tjstate.JWAITFORNEXTSTRUCTMEMBER:
					sti = this.Skipgarbage(ref st, ref i);
					if (i < st.Length)
					{
						if (sti == '}')
						{
							functionReturnValue = new YAPI.TJSONRECORD?(res);
							i++;
							return functionReturnValue;
						}
						value = this.ParseEx(YAPI.TJsonParser.Tjstate.JWAITFORNAME, "", ref st, ref i).Value;
						this.add2StructRecord(ref res, ref value);
						sti = this.Skipgarbage(ref st, ref i);
						if (i < st.Length)
						{
							if (sti == '}' & i < st.Length)
							{
								i--;
							}
							else if (sti != ' ' & sti != '\n' & sti != ' ' & sti != ',')
							{
								this.ParseError(ref st, i, "invalid char: vas expecting , or }");
							}
						}
					}
					break;
				case YAPI.TJsonParser.Tjstate.JWAITFORNEXTARRAYITEM:
					sti = this.Skipgarbage(ref st, ref i);
					if (i < st.Length)
					{
						if (sti == ']')
						{
							functionReturnValue = new YAPI.TJSONRECORD?(res);
							i++;
							return functionReturnValue;
						}
						value = this.ParseEx(YAPI.TJsonParser.Tjstate.JWAITFORDATA, res.itemcount.ToString(), ref st, ref i).Value;
						this.add2ArrayRecord(ref res, ref value);
						sti = this.Skipgarbage(ref st, ref i);
						if (i < st.Length)
						{
							if (sti == ']' & i < st.Length)
							{
								i--;
							}
							else if (sti != ' ' & sti != '\n' & sti != ' ' & sti != ',')
							{
								this.ParseError(ref st, i, "invalid char: vas expecting , or ]");
							}
						}
					}
					break;
				case YAPI.TJsonParser.Tjstate.JSCOMPLETED:
					functionReturnValue = new YAPI.TJSONRECORD?(res);
					return functionReturnValue;
				case YAPI.TJsonParser.Tjstate.JWAITFORSTRINGVALUE:
					if (sti == '"')
					{
						state = YAPI.TJsonParser.Tjstate.JSCOMPLETED;
						res = this.createStrRecord(name, svalue);
					}
					else if (sti < ' ')
					{
						this.ParseError(ref st, i, "invalid char: was expecting string value");
					}
					else
					{
						svalue += sti;
					}
					break;
				case YAPI.TJsonParser.Tjstate.JWAITFORINTVALUE:
					if (sti >= '0' & sti <= '9')
					{
						ivalue = ivalue * 10L + (long)((ulong)sti) - 48L;
					}
					else
					{
						res = this.createIntRecord(name, isign * ivalue);
						state = YAPI.TJsonParser.Tjstate.JSCOMPLETED;
						i--;
					}
					break;
				case YAPI.TJsonParser.Tjstate.JWAITFORBOOLVALUE:
					if (sti < 'A' | sti > 'Z')
					{
						if (svalue != "TRUE" & svalue != "FALSE")
						{
							this.ParseError(ref st, i, "unexpected value, was expecting \"true\" or \"false\"");
						}
						if (svalue == "TRUE")
						{
							res = this.createBoolRecord(name, true);
						}
						else
						{
							res = this.createBoolRecord(name, false);
						}
						state = YAPI.TJsonParser.Tjstate.JSCOMPLETED;
						i--;
					}
					else
					{
						svalue += sti.ToString().ToUpper();
					}
					break;
				}
				i++;
			}
			this.ParseError(ref st, i, "unexpected end of data");
			return null;
		}

		// Token: 0x06000071 RID: 113 RVA: 0x00004940 File Offset: 0x00002B40
		private void DumpStructureRec(ref YAPI.TJSONRECORD p, ref int deep)
		{
			string indent = "";
			for (int i = 0; i <= deep * 2; i++)
			{
				indent += " ";
			}
			string line = indent + p.name + ":";
			switch (p.recordtype)
			{
			case YAPI.TJSONRECORDTYPE.JSON_STRING:
				line = line + " str=" + p.svalue;
				Console.WriteLine(line);
				return;
			case YAPI.TJSONRECORDTYPE.JSON_INTEGER:
				line = line + " int =" + p.ivalue.ToString();
				Console.WriteLine(line);
				return;
			case YAPI.TJSONRECORDTYPE.JSON_BOOLEAN:
				if (p.bvalue)
				{
					line += " bool = TRUE";
				}
				else
				{
					line += " bool = FALSE";
				}
				Console.WriteLine(line);
				return;
			case YAPI.TJSONRECORDTYPE.JSON_STRUCT:
				Console.WriteLine(line + " struct");
				for (int i = 0; i <= p.membercount - 1; i++)
				{
					this.DumpStructureRec(ref p.members[i], ref deep);
				}
				return;
			case YAPI.TJSONRECORDTYPE.JSON_ARRAY:
				Console.WriteLine(line + " array");
				for (int i = 0; i <= p.itemcount - 1; i++)
				{
					this.DumpStructureRec(ref p.items[i], ref deep);
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06000072 RID: 114 RVA: 0x00004A7C File Offset: 0x00002C7C
		private void freestructure(ref YAPI.TJSONRECORD p)
		{
			switch (p.recordtype)
			{
			case YAPI.TJSONRECORDTYPE.JSON_STRUCT:
				for (int i = p.membercount - 1; i >= 0; i += -1)
				{
					this.freestructure(ref p.members[i]);
				}
				p.members = new YAPI.TJSONRECORD[1];
				return;
			case YAPI.TJSONRECORDTYPE.JSON_ARRAY:
				for (int j = p.itemcount - 1; j >= 0; j += -1)
				{
					this.freestructure(ref p.items[j]);
				}
				p.items = new YAPI.TJSONRECORD[1];
				return;
			default:
				return;
			}
		}

		// Token: 0x06000073 RID: 115 RVA: 0x00004B04 File Offset: 0x00002D04
		public void DumpStructure()
		{
			int i = 0;
			this.DumpStructureRec(ref this.data, ref i);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x00004B24 File Offset: 0x00002D24
		public YAPI.TJSONRECORD? GetChildNode(YAPI.TJSONRECORD? parent, string nodename)
		{
			YAPI.TJSONRECORD? functionReturnValue = null;
			YAPI.TJSONRECORD? p = parent;
			if (p == null)
			{
				p = new YAPI.TJSONRECORD?(this.data);
			}
			if (p.Value.recordtype == YAPI.TJSONRECORDTYPE.JSON_STRUCT)
			{
				for (int i = 0; i <= p.Value.membercount - 1; i++)
				{
					if (p.Value.members[i].name == nodename)
					{
						functionReturnValue = new YAPI.TJSONRECORD?(p.Value.members[i]);
						return functionReturnValue;
					}
				}
			}
			else if (p.Value.recordtype == YAPI.TJSONRECORDTYPE.JSON_ARRAY)
			{
				int index = Convert.ToInt32(nodename);
				if (index >= p.Value.itemcount)
				{
					string str = "index out of bounds ";
					string str2 = ">=";
					int itemcount = p.Value.itemcount;
					throw new Exception(str + nodename + str2 + itemcount.ToString());
				}
				functionReturnValue = new YAPI.TJSONRECORD?(p.Value.items[index]);
				return functionReturnValue;
			}
			return null;
		}

		// Token: 0x0400006E RID: 110
		private const int JSONGRANULARITY = 10;

		// Token: 0x0400006F RID: 111
		public int httpcode;

		// Token: 0x04000070 RID: 112
		private YAPI.TJSONRECORD data;

		// Token: 0x02000008 RID: 8
		private enum Tjstate
		{
			// Token: 0x04000072 RID: 114
			JSTART,
			// Token: 0x04000073 RID: 115
			JWAITFORNAME,
			// Token: 0x04000074 RID: 116
			JWAITFORENDOFNAME,
			// Token: 0x04000075 RID: 117
			JWAITFORCOLON,
			// Token: 0x04000076 RID: 118
			JWAITFORDATA,
			// Token: 0x04000077 RID: 119
			JWAITFORNEXTSTRUCTMEMBER,
			// Token: 0x04000078 RID: 120
			JWAITFORNEXTARRAYITEM,
			// Token: 0x04000079 RID: 121
			JSCOMPLETED,
			// Token: 0x0400007A RID: 122
			JWAITFORSTRINGVALUE,
			// Token: 0x0400007B RID: 123
			JWAITFORINTVALUE,
			// Token: 0x0400007C RID: 124
			JWAITFORBOOLVALUE
		}
	}

	// Token: 0x02000009 RID: 9
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct yDeviceSt
	{
		// Token: 0x0400007D RID: 125
		public ushort vendorid;

		// Token: 0x0400007E RID: 126
		public ushort deviceid;

		// Token: 0x0400007F RID: 127
		public ushort devrelease;

		// Token: 0x04000080 RID: 128
		public ushort nbinbterfaces;

		// Token: 0x04000081 RID: 129
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string manufacturer;

		// Token: 0x04000082 RID: 130
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 28)]
		public string productname;

		// Token: 0x04000083 RID: 131
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string serial;

		// Token: 0x04000084 RID: 132
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string logicalname;

		// Token: 0x04000085 RID: 133
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 22)]
		public string firmware;

		// Token: 0x04000086 RID: 134
		public byte beacon;
	}

	// Token: 0x0200000A RID: 10
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct YIOHDL
	{
		// Token: 0x04000087 RID: 135
		[MarshalAs(UnmanagedType.U1)]
		public byte raw;
	}

	// Token: 0x0200000B RID: 11
	public enum yDEVICE_PROP
	{
		// Token: 0x04000089 RID: 137
		PROP_VENDORID,
		// Token: 0x0400008A RID: 138
		PROP_DEVICEID,
		// Token: 0x0400008B RID: 139
		PROP_DEVRELEASE,
		// Token: 0x0400008C RID: 140
		PROP_FIRMWARELEVEL,
		// Token: 0x0400008D RID: 141
		PROP_MANUFACTURER,
		// Token: 0x0400008E RID: 142
		PROP_PRODUCTNAME,
		// Token: 0x0400008F RID: 143
		PROP_SERIAL,
		// Token: 0x04000090 RID: 144
		PROP_LOGICALNAME,
		// Token: 0x04000091 RID: 145
		PROP_URL
	}

	// Token: 0x0200000C RID: 12
	public enum yFACE_STATUS
	{
		// Token: 0x04000093 RID: 147
		YFACE_EMPTY,
		// Token: 0x04000094 RID: 148
		YFACE_RUNNING,
		// Token: 0x04000095 RID: 149
		YFACE_ERROR
	}

	// Token: 0x0200000D RID: 13
	// (Invoke) Token: 0x06000076 RID: 118
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate int yFlashCallback(uint stepnumber, uint totalStep, IntPtr context);

	// Token: 0x0200000E RID: 14
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct yFlashArg
	{
		// Token: 0x04000096 RID: 150
		public StringBuilder OSDeviceName;

		// Token: 0x04000097 RID: 151
		public StringBuilder serial2assign;

		// Token: 0x04000098 RID: 152
		public IntPtr firmwarePtr;

		// Token: 0x04000099 RID: 153
		public uint firmwareLen;

		// Token: 0x0400009A RID: 154
		public YAPI.yFlashCallback progress;

		// Token: 0x0400009B RID: 155
		public IntPtr context;
	}

	// Token: 0x0200000F RID: 15
	public class YAPI_Exception : ApplicationException
	{
		// Token: 0x06000079 RID: 121 RVA: 0x00004C37 File Offset: 0x00002E37
		public YAPI_Exception(int errType, string errMsg)
		{
		}

		// Token: 0x0400009C RID: 156
		public int errorType;
	}

	// Token: 0x02000010 RID: 16
	// (Invoke) Token: 0x0600007B RID: 123
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void HTTPRequestCallback(YAPI.YDevice device, ref YAPI.blockingCallbackCtx context, int returnval, string result, string errmsg);

	// Token: 0x02000011 RID: 17
	// (Invoke) Token: 0x0600007F RID: 127
	public delegate void yLogFunc(string log);

	// Token: 0x02000012 RID: 18
	// (Invoke) Token: 0x06000083 RID: 131
	public delegate void yDeviceUpdateFunc(YModule modul);

	// Token: 0x02000013 RID: 19
	// (Invoke) Token: 0x06000087 RID: 135
	public delegate double yCalibrationHandler(double rawValue, int calibType, int[] parameters, double[] rawValues, double[] refValues);

	// Token: 0x02000014 RID: 20
	// (Invoke) Token: 0x0600008B RID: 139
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void _yapiLogFunc(IntPtr log, uint loglen);

	// Token: 0x02000015 RID: 21
	// (Invoke) Token: 0x0600008F RID: 143
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void _yapiDeviceUpdateFunc(int dev);

	// Token: 0x02000016 RID: 22
	// (Invoke) Token: 0x06000093 RID: 147
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void _yapiFunctionUpdateFunc(int dev, IntPtr value);

	// Token: 0x02000017 RID: 23
	public class blockingCallbackCtx
	{
		// Token: 0x0400009D RID: 157
		public int res;

		// Token: 0x0400009E RID: 158
		public string response;

		// Token: 0x0400009F RID: 159
		public string errmsg;
	}

	// Token: 0x02000018 RID: 24
	public class YDevice
	{
		// Token: 0x06000097 RID: 151 RVA: 0x00004C47 File Offset: 0x00002E47
		public YDevice(int devdesc)
		{
			this._devdescr = devdesc;
			this._cacheStamp = 0L;
			this._cacheJson = null;
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00004C70 File Offset: 0x00002E70
		public void dispose()
		{
			if (this._cacheJson != null)
			{
				this._cacheJson.Dispose();
			}
			this._cacheJson = null;
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00004C8C File Offset: 0x00002E8C
		public static YAPI.YDevice getDevice(int devdescr)
		{
			for (int idx = 0; idx <= YAPI.YDevice_devCache.Count - 1; idx++)
			{
				if (YAPI.YDevice_devCache[idx]._devdescr == devdescr)
				{
					return YAPI.YDevice_devCache[idx];
				}
			}
			YAPI.YDevice dev = new YAPI.YDevice(devdescr);
			YAPI.YDevice_devCache.Add(dev);
			return dev;
		}

		// Token: 0x0600009A RID: 154 RVA: 0x00004CE8 File Offset: 0x00002EE8
		public int HTTPRequestSync(string device, string request, ref string reply, ref string errmsg)
		{
			StringBuilder buffer = new StringBuilder(256);
			IntPtr preply = 0;
			int replysize = 0;
			YAPI.YIOHDL iohdl;
			iohdl.raw = 0;
			int res = YAPI._yapiHTTPRequestSyncStart(ref iohdl, new StringBuilder(device), new StringBuilder(request), ref preply, ref replysize, buffer);
			if (res < 0)
			{
				errmsg = buffer.ToString();
				return res;
			}
			reply = Marshal.PtrToStringAnsi(preply, replysize);
			res = YAPI._yapiHTTPRequestSyncDone(ref iohdl, buffer);
			errmsg = buffer.ToString();
			return res;
		}

		// Token: 0x0600009B RID: 155 RVA: 0x00004D5C File Offset: 0x00002F5C
		public int HTTPRequestAsync(string request, ref string errmsg)
		{
			string fullrequest = "";
			StringBuilder buffer = new StringBuilder(256);
			int res = this.HTTPRequestPrepare(request, ref fullrequest, ref errmsg);
			res = YAPI._yapiHTTPRequestAsync(new StringBuilder(this._rootdevice), new StringBuilder(fullrequest), 0, 0, buffer);
			errmsg = buffer.ToString();
			return res;
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00004DBC File Offset: 0x00002FBC
		public int HTTPRequestPrepare(string request, ref string fullrequest, ref string errmsg)
		{
			StringBuilder errbuf = new StringBuilder(256);
			int neededsize = 0;
			StringBuilder root = new StringBuilder(20);
			int tmp = 0;
			this._cacheStamp = YAPI.GetTickCount();
			if (!this._subpathinit)
			{
				int res = YAPI._yapiGetDevicePath(this._devdescr, root, null, 0, ref neededsize, errbuf);
				if (YAPI.YISERR(res))
				{
					errmsg = errbuf.ToString();
					return res;
				}
				StringBuilder b = new StringBuilder(neededsize);
				res = YAPI._yapiGetDevicePath(this._devdescr, root, b, neededsize, ref tmp, errbuf);
				if (YAPI.YISERR(res))
				{
					errmsg = errbuf.ToString();
					return res;
				}
				this._rootdevice = root.ToString();
				this._subpath = b.ToString();
				this._subpathinit = true;
			}
			int p = request.IndexOf("/");
			fullrequest = request.Substring(0, p) + this._subpath + request.Substring(p + 1, request.Length - p - 1);
			return 0;
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00004EA8 File Offset: 0x000030A8
		public int HTTPRequest(string request, ref string buffer, ref string errmsg)
		{
			string fullrequest = "";
			int res = this.HTTPRequestPrepare(request, ref fullrequest, ref errmsg);
			if (YAPI.YISERR(res))
			{
				return res;
			}
			return this.HTTPRequestSync(this._rootdevice, fullrequest, ref buffer, ref errmsg);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00004EE0 File Offset: 0x000030E0
		public int requestAPI(out YAPI.TJsonParser apires, ref string errmsg)
		{
			string buffer = "";
			apires = null;
			if (this._cacheStamp > YAPI.GetTickCount())
			{
				apires = this._cacheJson;
				return 0;
			}
			int res = this.HTTPRequest("GET /api.json \r\n\r\n", ref buffer, ref errmsg);
			if (YAPI.YISERR(res))
			{
				res = YAPI.yapiUpdateDeviceList(1U, ref errmsg);
				if (YAPI.YISERR(res))
				{
					return res;
				}
				res = this.HTTPRequest("GET /api.json \r\n\r\n", ref buffer, ref errmsg);
				if (YAPI.YISERR(res))
				{
					return res;
				}
			}
			try
			{
				apires = new YAPI.TJsonParser(buffer);
			}
			catch (Exception E)
			{
				errmsg = "unexpected JSON structure: " + E.Message;
				return -8;
			}
			this._cacheJson = apires;
			this._cacheStamp = YAPI.GetTickCount() + 5L;
			return 0;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00004F9C File Offset: 0x0000319C
		public int getFunctions(ref List<uint> functions, ref string errmsg)
		{
			int neededsize = 0;
			int i = 0;
			IntPtr p = 0;
			int[] ids = null;
			if (this._functions.Count == 0)
			{
				int res = YAPI.apiGetFunctionsByDevice(this._devdescr, 0, IntPtr.Zero, 64, ref neededsize, ref errmsg);
				if (YAPI.YISERR(res))
				{
					return res;
				}
				p = Marshal.AllocHGlobal(neededsize);
				res = YAPI.apiGetFunctionsByDevice(this._devdescr, 0, p, 64, ref neededsize, ref errmsg);
				if (YAPI.YISERR(res))
				{
					Marshal.FreeHGlobal(p);
					return res;
				}
				int count = Convert.ToInt32(neededsize / Marshal.SizeOf(i));
				Array.Resize<int>(ref ids, count + 1);
				Marshal.Copy(p, ids, 0, count);
				for (i = 0; i <= count - 1; i++)
				{
					this._functions.Add(Convert.ToUInt32(ids[i]));
				}
				Marshal.FreeHGlobal(p);
			}
			functions = this._functions;
			return 0;
		}

		// Token: 0x040000A0 RID: 160
		private int _devdescr;

		// Token: 0x040000A1 RID: 161
		private long _cacheStamp;

		// Token: 0x040000A2 RID: 162
		private YAPI.TJsonParser _cacheJson;

		// Token: 0x040000A3 RID: 163
		private List<uint> _functions = new List<uint>();

		// Token: 0x040000A4 RID: 164
		private string _rootdevice;

		// Token: 0x040000A5 RID: 165
		private string _subpath;

		// Token: 0x040000A6 RID: 166
		private bool _subpathinit;
	}

	// Token: 0x02000019 RID: 25
	public enum yapiEventType
	{
		// Token: 0x040000A8 RID: 168
		YAPI_DEV_ARRIVAL,
		// Token: 0x040000A9 RID: 169
		YAPI_DEV_REMOVAL,
		// Token: 0x040000AA RID: 170
		YAPI_DEV_CHANGE,
		// Token: 0x040000AB RID: 171
		YAPI_FUN_UPDATE,
		// Token: 0x040000AC RID: 172
		YAPI_FUN_VALUE,
		// Token: 0x040000AD RID: 173
		YAPI_NOP
	}

	// Token: 0x0200001A RID: 26
	private struct yapiEvent
	{
		// Token: 0x040000AE RID: 174
		public YAPI.yapiEventType eventtype;

		// Token: 0x040000AF RID: 175
		public YModule modul;

		// Token: 0x040000B0 RID: 176
		public int fun_descr;

		// Token: 0x040000B1 RID: 177
		public string value;
	}
}
