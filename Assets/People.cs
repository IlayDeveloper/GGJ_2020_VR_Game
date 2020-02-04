using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public class People : MonoBehaviour
{
    [SerializeField] private NodeOfInterest m_Target;
    [SerializeField] private NavMeshAgent m_Agent;    
    [SerializeField] private bool m_IsAtPlace;
    [SerializeField] private float m_Workprogress = 0f;
    [SerializeField] private float m_MaxProgress = 5f;
    [Range(0,100f)]
    [SerializeField] private float m_ChanceToChangePlace = 50f;

    public NodeOfInterest Target 
    {
        get 
        {
           if (m_Target == null)
            {
                OnWantsToChangePlace?.Invoke(this);
            }
            return m_Target;
        }
        set => m_Target = value; 
    }

    public UnityEngine.Events.UnityEvent OnRun;
    public UnityEngine.Events.UnityEvent OnWork;

    public event System.Action<People> OnWantsToChangePlace; 
    public event System.Action<People> OnLookingForClosestPlace;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Agent == null)
            m_Agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        
            ForcedMovementTargeting(); // kostil
        

        CheckArrival();
        ProcessWork();
    }

    private void ForcedMovementTargeting()
    {
        if (m_Agent.isOnNavMesh) m_Agent.SetDestination(Target.transform.position);
    }

    private void ProcessWork()
    {
        if (m_IsAtPlace)
            if (m_Workprogress >= m_MaxProgress)
            {
                m_IsAtPlace = false;
                m_Workprogress = 0;
                Target.ProcessNode();
                if (UnityEngine.Random.Range(0, 100f) <= m_ChanceToChangePlace)
                {
                    OnWantsToChangePlace?.Invoke(this);
                    OnRun?.Invoke();
                }
            }
            else
            {
                
                m_Workprogress += Time.deltaTime;
            }
    }

    private void CheckArrival()
    {
        if (Vector3.Distance(transform.position, Target.transform.position) < Target.Radius)
        {
            if (m_Agent.isOnNavMesh) m_Agent.SetDestination(transform.position);
            m_IsAtPlace = true;
            OnWork?.Invoke();
        }
    }

    public void ForceTarget(NodeOfInterest target)
    {
        Target = target;
       // m_Agent.SetDestination(Target.transform.position);
    }

    public void ForceTarget()
    {
        OnLookingForClosestPlace?.Invoke(this);
    }
}
