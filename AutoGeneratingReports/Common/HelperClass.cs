using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AutoGeneratingReports.Common
{
    public static class HelperClass
    {
        public static void writeExceptionToDebugger(Exception ex)
        {
            //Debug.WriteLine("Exception the function " + System.Reflection.MethodBase.GetCurrentMethod().Name);
            Debug.WriteLine(ex.ToString());
        }
        public static string ChuyenSo(string number)
        {
            string[] strTachPhanSauDauPhay;
            if (number.Contains('.') || number.Contains(','))
            {
                strTachPhanSauDauPhay = number.Split(',', '.');
                return (ChuyenSo(strTachPhanSauDauPhay[0]) + "phẩy " + ChuyenSo(strTachPhanSauDauPhay[1]));
            }

            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "linh ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if ((i + j == len - 1) || (i + j + 3 == len - 1))
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += ((n - j) != 1) ? dv[n - j - 1] + " " : dv[n - j - 1];
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            return doc;
        }
        private static byte[] key = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
        private static byte[] iv = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };

        public static string Crypt(this string text)
        {
            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateEncryptor(key, iv);
            byte[] inputbuffer = Encoding.Unicode.GetBytes(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Convert.ToBase64String(outputBuffer);
        }

        public static string Decrypt(this string text)
        {

            SymmetricAlgorithm algorithm = DES.Create();
            ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            byte[] inputbuffer = Convert.FromBase64String(text);
            byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            return Encoding.Unicode.GetString(outputBuffer);
        }

        public static string DecryptPasswordFromConnectionString(this string strConnectionString)
        {
            if (strConnectionString.ToUpper().Contains("DATA SOURCE"))
                return strConnectionString;
            return Decrypt(strConnectionString);
            //int idx=strConnectionString.IndexOf("Password=");
            //string strPassword = strConnectionString.Substring(idx + 9, strConnectionString.Length - idx - 9);
            //string strNoPassword = strConnectionString.Substring(0,idx-1);
            //SymmetricAlgorithm algorithm = DES.Create();
            //ICryptoTransform transform = algorithm.CreateDecryptor(key, iv);
            //byte[] inputbuffer = Convert.FromBase64String(strPassword);
            //byte[] outputBuffer = transform.TransformFinalBlock(inputbuffer, 0, inputbuffer.Length);
            //string strDecryptedConnectionString = strNoPassword+";Password="+Encoding.Unicode.GetString(outputBuffer);
            //return strDecryptedConnectionString;
        }
    }
}
