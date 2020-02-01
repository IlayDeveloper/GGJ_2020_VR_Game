using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace FaithRepair.Godpart
{
    public class GodController : MonoBehaviour
    {
        [SerializeField] private float m_RayCastRange;
        private Transform m_LeftFinger;
        private Transform m_RightFiner;

        private void Start()
        {
          
        }

        private void FixedUpdate()
        {
            if (Player.instance.rightHand.AttachedObjects.Count == 0 && Player.instance.rightHand.grabGripAction.state && !Player.instance.rightHand.grabPinchAction.state)
                FindClosers(Player.instance.rightHand);
            
            if (Player.instance.leftHand.AttachedObjects.Count == 0 && Player.instance.leftHand.grabGripAction.state && !Player.instance.leftHand.grabPinchAction.state)
                FindClosers(Player.instance.leftHand);
            
        }

        private void FaithKick()
        {
            
        }

        private void FindClosers(Hand hand)
        {
            Transform rayOut;

            if (hand == Player.instance.rightHand)
            {
                if (m_RightFiner == null)
                    m_RightFiner = GameObject.Find("finger_index_r_end").transform;

                rayOut = m_RightFiner;
            }
            else
            {
                if (m_LeftFinger == null)
                    m_LeftFinger = GameObject.Find("finger_index_l_end").transform;

                rayOut = m_LeftFinger;
            }
            
            RaycastHit hit;
            Ray ray = new Ray(rayOut.position, rayOut.right);
            Physics.Raycast(ray, out hit, m_RayCastRange);
           
            if (hit.collider != null)
            {
                ISlave slave = (ISlave)hit.collider.gameObject.GetComponent<ISlave>();
                if (slave == null)
                    return;
                
                slave.Pray();
                Debug.Log((hit.collider.name));
            }
        }
    }
}