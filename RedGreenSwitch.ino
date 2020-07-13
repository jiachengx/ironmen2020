#include <DigiUSB.h>
#pragma chip attiny85
#pragma efuse 0xFF        // default value
#pragma hfuse 0xDF        // default value
#pragma lfuse 0x62        // default value

#include "Arduino.h"

#define PA PB0
#define PB PB1

void setup() {
  DigiUSB.begin();

  // initialize digital pins as inputs.
  pinMode(PA, OUTPUT);
  pinMode(PB, OUTPUT);

  // Initial Pins before selection (Default:Port A)- Casper 20200215
  digitalWrite(PA, HIGH);
  digitalWrite(PB, LOW);

  DigiUSB.println("HelloWorld");
}

void loop() {
  int sRead;
  
  DigiUSB.print("\n1-Red_GR_Switch\n");
  while (true) { // loop forever
    if (DigiUSB.available()) {
      sRead = DigiUSB.read();
      switch (sRead) {
        case '1':
          controlSwitch(0);
          controlSwitch(2);
          _delay_ms(5000);
          for ( byte a = 0; a < 10; a++ ) {
            PAGo();
            PBGo();
          }
          break;
      }

      if (sRead == '\n') {
        break; 
      }
    }

    DigiUSB.delay(12);
  }
  cSwitch(0);
  cSwitch(2);
}


void PAGo(){
  DigiUSB.print("RED_Blink\n");
  cSwitch(1);
  DigiUSB.delay(15000);
  cSwitch(0);
  DigiUSB.delay(5000);
  }
void PBGo(){
  DigiUSB.print("GR_Blink\n");
            cSwitch(3);
            DigiUSB.delay(15000);
            cSwitch(2);
            DigiUSB.delay(5000);
  }
void cSwitch(int opt) {
  switch (opt) {
    case '0':
      digitalWrite(PA, LOW);
      break;
    case '1':
      digitalWrite(PA, HIGH);
      break;
    case '2':
      digitalWrite(PB, LOW);
      break;
    case '3':
      digitalWrite(PB, HIGH);
      break;
    default:
      break;
  }
}
