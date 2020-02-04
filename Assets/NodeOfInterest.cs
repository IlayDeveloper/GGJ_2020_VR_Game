using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tag-class
/// </summary>
public class NodeOfInterest : MonoBehaviour
{
    [Range(0,10f)]
    [SerializeField] private float m_Radius;

    public float Radius { get => m_Radius; set => m_Radius = value; }

    public virtual void ProcessNode(){ }

}
