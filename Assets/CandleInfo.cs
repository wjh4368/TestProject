using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CandleInfo : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public MainGame mainGame;
    public Recognizer recognizer;
    public Sprite outCandle;
    public bool isOnFire = true;


    void Start()
    {
        mainGame = FindObjectOfType<MainGame>();
        recognizer = FindObjectOfType<Recognizer>();
    }
    [SerializeField]
    Vector3 currentPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isOnFire == false)
        {
            currentPosition = transform.position;
        }
        else
        {
            currentPosition = transform.position;
            StartCoroutine("ActiveHand");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isOnFire == false)
        {
            transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isOnFire == false)
        {
           // transform.position = currentPosition;
            if(transform.position.y > currentPosition.y)
            {
                mainGame.candleList.Remove(gameObject);
                Destroy(gameObject);
                if(mainGame.candleList.Count == 0)
                {
                    mainGame.smoke.SetActive(false);
                }
            }
        }
    }

    public void CheckBreath()
    {
        int breathValue = recognizer.resultValue;
        
        if(breathValue >= 70)
        {
            Image candleImg = gameObject.GetComponent<Image>();
            if(candleImg != null)
            {
                candleImg.sprite = outCandle;
                isOnFire = false;
                mainGame.smoke.SetActive(true);
                mainGame.ChangeChildSprite(1);
            }
        }
    }


    IEnumerator ActiveHand()
    {
        mainGame.ChangeChildSprite(1);
        mainGame.hand.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        mainGame.ChangeChildSprite(0);
        mainGame.hand.SetActive(false);
    }
}
