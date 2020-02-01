using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Construction : MonoBehaviour
{
   
    [SerializeField] private float m_Durability=10.0f;
    private float m_CurrentDurability;
    [SerializeField] private float m_DurabilityConst;
    private BoxCollider m_Collider;
    public event Action<Construction> OnProcessed;
    public virtual float Break()
    {
        if (m_CurrentDurability > 0)
        {
            m_CurrentDurability -= Time.deltaTime;
   
            return (m_CurrentDurability / m_Durability);
            
            

        }
        else
        {

            OnProcessed?.Invoke(this);
           // Debug.Log("Finish");
            return 0f;
        }
        
    }
    
    public virtual void Destroy()
    {
        
    }

    public float ProcessValue()
    {

        float value;

        value = m_CurrentDurability / m_Durability *100.0f;
        return 100.0f-value;
    }

    private void Start()
    {
        //m_Collider = GetComponent<BoxCollider>();
        m_CurrentDurability = m_Durability;

    }
    private void Update()
    {
        
    }
}
