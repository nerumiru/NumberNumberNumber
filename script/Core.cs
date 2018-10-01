using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Numerics;
public class Core : MonoBehaviour {
    private const string SNscore = "number";
    public Text txt;
    float timeChecker;
    bool running = true;
    StringBuilder sb;
    BigInteger a;

    private void Awake()
    {
        SingletonInitialization();
    }

    private void SingletonInitialization()
    {
        //암호화 초기화시 복호화도 같이 초기화 된다.
        ValueEncryptor.Instance.Initialization();
        ValueEncryptor.Instance.LocalInitialization();
        SaveManger.Instance.Initialization();
    }
    // Use this for initialization
    void Start ()
    {
        
        sb = new StringBuilder();
        StartCoroutine(SaveCoroutine());
        StartCoroutine(ScoreCoroutine());


    }
	
	// Update is called once per frame
	void Update ()
    {
        Debug.Log("score :  " + NumberManager.Instance.Read() + "\n" + NumberManager.Instance.ReadWithUnit());
        txt.text = NumberManager.Instance.Read() + "점" ;
    }
    //5.1초마다 플레이 타임을 기록과 저장을 하는 코루틴
    IEnumerator SaveCoroutine()
    {
        while (running)
        {
            yield return new WaitForSeconds(5.1f);
            SaveManger.Instance.SaveFloat("lifetime", SaveManger.Instance.LoadFloat("lifetime") + 5.1f);
            SaveManger.Instance.Save();
        }
    }

    //1초마다 숫자를 1씩올리는 코루틴
    IEnumerator ScoreCoroutine()
    {
        while (running)
        {
            yield return new WaitForSeconds(1.0f);
            NumberManager.Instance.Increase(); //점수 저장은 따로 만든다.
        }
        
    }
}
