int trigPin=6;
int echoPin=7;
long sure;
long uzaklik;

 void setup() {
  pinMode(trigPin,OUTPUT);
  pinMode(echoPin,INPUT);
  Serial.begin(9600);//Seri haberleşmeyi başlatıyoruz (9600 bps hızında)

 }
 void loop() {
digitalWrite(trigPin,LOW);
delayMicroseconds(5);
digitalWrite(trigPin,HIGH);
 delayMicroseconds(10);
 digitalWrite(trigPin,LOW);
 sure=pulseIn(echoPin,HIGH);//sensörümüzden gönderilen dalganın gidiş geliş süresini sure degişkenine atıyoruz
 uzaklik=sure/29.1/2; //süre degişkenini yanı gidiş geliş hızını kullanarak formül yardımı ile cm cinsine çevirip uzaklık degıskenıne atıyoruz
 if(uzaklik>200){
  //uzaklıgımızın 200 cm den fazla olmasını istemedım eger fazla ise 200 olsun dıye sınırlandırma yaptım
  uzaklik=200;
  
  }
  Serial.write(uzaklik);//serial write komutu ile uzaklık degerımızı UART baglantısı ile 1.devremıze gönderdim
 delay(500);
 }
