import os, sys, datetime, hashlib, csv, time, signal
import subprocess as sub, numpy as np, threading, filecmp

# definition
disk = ''
diskSN = ''
log_folder_name = ''
genMd5 = 0
total_sectors = 0
remark_data = "ironmen demo test"
block_size = 512
cmp = 0
ver = "2.0.1"




def runcmd(cmd):
    lines = []
    p = sub.Popen(cmd, stdout=sub.PIPE, stderr=sub.PIPE, shell=True)
    out, err = p.communicate()
    if len(err) > 0:
        log.toLog("[Debug] %s\n%s" % (cmd, err))

    for line in out.split("\r\n"):
        lines.append(line)
    return lines


def toolchk():
    tool = ['/usr/bin/smartctl', '/sbin/hdparm', '/usr/local/sbin/nvme']
    for chk_tools in tool:
        if not os.path.isfile(chk_tools):
            print "Please install the required tools first"
            sys.exit(1)


def getTotalLBA(disk):
    return ''.join(runcmd("fdisk -l |grep %s | awk '{print $5}'| tr -d '\r\n'" % (disk)))


def getTotalbytes(disk):
    return ''.join(runcmd("fdisk -l | grep '^Disk /dev/%s' | awk -F ',' '{print $2}' | tr -dc '0-9'" % (disk)))


def calc_string_size(num_remark):
    return block_size - num_remark - num_remark


def get_currtime():
    return ''.join(datetime.datetime.strftime(datetime.datetime.now(), '%Y/%m/%d %H:%M:%S '))


def get_random_string(length):
    return np.random.bytes(length)


def cmp_MD5File(fn1, fn2):
    if filecmp.cmp(fn1, fn2):
        print ("Success")
        log.toLog("Success")
    else:
        print ("Before: %s \nAfter: %s\n[cmp err] Files Mismatch error\nDeep scan....\n" % (
            sys.argv[1], sys.argv[2]))
        print ("Comparison is slow to respond, please be patient.\n")
        fw = open(log_folder_name + "stressTest.diff", "a+")
        with open(fn1) as f1, open(fn2) as f2:
            while True:
                try:
                    f1_line, f2_line = next(f1), next(f2)
                    if f1_line != f2_line:
                        fw.write("%s\t%s\n" % (str(f1_line).strip("\r\n"), str(f2_line).strip("\r\n")))
                        fw.flush()
                except StopIteration:
                    break
        fw.close()
        print ("Calculate the difference ....")
        fn_lineNum = runcmd("sed -n '$=' '%s/stressTest.diff' | tr -d '\r\n'" % (log_folder_name))
        log.toLog("Difference: %s" % (fn_lineNum))
        print ("Please check difference in file [%s/stressTest.diff]\n" % (log_folder_name))


def readMD5From(disk):
    fd = open("/dev/" + disk, "rb")
    sector = 0
    try:
        with open(log_folder_name + "stressTest.md5", "a+") as recMD5:
            writer = csv.writer(recMD5, delimiter=',')
            md5 = hashlib.md5()
            while sector < total_sectors:
                for data in iter(lambda: fd.read(block_size), b""):
                    if (sector % 104857600) == 0:
                        print "Read MD5 from %s ..." % (disk)
                    md5.update(data)
                    writer.writerow([sector, md5.hexdigest()])
                    sector = sector + block_size
    except:
        log.toLog("[Debug] Error to read MD5 from sector %s" % (sector))
    log.toLog("[Debug] read MD5 progress .. (%s/%s)" % (total_sectors, total_sectors))
    fd.close()


def getDiskSN(disk):
    if "sd" in disk:
        return ''.join(runcmd("smartctl -i /dev/%s | grep Serial | awk '{print $3}' | tr -d '\r\n'" % (disk)))
    elif "nvme" in disk:
        return ''.join(runcmd("nvme id-ctrl /dev/%s | grep sn | awk '{print $3}' | tr -d '\r\n'" % (disk)))
    else:
        return "cmp_mode"


def main():
    global diskSN
    disablectrlc()
    sector = 0
    global log_folder_name, total_sectors
    diskSN = getDiskSN(disk)
    log_folder_name = ".//logs//" + datetime.datetime.strftime(datetime.datetime.now(), '%Y%m%d_%H%M%S') + \
                      "//" + diskSN + "//"
    print (" %s ver. %s" % (sys.argv[0], ver))
    print ("GO\n")
    if cmp == 0:
        print ("Disk: [%s]" % (disk))
    if cmp == 1:
        print ("[Compare Mode]")
    log.toLog("%s ver. %s" %(sys.argv[0], ver))
    log.toLog("Begin")
    if "md5" not in [sys.argv[1][-3:], sys.argv[2][-3:]]:
        total_sectors = int(getTotalLBA(disk))
    if genMd5 == 0 and cmp == 0:
        print ("Seque. Write per %s byte" % (block_size))
        log.toLog("Total sectors: %s" % (total_sectors))
        f = open("/dev/%s" % (disk), "w+")
        try:
            while sector < total_sectors:
                f.seek(sector)
                lth_remark = len(remark_data + str(sector) + " sector")
                lth_rnd_data = block_size - lth_remark - lth_remark
                f.write(remark_data + str(sector) + " sector" + get_random_string(lth_rnd_data) + remark_data + str(
                    sector) + " sector")
                f.flush()
                sector = sector + block_size
                if (sector % 1048576000) == 0:
                    print ("Write Data .... ")
        except:
            log.toLog("[Debug] sector: %s Write Error!" % (sector))
        f.close()
        print ("[Finish] Seq. Write %s byte random data from sector 0 to sector %s" % (block_size, total_sectors))
        log.toLog("[Finish] Seq. Write %s byte random data from sector 0 to sector %s" % (block_size, total_sectors))


    # Data Compare
    if genMd5 == 1 and cmp == 0:
        log.toLog("Record Full disk MD5")
        print "[Start] Record Full disk MD5."
        thread_md5 = threading.Thread(target=readMD5From(disk))
        thread_md5.start()
        time.sleep(1)
        thread_md5.join()
        print( "Finish")
        log.toLog("Finish")

    if cmp == 1:
        cmp_MD5File(sys.argv[1], sys.argv[2])
    log.toLog("End")
    print ("Finish\n")
    sys.exit(0)


if __name__ == "__main__":
    if len(sys.argv) < 3:
        print ("RT Tools - %s (ver.%s)" % (sys.argv[0], ver))
        sys.exit(1)
    else:
        if [s for s in ['nvme', 'sd'] if str(sys.argv[1])[0:1] in s]:
            disk = sys.argv[1]
        if "md5" in sys.argv[1] and "md5" in sys.argv[2]:
            cmp = 1
        else:
            genMd5 = int(sys.argv[2])
        if len(sys.argv) > 3:
            block_size = int(sys.argv[3])
        main()
