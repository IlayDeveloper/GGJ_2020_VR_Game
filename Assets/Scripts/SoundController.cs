using System;
using UnityEngine;

namespace FaithRepair.Godpart
{
    public class SoundController : MonoBehaviour
    {
        public enum SoundType
        {
            MainTheme,
            ActiveTheme,
            Menu,
            GodWin,
            AteistWin
        }
        
        public static SoundController Instance { get; private set; }
        [SerializeField] private AudioSource m_AudioSource;
        [SerializeField] private AudioClip m_MainTheme;
        [SerializeField] private AudioClip m_ActiveTheme;
        [SerializeField] private AudioClip m_Menu;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(this);
            }
            
        }

        private void Start()
        {
            Play(SoundType.MainTheme);
        }

        public void Play(SoundType type)
        {
            m_AudioSource.Stop();
            
            switch (type)
            {
                case SoundType.MainTheme:
                    m_AudioSource.clip = m_MainTheme;
                    break;
                case SoundType.ActiveTheme:
                    m_AudioSource.clip = m_MainTheme;
                    break;
                case SoundType.Menu:
                    m_AudioSource.clip = m_Menu;
                    break;
                //default:
                    //throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            
            m_AudioSource.Play();
        }
    }
}