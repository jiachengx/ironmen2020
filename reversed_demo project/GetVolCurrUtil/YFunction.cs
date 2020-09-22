using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

// Token: 0x0200001C RID: 28
public abstract class YFunction
{
	// Token: 0x060000A1 RID: 161 RVA: 0x0000507C File Offset: 0x0000327C
	public YFunction(string classname, string func)
	{
		this._className = classname;
		this._func = func;
		this._lastErrorType = 0;
		this._lastErrorMsg = "";
		this._cacheExpiration = 0L;
		this._fundescr = -1;
		this._userData = null;
		this._genCallback = null;
		YFunction._FunctionCache.Add(this);
	}

	// Token: 0x060000A2 RID: 162 RVA: 0x000050D7 File Offset: 0x000032D7
	protected void _throw(int errType, string errMsg)
	{
		this._lastErrorType = errType;
		this._lastErrorMsg = errMsg;
		if (!YAPI.ExceptionsDisabled)
		{
			throw new YAPI.YAPI_Exception(errType, "YoctoApi error : " + errMsg);
		}
	}

	// Token: 0x060000A3 RID: 163 RVA: 0x00005100 File Offset: 0x00003300
	protected int _getDescriptor(ref int fundescr, ref string errMsg)
	{
		int tmp_fundescr = YAPI.yapiGetFunction(this._className, this._func, ref errMsg);
		if (YAPI.YISERR(tmp_fundescr))
		{
			int res = YAPI.yapiUpdateDeviceList(1U, ref errMsg);
			if (YAPI.YISERR(res))
			{
				return res;
			}
			tmp_fundescr = YAPI.yapiGetFunction(this._className, this._func, ref errMsg);
			if (YAPI.YISERR(tmp_fundescr))
			{
				return tmp_fundescr;
			}
		}
		this._fundescr = (fundescr = tmp_fundescr);
		return 0;
	}

	// Token: 0x060000A4 RID: 164 RVA: 0x00005168 File Offset: 0x00003368
	protected int _getDevice(ref YAPI.YDevice dev, ref string errMsg)
	{
		int fundescr = 0;
		int res = this._getDescriptor(ref fundescr, ref errMsg);
		if (YAPI.YISERR(res))
		{
			return res;
		}
		int devdescr = YAPI.yapiGetDeviceByFunction(fundescr, ref errMsg);
		if (YAPI.YISERR(devdescr))
		{
			return devdescr;
		}
		dev = YAPI.YDevice.getDevice(devdescr);
		return 0;
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x000051B0 File Offset: 0x000033B0
	protected int _nextFunction(ref string hwid)
	{
		int fundescr = 0;
		int devdescr = 0;
		string serial = "";
		string funcId = "";
		string funcName = "";
		string funcVal = "";
		string errmsg = "";
		int neededsize = 0;
		IntPtr p = 0;
		int[] pdata = new int[1];
		int res = this._getDescriptor(ref fundescr, ref errmsg);
		if (YAPI.YISERR(res))
		{
			this._throw(res, errmsg);
			return res;
		}
		int maxsize = Marshal.SizeOf(pdata[0]);
		p = Marshal.AllocHGlobal(maxsize);
		res = YAPI.apiGetFunctionsByClass(this._className, fundescr, p, maxsize, ref neededsize, ref errmsg);
		Marshal.Copy(p, pdata, 0, 1);
		Marshal.FreeHGlobal(p);
		if (YAPI.YISERR(res))
		{
			this._throw(res, errmsg);
			return res;
		}
		if (Convert.ToInt32(neededsize / Marshal.SizeOf(pdata[0])) == 0)
		{
			hwid = "";
			return 0;
		}
		res = YAPI.yapiGetFunctionInfo(pdata[0], ref devdescr, ref serial, ref funcId, ref funcName, ref funcVal, ref errmsg);
		if (YAPI.YISERR(res))
		{
			this._throw(res, errmsg);
			return 0;
		}
		hwid = serial + "." + funcId;
		return 0;
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x000052E8 File Offset: 0x000034E8
	private int _buildSetRequest(string changeattr, string changeval, ref string request, ref string errmsg)
	{
		int fundesc = 0;
		StringBuilder funcid = new StringBuilder(20);
		StringBuilder errbuff = new StringBuilder(256);
		int devdesc = 0;
		funcid.Length = 0;
		errbuff.Length = 0;
		int res = this._getDescriptor(ref fundesc, ref errmsg);
		if (YAPI.YISERR(res))
		{
			return res;
		}
		res = YAPI._yapiGetFunctionInfo(fundesc, ref devdesc, null, funcid, null, null, errbuff);
		if (YAPI.YISERR(res))
		{
			errmsg = errbuff.ToString();
			this._throw(res, errmsg);
			return res;
		}
		request = "GET /api/" + funcid.ToString() + "/";
		string uchangeval = "";
		if (changeattr != "")
		{
			request = string.Concat(new string[]
			{
				request,
				changeattr,
				"?",
				changeattr,
				"="
			});
			for (int i = 0; i <= changeval.Length - 1; i++)
			{
				char c = changeval[i];
				if (!(c >= '0' & c <= '9') & !(c >= 'A' & c <= 'Z') & !(c >= 'a' & c <= 'z'))
				{
					int hh = (int)c;
					string h = hh.ToString("X");
					if (h.Length < 2)
					{
						h = "0" + h;
					}
					uchangeval = uchangeval + "%" + h;
				}
				else
				{
					uchangeval += c;
				}
			}
		}
		request = request + uchangeval + " \r\n\r\n";
		return 0;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x000054A0 File Offset: 0x000036A0
	protected int _setAttr(string attrname, string newvalue)
	{
		string errmsg = "";
		string request = "";
		YAPI.YDevice dev = null;
		int res = this._buildSetRequest(attrname, newvalue, ref request, ref errmsg);
		if (YAPI.YISERR(res))
		{
			this._throw(res, errmsg);
			return res;
		}
		res = this._getDevice(ref dev, ref errmsg);
		if (YAPI.YISERR(res))
		{
			this._throw(res, errmsg);
			return res;
		}
		res = dev.HTTPRequestAsync(request, ref errmsg);
		if (YAPI.YISERR(res))
		{
			res = YAPI.yapiUpdateDeviceList(1U, ref errmsg);
			if (YAPI.YISERR(res))
			{
				this._throw(res, errmsg);
				return res;
			}
			res = dev.HTTPRequestAsync(request, ref errmsg);
			if (YAPI.YISERR(res))
			{
				this._throw(res, errmsg);
				return res;
			}
		}
		this._cacheExpiration = 0L;
		return 0;
	}

	// Token: 0x060000A8 RID: 168
	protected abstract int _parse(YAPI.TJSONRECORD parser);

	// Token: 0x060000A9 RID: 169 RVA: 0x0000554C File Offset: 0x0000374C
	public string describe()
	{
		int devdescr = 0;
		string errmsg = "";
		string serial = "";
		string funcId = "";
		string funcName = "";
		string funcValue = "";
		int fundescr = YAPI.yapiGetFunction(this._className, this._func, ref errmsg);
		if (!YAPI.YISERR(fundescr) && !YAPI.YISERR(YAPI.yapiGetFunctionInfo(fundescr, ref devdescr, ref serial, ref funcId, ref funcName, ref funcValue, ref errmsg)))
		{
			return string.Concat(new string[]
			{
				this._className,
				"(",
				this._func,
				")=",
				serial,
				".",
				funcId
			});
		}
		return this._className + "(" + this._func + ")=unresolved";
	}

	// Token: 0x060000AA RID: 170 RVA: 0x00005618 File Offset: 0x00003818
	public string get_hardwareId()
	{
		int fundesc = 0;
		int devdesc = 0;
		string funcName = "";
		string funcVal = "";
		string errmsg = "";
		string snum = "";
		string funcid = "";
		int retcode = this._getDescriptor(ref fundesc, ref errmsg);
		if (YAPI.YISERR(retcode))
		{
			this._throw(retcode, errmsg);
			return "!INVALID!";
		}
		retcode = YAPI.yapiGetFunctionInfo(fundesc, ref devdesc, ref snum, ref funcid, ref funcName, ref funcVal, ref errmsg);
		if (YAPI.YISERR(retcode))
		{
			this._throw(retcode, errmsg);
			return "!INVALID!";
		}
		return snum + '.' + funcid;
	}

	// Token: 0x060000AB RID: 171 RVA: 0x000056A8 File Offset: 0x000038A8
	public string get_functionId()
	{
		int fundesc = 0;
		int devdesc = 0;
		string funcName = "";
		string funcVal = "";
		string errmsg = "";
		string snum = "";
		string funcid = "";
		int retcode = this._getDescriptor(ref fundesc, ref errmsg);
		if (YAPI.YISERR(retcode))
		{
			this._throw(retcode, errmsg);
			return "!INVALID!";
		}
		retcode = YAPI.yapiGetFunctionInfo(fundesc, ref devdesc, ref snum, ref funcid, ref funcName, ref funcVal, ref errmsg);
		if (YAPI.YISERR(retcode))
		{
			this._throw(retcode, errmsg);
			return "!INVALID!";
		}
		return funcid;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x0000572C File Offset: 0x0000392C
	public virtual string get_friendlyName()
	{
		int fundesc = 0;
		int devdesc = 0;
		string funcName = "";
		string dummy = "";
		string errmsg = "";
		string snum = "";
		string funcid = "";
		string mod_name = "";
		int retcode = this._getDescriptor(ref fundesc, ref errmsg);
		if (YAPI.YISERR(retcode))
		{
			this._throw(retcode, errmsg);
			return "!INVALID!";
		}
		retcode = YAPI.yapiGetFunctionInfo(fundesc, ref devdesc, ref snum, ref funcid, ref funcName, ref dummy, ref errmsg);
		if (YAPI.YISERR(retcode))
		{
			this._throw(retcode, errmsg);
			return "!INVALID!";
		}
		int moddescr = YAPI.yapiGetFunction("Module", snum, ref errmsg);
		if (YAPI.YISERR(moddescr))
		{
			this._throw(retcode, errmsg);
			return "!INVALID!";
		}
		retcode = YAPI.yapiGetFunctionInfo(moddescr, ref devdesc, ref snum, ref dummy, ref mod_name, ref dummy, ref errmsg);
		if (YAPI.YISERR(retcode))
		{
			this._throw(retcode, errmsg);
			return "!INVALID!";
		}
		string friendly;
		if (mod_name != "")
		{
			friendly = mod_name + '.';
		}
		else
		{
			friendly = snum + '.';
		}
		if (funcName != "")
		{
			friendly += funcName;
		}
		else
		{
			friendly += funcid;
		}
		return friendly;
	}

	// Token: 0x060000AD RID: 173 RVA: 0x00005864 File Offset: 0x00003A64
	public override string ToString()
	{
		return this.describe();
	}

	// Token: 0x060000AE RID: 174 RVA: 0x0000586C File Offset: 0x00003A6C
	public int get_errorType()
	{
		return this._lastErrorType;
	}

	// Token: 0x060000AF RID: 175 RVA: 0x00005874 File Offset: 0x00003A74
	public int errorType()
	{
		return this._lastErrorType;
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0000587C File Offset: 0x00003A7C
	public int errType()
	{
		return this._lastErrorType;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x00005884 File Offset: 0x00003A84
	public string get_errorMessage()
	{
		return this._lastErrorMsg;
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x0000588C File Offset: 0x00003A8C
	public string errorMessage()
	{
		return this._lastErrorMsg;
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x00005894 File Offset: 0x00003A94
	public string errMessage()
	{
		return this._lastErrorMsg;
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x0000589C File Offset: 0x00003A9C
	public bool isOnline()
	{
		YAPI.YDevice dev = null;
		string errmsg = "";
		YAPI.TJsonParser apires;
		return this._cacheExpiration > YAPI.GetTickCount() || (!YAPI.YISERR(this._getDevice(ref dev, ref errmsg)) && !YAPI.YISERR(dev.requestAPI(out apires, ref errmsg)));
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x000058E8 File Offset: 0x00003AE8
	public int load(int msValidity)
	{
		YAPI.YDevice dev = null;
		string errmsg = "";
		YAPI.TJsonParser apires = null;
		string errbuf = "";
		string funcId = "";
		int devdesc = 0;
		string serial = "";
		string funcName = "";
		string funcVal = "";
		YAPI.TJSONRECORD? node = null;
		int res = this._getDevice(ref dev, ref errmsg);
		if (YAPI.YISERR(res))
		{
			this._throw(res, errmsg);
			return res;
		}
		res = dev.requestAPI(out apires, ref errmsg);
		if (YAPI.YISERR(res))
		{
			this._throw(res, errmsg);
			return res;
		}
		int fundescr = YAPI.yapiGetFunction(this._className, this._func, ref errmsg);
		if (YAPI.YISERR(fundescr))
		{
			this._throw(res, errmsg);
			return fundescr;
		}
		devdesc = 0;
		res = YAPI.yapiGetFunctionInfo(fundescr, ref devdesc, ref serial, ref funcId, ref funcName, ref funcVal, ref errbuf);
		if (YAPI.YISERR(res))
		{
			this._throw(res, errmsg);
			return res;
		}
		node = apires.GetChildNode(null, funcId);
		if (node == null)
		{
			this._throw(-8, "unexpected JSON structure: missing function " + funcId);
			return -8;
		}
		this._parse(node.GetValueOrDefault());
		this._cacheExpiration = YAPI.GetTickCount() + (long)msValidity;
		return 0;
	}

	// Token: 0x060000B6 RID: 182 RVA: 0x00005A2C File Offset: 0x00003C2C
	public YModule get_module()
	{
		int devdescr = 0;
		string errmsg = "";
		string serial = "";
		string funcId = "";
		string funcName = "";
		string funcValue = "";
		int fundescr = YAPI.yapiGetFunction(this._className, this._func, ref errmsg);
		if (!YAPI.YISERR(fundescr) && !YAPI.YISERR(YAPI.yapiGetFunctionInfo(fundescr, ref devdescr, ref serial, ref funcId, ref funcName, ref funcValue, ref errmsg)))
		{
			return YModule.FindModule(serial + ".module");
		}
		return YModule.FindModule("module_of_" + this._className + "_" + this._func);
	}

	// Token: 0x060000B7 RID: 183 RVA: 0x00005AC4 File Offset: 0x00003CC4
	public YModule module()
	{
		return this.get_module();
	}

	// Token: 0x060000B8 RID: 184 RVA: 0x00005ACC File Offset: 0x00003CCC
	public int get_functionDescriptor()
	{
		return this._fundescr;
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x00005AD4 File Offset: 0x00003CD4
	public int functionDescriptor()
	{
		return this.get_functionDescriptor();
	}

	// Token: 0x060000BA RID: 186 RVA: 0x00005ADC File Offset: 0x00003CDC
	public object get_userData()
	{
		return this._userData;
	}

	// Token: 0x060000BB RID: 187 RVA: 0x00005AE4 File Offset: 0x00003CE4
	public object userData()
	{
		return this.get_userData();
	}

	// Token: 0x060000BC RID: 188 RVA: 0x00005AEC File Offset: 0x00003CEC
	public void set_userData(object data)
	{
		this._userData = data;
	}

	// Token: 0x060000BD RID: 189 RVA: 0x00005AF5 File Offset: 0x00003CF5
	public void setUserData(object data)
	{
		this.set_userData(data);
	}

	// Token: 0x060000BE RID: 190 RVA: 0x00005AFE File Offset: 0x00003CFE
	protected void _registerFuncCallback(YFunction func)
	{
		if (!YFunction._FunctionCallbacks.Contains(this))
		{
			YFunction._FunctionCallbacks.Add(this);
		}
	}

	// Token: 0x060000BF RID: 191 RVA: 0x00005B18 File Offset: 0x00003D18
	protected void _unregisterFuncCallback(YFunction func)
	{
		YFunction._FunctionCallbacks.Remove(this);
	}

	// Token: 0x060000C0 RID: 192 RVA: 0x00005B26 File Offset: 0x00003D26
	public void registerValueCallback(YFunction.GenericUpdateCallback callback)
	{
		if (callback != null)
		{
			this._registerFuncCallback(this);
		}
		else
		{
			this._unregisterFuncCallback(this);
		}
		this._genCallback = new YFunction.GenericUpdateCallback(callback.Invoke);
	}

	// Token: 0x060000C1 RID: 193 RVA: 0x00005B4D File Offset: 0x00003D4D
	public virtual void advertiseValue(string value)
	{
		if (this._genCallback != null)
		{
			this._genCallback(this, value);
		}
	}

	// Token: 0x040000B2 RID: 178
	public const int FUNCTIONDESCRIPTOR_INVALID = -1;

	// Token: 0x040000B3 RID: 179
	public static List<YFunction> _FunctionCache = new List<YFunction>();

	// Token: 0x040000B4 RID: 180
	public static List<YFunction> _FunctionCallbacks = new List<YFunction>();

	// Token: 0x040000B5 RID: 181
	public static Dictionary<string, YAPI.yCalibrationHandler> _CalibHandlers = new Dictionary<string, YAPI.yCalibrationHandler>();

	// Token: 0x040000B6 RID: 182
	protected string _className;

	// Token: 0x040000B7 RID: 183
	protected string _func;

	// Token: 0x040000B8 RID: 184
	protected int _lastErrorType;

	// Token: 0x040000B9 RID: 185
	protected string _lastErrorMsg;

	// Token: 0x040000BA RID: 186
	protected int _fundescr;

	// Token: 0x040000BB RID: 187
	protected object _userData;

	// Token: 0x040000BC RID: 188
	protected YFunction.GenericUpdateCallback _genCallback;

	// Token: 0x040000BD RID: 189
	protected long _cacheExpiration;

	// Token: 0x0200001D RID: 29
	// (Invoke) Token: 0x060000C4 RID: 196
	public delegate void GenericUpdateCallback(YFunction func, string value);
}
