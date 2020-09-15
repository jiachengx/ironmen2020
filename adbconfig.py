import subprocess


class adbCMD:
    def __init__(self):
        self.shell = "adb shell "
        self.killServer = "kill-server"
        self.devices = "devices"
        self.enableWifi = "svc wifi enable"
        self.disableWifi = "svc wifi disable"
        self.OpenSetting = "am start com.android.settings/com.android.settings.Settings"
        self.OpenSettingApp = "am start com.android.settings/com.android.settings.Settings.ApplicationSettings"
        self.getWifiInfo = "dumpsys wifi |grep 'mWifiInfo SSID'"
        self.scr_turnOffTheAutomaticRotation = "content insert --uri content://settings/system --bind " \
                                               "name:s:accelerometer_rotation --bind value:i:0 "
        self.scr_rotateToLandscape = "content insert --uri content://settings/system --bind " \
                                     "name:s:user_rotation --bind value:i:1 "
        self.scr_rotateToPortrait = "content insert --uri content://settings/system --bind name:s:user_rotation " \
                                    "--bind value:i:0 "
        self.scr_timeOut = "settings put system screen_off_timeout 600000"
        self.scr_brightnessAsZero = "settings put system screen_brightness 0"
        self.backToHome = "input keyevent 3"
        self.launchIperf3 = "am start -n com.nextdoordeveloper.miperf.miperf/com.nextdoordeveloper.miperf.miperf" \
                            ".MainActivity "
        self.iperfPackage = "com.nextdoordeveloper.miperf.miperf"
        self.iperf3APK = "mperf.apk"
        self.accessStorage = "android.permission-group.STORAGE"


class util:
    def __init__(self):
        pass

    def runCmd(self, cmd):
        try:
            p = subprocess.Popen(cmd, shell=True, stdout=subprocess.PIPE, stderr=subprocess.STDOUT)
            return p.stdout.read().decode()
        except Exception as e:
            return "{0}\n".format(e.args)
            pass

    def conv_numberlstToOneNum(self, numlist):
        return int(''.join([str(i) for i in numlist]))
