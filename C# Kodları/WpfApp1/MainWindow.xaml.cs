using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;    
using System.IO.Ports;
using System.Threading;
using System.Windows.Threading;
using System.Globalization;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    { 
        SerialPort serialPort1 = new SerialPort();
        DispatcherTimer timer1 = new DispatcherTimer();
        delegate void update_RPY_callback(int angle);   

        public MainWindow()
        {
            InitializeComponent();  
            
            serialPort1.PortName = "COM4";
            serialPort1.BaudRate = 115200;
            timer1.Interval = TimeSpan.FromMilliseconds(25);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            serialPort1.Open();
            timer1.Start();
            button.IsEnabled = false;

            timer1.Tick += delegate (object sndr, EventArgs e1)
            {


                serialPort1.Write("1");

                

                int roll = 0, pitch = 0, yaw = 0;



                string[] veri = serialPort1.ReadLine().Split('\t');


                progressBar1.Value = int.Parse(veri[0]) % 100;
                slider.Value = int.Parse(veri[1])%100;
                label19.Content = progressBar1.Value;

                roll = int.Parse(veri[2]);
                pitch = int.Parse(veri[3]);
                yaw = int.Parse(veri[4]);

                    
                
                

                Dispatcher.BeginInvoke(new update_RPY_callback(rotateRoll), new object[] {roll});
                Dispatcher.BeginInvoke(new update_RPY_callback(rotatePitch), new object[] { pitch });
                Dispatcher.BeginInvoke(new update_RPY_callback(rotateYaw), new object[] { yaw });


                serialPort1.DiscardInBuffer();
            };
        }


        private void rotateYaw(int yaw)
        {
            
            RotateTransform rotate2 = new RotateTransform(yaw);
            rotate2.CenterX = 40;
            rotate2.CenterY = 40;
            image2.RenderTransform = rotate2;
            label7.Content = yaw;
        }

        private void rotatePitch(int pitch)
        {
            
            RotateTransform rotate1 = new RotateTransform(-pitch);
            rotate1.CenterX = 40;
            rotate1.CenterY = 40;
            image1.RenderTransform = rotate1;
            label2.Content = pitch;

        }

        private void rotateRoll(int roll)
        {
            
            RotateTransform rotate = new RotateTransform(roll);
            rotate.CenterY = 40;
            image.RenderTransform = rotate;
            label6.Content = roll;

        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            serialPort1.Close();
            timer1.Stop();
            button.IsEnabled = true;
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
