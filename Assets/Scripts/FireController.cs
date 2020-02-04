using DigitalRuby.PyroParticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    private List<GameObject> m_FireObj;
    [SerializeField] private GameObject m_FirePrefabDoor;
    [SerializeField] private GameObject m_FirePrefabWindow;

    public void SetFireValue(float value)
    {
        int pointer =(int) (m_FireObj.Count * value);

        for(int i = 0; i< m_FireObj.Count; i++)
        {
            if (i < pointer)
            {
                m_FireObj[i].gameObject.SetActive(true);
            }
            else
            {
                m_FireObj[i].gameObject.SetActive(false);
            }
            
        }

    }
    private void Start()
    {
        m_FireObj = new List<GameObject>();
        
        foreach(Transform f in transform)
        {
            if(f.gameObject.name.Contains("Door") )
            {
                m_FireObj.Add(Instantiate(m_FirePrefabDoor, f.position, f.rotation));
                Debug.Log("Instantiate");
            }
            if (f.gameObject.name.Contains("Window"))
            {
                m_FireObj.Add(Instantiate(m_FirePrefabWindow, f.position, f.rotation));
            }
        }

        foreach (var f in m_FireObj)
        {
            f.gameObject.SetActive(true);
        }
    }
}
