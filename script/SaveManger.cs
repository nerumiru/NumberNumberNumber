using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class SaveManger : Singleton<SaveManger>
{
    int version = 1;
    bool onLogining = false;
    bool loading = false;
    //playertprefs 저장
    //Windows, PlayerPrefs are stored in the registry under HKCU\Software\[company name]\[product name] key, where company and product names are the names set up in Project Settings.
    //On Android data is stored (persisted) on the device. The data is saved in SharedPreferences. C#/JavaScript, Android Java and Native code can all access the PlayerPrefs data. The PlayerPrefs data is physically stored in /data/data/pkg-name/shared_prefs/pkg-name.xml.

    public void Initialization()
    {
        //PlayerPrefs.DeleteAll();
        FirstSaveChecker();
        SaveVersionChecker();
    }

    void FirstSaveChecker()
    {
        if (1 != PlayerPrefs.GetInt("first", 999))
        {
           
            //퍼스트 세이브 내역
            PlayerPrefs.SetInt("first", 1);
            SaveFloat("lifetime", 0f);
            SaveInt("number", 0);

            //최종체크
            SaveInt("version", version);
            Save();
        }
    }

    void SaveVersionChecker()
    {
        if (version > LoadInt("version"))
        {

            Debug.Log("version" + LoadInt("version"));
            //버전 관리. (버전관리는 옛 버전도 고려해서 한다.)

            //최종
            SaveInt("version", version);
            Save();
            Debug.Log("version2" + LoadInt("version"));
        }
    }

    public void KeyChanger() { }
    public void ValueChanger() { }

    public void Save()
    {
        PlayerPrefs.Save();
    }

    public void SaveBiG(string name, BigInteger value)
    {
        PlayerPrefs.SetString(ValueEncryptor.Instance.EncryptKey(name), ValueEncryptor.Instance.EncryptValue(value.ToString()));
    }
    public void SaveInt(string name, int value)
    {
        PlayerPrefs.SetString(ValueEncryptor.Instance.EncryptKey(name), ValueEncryptor.Instance.EncryptValue(value.ToString()));
    }
    public void SaveFloat(string name, float value)
    {
        PlayerPrefs.SetString(ValueEncryptor.Instance.EncryptKey(name), ValueEncryptor.Instance.EncryptValue(value.ToString()));
    }
    public void SaveStirng(string name, string value)
    {
        PlayerPrefs.SetString(ValueEncryptor.Instance.EncryptKey(name), ValueEncryptor.Instance.EncryptValue(value.ToString()));
    }
    string GetValue(string name)
    {
        name = ValueEncryptor.Instance.EncryptKey(name);
        return ValueDecryptor.Instance.DecryptValue(PlayerPrefs.GetString(name));
    }
    public string LoadString(string name)
    {
        return GetValue(name);
    }
    public BigInteger LoadBig(string name)
    {
        BigInteger bi = new BigInteger(GetValue(name));
        return bi;
    }
    public float LoadFloat(string name)
    {
        float result;
        if (!float.TryParse(GetValue(name), out result))
            result = 0f;
        return result;
    }
    public int LoadInt(string name)
    {
        //gc 36
        int result;
        if (!int.TryParse(GetValue(name), out result))
            result = 0;
        return result;
    }
}
