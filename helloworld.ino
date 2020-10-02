#include "DigiKeyboard.h"
void setup()
{
pinMode(1,OUTPUT);
DigiKeyboard.delay(2000);
DigiKeyboard.sendKeyStroke(KEY_R, MOD_GUI_LEFT);
DigiKeyboard.delay(500);
DigiKeyboard.print("cmd");
DigiKeyboard.sendKeyStroke(KEY_ENTER);
DigiKeyboard.delay(500);
DigiKeyboard.print("Hello World");
digitalWrite(1,HIGH);
delay(500);
digitalWrite(1,LOW);
}
void loop()
{  }
