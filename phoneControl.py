import threading
import adbconfig, time, datetime, sys
import uiautomator2 as u2

adb = adbconfig.adbCMD()
tool = adbconfig.util()
def phoneControl(phoneSN):
    d = u2.connect(phoneSN)
    d.set_fastinput_ime(True)
    tool.runCmd(adb.shell +  adb.backToHome)
    tool.runCmd(adb.shell + adb.OpenSetting)
    time.sleep(1)
    if d(text='連接').exists:
        d(resourceId="android:id/title", text="連接").click()
        time.sleep(1)
        if d(text='Wi-Fi').exists:
            d(resourceId="android:id/title", text="Wi-Fi").click()
            time.sleep(1)
        log("Delete the network saved profile.")
        if d(resourceId="com.android.settings:id/layout_details").exists:
            d(resourceId="com.android.settings:id/layout_details").click()
            time.sleep(1)
            d(resourceId="com.android.settings:id/forget_button").click()
            time.sleep(1)
        log("Find out the Add network icon.")
        while not d(text='新增網路').exists:
            d.swipe(0, 1080, 0, 500)
        if not d(text="新增網路").exists:
            while not d(text='新增網路').exists:
                d.swipe(0, 1080, 0, 900)
        if not d(text="新增網路").exists:
            while not d(text='新增網路').exists:
                d.swipe(0, 1080, 0, 900)
        if not d(text="新增網路").exists:
            while not d(text='新增網路').exists:
                d.swipe(0, 1080, 0, 900)
        d(resourceId="android:id/title", text="新增網路").click()
        if d(text='拒絕').exists:
            d(resourceId="android:id/button2").click()
            time.sleep(1)
        d.send_keys(ssid)
        log("Select the Wi-Fi security type")
        d(resourceId="com.android.settings:id/spinner").click()
        d(resourceId="android:id/text1", text="WPA/WPA2-Personal").click()
        d(resourceId="com.android.settings:id/edittext", text="輸入密碼").click()
        d.send_keys(WLanPasswd)
        d(resourceId="com.android.settings:id/button").click()
        log("Profile is created successfully.")
    else:
        print("configuration error\n")


s1 = threading.Thread(target=phoneControl, args=(phoneSN1,))
s2 = threading.Thread(target=phoneControl, args=(phoneSN2,))
s1.start()
s2.start()
