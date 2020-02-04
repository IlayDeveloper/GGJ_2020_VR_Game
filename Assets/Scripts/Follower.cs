using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace FaithRepair.Godpart
{
    public class Follower : MonoBehaviour
    {
        [SerializeField] private Transform m_Targert;
        [SerializeField] private Vector3 m_Offset;

        private void Update() 
        {
            Vector3 dir = Vector3.ProjectOnPlane(m_Targert.forward, Vector3.up);
            transform.rotation = Quaternion.LookRotation(dir);
            //transform.position = new Vector3( transform.position.x, );
        }
    }
}