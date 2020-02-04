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

        public enum Station
        {
            GodWin,
            AteistWin,
            SlowGame,
            ActiveGame,
        }

        public System.Action<Station> stateChanged;
        private Station m_Station;
        public Station station 
        { 
            get => m_Station;
            private set
            {
                if(m_Station == value) return;

                switch (value)
                {
                    case Station.GodWin:
                        SoundController.Instance.Play(SoundController.SoundType.GodWin);
                        break;
                    case Station.AteistWin:
                        SoundController.Instance.Play(SoundController.SoundType.AteistWin);
                        break;
                    case Station.SlowGame:
                        SoundController.Instance.Play(SoundController.SoundType.MainTheme);
                        break;
                    case Station.ActiveGame:
                        SoundController.Instance.Play(SoundController.SoundType.ActiveTheme);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
                
                stateChanged?.Invoke(value);
                m_Station = value;
            }
        }
        
        public static GamePlayController Instance { get; private set; }
        [Header("Percent of fired buildings to win")] [SerializeField]
        private float m_AteistWinPercent;

        [SerializeField] [Range(0, 1)] private float m_ActiveGamePercent;
        //[Header("Percent completion of church building")] [SerializeField]
        //private float m_GodWinPercent;
        
        [SerializeField]private Construction m_Church;
        private Construction[] m_Buildings;
        private List<Construction> m_FiredBuildings;
        [SerializeField] private PlayerController m_Ateist;
        [SerializeField] private GodController m_GodPlayer;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

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
            
            //Debug.Log(m_Buildings.Length);
            station = Station.SlowGame;
        }

        private void CheckAteistWinCondition()
        {
            float percent = m_FiredBuildings.Count / m_Buildings.Length;
            Debug.LogError(percent);
            if(percent >= m_AteistWinPercent)
                GameEnd(Player.Ateist);
            else if (percent >= m_ActiveGamePercent && station != Station.ActiveGame)
            {
                station = Station.ActiveGame;
            }
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
                    station = Station.GodWin;
                    break;
                case Player.Ateist:
                    Debug.Log("Ateist win");
                    station = Station.AteistWin;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(winner), winner, null);
            }
        }
    }
}