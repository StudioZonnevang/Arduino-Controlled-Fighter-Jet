#include <Arduino_LSM9DS1.h>

#define RED 22

float prevTime, currentTime;
float x, y, z;

void setup() {
  pinMode(RED, OUTPUT);

  Serial.begin(9600);
  while(!Serial);

  IMU.begin();

  currentTime = millis();
  prevTime = currentTime;
  pinMode(RED, HIGH);
}

void loop() {
  if (currentTime - prevTime > 200){
    if (IMU.accelerationAvailable()){
      IMU.readAcceleration(x, y, z);

      int header = 1;

      String sequence = String(header) + ";" + String(x) + ";" + String(y) + ";" + String(z);
      Serial.println(sequence);
    }
    prevTime = currentTime;
  }
  currentTime = millis();
}

void SetHeader(){
  
}
