using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
/// <summary>
/// 숫자를 올리고 내리고 불러올때 사용한다.
/// </summary>
public class NumberManager : Singleton<NumberManager>
{
    int a = 0;
    public void Initialization()
    {
        a = 1;
    }
    /// <summary>
    /// biginteager 변수로 읽기
    /// </summary>
    public BigInteger Read()
    {
        return SaveManger.Instance.LoadBig("number");
    }

    /// <summary>
    /// string으로 읽기
    /// </summary>
    public string ReadAsString()
    {
        return SaveManger.Instance.LoadString("number");
    }
    /// <summary>
    /// 1000단위를 xn으로 끊어 읽는다.
    /// </summary>
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
        if (value.Contains("."))
        {
            temp = value.Split('.'); // 000 . 000x0
            temp2 = temp[1].Split('x');// 000 x 0
            sb.Append(temp[0]);
            sb.Append(temp2[0]);
            if (temp2[0].Length == 1) sb.Append("00");
            if (temp2[0].Length == 2) sb.Append("0");
            for (int i = 1; i< int.Parse(temp2[1]); i++) sb.Append("000");
            tempBi = new BigInteger(sb.ToString());
            SaveManger.Instance.SaveBiG("number", bi + tempBi);
            return;

        }
        //유닛이 없다면
        if (!value.Contains("x"))
        {
            SaveManger.Instance.SaveBiG("number", bi + new BigInteger(value));
            return;
        }

        //소수점이 없다면
        temp = value.Split('x');        
        sb.Append(temp[0]);
        for (int i = 0; i < int.Parse(temp[1]); i++) sb.Append("000");
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
        if (value.Contains("."))
        {
            string[] temp = value.Split('.');
            sb.Append(temp[0]);
            sb.Append(temp[1]);

            if (temp[1].Length == 1) sb.Append("00");
            if (temp[1].Length == 2) sb.Append("0");

            for (int i = 1; i < unit; i++) sb.Append("000");
            tempBi = new BigInteger(sb.ToString());
            SaveManger.Instance.SaveBiG("number", bi + tempBi);
            return;
        }

        //소수점이 없다면
        sb.Append(value);
        for (int i = 0; i < unit; i++) sb.Append("000");
        tempBi = new BigInteger(sb.ToString());
        SaveManger.Instance.SaveBiG("number", bi + tempBi);
        return;
    }
}
