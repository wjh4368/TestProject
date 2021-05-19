using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    public GameObject candle; // 촛불 프리팹
    public GameObject hand;   // 불에 데인 손 이미지
    public GameObject smoke;  // 연기 이미지
    public Transform candleBundle;
    public InputField answerText;
    public Text clearText;
    public Text failedText;

    public List<GameObject> gameObjChild;
    public List<GameObject> candleList;
    public Sprite[] child00Sprites;
    public Sprite[] child01Sprites;
    public Sprite[] child02Sprites;
    

    [SerializeField]
    private Vector3[] candlePosition;
    [SerializeField]
    private int candleCount;
    [SerializeField]
    private int refreshCount;
    // Start is called before the first frame update
    void Start()
    {
        CreateCandle();
    }

    public void ChangeChildSprite(int temp)
    {
        if(temp == 0)  // 기본 아이들 이미지
        {
            for(int i = 0; i < gameObjChild.Count; i++)
            {
                Image changeImg = gameObjChild[i].GetComponent<Image>();
                if (changeImg != null)
                {
                    if (i == 0)
                    {
                        changeImg.sprite = child00Sprites[0];
                    }
                    else if (i == 1)
                    {
                        changeImg.sprite = child01Sprites[0];
                    }
                    else
                    {
                        changeImg.sprite = child02Sprites[0];
                    }
                }
            }
        }
        if(temp == 1) // 손을 데였을 때 이미지
        {
            for (int i = 0; i < gameObjChild.Count; i++)
            {
                Image changeImg = gameObjChild[i].GetComponent<Image>();
                if (changeImg != null)
                {
                    if (i == 0)
                    {
                        changeImg.sprite = child00Sprites[1];
                    }
                    else if (i == 1)
                    {
                        changeImg.sprite = child01Sprites[2];
                    }
                    else
                    {
                        changeImg.sprite = child02Sprites[1];
                    }
                }
            }
        }
        if (temp == 2) // 게임 클리어 이미지
        {
            for (int i = 0; i < gameObjChild.Count; i++)
            {
                Image changeImg = gameObjChild[i].GetComponent<Image>();
                if (changeImg != null)
                {
                    if (i == 0)
                    {
                        changeImg.sprite = child00Sprites[0];
                    }
                    else if (i == 1)
                    {
                        changeImg.sprite = child01Sprites[1];
                    }
                    else
                    {
                        changeImg.sprite = child02Sprites[0];
                    }
                }
            }
        }
    }

    public void CreateCandle()
    {
        candleCount = Random.Range(8, 13);
        int posCount = 0;
        Debug.Log(candleCount);
        for(int i = 0; i < candleCount; i++)
        {
            int candleNum = i+1;
            GameObject obj = Instantiate(candle, candleBundle);
            obj.name = obj.name.Replace("(Clone)", 0+candleNum.ToString());
            candleList.Add(obj); 
            if (posCount < 5)
            {
                obj.transform.localPosition = candlePosition[posCount];
                posCount++;
            }
            else
            {
                posCount = 0;
                obj.transform.localPosition = candlePosition[posCount];
            }

            if (refreshCount > 0) 
            {
                CandleInfo candleinfo = obj.GetComponent<CandleInfo>();
                if (candleinfo != null)
                {
                    candleinfo.enabled = false;
                }
            }
        }
    }

    public void CheckAnswer()
    {
        StartCoroutine("ResultText");
    }
    public void RefreshGame()
    {
        StartCoroutine("DelayReFresh");
    }
    public void ClearCandleList()
    {
        for(int i = 0; i < candleList.Count; i++)
        {
            GameObject tempObj = candleList[i].gameObject;
            Destroy(tempObj);
        }
        candleList.Clear();
    }

    public void EndGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
    Application.Quit();
#endif

    }

    IEnumerator DelayReFresh() // 게임 리셋버튼시 CandleInfo 스크립트에 CheckBreath함수에서 에러가 발생하여 에러 발생방지를 위해 딜레이를 줌.
    {
        refreshCount++;
        answerText.text = null;
        ClearCandleList();
        clearText.gameObject.SetActive(false);
        ChangeChildSprite(0);
        smoke.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        CreateCandle();
        for (int i = 0; i < candleList.Count; i++)
        {
            CandleInfo candleinfo = candleList[i].GetComponent<CandleInfo>();
            if (candleinfo != null)
                candleinfo.enabled = true;
        }
    }

    IEnumerator ResultText()
    {
        int answerNum = int.Parse(answerText.text);
        if (candleList.Count == 0)
        {
            if (answerNum == candleCount)
            {
                clearText.gameObject.SetActive(true);
                failedText.gameObject.SetActive(false);
                ChangeChildSprite(2);
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                clearText.gameObject.SetActive(false);
                failedText.gameObject.SetActive(true);
                yield return new WaitForSeconds(1.0f);
                failedText.gameObject.SetActive(false);
            }
        }
        else
        {
            clearText.gameObject.SetActive(false);
            failedText.gameObject.SetActive(true);
            yield return new WaitForSeconds(1.0f);
            failedText.gameObject.SetActive(false);
        }
    }
}
