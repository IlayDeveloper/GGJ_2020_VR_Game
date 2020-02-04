using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace FaithRepair.Godpart
{
    public class GodController : MonoBehaviour
    {
        [SerializeField] private float m_RayCastRange;
        [SerializeField] private TextMeshPro m_Text;
        private Transform m_LeftFinger;
        private Transform m_RightFiner;

        private void Start()
        {
            ReSpawn();
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

        public void GameOver()
        {
            m_Text.gameObject.SetActive(true);
            m_Text.SetText("You lose!");
            StartCoroutine(WaitSpawn(10));
        }

        public void GameWin()
        {
            m_Text.gameObject.SetActive(true);
            m_Text.SetText("You Win!");
            StartCoroutine(WaitSpawn(10));
        }

        public void ReSpawn()
        {
            //m_Text.gameObject.SetActive(false);
            StartCoroutine(WaitSpawn());
        }
        
        IEnumerator WaitSpawn(float spawnTime = 5)
        {
            float timer = Time.time;
            m_Text.gameObject.SetActive(true);
            while ( (Time.time - timer) < spawnTime)
            {
                m_Text.SetText("Game start in " + (spawnTime - (int)(Time.time - timer)).ToString());
                yield return null;
            }
            
            m_Text.gameObject.SetActive(false);
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