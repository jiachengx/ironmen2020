#! /usr/bin/env python
#  -*- coding: utf-8 -*-
#
# Support module generated by PAGE version 4.26
#  in conjunction with Tcl version 8.6
#    Sep 21, 2020 11:27:58 AM CST  platform: Windows NT

import sys

try:
    import Tkinter as tk
except ImportError:
    import tkinter as tk

try:
    import ttk
    py3 = False
except ImportError:
    import tkinter.ttk as ttk
    py3 = True

def set_Tk_var():
    global che59
    che59 = tk.IntVar()
    global che50
    che50 = tk.IntVar()
    global che47
    che47 = tk.IntVar()
    global che49
    che49 = tk.IntVar()
    global che51
    che51 = tk.IntVar()
    global che53
    che53 = tk.IntVar()
    global che56
    che56 = tk.IntVar()
    global che63
    che63 = tk.IntVar()
    global che68
    che68 = tk.IntVar()
    global selectedButton
    selectedButton = tk.IntVar()

def init(top, gui, *args, **kwargs):
    global w, top_level, root
    w = gui
    top_level = top
    root = top

def destroy_window():
    # Function which closes the window.
    global top_level
    top_level.destroy()
    top_level = None

if __name__ == '__main__':
    import gp3
    gp3.vp_start_gui()




