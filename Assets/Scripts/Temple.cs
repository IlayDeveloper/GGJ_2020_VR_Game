using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple : Construction
{
    [SerializeField] GameObject templeStage1;
    [SerializeField] GameObject templeStage2;
    [SerializeField] GameObject templeStage3;
    public event System.Action<Temple> OnTempleBuilded;
    public override void ProcessNode()
    {
        if (!m_IsDestroyed)
        {
            if (m_CurrentFireSpread > 0)
            {
                m_CurrentFireSpread -= m_FireFightingEfficiency;
                return;
            }
            if (m_CurrentDurability < m_MaxDurability)
            {
                m_CurrentDurability += m_RepairPower;
                return;
            }
            else
            {
                OnTempleBuilded?.Invoke(this);
            }

            float persent = m_CurrentDurability / m_MaxDurability;
            if(persent>=0f && persent < 0.33f)
            {
                templeStage2.SetActive(false);
                templeStage1.SetActive(true);
                templeStage3.SetActive(false);

            }
            if(persent>0.33f&& persent < 0.66f)
            {
                templeStage2.SetActive(true);
                templeStage1.SetActive(false);
                templeStage3.SetActive(false);

            }
            if(persent>0.66f&& persent <=1f)
            {
                templeStage2.SetActive(true);
                templeStage1.SetActive(false);
                templeStage3.SetActive(false);

            }

        }
    }
    private void Start()
    {
        templeStage1.SetActive(true);
        templeStage2.SetActive(false);
        templeStage3.SetActive(false);
    }
}
