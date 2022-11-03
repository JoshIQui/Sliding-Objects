const int dialPin = A0;
int dialValue = 0;

const int buttonPin = 4;
int buttonValue = 0;
int consecutiveHighs = 0;

const int photocellPin = A1;
int photocellValue = 0;

void setup() {
  // put your setup code here, to run once:
  pinMode(buttonPin, INPUT);
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:
  dialValue = analogRead(dialPin);

  ReadButton();

  photocellValue = analogRead(photocellPin);

  PrintToSerial(dialValue, buttonValue, photocellValue);
}

void PrintToSerial(int dialValue, int buttonValue, int photocellValue) {
  Serial.flush();
  Serial.print(dialValue);
  Serial.print(",");
  Serial.print(buttonValue);
  Serial.print(",");
  Serial.println(photocellValue);
  delay(10);
}

void ReadButton()
{
  buttonValue = digitalRead(buttonPin);

  if (buttonValue == HIGH)
  {
    consecutiveHighs++;
  }
  if (buttonValue == LOW)
  {
    consecutiveHighs = 0;
  }

  if (consecutiveHighs < 10)
  {
    buttonValue = LOW;
  }
}