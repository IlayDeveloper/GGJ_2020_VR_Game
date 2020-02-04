using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : NodeOfInterest
{
    [SerializeField] protected float radius=0.25f;
    [SerializeField] protected float m_MaxDurability=100.0f;
    [SerializeField]protected float m_CurrentDurability;
    [SerializeField] protected float m_MaxFireSpread=10.0f;
    protected float m_CurrentFireSpread=0f;
    [SerializeField] protected float m_RepairPower=1.0f;
    [SerializeField] protected float m_FireFightingEfficiency = 0.2f;
    [SerializeField] protected float m_FireRaisingSpeed = 1.0f;
    [SerializeField] protected GameObject m_FirePrefab;
    protected bool m_IsDestroyed = false;
    protected FireController fireController;
    public event Action<Construction> OnProcessed;
    public virtual float Break()
    {
        if (m_CurrentFireSpread < m_MaxFireSpread)
        {
            m_CurrentFireSpread += m_FireRaisingSpeed * Time.deltaTime;

            
            return m_CurrentFireSpread / m_MaxFireSpread;
            
        }
        else
        {
           
            return 1f;
        }
        
    }

    public override void ProcessNode()
    {
        base.ProcessNode();
        if (!m_IsDestroyed)
        {
            if (m_CurrentFireSpread > 0 )
            {
                m_CurrentFireSpread -= m_FireFightingEfficiency;
                return;
            }
            if(m_CurrentDurability< m_MaxDurability)
            {
                m_CurrentDurability += m_RepairPower;
                return;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, radius);
    }

    protected void Start()
    {

        m_CurrentDurability = m_MaxDurability;
        Radius = radius;
        fireController = GetComponent<FireController>();

    }
    
    protected void Update()
    {

        if (fireController != null)
        {
            fireController.SetFireValue(m_CurrentFireSpread / m_MaxFireSpread);
        }
        if (!m_IsDestroyed)
        {
            if (m_CurrentFireSpread > 0)
            {
                m_CurrentDurability -= m_CurrentFireSpread * Time.deltaTime;
            }
            
            if (m_CurrentDurability <= 0)
            {
                m_IsDestroyed = true;
                OnProcessed?.Invoke(this);
            }
        }
       
    }
}
