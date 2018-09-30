using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;
using Boo.Lang;

[System.Serializable]
public class ValueDecryptor : Singleton<ValueDecryptor>
{

    RijndaelManaged aes;
    RijndaelManaged aesLocal;
    byte[] bytesTemp;
    string valueTemp;
    private StringBuilder sb;
    List<byte> arrayTemp;
    //값 = temp.Remove(0, 40);
    //해시 = temp.Remove(40);
    public void Initialization(RijndaelManaged temp)
    {
        aes = temp;
        arrayTemp = new List<byte>();

    }
    public void SetLoaclAES(RijndaelManaged temp)
    {
        aesLocal = temp;
    }
    public string Open(string SD52C5Q)
    {
        //1분해
        valueTemp = SD52C5Q.Remove(40);
        SD52C5Q = GetLocalDecrypt(SD52C5Q.Remove(0, 40));
        //3변조 확인
        if (valueTemp == ValueEncryptor.Instance.GetHash(SD52C5Q))
            return SD52C5Q;
        else
            return "polluted";
    }
    public string DecryptValue(string value)
    {
        //앞40글자와 뒤의 해독된 해쉬값이 같아야 한다.
        //1분해

        valueTemp = value.Remove(40);

        value = GetDecrypt(value.Remove(0, 40));
        //3변조 확인
        if (valueTemp == ValueEncryptor.Instance.GetHash(value))
            return value;
        else
            return "polluted";
    }
    public string GetDecrypt(string input)
    {
        bytesTemp = RestoreToBytes(input);
        using (ICryptoTransform ct = aes.CreateDecryptor())
        {
            bytesTemp = ct.TransformFinalBlock(bytesTemp, 0, bytesTemp.Length);
        }
        return Encoding.UTF8.GetString(bytesTemp);
    }
    public byte[] GetDecrypt(byte[] input)
    {
        ICryptoTransform decrypt = aes.CreateDecryptor();
        using (var ms = new MemoryStream())
        {
            using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
            {               
                cs.Write(input, 0, input.Length);
            }

            input = ms.ToArray();            
        }

        //String Output = GetString(xBuff);
        return input;
    }
    private string GetLocalDecrypt(string input)
    {
        bytesTemp = RestoreToBytes(input);
        using (ICryptoTransform ct = aesLocal.CreateDecryptor())
        {
            bytesTemp = ct.TransformFinalBlock(bytesTemp, 0, bytesTemp.Length);
        }
        return Encoding.UTF8.GetString(bytesTemp);
    }

    byte[] RestoreToBytes(string str)
    {
        char[] temp = str.ToCharArray();
        int temp2 = 0, temp3 = 0;
        byte[] result;
        Array.ForEach(temp, x =>
        {
            if ((x == '\"'))
            {
                arrayTemp.Add((byte)temp2);
                temp2 = 0;
            }
            else
            {
                temp3 = hexToint(x);
                temp2 = temp2 == 0 ? temp3 : temp2*16 + temp3;
            }
        });
        arrayTemp.Add((byte)temp2);

        result = arrayTemp.ToArray();
        arrayTemp.Clear();

        return result;
    }
    int hexToint(char ch)
    {
        if (ch >= '0' && ch <= '9')
            return ch - '0';
        if (ch >= 'A' && ch <= 'F')
            return ch - 'A' + 10;
        if (ch >= 'a' && ch <= 'f')
            return ch - 'a' + 10;
        return -1;
    }
}
