using UnityEngine;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Collections.Generic;

[System.Serializable]
public class ValueEncryptor : Singleton<ValueEncryptor>
{
    // 암호화의 이해
    // 데이터 암호화 >> SHA(키) : SHA(값)+AES(값)
    // 봉인 >> SHA(값)+AES(값)
    private byte[] _keys;
    private byte[] _iv;
    private byte[] _keysLocal;
    private byte[] _ivLocal;
    private int keySize = 256;
    private int blockSize = 128;
    byte[] bytesTemp;
    string hashTemp;
    string valueTemp;
    string a = "-", b = "", c="\"";
    private StringBuilder sb;
    private StringBuilder sb2;
    RijndaelManaged aes;
    RijndaelManaged aesLocal;
    SHA1CryptoServiceProvider sha;

    public void Initialization()
    {
        sha = new SHA1CryptoServiceProvider();
        sb = new StringBuilder();
        sb2 = new StringBuilder();
        aes = new RijndaelManaged();
        byte[] saltBytes = new byte[] { 48, 15, 55, 68, 33, 83, 20, 36 };
        string randomSeedForValue = "cca254c7ab33ca15";

        {
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(randomSeedForValue + "1c5a82104ccbe117", saltBytes, 1000);
            _keys = key.GetBytes(keySize / 8);
            _iv = key.GetBytes(blockSize / 8);
        }

        aes.KeySize = keySize;
        aes.BlockSize = blockSize;

        aes.Key = _keys;
        aes.IV = _iv;

        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;

        ValueDecryptor.Instance.Initialization(aes);
    }
    public void LocalInitialization()
    {
        aesLocal = new RijndaelManaged();
        byte[] saltBytes = new byte[] { 25, 44, 87, 25, 15, 24, 33, 58 };
        {
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(UnityEngine.Random.Range(100000, 999999).ToString() + "21c851aaac58a5185270a4d2ab", saltBytes, 1000);
            _keysLocal = key.GetBytes(keySize / 8);
            _ivLocal = key.GetBytes(blockSize / 8);
        }

        aesLocal.KeySize = keySize;
        aesLocal.BlockSize = blockSize;

        aesLocal.Key = _keysLocal;
        aesLocal.IV = _ivLocal;

        aesLocal.Mode = CipherMode.CBC;
        aesLocal.Padding = PaddingMode.PKCS7;

        ValueDecryptor.Instance.SetLoaclAES(aesLocal);
    }

    public string EncryptKey(string key)
    {
        return GetHash(key);
    }
    public string EncryptValue(string value)
    {
     
        return GetHash(value) + GetEncrypt(value);
        //40글자 + 암호화값.
    }
    public string Seal(string D5XC8X5A)
    {
        return GetHash(D5XC8X5A) + GetLocalEncrypt(D5XC8X5A);
    }
    /// <summary>
    /// sha 해시 값
    /// </summary>
    public string GetHash(string original)
    {
        //string >> bytes[]
        bytesTemp = sha.ComputeHash(Encoding.UTF8.GetBytes(original));
        //byte[] > string
        return BitConverter.ToString(bytesTemp).Replace(a,b);
    }

    /// <summary>
    /// aes 암호화
    /// </summary>
    public byte[] GetEncrypt(byte[] bytesToBeEncrypted)
    {
        using (ICryptoTransform ct = aes.CreateEncryptor())
        {
            return ct.TransformFinalBlock(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
        }
    }
    public string GetEncrypt(string input)
    {
        bytesTemp = Encoding.UTF8.GetBytes(input);
        bytesTemp = GetEncrypt(bytesTemp);
        return TrasnformToString(bytesTemp);
    }
    /// <summary>
    /// local aes 암호화
    /// </summary>
    private byte[] GetLocalEncrypt(byte[] bytesToBeEncrypted)
    {
        using (ICryptoTransform ct = aesLocal.CreateEncryptor())
        {
            return ct.TransformFinalBlock(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
        }
    }

    private string GetLocalEncrypt(string input)
    {
        bytesTemp = Encoding.UTF8.GetBytes(input);
        bytesTemp = GetLocalEncrypt(bytesTemp);
        return TrasnformToString(bytesTemp);
    }

    string TrasnformToString(byte[] bytes)
    {
        return BitConverter.ToString(bytes).Replace(a, c);
    }   

}
