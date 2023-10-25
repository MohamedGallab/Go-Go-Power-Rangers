using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptiomsMenu : MonoBehaviour
{
    [SerializeField]
    GameObject _muteToggle;
    bool _isMuted;
    public bool IsMuted
    {
        get { return _isMuted; }
        private set
        {
            _muteToggle.GetComponent<Toggle>().isOn = value;
            if (value)
            {
                PlayerPrefs.SetInt("IsMuted", 1);
            }
            else
            {
                PlayerPrefs.SetInt("IsMuted", 0);
            }
        }
    }
    void Start()
    {
        IsMuted = Convert.ToBoolean(PlayerPrefs.GetInt("IsMuted"));
    }

    public void ToggleSound(bool value)
    {
        IsMuted = value;
    }
}
