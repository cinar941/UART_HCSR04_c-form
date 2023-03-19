int giris=0;
int cikisled=13;
int ata=0;
int led=3;

 void setup() {
  pinMode(cikisled, OUTPUT);
  pinMode(led,OUTPUT);
  pinMode(giris,INPUT);
  Serial.begin(9600);// Seri haberleşmeyi başlatıyoruz (9600 bps hızında)
 }
 void loop() {
  
     //Bu fonksiyon sonsuza kadar çalışır
     //Arduino'nun yapması gereken işlemler buraya yazılır
    
digitalWrite(cikisled,LOW);

if(Serial.available()>0)//ardunıo devresıne gelen veri 0 dan büyükmü 
{
  
   ata=Serial.read();//gelen verileri oku ata degıskenınde ata
    Serial.println(ata);//ata degişkenını ekrana yaz alt satısa geç
   if(ata=='1'){
    //ata degişkenı 1 ise ledi yak
        digitalWrite(led,HIGH);
    }else if(ata=='2'){
      //ata degişkenı 2 ise ledi söndür
       digitalWrite(led,LOW);
    }
    if(ata<10)//ata degişkeni 10 dan küçük ise led yak 
    {
      digitalWrite(cikisled,HIGH);
  delay(3);//3 ms bekleme yapar yanıp söner yani
      }else
      { //ata degişkeni 10 dan büyükse led söner ve yanmaz 
         digitalWrite(cikisled,LOW);
  delay(5);
        }
  }
   
  
 }
