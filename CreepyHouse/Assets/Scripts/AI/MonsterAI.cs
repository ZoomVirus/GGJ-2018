using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MonsterAI : MonoBehaviour
{

    [SerializeField]
    float m_falloffThreshold = 0.01f;
    [SerializeField]
    float m_closeRangeThreshold = 0.1f;
    [SerializeField]
    float m_proximityVolumeTolerance = 0.05f;
    NavMeshAgent m_navAgent;
    SoundObject[] m_sounds;
    SoundObject m_seekingSound;
    Vector3 m_seekPosition;
    STATE m_state;
    float m_decisionTimer;
    bool m_repopulate = false;

    enum STATE
    {
        IDLE,
        HUNTING,
        PROXIMITY,
        NONE // Default, shouldn't hit. Also serves as MAX if needed.
    }

    void Start()
    {
        PopulateList();
        m_navAgent = GetComponent<NavMeshAgent>();
        m_state = STATE.IDLE;
        StartCoroutine(PingForTargets());
    }

    IEnumerator PingForTargets()
    {
        while (enabled)
        {
            if (m_state == STATE.IDLE)
            {
                yield return new WaitForSeconds(0.5f);
                m_seekingSound = GetMostProminentSound();
                if (m_seekingSound != null)
                {
                    SetDestination(m_seekingSound.transform.position);
                }
            }
            else if (m_state == STATE.HUNTING)
            {
                if (m_seekingSound == null)
                {
                    SetIdle();
                    break;
                }

                // We want to emulate a focus, so won't always choose the loudest target if hunting. 
                // We also want to do this quickly, so a comparison of distance and sound level should do the trick...
                SoundObject sound = null;
                if ((sound = GetMostProminentSound(m_seekingSound)) != null)
                {
                    bool overwrite = false;

                    // If new sound is closer
                    if ((sound.transform.position - transform.position).sqrMagnitude < (m_seekPosition - transform.position).sqrMagnitude)
                    {
                        // Louder
                        if (sound.GetVolume() > m_seekingSound.GetVolume())
                        {
                            // Lets still give it some random...
                            overwrite = Random.Range(0, 100) > 20;
                        }
                        else
                        {
                            // Very unlikely. randModifier should be in range 0-10 so the bigger the sound gap, 
                            // the less likely the new, quieter sound, will distract...
                            int randModifier = Mathf.CeilToInt((m_seekingSound.GetVolume() - sound.GetVolume()) * 10f);
                            overwrite = Random.Range(0, 100 - randModifier) > 95;
                        }
                    }
                    // Sound is further away. 
                    else
                    {
                        // Louder
                        if (sound.GetVolume() >= m_seekingSound.GetVolume())
                        {
                            // Less likely than closer sounds, but lets give it some random...
                            overwrite = Random.Range(0, 100) > 60;
                        }
                        // If it's quieter *and* further away then fuck it. 
                    }
                    if (overwrite)
                    {
                        m_seekingSound = sound;
                        m_seekPosition = sound.transform.position;

                        SetDestination(m_seekingSound.transform.position);
                    }
                }


                // We also shouldn't check this as often
                yield return new WaitForSeconds(2f);

            }

            yield return null;
        }
    }

    void PopulateList()
    {
        //Get every single sound producer in the scene.
        m_sounds = Object.FindObjectsOfType(typeof(SoundObject)) as SoundObject[];
    }

    // Update is called once per frame
    void Update()
    {

        switch (m_state)
        {
            case STATE.IDLE:
                {
                    GetComponent<MeshRenderer>().material.color = Color.blue;
                    UpdateIdle();
                    break;
                }
            case STATE.HUNTING:
                {
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    UpdateHunting();
                    break;
                }
            case STATE.PROXIMITY:
                {
                    GetComponent<MeshRenderer>().material.color = Color.green;
                    UpdateProximity();
                    break;
                }
        }
    }

    void UpdateIdle()
    {

        if (m_seekingSound)
        {
            m_state = STATE.HUNTING;
        }

        else
        {
            if (AtDestination())
            {
                SetDestination(RandomNavmeshLocation(10f));
            }

        }

        //TODO Idle wanderings
    }

    void UpdateHunting()
    {
        if (m_seekingSound != null)
        {
            if (m_seekPosition != m_navAgent.destination)
            {
                SetDestination(m_seekPosition);
            }
            else if (AtDestination())
            {
                m_state = STATE.PROXIMITY;
                m_decisionTimer = Random.Range(0.8f, 2.3f);
            }
        }
        else
        {
            SetIdle();
        }
    }

    void UpdateProximity()
    {
        if (m_decisionTimer < 0)
        {
            //Now we check if the actual object is still around (i.e. the player may haver move away or an object may have been thrown and is now destroyed)    
            if (m_seekingSound != null)
            {
                if (AtDestination())
                {
                    //Dirty way to tell if player.
                    if (m_seekingSound.GetComponent<PlayerSoundObject>())
                    {
                        if (Random.Range(0f, 1f) + Random.Range(0f, 1f) > 1.5f)
                        {
                            Attack();
                        }
                        else
                        {
                            SetIdle();
                        }
                    }
                    else
                    {
                        Attack();
                    }
                }
            }
            else
            {
                SetIdle();
            }
        }
        else if (m_seekingSound != null)
        {
            if (m_seekingSound.GetVolume() > m_proximityVolumeTolerance)
            {
                Attack();
            }
        }
        m_decisionTimer -= Time.deltaTime;
    }

    // Kill, Turn off, Destroy. Do it all!
    void Attack()
    {
        //Stub. Add kill things code here. check for player/destructable/destroyed/deactivatable

        // If a thrown item, this should probably still be the thing to turn it off. Just turn off the pickup element beforehand.
        m_seekingSound.Attacked();

        SetIdle();
    }

    void SetIdle()
    {
        m_seekingSound = null;
        m_state = STATE.IDLE;

    }

    SoundObject GetMostProminentSound(SoundObject exclude = null)
    {
        if (m_repopulate)
        {
            // Shitty GC to keep list free of nulls.
            PopulateList();
            m_repopulate = false;
        }

        SoundObject returnObj = null;
        float loudestSound = 0.0f;

        if (m_sounds != null)
        {
            foreach (SoundObject soundObject in m_sounds)
            {
                if (soundObject != null && soundObject != exclude)
                {
                    float emitterVolume = soundObject.GetVolume();

                    if (emitterVolume > 0.01f)
                    {
                        Vector3 position = soundObject.transform.position;
                        float distance = (position - transform.position).magnitude;

                        //We don't care about this number, it's just a measurement against other sounds
                        float falloff = emitterVolume / distance;

                        if (loudestSound < falloff && falloff > m_falloffThreshold)
                        {
                            loudestSound = falloff;
                            returnObj = soundObject;
                        }
                    }
                }
                else
                {
                    m_repopulate = true;
                }
            }
        }
        else
        {
            m_repopulate = true;
        }

        return returnObj;
    }

    bool AtDestination()
    {
        return (transform.position - m_navAgent.destination).magnitude <= m_closeRangeThreshold;
    }

    void SetDestination(Vector3 point)
    {
        m_navAgent.destination = point;
        m_seekPosition = m_navAgent.destination;

        Debug.Log("new destination!");
    }

    public Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}