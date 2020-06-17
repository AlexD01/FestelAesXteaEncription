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

namespace WpfApp16
{


    class Festel
    {
        char[] alfarray = { 'а', 'б', 'в', 'г', 'д', 'е', 'ё', 'ж', 'з', 'и', 'й', 'к', 'л', 'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ъ', 'ы', 'ь', 'э', 'ю', 'я', '.', ',', '_' };
        string key1;
        string key2;
        string text;
        string rez;
        public Festel(string k,string t)
        {
            key1 = k;
            text = t;
            rez = "";
        }

        public string Key1
        {
            get
            {
                return key1;
            }
            set
            {
                key1 = value;
            }
        }
        public string Key2
        {
            get
            {
                return key2;
            }
            set
            {
                key2 = value;
            }
        }
        public string Text
        {
            get
            {
                return text;
            }
            set
            {
                text = value;
            }
        }
        private void generateKey2()
        {
            key2 = "";
            for (int i = 0; i < key1.Length; i++)
            {
                int k = 0;
                for (int i1 = 0; i1 < alfarray.Length; i1++)
                {
                    if (alfarray[i1] == key1[i]) k = i1;
                }
                for (int j = 0; j < 5; j++)
                {
                    if (k >= 35) k = 0;
                    else k++;
                }
                key2 += alfarray[k];
            }
        }

        public string  Encrypt()
        {
            generateKey2();
            rez = "";
            if (key1.Length != 4 || key2.Length != 4) { MessageBox.Show("Введите ключ"); return ""; }
            if (text == "") { MessageBox.Show("Введите текст"); return ""; }

            int kblock = text.Length / 8;
            if (text.Length % 8 != 0) kblock++;
            string[] blocks = new string[kblock];
            string temp = text.ToLower();
            int ii = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] != ' ') blocks[ii] += temp[i];
                else blocks[ii] += "_";
                if (blocks[ii].Length == 8) ii++;
            }
            while (blocks[blocks.Length - 1].Length != 8)
            {
                blocks[blocks.Length - 1] += "_";
            }

            for (int i = 0; i < blocks.Length; i++)
            {
                string[] rangs = new string[2];
                ii = 0;
                for (int i1 = 0; i1 < blocks[i].Length; i1++)
                {
                    rangs[ii] += blocks[i][i1];
                    if (rangs[ii].Length == 4) ii++;
                }

                int[,] tab1 = new int[3, 4];

                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == rangs[1][j1]) k = ii1;
                    }
                    tab1[0, j1] = k;
                }
                
                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == key1[j1]) k = ii1;
                    }
                    tab1[1, j1] = k;
                }

                for (int j1 = 0; j1 < 4; j1++)
                {
                    tab1[2, j1] = (tab1[0, j1] + tab1[1, j1]) % 36;

                }
            

                int[,] tab2 = new int[3, 4];

                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == rangs[0][j1]) k = ii1;
                    }
                    tab2[0, j1] = k;
                    tab2[1, j1] = tab1[2, j1];
                }
                string rez1 = "";
                for (int j1 = 0; j1 < 4; j1++)
                {
                    tab2[2, j1] = (tab2[0, j1] + tab2[1, j1]) % 36;
                    rez1 += alfarray[tab2[2, j1]];
                }
              
                int[,] tab3 = new int[3, 4];

                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == rez1[j1]) k = ii1;
                    }
                    tab3[0, j1] = k;
                }
                
                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == key2[j1]) k = ii1;
                    }
                    tab3[1, j1] = k;
                }

                for (int j1 = 0; j1 < 4; j1++)
                {
                    tab3[2, j1] = (tab3[0, j1] + tab3[1, j1]) % 36;

                }

                int[,] tab4 = new int[3, 4];

                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == rangs[1][j1]) k = ii1;
                    }
                    tab4[0, j1] = k;
                    tab4[1, j1] = tab3[2, j1];
                }
                string rez2 = "";
                for (int j1 = 0; j1 < 4; j1++)
                {
                    tab4[2, j1] = (tab4[0, j1] + tab4[1, j1]) % 36;
                    rez2 += alfarray[tab4[2, j1]];
                }
              
                rez += rez1 + rez2;
            }

            return rez;
        }

        public string Decrypt()
        {
            generateKey2();
            rez = "";
            if (key1.Length != 4 || key2.Length != 4) { MessageBox.Show("Введите ключ"); return ""; }
            if (text == "") { MessageBox.Show("Введите текст"); return ""; }


            int kblock = text.Length / 8;
            if (text.Length % 8 != 0) kblock++;
            string[] blocks = new string[kblock];
            string temp =text.ToLower();
            int ii = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                if (temp[i] != ' ') blocks[ii] += temp[i];
                else blocks[ii] += "_";
                if (blocks[ii].Length == 8) ii++;
            }
            while (blocks[blocks.Length - 1].Length != 8)
            {
                blocks[blocks.Length - 1] += "_";
            }


            for (int i = 0; i < blocks.Length; i++)
            {
                string[] rangs = new string[2];
                ii = 0;
                for (int i1 = 0; i1 < blocks[i].Length; i1++)
                {
                    rangs[ii] += blocks[i][i1];
                    if (rangs[ii].Length == 4) ii++;
                }

                int[,] tab1 = new int[3, 4];

                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == rangs[0][j1]) k = ii1;
                    }
                    tab1[0, j1] = k;
                }
                
                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == key2[j1]) k = ii1;
                    }
                    tab1[1, j1] = k;
                }

                for (int j1 = 0; j1 < 4; j1++)
                {
                    tab1[2, j1] = (tab1[0, j1] + tab1[1, j1]) % 36;

                }
               

                int[,] tab2 = new int[3, 4];

                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == rangs[1][j1]) k = ii1;
                    }
                    tab2[0, j1] = k;
                    tab2[1, j1] = tab1[2, j1];
                }
                string rez1 = "";
                for (int j1 = 0; j1 < 4; j1++)
                {
                    tab2[2, j1] = (tab2[0, j1] + 36 - tab2[1, j1]) % 36;
                    rez1 += alfarray[tab2[2, j1]];
                }
              
             

                int[,] tab3 = new int[3, 4];

                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == rez1[j1]) k = ii1;
                    }
                    tab3[0, j1] = k;
                }
            
                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == key1[j1]) k = ii1;
                    }
                    tab3[1, j1] = k;
                }

                for (int j1 = 0; j1 < 4; j1++)
                {
                    tab3[2, j1] = (tab3[0, j1] + tab3[1, j1]) % 36;

                }
             

                int[,] tab4 = new int[3, 4];

                for (int j1 = 0; j1 < 4; j1++)
                {
                    int k = 0;
                    for (int ii1 = 0; ii1 < alfarray.Length; ii1++)
                    {
                        if (alfarray[ii1] == rangs[0][j1]) k = ii1;
                    }
                    tab4[0, j1] = k;
                    tab4[1, j1] = tab3[2, j1];
                }
                string rez2 = "";
                for (int j1 = 0; j1 < 4; j1++)
                {
                    tab4[2, j1] = (tab4[0, j1] + 36 - tab4[1, j1]) % 36;
                    rez2 += alfarray[tab4[2, j1]];
                }
              
                rez += rez2 + rez1;
            }

            return rez;
        }

    }
}
