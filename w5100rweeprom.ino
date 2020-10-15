#include <EEPROM.h>
#include <SPI.h>
#include <Ethernet.h>
#define EEPROM_ARRAY_NUM 26
#define MAC_ADDRESS_ARRAY_NUM 6
#define IP_ADDRESS_ARRAY_NUM 4
#define NET_MASK_ARRAY_NUM 4
#define GATEWAY_ARRAY_NUM 4
#define DNS_ARRAY_NUM 4
#define NET_EEPROM_MAGIC 0x22
#define EEPROMOFFSET 0
#define MAC_OFFSET (EEPROMOFFSET + 1)
#define DHCP_OFFSET (MAC_OFFSET + 6)
#define IP_OFFSET (DHCP_OFFSET + 1)
#define DNS_OFFSET (IP_OFFSET + 4)
#define GW_OFFSET (DNS_OFFSET + 4)
#define SUBNET_OFFSET (GW_OFFSET + 4)


int EEPROMAddress;
byte EEPROMValue;
byte mac[MAC_ADDRESS_ARRAY_NUM];
byte ip[IP_ADDRESS_ARRAY_NUM];
byte subnet[NET_MASK_ARRAY_NUM];
byte gw[GATEWAY_ARRAY_NUM];
byte dns[DNS_ARRAY_NUM];

void setup() {
  byte mac[6] = { 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };
  byte ip[4] = { 192, 168, 60, 200 };
  byte dns[4] = { 8, 8, 8, 8 };
  byte gw[4] = { 192, 168, 60, 1 };
  byte subnet[4] = { 255, 255, 255, 0 };
  
   
  Serial.begin(115200);
  Serial.println("Initial");
  Serial.println("EEPROM CLR");
  for (int i = 0 ; i < EEPROM.length() ; i++) {
    EEPROM.write(i, 0);
  }
  //delay(100);
  Serial.println("EEPROM CONFIG");
  EEPROM.write(EEPROMOFFSET, NET_EEPROM_MAGIC);
  writeEEPROM(mac, MAC_OFFSET, 6);
  writeEEPROM(ip, IP_OFFSET, 4);
  writeEEPROM(subnet, SUBNET_OFFSET, 4);
  writeEEPROM(gw, GW_OFFSET, 4);
  writeEEPROM(dns, DNS_OFFSET, 4);

  delay(5000);
  Serial.println("EEPROM READ");
  readEEPROM();
  initAP();
}

void loop() {
  // put your main code here, to run repeatedly:
  
}

void writeEEPROM(byte data[], int offset, int length) {
  for (int i = 0; i < length; i++) {
    EEPROM.write(offset + i, data[i]);
  }
}

void readEEPROM(byte data[], int offset, int length) {
  for (int i = 0; i < length; i++) {
    data[i] = EEPROM.read(offset + i);
    Serial.println(data[i]);
  }
}

EthernetServer server( 1234 );

void initAP() {
  Ethernet.begin( mac, ip, dns, gw, subnet );
  // Initial Server
  server.begin();
  Serial.print("Arduino IP: ");
  Serial.println(Ethernet.localIP());
  }
  
void readEEPROM() {
  if ( EEPROM.read( 0 ) > 1) {
    Serial.println("MAC:");
    readEEPROM(mac, MAC_OFFSET, 6);
    Serial.println("IP:");
    readEEPROM(ip, IP_OFFSET, 4);
    Serial.println("NETMASK:");
    readEEPROM(subnet, SUBNET_OFFSET, 4);
    Serial.println("Gateway:");
    readEEPROM(gw, GW_OFFSET, 4);
    Serial.println("DNS:");
    readEEPROM(dns, DNS_OFFSET, 4);
  }
} 
