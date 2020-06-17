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
using System.Windows.Shapes;
using System.IO.Compression;
using System.IO;

using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using PPoint = Microsoft.Office.Interop.PowerPoint;

using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Collections;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WpfApp16
{
    /// <summary>
    /// Логика взаимодействия для mein.xaml
    /// </summary>
    public partial class Mein : Window
    {
        
        public Mein(string log,string k11,string k22,string k33)
        {
            InitializeComponent();

            Hi.Text += log;
            k1.Text = k11.Split(' ')[0];
            k2.Text = k22.Split(' ')[0];
            k3.Text = k33.Split(' ')[0]; 
            
        }



        private void r1_Checked(object sender, RoutedEventArgs e)
        {
                gf.Visibility = Visibility.Visible;
                ga.Visibility = Visibility.Hidden;
                gx.Visibility = Visibility.Hidden;
        }

        private void r2_Checked(object sender, RoutedEventArgs e)
        {
                gf.Visibility = Visibility.Hidden;
                ga.Visibility = Visibility.Visible;
                gx.Visibility = Visibility.Hidden;
        }

        private void r3_Checked(object sender, RoutedEventArgs e)
        {
                gf.Visibility = Visibility.Hidden;
                ga.Visibility = Visibility.Hidden;
                gx.Visibility = Visibility.Visible;
        }

        private void fencry_Click(object sender, RoutedEventArgs e)
        {
            Festel temp = new Festel(k1.Text, ft.Text);
            ft.Text = temp.Encrypt();
        }

        private void fdecry_Click(object sender, RoutedEventArgs e)
        {
            Festel temp = new Festel(k1.Text, ft.Text);
            ft.Text = temp.Decrypt();
        }

        private void fopen_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents|*.txt";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                filename = dlg.FileName;
                string[] t = filename.Split('.');
                if (t.Length == 1)
                {
                    string temp = System.IO.File.ReadAllText(filename);
                    ft.Text = temp;
                    return;
                }
                if (t[1] == "txt")

                {
                    string temp = System.IO.File.ReadAllText(filename);
                    ft.Text = temp;
                }

                if (t[1] == "docx")
                {
                    var WordApp = new Word.Application();
                    WordApp.Visible = false;
                    var WordDoc = WordApp.Documents.Open(filename);
                    ft.Text = WordDoc.Content.Text;
                    WordDoc.Close();

                }
                if (t[1] == "xlsx")
                {
                    Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
                    ObjWorkExcel.Visible = false;
                    Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(filename); //открыть файл
                    for (int i = 1; i <= ObjWorkBook.Sheets.Count; i++)
                    {
                        Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[i]; //получить 1 лист
                        var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейк
                        for (int i2 = 0; i2 < (int)lastCell.Column; i2++) //по всем колонкам
                            for (int j = 0; j < (int)lastCell.Row; j++) // по всем строкам
                                ft.Text += ObjWorkSheet.Cells[i2 + 1, j + 1].Text.ToString();
                    }
                    ObjWorkBook.Close(); //закрыть не сохраняя
                }

                if (t[1] == "pptx")
                {
                    PPoint.Application app = new PPoint.Application();
                    app.Visible = Microsoft.Office.Core.MsoTriState.msoCTrue;
                    app.Presentations.Open(filename);
                    PPoint.Presentation presentation = app.ActivePresentation;
                    for (int i = 1; i <= presentation.Slides.Count; i++)
                    {
                        for (int j = 1; j <= presentation.Slides[i].Shapes.Count; j++)
                            try
                            {
                                ft.Text += presentation.Slides[i].Shapes[j].TextFrame.TextRange.Text;
                            }
                            catch (Exception ee)
                            {

                            }
                    }
                    app.ActivePresentation.Close();
                    app.Quit();

                }

                if (t[1] == "pdf")
                {
                    PdfReader pdfReader = new PdfReader(filename);
                    for (int i = 0; i < pdfReader.NumberOfPages; i++)
                    {
                        ft.Text += "\n" + PdfTextExtractor.GetTextFromPage(pdfReader, i + 1);
                    }
                }

            }
        }

        private void fsave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents|*.txt";
            if (result == true)
            {
                string temp = ft.Text;
                string filename = dlg.FileName;
                System.IO.File.WriteAllText(filename, temp);
            }
        }

        private void aencry_Click(object sender, RoutedEventArgs e)
        {
            Aes temp = new Aes(k2.Text, at.Text);
            at.Text = temp.Encrypt();
        }

        private void adecry_Click(object sender, RoutedEventArgs e)
        {
            Aes temp = new Aes(k2.Text, at.Text);
            at.Text = temp.Decrypt();
        }

        private void aopen_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Documents|*.*";
            Nullable<bool> result = dlg.ShowDialog();


            if (result == true)
            {
                filename = dlg.FileName;
                string[] t = filename.Split('.');

                if (t.Length == 1)
                {
                    string temp = System.IO.File.ReadAllText(filename);
                    at.Text = temp;
                    return;
                }

                if (t[1] == "txt")

                { 
                string temp = System.IO.File.ReadAllText(filename);
                at.Text = temp;
                }

                if (t[1] == "docx")
                {
                    var WordApp = new Word.Application();
                    WordApp.Visible = false;
                    var WordDoc = WordApp.Documents.Open(filename);
                    at.Text = WordDoc.Content.Text;
                    WordDoc.Close();

                }
                if (t[1] == "xlsx")
                {
                    Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
                    ObjWorkExcel.Visible = false;
                    Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(filename); //открыть файл
                    for (int i = 1; i <= ObjWorkBook.Sheets.Count; i++)
                    {
                        Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[i]; //получить 1 лист
                        var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейк
                        for (int i2 = 0; i2 < (int)lastCell.Column; i2++) //по всем колонкам
                            for (int j = 0; j < (int)lastCell.Row; j++) // по всем строкам
                                at.Text += ObjWorkSheet.Cells[i2 + 1, j + 1].Text.ToString();
                    }
                    ObjWorkBook.Close(); //закрыть не сохраняя
                }

                if (t[1] == "pptx")
                {
                    PPoint.Application app = new PPoint.Application();
                    app.Visible = Microsoft.Office.Core.MsoTriState.msoCTrue;
                    app.Presentations.Open(filename);
                    PPoint.Presentation presentation = app.ActivePresentation;
                    for (int i = 1; i <= presentation.Slides.Count; i++)
                    {
                        for (int j = 1; j <= presentation.Slides[i].Shapes.Count; j++)
                            try
                            {
                                at.Text += presentation.Slides[i].Shapes[j].TextFrame.TextRange.Text;
                            }
                            catch(Exception ee)
                            {

                            }
                    }
                    app.ActivePresentation.Close();
                    app.Quit();

                }

                if (t[1] == "pdf")
                {
                    PdfReader pdfReader = new PdfReader(filename);
                    for (int i = 0; i < pdfReader.NumberOfPages; i++)
                    {
                        at.Text += "\n" + PdfTextExtractor.GetTextFromPage(pdfReader, i + 1);
                    }
                }

            }
        }

        private void asave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents|*.txt";
            if (result == true)
            {
                string temp = at.Text;
                string filename = dlg.FileName;
                System.IO.File.WriteAllText(filename, temp);
            }
        }

        private void aencrypt_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = "";
            dlg.Filter = "";
            Nullable<bool> result = dlg.ShowDialog();
            
            if (result == true)
            {
                filename = dlg.FileName;
                string temp = System.IO.File.ReadAllText(filename, Encoding.Default);
                Aes temp1 = new Aes(k2.Text, temp);
                temp = temp1.Encrypt();
                string[] t1 = filename.Split('\\');
                string[] t2 = t1[t1.Length - 1].Split('.');
                
                string t3 = "";
                for (int i = 0; i < t1.Length - 1; i++)
                    t3 += t1[i]+"\\";

                t3 += t2[0]+ "ƒ" + t2[1]+ "ƒ" + "encrypt";
                System.IO.File.Copy(filename, t3);
                System.IO.File.WriteAllText(t3, temp, Encoding.Default);
            }


        }

        private void adecrypt_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = "";
            dlg.Filter = "";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                filename = dlg.FileName;            
                string temp = System.IO.File.ReadAllText(filename, Encoding.Default);
                Aes temp1 = new Aes(k2.Text, temp);
                temp = temp1.Decrypt();
                string[] t1 = filename.Split('\\');
                string[] t2 = t1[t1.Length - 1].Split('ƒ');

                string t3 = "";
                for (int i = 0; i < t1.Length - 1; i++)
                    t3 += t1[i] + "\\";

                t3 += t2[0] + "decrypt." + t2[1];

                System.IO.File.Copy(filename, t3);
                System.IO.File.WriteAllText(t3, temp, Encoding.Default);
            }
        }

        private void xencry_Click(object sender, RoutedEventArgs e)
        {
            Xtea temp = new Xtea(k3.Text, xt.Text);
            xt.Text = temp.Encrypt();
        }

        private void xdecry_Click(object sender, RoutedEventArgs e)
        {
            Xtea temp = new Xtea(k3.Text, xt.Text);
            xt.Text = temp.Decrypt();
        }

        private void xopen_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents|*.txt";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                filename = dlg.FileName;
                string[] t = filename.Split('.');
                if (t.Length == 1)
                {
                    string temp = System.IO.File.ReadAllText(filename);
                    xt.Text = temp;
                    return;
                }

                if (t[1] == "txt")

                {
                    string temp = System.IO.File.ReadAllText(filename);
                    xt.Text = temp;
                }

                if (t[1] == "docx")
                {
                    var WordApp = new Word.Application();
                    WordApp.Visible = false;
                    var WordDoc = WordApp.Documents.Open(filename);
                    xt.Text = WordDoc.Content.Text;
                    WordDoc.Close();

                }
                if (t[1] == "xlsx")
                {
                    Excel.Application ObjWorkExcel = new Excel.Application(); //открыть эксель
                    ObjWorkExcel.Visible = false;
                    Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(filename); //открыть файл
                    for (int i = 1; i <= ObjWorkBook.Sheets.Count; i++)
                    {
                        Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[i]; //получить 1 лист
                        var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);//1 ячейк
                        for (int i2 = 0; i2 < (int)lastCell.Column; i2++) //по всем колонкам
                            for (int j = 0; j < (int)lastCell.Row; j++) // по всем строкам
                                xt.Text += ObjWorkSheet.Cells[i2 + 1, j + 1].Text.ToString();
                    }
                    ObjWorkBook.Close(); //закрыть не сохраняя
                }

                if (t[1] == "pptx")
                {
                    PPoint.Application app = new PPoint.Application();
                    app.Visible = Microsoft.Office.Core.MsoTriState.msoCTrue;
                    app.Presentations.Open(filename);
                    PPoint.Presentation presentation = app.ActivePresentation;
                    for (int i = 1; i <= presentation.Slides.Count; i++)
                    {
                        for (int j = 1; j <= presentation.Slides[i].Shapes.Count; j++)
                            try
                            {
                                xt.Text += presentation.Slides[i].Shapes[j].TextFrame.TextRange.Text;
                            }
                            catch (Exception ee)
                            {

                            }
                    }
                    app.ActivePresentation.Close();
                    app.Quit();


                }
                if (t[1] == "pdf")
                {
                    PdfReader pdfReader = new PdfReader(filename);
                    for (int i = 0; i < pdfReader.NumberOfPages; i++)
                    {
                        xt.Text += "\n" + PdfTextExtractor.GetTextFromPage(pdfReader, i + 1);
                    }
                }
            }
        }

        private void xsave_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            Nullable<bool> result = dlg.ShowDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents|*.txt";
            if (result == true)
            {
                string temp = xt.Text;
                string filename = dlg.FileName;
                System.IO.File.WriteAllText(filename, temp);
            }
        }

        private void xencrypt_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = "";
            dlg.Filter = "";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                filename = dlg.FileName;
                string temp = System.IO.File.ReadAllText(filename, Encoding.Default);
                Xtea temp1 = new Xtea(k3.Text, temp);
                temp = temp1.Encrypt();
                string[] t1 = filename.Split('\\');
                string[] t2 = t1[t1.Length - 1].Split('.');

                string t3 = "";
                for (int i = 0; i < t1.Length - 1; i++)
                    t3 += t1[i] + "\\";

                t3 += t2[0] + "ƒ" + t2[1] + "ƒ" + "encrypt";
                System.IO.File.Copy(filename, t3);
                System.IO.File.WriteAllText(t3, temp, Encoding.Default);

            }
        }

        private void xdecrypt_Click(object sender, RoutedEventArgs e)
        {
            string filename = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "";
            dlg.DefaultExt = "";
            dlg.Filter = "";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                filename = dlg.FileName;
                string temp = System.IO.File.ReadAllText(filename,Encoding.Default);
                Xtea temp1 = new Xtea(k3.Text, temp);
                temp = temp1.Decrypt();
                string[] t1 = filename.Split('\\');
                string[] t2 = t1[t1.Length - 1].Split('ƒ');

                string t3 = "";
                for (int i = 0; i < t1.Length - 1; i++)
                    t3 += t1[i] + "\\";

                t3 += t2[0] + "decrypt." + t2[1];

                System.IO.File.Copy(filename, t3);
                System.IO.File.WriteAllText(t3, temp, Encoding.Default);


            }
        }
    }
}
