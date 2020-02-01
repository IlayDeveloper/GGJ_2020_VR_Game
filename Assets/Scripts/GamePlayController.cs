using System;
using System.Collections.Generic;
using UnityEngine;

namespace FaithRepair.Godpart
{
    public class GamePlayController : MonoBehaviour
    {
        enum Player
        {
            God,
            Ateist
        }
        
        [Header("Percent of fired buildings to win")] [SerializeField]
        private float m_AteistWinPercent;
        //[Header("Percent completion of church building")] [SerializeField]
        //private float m_GodWinPercent;
        
        [SerializeField]private Construction m_Church;
        private Construction[] m_Buildings;
        private List<Construction> m_FiredBuildings;

        private void Start()
        {
            m_Church.OnProcessed += OnChurchCompleted;
            //m_Church = (Temple)GameObject.FindObjectOfType(typeof(Temple));
            Construction[] buildings = (Construction[])GameObject.FindObjectsOfType(typeof(Construction));
            
            List<Construction> listBuildings = new List<Construction>();
            m_FiredBuildings = new List<Construction>();
            
            foreach (var b in buildings)
            {
                listBuildings.Add(b.GetComponent<Construction>());
                b.OnProcessed += OnBuildingFired;
            }

            m_Buildings = listBuildings.ToArray();
        }

        private void CheckAteistWinCondition()
        {
            float percent = m_FiredBuildings.Count / m_Buildings.Length;
            
            if(percent >= m_AteistWinPercent)
                GameEnd(Player.Ateist);
        }

        private void OnBuildingFired(Construction constraction)
        {
            m_FiredBuildings.Add(constraction);
            CheckAteistWinCondition();
        }

        private void OnChurchCompleted(Construction constraction)
        {
            GameEnd(Player.God);
        }

        private void GameEnd(Player winner)
        {
            switch (winner)
            {
                case Player.God:
                    Debug.Log("God win");
                    break;
                case Player.Ateist:
                    Debug.Log("Ateist win");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(winner), winner, null);
            }
        }
    }
}