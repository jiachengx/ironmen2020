# coding: utf-8
from ctypes import *
fn_opt = cdll.LoadLibrary("multiplier.dll")
fn_opt.helloworld()
fn_opt.Sum(3,5)
