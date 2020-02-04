using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScaler : MonoBehaviour
{
    [SerializeField] private float scale = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        SetScale(scale);
    }
    
    public void SetScale(float scale)
    {
        transform.localScale *= scale;  // (0.3 or anything you want value)
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
