
#include <SPI.h>
#include <Ethernet.h>


byte mac[] = { 
  0xDE, 0xAD, 0xBE, 0xEF, 0xFE, 0xEC };
IPAddress ip(192,168,50,177);

EthernetServer server(80);

void setup() {
  Serial.begin(9600);
   while (!Serial) {
    ; 
  }

  Ethernet.begin(mac, ip);
  server.begin();
  Serial.print("server IP: ");
  Serial.println(Ethernet.localIP());
}


void loop() {
  EthernetClient client = server.available();
  if (client) {
    Serial.println("new client");
    boolean currentLineIsBlank = true;
    while (client.connected()) {
      if (client.available()) {
        char c = client.read();
        Serial.write(c);
        if (c == '\n' && currentLineIsBlank) {
          client.println("HTTP/1.1 200 OK");
          client.println("Content-Type: text/html");
          client.println("Connection: close");  
      client.println("Refresh: 5");  // refresh the page automatically every 5 sec
          client.println();
          client.println("<!DOCTYPE HTML>");
          client.println("<html>");
          for (int analogChannel = 0; analogChannel < 6; analogChannel++) {
            int sensorReading = analogRead(analogChannel);
            client.print("analog input ");
            client.print(analogChannel);
            client.print(" is ");
            client.print(sensorReading);
            client.println("<br />");       
          }
          client.println("</html>");
          break;
        }
        if (c == '\n') {
          currentLineIsBlank = true;
        } 
        else if (c != '\r') {
          currentLineIsBlank = false;
        }
      }
    }
    delay(1);
    // close the connection:
    client.stop();
    Serial.println("client disconnected");
  }
}
