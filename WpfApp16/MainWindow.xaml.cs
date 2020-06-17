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

namespace WpfApp16
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            string rez="";
            string filename = "logpass";
            if (lg1.Text == "" || ps1.Text == "") { MessageBox.Show("Введите данные"); return; }
            if (k1.Text == "" || k2.Text == "" || k3.Text == ""||k1.Text.Length!=4 || k2.Text.Length !=16 || k3.Text.Length != 16) { MessageBox.Show("Введите ключи"); return; }
            Xtea lg = new Xtea("MY WORLDMY WORLD", lg1.Text);
            Xtea ps = new Xtea("MY WORLDMY WORLD", ps1.Text);
            Xtea k11 = new Xtea("MY WORLDMY WORLD", k1.Text);
            Xtea k22 = new Xtea("MY WORLDMY WORLD", k2.Text);
            Xtea k33 = new Xtea("MY WORLDMY WORLD", k3.Text);
            string dec1 = lg.Encrypt();
            string dec2 = ps.Encrypt();
            string dec3 = k11.Encrypt();
            string dec4 = k22.Encrypt();
            string dec5 = k33.Encrypt();


            string temp = System.IO.File.ReadAllText(filename);
            string[] temp1 = temp.Split('\n');

            bool b = false;
            for (int i = 0; i < temp1.Length; i++)
            {
                string[] temp2;
                temp2 = temp1[i].Split('ƒ');
                if (dec1 == temp2[0])
                {
                    b = true;
                    break;
                }
            }


            if (b == true) { MessageBox.Show("Такой аккаунт уже существует"); return; }

            rez = dec1 + "ƒ" + dec2 + "ƒ" + dec3 + "ƒ" + dec4 + "ƒ" + dec5 + "ƒ" + Environment.NewLine;
            System.IO.File.AppendAllText(filename,rez);
            g1.Visibility = Visibility.Visible;
            g2.Visibility = Visibility.Hidden;
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            g1.Visibility = Visibility.Visible;
            g2.Visibility = Visibility.Hidden;
        }

        private void toreg_Click(object sender, RoutedEventArgs e)
        {
            g1.Visibility = Visibility.Hidden;
            g2.Visibility = Visibility.Visible;
        }

        private void log_Click(object sender, RoutedEventArgs e)
        {

            string filename = "logpass";
            string temp = System.IO.File.ReadAllText(filename);
            if (lg.Text == "" || ps.Text == "") { MessageBox.Show("Введите данные"); return; }
            Xtea lg1 = new Xtea("MY WORLDMY WORLD", lg.Text);
            Xtea ps1 = new Xtea("MY WORLDMY WORLD", ps.Text);
            string dec1 = lg1.Encrypt();
            string dec2 = ps1.Encrypt();
            string[] temp1 = temp.Split('\n');

            bool b = false;

            string dec3 = "";
            string dec4 = "";
            string dec5 = ""; 
            for (int i = 0; i < temp1.Length; i++)
            {
                string[] temp2;
                temp2 = temp1[i].Split('ƒ');
                if (dec1 == temp2[0] && dec2 == temp2[1])
                {

                    Xtea k11 = new Xtea("MY WORLDMY WORLD", temp2[2]);
                    Xtea k22 = new Xtea("MY WORLDMY WORLD", temp2[3]);
                    Xtea k33 = new Xtea("MY WORLDMY WORLD", temp2[4]);
                    dec3 = k11.Decrypt();
                    dec4 = k22.Decrypt();
                    dec5 = k33.Decrypt();
                    b = true;
                    break;
                }
            }
           



            if (b == true) {
                Mein win = new Mein(lg.Text, dec3,dec4,dec5);
                win.Show();
                this.Close();
            }
            else
            {
               MessageBox.Show("Данные не верные");
            }


        }
    }
}
