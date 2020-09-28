#!/bin/bash
# Copy the content of root folder into the tsblog folder in USB key and then safe remove it.
mkdir -p /media/tsblog/tsblog
cp -rv /root/*.log /media/tsblog/tsblog
printf "Copy total log into USB key.\n"
rm -f /media/tsblog/tsblog/fio_pcie.sh
umount /media/tsblog
printf "Safe remove the USB key.\n"
