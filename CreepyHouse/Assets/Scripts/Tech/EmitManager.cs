using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EmitManager : MonoBehaviour
{
    static EmitManager m_Instance;
    public static EmitManager Instance
    {
        get { return m_Instance; }
    }

    void Start()
    {
        m_Instance = this;
        Shader.SetGlobalVectorArray("_EmitLocations", m_EmitLocations);
        Shader.SetGlobalVectorArray("_EmitData", m_EmitData);
        if (m_Auto)
        {
            StartCoroutine(AutoEmit(m_Location1));
            StartCoroutine(AutoEmit(m_Location2));
            StartCoroutine(AutoEmit(m_Location3));
        }

    }

    IEnumerator AutoEmit(Transform location)
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 6f));
            Emit(location.position);
        }
    }

    public Transform m_Location1, m_Location2, m_Location3;
    public bool m_Auto = false;



    Vector4[] m_EmitLocations = new Vector4[100];//reduce number?
    Vector4[] m_EmitData = new Vector4[100];//reduce number//x time, y speed, z falloff, w width
    Vector4[] m_EmitSamples = new Vector4[100];//increase number?
                                               //x: time (could be id in increments of 0.1s from original time?),
                                               //y: posid, 
                                               //z: loudness value (what is the range of this?
                                               //with time and pos as ids they could be compressed into one variable?

    float[] m_CurrentEmitTimeIDS = new float[100];

    //float[] m_EmitTimes = new float[100];
    //float[] m_EmitFallOffs = new float[100];
    //float[] m_EmitSpeeds = new float[100];
    //float[] m_EmitWidths = new float[100];

    // Update is called once per frame
    //Speed
    //falloff distance
    //location


    public void EmitAudioMatching(Vector3 location, float speed, float fallOff, float loudness)//, float width = 5)
    {
        Vector4 location4 = location;
        int DataID = -1;
        float lowestTime = float.MaxValue;
        for (int i = 0; i < m_EmitLocations.Length; i++)
        {
            if (location4 == m_EmitLocations[i])
                DataID = i;
        }
        if (DataID == -1)
        {
            for (int i = 0; i < m_EmitData.Length; i++)
            {
                if (m_EmitData[i].x < lowestTime)
                {
                    lowestTime = m_EmitData[i].x;
                    DataID = i;
                    //could break if 0?
                }
            }
            m_EmitLocations[DataID] = location;
            m_EmitData[DataID].x = Time.timeSinceLevelLoad; //not needed?
            m_EmitData[DataID].y = speed; // if this was to be calulated: speed = distance/time.
            m_EmitData[DataID].z = fallOff;

            m_EmitData[DataID].w = speed * 0.1f;//0.1 is the sample length, Speed * Time is distance //could be calculated in shader?
            Shader.SetGlobalVectorArray("_EmitLocations", m_EmitLocations);
            Shader.SetGlobalVectorArray("_EmitData", m_EmitData);
        }
        lowestTime = float.MaxValue;

        int SampleID = -1;
        for (int i = 0; i < m_EmitSamples.Length; i++)
        {
            if (m_EmitSamples[i].x < lowestTime)
            {
                lowestTime = m_EmitData[i].x;
                SampleID = i;
                //could break if 0?
            }
        }
        m_EmitSamples[SampleID].x = Time.timeSinceLevelLoad;
        m_EmitSamples[SampleID].y = DataID;
        m_EmitSamples[SampleID].z = loudness;
        Shader.SetGlobalVectorArray("_EmitSamples", m_EmitSamples);

        //m_EmitSamples[SampleID].w //w is not currently used for anything
        //fall off is assumed not to change per sound clip so will only change if location changed
    }

    public void Emit(Vector3 location, float speed = 3, float fallOff = 10, float width = 5)
    {
        return;

        if (speed <= 0 || fallOff <= 0 || width <= 0)
        {
            Debug.LogError("Cannot Emit if any of the speed/fallOff/width values are <= 0");

            return;
        }

        float lowestTime = float.MaxValue;
        int id = -1;
        for (int i = 0; i < m_EmitData.Length; i++)
        {
            if (m_EmitData[i].x < lowestTime)
            {
                lowestTime = m_EmitData[i].x;
                id = i;
            }
        }
        m_EmitLocations[id] = location;
        m_EmitData[id].x = Time.timeSinceLevelLoad;
        m_EmitData[id].y = speed;
        m_EmitData[id].z = fallOff;
        m_EmitData[id].w = width;

        //m_EmitTimes[id] = Time.time;
        //m_EmitSpeeds[id] = speed;
        //m_EmitFallOffs[id] = fallOff;
        //m_EmitWidths[id] = width;
        Shader.SetGlobalVectorArray("_EmitLocations", m_EmitLocations);
        Shader.SetGlobalVectorArray("_EmitData", m_EmitData);
        //Shader.SetGlobalFloatArray("_EmitTimes", m_EmitTimes);
        //Shader.SetGlobalFloatArray("_EmitFallOffs", m_EmitFallOffs);
        //Shader.SetGlobalFloatArray("_EmitSpeeds", m_EmitSpeeds);
        //Shader.SetGlobalFloatArray("_EmitWidths", m_EmitWidths);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            Emit(m_Location1.position);
        if (Input.GetKeyDown(KeyCode.Alpha2))
            Emit(m_Location2.position);
        if (Input.GetKeyDown(KeyCode.Alpha3))
            Emit(m_Location3.position);
    }


    private void OnEnable()
    {
        Shader.SetGlobalFloat("_Discard0", 1f);
    }

    private void OnDestroy()
    {
        Shader.SetGlobalFloat("_Discard0", 0f);
        Shader.SetGlobalVectorArray("_EmitLocations", new Vector4[100]);
        Shader.SetGlobalVectorArray("_EmitData", new Vector4[100]);
    }
}

