using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// 유니티의 캔버스안의 UI를 비율을 통하여 크기를 조작 및 배치 하기위 한 클래스입니다.
/// 1.0.0 "타켓 UI를 기반한 회전 기능을 제외한 기능들이 완료되었습니다."
/// 1.1.0 "A : 화면 크기를 통한 리사이즈 및 리포시션 기능을 추가하였습니다."
/// 1.1.0 "B : 에디터를 이용헤 인스펙터를 변형시켰습니다."
/// 참고1 : 에디터 커스터마이즈 스크립트들은 반드시 "Project Folder/Assets/Editor"밑에 있어야 동작  :: ResizerByRectInspector 와 세트이다.
public class ResizerByRect : MonoBehaviour
{
    [Header("== 모드 설정 ==")]
    public bool endOfParent = false;
    public bool isSizeTargetMode = false;
    public bool isPositionTargetMode = false;
    [Header("== 타켓 모드용 기능 ==")]
    public GameObject sizeTarget;
    public GameObject positionTarget;
    public RectTransform finalAncestor;
    public int activeDepth = 0; //small is first, msx is 10
    [Header("== 크기 조절용 ==")]
    public bool isReSize = true;
    [Space(10)]
    public bool isSquare = false; //기준은 가로값
    public bool isHightSquare = false; //기준은 가로값
    public bool staticWidth = false;
    public bool staticHeight = false;
    public float widthRatio = 1f;
    public float heightRatio = 1f;
    [Header("== 위치 조절용 ==")]
    public bool isRePosition = false;
    //stick일 때 사용하는 옵션으로 stick된 위치로 부터 자신의 크기 만큼씩 이동 한다.
    public bool selfSizeWidth = false;
    public bool selfSizeHeight = false;
    [Space(10)]
    public bool stickLeft = false;
    public bool stickRight = false;
    public bool stickTop = false;
    public bool stickBottom = false;
    public float positionXRatio = 0f;
    public float positionYRatio = 0f;
    [Header("== 회전 용 ==")]
    public float rotationZ = 0f;
    float sizeWidth, sizeHeight;
    float positionWidth, positionHeight;
    RectTransform selfRec;
    //다른 씬에서 초기화
    List<RectTransform> lineage;
    void Start()
    {
        if (activeDepth == 0)
        {
            ValueInitialization();

            if (isReSize == true) SizeInitialization();
            if (isRePosition == true) RePosition();
        }
        else
        {
            StartCoroutine("WaitForDepth");
        }
        //Ondi
    }

    void MakeLineage() {
        lineage = new List<RectTransform>();
        lineage.Add(selfRec.parent.GetComponent<RectTransform>());
        for (int i = 0;;i++) {
            if (finalAncestor == lineage[i]) break;
            lineage.Add(lineage[i].parent.GetComponent<RectTransform>());
        }       

    }
    
    void ValueInitialization()
    {
        //nextRotation = new Vector3(0f, 0f, 90f);
        RectTransform objectRectTransform;
        if (isSizeTargetMode)
        {
            objectRectTransform = sizeTarget.GetComponent<RectTransform>();
            sizeWidth = objectRectTransform.rect.width;
            sizeHeight = objectRectTransform.rect.height;
        }
        else
        {
            sizeWidth = Screen.width;
            sizeHeight = Screen.height;
        }

        if (isPositionTargetMode)
        {
            objectRectTransform = positionTarget.GetComponent<RectTransform>();
            positionWidth = objectRectTransform.rect.width;
            positionHeight = objectRectTransform.rect.height;
        }
        else
        {
            positionWidth = sizeWidth;
            positionHeight = sizeHeight;
        }
        selfRec = this.GetComponent<RectTransform>();
        if (endOfParent)
        {
            MakeLineage();
        }
    }
    void SizeInitialization()
    {

        float x = 0f;
        float y = 0f;
        
        x = sizeWidth * widthRatio;
        y = sizeHeight * heightRatio;
        if (staticWidth == true) x = selfRec.rect.width;
        if (staticHeight == true) y = selfRec.rect.height;
        if (isSquare == true) y = x;
        if (isHightSquare == true) x = y;

        selfRec.sizeDelta = new Vector2(x, y);
    }
    public void RePosition()
    {
        float x = 0f, y = 0f;
        //1 기본 위치를 설정한다. 화면의 00% 위치에 넣어진다. 
        x = -0.5f * positionWidth + positionWidth * positionXRatio * 0.01f;
        y = -0.5f * positionHeight + positionHeight * positionYRatio * 0.01f;

        if (selfSizeWidth == false)
        {
            if (stickLeft == true) x = -0.5f * positionWidth + selfRec.rect.width * 0.5f + positionWidth * positionXRatio * 0.01f;
            if (stickRight == true) x = 0.5f * positionWidth - selfRec.rect.width * 0.5f - positionWidth * positionXRatio * 0.01f;
        }
        else if (selfSizeWidth == true)
        {
            if (stickLeft == true) x = -0.5f * positionWidth + selfRec.rect.width * 0.5f + selfRec.rect.width * positionXRatio * 0.01f;
            if (stickRight == true) x = 0.5f * positionWidth - selfRec.rect.width * 0.5f - selfRec.rect.width * positionXRatio * 0.01f;
        }
        if (selfSizeHeight == false)
        {
            if (stickBottom == true) y = -0.5f * positionHeight + selfRec.rect.height * 0.5f + positionHeight * positionYRatio * 0.01f; ;
            if (stickTop == true) y = 0.5f * positionHeight - selfRec.rect.height * 0.5f - positionHeight * positionYRatio * 0.01f; ;
        }
        else if (selfSizeHeight == true)
        {
            if (stickBottom == true) y = -0.5f * positionHeight + selfRec.rect.height * 0.5f + selfRec.rect.height * positionYRatio * 0.01f; ;
            if (stickTop == true) y = 0.5f * positionHeight - selfRec.rect.height * 0.5f - selfRec.rect.height * positionYRatio * 0.01f; ;
        }

        if (endOfParent)
        {
            float lineageX = 0f;
            float lineageY = 0f;
            // 최고 조상을 제외한 모든 조상의 x, y값을 더한다.

            for (int i = 0; i < lineage.Count - 1; i++)
            {
                lineageX = lineageX + lineage[i].anchoredPosition.x;
                lineageY = lineageY + lineage[i].anchoredPosition.y;
            }

            x = x - lineageX;
            y = y - lineageY;
        }

        selfRec.anchoredPosition = new Vector2(x, y);

    }

    IEnumerator WaitForDepth()
    {
        for (int i = 0; i < activeDepth; i++) yield return null;
        ValueInitialization();
        if (isReSize == true) SizeInitialization();
        if (isRePosition == true) RePosition();
    }

}

