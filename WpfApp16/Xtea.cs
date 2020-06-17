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
    class Xtea
    {
        string key;
        string text;
        string rez;

        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
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

        public Xtea(string k, string t)
        {
            key = k;
            text = t;
            rez = "";
        }

        public string Encrypt()
        {
            rez = "";
            if (key.Length != 16) { MessageBox.Show("Введите ключ"); return ""; }
            if (text == "") { MessageBox.Show("Введите текст"); return ""; }
            int kblock = text.Length / 4;
            if (text.Length % 4 != 0) kblock++;
            if (kblock % 2 != 0) kblock++;
            string[] blocks = new string[kblock];
            string temp = text;
            int ii = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                blocks[ii] += temp[i];
                if (blocks[ii].Length == 4) ii++;
            }
            if (blocks[blocks.Length - 1] == null) blocks[blocks.Length - 1] = "";
            while (blocks[blocks.Length - 1].Length != 4)
            {
                blocks[blocks.Length - 1] += " ";
            }
            while (blocks[blocks.Length - 2].Length != 4)
            {
                blocks[blocks.Length - 2] += " ";
            }

            string[] blockskey = new string[4];
            string temp1 = key;
            ii = 0;
            for (int i = 0; i < temp1.Length; i++)
            {
                blockskey[ii] += temp1[i];
                if (blockskey[ii].Length == 4) ii++;
            }
            while (blockskey[blockskey.Length - 1].Length != 4)
            {
                blockskey[blockskey.Length - 1] += " ";
            }

            for (int iii = 0; iii < blocks.Length; iii += 2) {

                int[] text1 = { BitConverter.ToInt32(Encoding.Default.GetBytes(blocks[iii]), 0), BitConverter.ToInt32(Encoding.Default.GetBytes(blocks[iii+1]), 0) };
                int[] key1 = { BitConverter.ToInt32(Encoding.Default.GetBytes(blockskey[0]), 0), BitConverter.ToInt32(Encoding.Default.GetBytes(blockskey[1]), 0), BitConverter.ToInt32(Encoding.Default.GetBytes(blockskey[2]), 0), BitConverter.ToInt32(Encoding.Default.GetBytes(blockskey[3]), 0) };

                EncryptOneBlock(32, text1, key1);
                string rez1 = Encoding.Default.GetString(BitConverter.GetBytes(text1[0]));
                string rez2 = Encoding.Default.GetString(BitConverter.GetBytes(text1[1]));
                
                rez += rez1 + rez2;
            }          
            return rez;
        }

        void EncryptOneBlock(int num_rounds, int[] v, int[] k)
        {
            int i;
            int v0 = v[0], v1 = v[1];
            long sum = 0;
            long delta = 0x9E3779B9; ;
            for (i = 0; i < num_rounds; i++)
            {
                v0 += (((v1 << 4) ^ (v1 >> 5)) + v1) ^ ((int)sum + k[sum & 3]);
                sum += delta;
                v1 += (((v0 << 4) ^ (v0 >> 5)) + v0) ^ ((int)sum + k[(sum >> 11) & 3]);
            }
            v[0] = v0; v[1] = v1;
        }


        public string Decrypt()
        {
           
            rez = "";
            if (key.Length != 16) { MessageBox.Show("Введите ключ"); return ""; }
            if (text == "") { MessageBox.Show("Введите текст"); return ""; }
            int kblock = text.Length / 4;
            if (text.Length % 4 != 0) kblock++;
            if (kblock % 2 != 0) kblock++;
            string[] blocks = new string[kblock];
            string temp = text;
            int ii = 0;
            for (int i = 0; i < temp.Length; i++)
            {
                blocks[ii] += temp[i];
                if (blocks[ii].Length == 4) ii++;
            }
            if (blocks[blocks.Length - 1] == null) blocks[blocks.Length - 1] = "";
            while (blocks[blocks.Length - 1].Length != 4)
            {
                blocks[blocks.Length - 1] += " ";
            }
            while (blocks[blocks.Length - 2].Length != 4)
            {
                blocks[blocks.Length - 2] += " ";
            }


            string[] blockskey = new string[4];
            string temp1 = key;
            ii = 0;
            for (int i = 0; i < temp1.Length; i++)
            {
                blockskey[ii] += temp1[i];
                if (blockskey[ii].Length == 4) ii++;
            }
            while (blockskey[blockskey.Length - 1].Length != 4)
            {
                blockskey[blockskey.Length - 1] += " ";
            }

            for (int iii = 0; iii < blocks.Length; iii += 2)
            {

                int[] text1 = { BitConverter.ToInt32(Encoding.Default.GetBytes(blocks[iii]), 0), BitConverter.ToInt32(Encoding.Default.GetBytes(blocks[iii + 1]), 0) };
                int[] key1 = { BitConverter.ToInt32(Encoding.Default.GetBytes(blockskey[0]), 0), BitConverter.ToInt32(Encoding.Default.GetBytes(blockskey[1]), 0), BitConverter.ToInt32(Encoding.Default.GetBytes(blockskey[2]), 0), BitConverter.ToInt32(Encoding.Default.GetBytes(blockskey[3]), 0) };
                
                DecryptOneBlock(32, text1, key1);
                string rez1 = Encoding.Default.GetString(BitConverter.GetBytes(text1[0]));
                string rez2 = Encoding.Default.GetString(BitConverter.GetBytes(text1[1]));
                rez += rez1 + rez2;
            }

            return rez;
        }

        private void DecryptOneBlock(int rounds, int[] v, int[] k)
        {
            long delta = 0x9E3779B9; ;
            long sum = delta * rounds;
            int v0 = v[0], v1 = v[1];
            for (int i = 0; i < rounds; i++)
            {
                v1 -= (((v0 << 4) ^ (v0 >> 5)) + v0) ^ ((int)sum + k[(sum >> 11) & 3]);
                sum -= delta;
                v0 -= (((v1 << 4) ^ (v1 >> 5)) + v1) ^ ((int)sum + k[sum & 3]);
            }

            v[0] = v0;
            v[1] = v1;
        }
    }
}
