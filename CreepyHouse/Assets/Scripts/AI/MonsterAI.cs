using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    static float MAXHEALTH = 3;
    static float RECOVERYRATESECOND = 0.1f;

    [SerializeField]
    bool debug = false;

    [SerializeField]
    Renderer m_meshRenderer;

    PlayerSoundObject m_player;

    [SerializeField]
    float m_falloffThreshold = 0.01f;
    [SerializeField]
    float m_closeRangeThreshold = 0.5f;
    [SerializeField]
    float m_proximityVolumeTolerance = 0.05f;
    SoundObject[] m_sounds;
    SoundObject m_seekingSound;
    Vector3 m_seekPosition;
    Vector3 m_nextPosition;
    STATE m_state;
    float m_decisionTimer = 0;
    bool m_repopulate = false;
    bool m_doneAtDestination = false;

    //Scale reference Player Size 0.5f
    [SerializeField] float m_minMoveRange;
    [SerializeField] float m_maxMoveRange;
    float m_playerHealth = MAXHEALTH;
    float m_moveTimer = 0;

    static private MonsterAI m_instance;
    static public MonsterAI Get() { return m_instance; }

    enum STATE
    {
        IDLE,
        HUNTING,
        PROXIMITY,
        NONE // Default, shouldn't hit. Also serves as MAX if needed.
    }

    void Start()
    {
        m_instance = this;
         
        PopulateList();
        m_state = STATE.IDLE;
        //StartCoroutine(PingForTargets());
    }


    // If the monster isn't being looked at, and hasn't moved for a while then it can move again
    bool CanMove()
    {
        if (!m_meshRenderer.isVisible || debug)
        {
            if (m_moveTimer <= 0)
            {
                return true;
            }
        }
        return false;
    }

    // Actually move
    void Move()
    {
        // If there's a sound it's tracking, home in on that!
        if (m_seekingSound != null)
        {
            Vector3 direction = m_seekingSound.transform.position - transform.position;
            float mag = Mathf.Min(direction.magnitude, m_maxMoveRange);

            SetNextDestination(direction.normalized * mag);
            transform.position = m_nextPosition;
        }
        // Otherwise get another destination
        else
        {
            GetNewDestination();
            transform.position = m_nextPosition;
        }

        m_moveTimer = 10f;
        m_playerHealth /= 2f;
        m_doneAtDestination = false;
    }

    void GetNewDestination()
    {
        // If we can seek to a sound, seek to it
        SoundObject newTarget = GetMostProminentSound();
        if (newTarget != null)
        {
            m_seekingSound = newTarget;
            SetDestination(newTarget.transform.position);

            Vector3 direction = m_seekingSound.transform.position - transform.position;
            float mag = Mathf.Min(direction.magnitude, m_maxMoveRange);

            SetNextDestination(direction.normalized * mag);
            transform.position = m_nextPosition;
        }
        else
        {
            SetNextDestination(RandomNavmeshLocation(Random.Range(m_minMoveRange, m_maxMoveRange)));
            transform.position = m_nextPosition;
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
        if(!m_player)
        { 
            m_player = Object.FindObjectOfType<PlayerSoundObject>();
        }
        if (AtDestination() && !m_doneAtDestination)
        {
            Attack();
            SetIdle();
            m_moveTimer = 0;
            if(CanMove())
            {
                Move();
            }
            else
            {
                m_doneAtDestination = true;
            }
        }
        else
        {
            if (CanMove())
            {
                Move();
            }
        }

        if (m_player)
        {
            float loudness = m_player.GetLoudness();
            float distance = (m_player.transform.position - transform.position).magnitude;

            if (loudness < 0.1f)
            {
                m_playerHealth += Time.deltaTime * RECOVERYRATESECOND;
                m_playerHealth = Mathf.Clamp(m_playerHealth, 0, MAXHEALTH);
            }
            else
            {
                float multiplier = Mathf.InverseLerp(0f, 10f, Mathf.Clamp(distance, 0.1f, 10));
                multiplier = Mathf.Lerp(0.75f, 0.1f, multiplier);

                float healthLoss = distance > 0 ? Mathf.Clamp01(loudness / multiplier) : 0;

                // Maximum loss is 1 per second
                m_playerHealth -= healthLoss * Time.deltaTime;
            }

            Debug.Log(m_playerHealth);

            if (m_playerHealth <= 0)
            {
                m_player.Attacked();
            }
        }
        else
        {
            m_player = Object.FindObjectOfType<PlayerSoundObject>();
        }

        m_moveTimer -= Mathf.Max(Time.deltaTime, 0f);

    }

    void OnDrawGizmos()
    {
        if (debug)
        {
            if (m_seekingSound)
            {
                Gizmos.DrawWireSphere(m_seekingSound.transform.position, 0.5f);
            }

            Gizmos.DrawWireCube(transform.position, Vector3.one * 0.5f);
        }
    }

    // Kill, Turn off, Destroy. Do it all!
    void Attack()
    {
        if (m_seekingSound != null)
        {
            //Stub. Add kill things code here. check for player/destructable/destroyed/deactivatable

            // If a thrown item, this should probably still be the thing to turn it off. Just turn off the pickup element beforehand.
            m_seekingSound.Attacked();

            SetIdle();
        }
    }

    void SetIdle()
    {
        m_seekingSound = null;
        m_state = STATE.IDLE;

    }

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.GetComponent<PlayerSoundObject>() != null)
        {
            m_player.Attacked();
        }

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

    public void SoundEmitted(SoundObject sound)
    {
        if (!m_seekingSound)
        {
            m_seekingSound = sound;
        }

        if (sound)
        { 
            bool overwrite = false;

            // If new sound is closer
            float newSoundDist = (sound.transform.position - transform.position).sqrMagnitude;
            float oldSoundDist = (m_seekPosition - transform.position).sqrMagnitude;

            if (newSoundDist < oldSoundDist)
            {
                // Louder
                if (sound.GetVolume() / newSoundDist  >= m_seekingSound.GetVolume() / oldSoundDist)
                {
                    // Lets still give it some random...
                    overwrite = Random.Range(0, 100) > 20;
                }
                else
                {
                    // Very unlikely. randModifier should be in range 0-10 so the bigger the sound gap, 
                    // the less likely the new, quieter sound, will distract...
                    int randModifier = Mathf.CeilToInt((m_seekingSound.GetVolume() - sound.GetVolume()) * 5f);
                    overwrite = Random.Range(0, 100 - randModifier) > 60;
                }
            }
            // Sound is further away. 
            else
            {
                // Louder
                if (sound.GetVolume() / newSoundDist >= m_seekingSound.GetVolume() / oldSoundDist)
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
    }

    bool AtDestination()
    {
        return (transform.position - m_seekPosition).magnitude <= m_minMoveRange + m_closeRangeThreshold;
    }

    void SetDestination(Vector3 point)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, 10, 1))
        {
            m_seekPosition = point;
        }

        Debug.Log("new destination!");
    }

    void SetNextDestination(Vector3 point)
    {
        //Debug.Log("New Target distance" + (point - transform.position).magnitude);

        NavMeshHit hit;
        if (NavMesh.SamplePosition(point, out hit, 10, 1))
        {
            m_nextPosition = point;
        }

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