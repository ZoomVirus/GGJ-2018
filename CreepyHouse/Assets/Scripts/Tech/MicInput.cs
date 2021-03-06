﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicInput : MonoBehaviour
{

    public static bool On = true;
    public static float MicLoudness;
    public static float MicAverage;
    private static float MicAverageTimer;

    private string _device;

    //mic initialization
    void InitMic()
    {
        if (_device == null) _device = Microphone.devices[0];
        _clipRecord = Microphone.Start(_device, true, 5, 44100);
    }

    void StopMicrophone()
    {
        Microphone.End(_device);
    }


    AudioClip _clipRecord = new AudioClip();
    int _sampleWindow = 64;

    //get data from microphone into audioclip
    float LevelMax()
    {
        float levelMax = 0;
        float[] waveData = new float[_sampleWindow];
        int micPosition = Microphone.GetPosition(null) - (_sampleWindow + 1); // null means the first microphone
        if (micPosition < 0) return 0;
        _clipRecord.GetData(waveData, micPosition);

        // Getting a peak on the last 128 samples
        for (int i = 0; i < _sampleWindow; i++)
        {
            float wavePeak = waveData[i] * waveData[i];
            if (levelMax < wavePeak)
            {
                levelMax = wavePeak;
            }
        }
        return levelMax;
    }

    IEnumerator AverageCalculator()
    {
        while (enabled)
        {
            float currentDecibel = 0;
            float timer = 0.3f;
            while ((timer -= Time.deltaTime) > 0)
            {
                currentDecibel += MicLoudness;

                MicAverage = (MicAverage + (currentDecibel / (0.3f - timer))) /2;

                yield return null;
            }
            MicAverage = currentDecibel / 0.3f;
            yield return null;
        }
    }

    void Update()
    {
        if (!On)
        {
            MicLoudness = 0;
            return;
        }

        //MicAverage = (MicAverage + MicLoudness) / 2;
        //MicAverageTimer += Time.deltaTime;
        //if (MicAverageTimer > 0.2) // Refresh average every 2 seconds
        //{
        //    MicAverage = MicLoudness;
        //    MicAverageTimer = 0;
        //}

        // levelMax equals to the highest normalized value power 2, a small number because < 1
        // pass the value to a static var so we can access it from anywhere
        MicLoudness = LevelMax();
    }

    bool _isInitialized;
    // start mic when scene starts
    void OnEnable()
    {
        StartCoroutine(AverageCalculator());
        InitMic();
        _isInitialized = true;
    }

    //stop mic when loading a new level or quit application
    void OnDisable()
    {
        StopAllCoroutines();
        StopMicrophone();
    }

    void OnDestroy()
    {
        StopMicrophone();
    }


    // make sure the mic gets started & stopped when application gets focused
    void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            //Debug.Log("Focus");

            if (!_isInitialized)
            {
                //Debug.Log("Init Mic");
                InitMic();
                _isInitialized = true;
            }
        }
        if (!focus)
        {
            //Debug.Log("Pause");
            StopMicrophone();
            //Debug.Log("Stop Mic");
            _isInitialized = false;

        }
    }
}
