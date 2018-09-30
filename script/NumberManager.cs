using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;

public class NumberManager : Singleton<NumberManager>
{
    int a = 0;
    public void Initialization()
    {
        a = 1;
    }
    public BigInteger Read()
    {
        return SaveManger.Instance.LoadBig("number");
    }
    public string ReadAsString()
    {
        return SaveManger.Instance.LoadString("number");
    }
    public string ReadWithUnit()
    {
        StringBuilder sb = new StringBuilder(); ;
        string temp = ReadAsString();
        int len = temp.Length;
        if (len < 4)
        {
            sb.Append(temp);
            sb.Append('x');
            sb.Append(0);
            return temp.ToString();
        }
        BigInteger bi = new BigInteger(temp);
        
        len = (len - 1) / 3;
        sb.Append(1);        
        for(int i = 0; i< len -1; i++) sb.Append("000");
        BigInteger temp2 = new BigInteger(sb.ToString());        
        bi = (bi / (temp2));
        sb.Length = 0;
        sb.Append(bi/1000);
        sb.Append('.');
        sb.Append(bi%1000);
        sb.Append('x');
        sb.Append(len);
        return sb.ToString();
    }
    public void Increase( )
    {
        SaveManger.Instance.SaveBiG("number", SaveManger.Instance.LoadBig("number") + 1);
    }
    public void Increase(string value)
    {
        SaveManger.Instance.SaveBiG("number", SaveManger.Instance.LoadBig("number") + new BigInteger(value));
    }
    public void Increase(BigInteger value)
    {
        SaveManger.Instance.SaveBiG("number", SaveManger.Instance.LoadBig("number") + value);
    }
    public void Increase(string value, bool isUnit)
    {
        if (!isUnit) return;
        string[] temp;
        string[] temp2;
        BigInteger bi = SaveManger.Instance.LoadBig("number");
        BigInteger tempBi;
        StringBuilder sb = new StringBuilder();
        //소수점이 잇다면 
        if (value.Length > 5)
        {
            temp = value.Split('.');
            temp2 = temp[1].Split('x');
            sb.Append(temp[0]);
            sb.Append(temp2[0]);
            for(int i = 0; i< int.Parse(temp2[1]); i++) sb.Append("000");
            tempBi = new BigInteger(sb.ToString());
            SaveManger.Instance.SaveBiG("number", bi + tempBi);
            return;
        }
        //소수점이 없다면
        temp = value.Split('x');        
        sb.Append(temp[0]);
        for (int i = 1; i < int.Parse(temp[1]); i++) sb.Append("000");
        tempBi = new BigInteger(sb.ToString());
        SaveManger.Instance.SaveBiG("number", bi + tempBi);
        return;

    }
    public void Increase(string value, int unit)
    {
        BigInteger bi = SaveManger.Instance.LoadBig("number");
        BigInteger tempBi;
        StringBuilder sb = new StringBuilder();
        //소수점이 잇다면 
        if (value.Length > 5)
        {
            string[] temp = value.Split('.');
            sb.Append(temp[0]);
            sb.Append(temp[1]);
            for (int i = 0; i < unit; i++) sb.Append("000");
            tempBi = new BigInteger(sb.ToString());
            SaveManger.Instance.SaveBiG("number", bi + tempBi);
            return;
        }
        //소수점이 없다면
        sb.Append(value);
        for (int i = 1; i < unit; i++) sb.Append("000");
        tempBi = new BigInteger(sb.ToString());
        SaveManger.Instance.SaveBiG("number", bi + tempBi);
        return;
    }
}
