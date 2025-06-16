using UnityEngine;
using UnityEngine.AI;

public class MobController : MonoBehaviour
{
    [SerializeField]
    Transform m_Tower;

    NavMeshAgent m_Agent;

    void Start()
    {
        // Set up members
        m_Agent = GetComponent<NavMeshAgent>();
        if (m_Tower == null )
        {
            m_Tower = GameObject.Find("Tower").transform;
        }
        // Set agent destination
        m_Agent.destination = m_Tower.position;
    }

    void Update()
    {
        
    }
}
