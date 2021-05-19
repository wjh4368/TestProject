using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recognizer : MonoBehaviour
{
    public MainGame mainGame;
    public AudioClip aud;
    int sampleRate = 44100;
    private float[] samples;
    public float rmsValues;
    public float modulate;
    public int resultValue;
    public int cutValue;

    void Start()
    {
        samples = new float[sampleRate];
        aud = Microphone.Start(Microphone.devices[0].ToString(),true,1,sampleRate);
    }

    // Update is called once per frame
    void Update()
    {
        aud.GetData(samples, 0);  // -1 ~ 1 까지 나온다.
        float sum = 0;
        for (int i = 0; i < samples.Length; i++)
        {
            sum += samples[i] * samples[i];
        }
        rmsValues = Mathf.Sqrt(sum/samples.Length);
        rmsValues = rmsValues * modulate;
        rmsValues = Mathf.Clamp(rmsValues, 0, 100);
        resultValue = Mathf.RoundToInt(rmsValues);
        if(resultValue < cutValue)
        {
            resultValue = 0;
        }

        for(int i = 0; i < mainGame.candleList.Count; i++)
        {
            CandleInfo candle = mainGame.candleList[i].GetComponent<CandleInfo>();
            if(candle != null)
            {
                candle.CheckBreath();
            }
        }
    }
}
