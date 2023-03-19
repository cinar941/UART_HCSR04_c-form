using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO.Ports;

namespace VERI
{
    public partial class Form1 : Form
    {

        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-RLCKHNM\SQLEXPRESS;Initial Catalog=veri;Integrated Security=True;Pooling=False");
        int a = 0;
        string gelen = "0";
        SerialPort serialport;
        public Form1()
        {
            InitializeComponent();
            serialport = new SerialPort();
            serialport.BaudRate = 9600;
            comboBox1.Text = "COM3";//program başlar başlamaz com3 ile baglanmayı denıycek olmassa farklı port seçebiliriz comboboxdan
            serialPort1.PortName = comboBox1.Text;
            serialPort1.Open();//seri port baglantımızı açıyoruz
            serialPort1.Write("1");
            //baglantı saglandıgı zaman serialport1.write komutu ile devremıze 1 göneriyoruz
            //bu şekilde 1 degerını alan  devre ledımızı yakıcak buda baglantı saglandıgını göstericek
            timer1.Start();
          
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();  //Seri portları diziye ekleme
            foreach (string port in ports)
            comboBox1.Items.Add(port);               //Seri portları comboBox1'e ekleme
            guncelle();

        }
        private void button1_Click(object sender, EventArgs e)
        {
           //baglan butonumuzun kodları
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.Open();//seri port baglantımızı açıyoruz
                serialPort1.Write("1");//baglantı saglandıgı zaman serialport1.write komutu ile devremıze 1 göneriyoruz
                //bu şekilde 1 degerını alan  devre ledımızı yakıcak buda baglantı saglandıgını göstericek
                timer1.Enabled = true;
               
            }
            catch
            {
                MessageBox.Show("Seri Port Hatası!");
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            serialPort1.Write("2");
            //baglantı saglandıgı zaman serialport1.write komutu ile devremıze 2 göneriyoruz
            //bu şekilde 1 degerını alan  devre ledımızı sönecek buda baglantı kesıldıgını göstericek
            timer1.Stop();
            serialport.Close();//seri port baglantımızı kesıyoruz
            
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
            gelen = serialPort1.ReadLine();
                //seri port ıle devremızden gelen mesafe degerını gelen degişkenıne atıyoruz.
            a = Convert.ToInt32(gelen);
            if (a >= 0 && a < 200)
            {
             //gelen mesafe verisini textboxa yazıyoruz
            textBox1.Text = gelen;
            }
            progressBar1.Value = a;//gelen verı degerını progres  bara atıyoruz bu şekilde görsel olarak doluluk gosterıyor
            label3.Text = DateTime.Now.ToLongDateString();//tarihi labele yazdırıyoruz
            label4.Text = DateTime.Now.ToLongTimeString();//saati labele yazıyoruz
            }
            catch (Exception) { }
        }
        private void guncelle()
        {//guncel verı tabanı bılgılerıı grıd view e çagırıyoruz
            try
            {
            baglanti.Open();
            string kayit = "SELECT * from VERİ";
            //veri tablosundaki tüm kayıtları çekecek olan sql sorgusu.
            SqlCommand komut = new SqlCommand(kayit, baglanti);
            //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
            SqlDataAdapter da = new SqlDataAdapter(komut);
            //SqlDataAdapter sınıfı verilerin databaseden aktarılması işlemini gerçekleştirir.
            DataTable dt = new DataTable();
            da.Fill(dt);
            //Bir DataTable oluşturarak DataAdapter ile getirilen verileri tablo içerisine dolduruyoruz.
            dataGridView1.DataSource = dt;
            //Formumuzdaki DataGridViewin veri kaynağını oluşturduğumuz tablo olarak gösteriyoruz.
            baglanti.Close();
            }catch(Exception hata)
            {
                MessageBox.Show(Convert.ToString( hata));
            }
        }
        private void ekle()
        {
            //kaydetme kodlarımız olculen mesafe degerını ve tarih ve saatı verı tabanına kaydedıyoruz
            try
            {
            baglanti.Open();
            SqlCommand veriekle = new SqlCommand("insert into VERİ (TARIH,SAAT,MESAFE) values (@TRH,@ST,@MSF)", baglanti);
            veriekle.Parameters.AddWithValue("@TRH", label3.Text);
            veriekle.Parameters.AddWithValue("@ST", label4.Text);
            veriekle.Parameters.AddWithValue("@MSF", textBox1.Text);
            veriekle.ExecuteNonQuery();
            baglanti.Close();
            guncelle();
            }catch(Exception hata)
            {
                MessageBox.Show(hata.ToString());
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            //kaydet butonumuza ekle fonksiyonunu çagırıyouz.
            ekle();
           
        }
        private void button5_Click(object sender, EventArgs e)
        { //kayıt sil butonumuzun kodları
            try
            {
            for (int i=0;i<dataGridView1.SelectedRows.Count;i++)
            {
            baglanti.Open();
            SqlCommand kmt = new SqlCommand("delete from VERİ where SAAT='"+dataGridView1.SelectedRows[i].Cells["SAAT"].Value.ToString()+"'",baglanti);
            kmt.ExecuteNonQuery();
            baglanti.Close();
            }
            MessageBox.Show("KAYITLAR SİLİNDİ");
            guncelle();
        
            }catch(Exception hata)
            {
                MessageBox.Show(hata.ToString());
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {//guncel butonu ile verı tabanı bılgılerıı grıd view e çagırıyoruz
            guncelle();
        }
    }
}
