# -*- coding: utf-8 -*-
import winpexpect

def main():
    cmdpath=r"C:\Windows\System32\cmd.exe"
    child = winpexpect.winspawn(cmdpath)
    child.expect(["C:\Users\cchhsu>"])
    child.sendline("Hello World")

if __name__ == '__main__':
    main()
