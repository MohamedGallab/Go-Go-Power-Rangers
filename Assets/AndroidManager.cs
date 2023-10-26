using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class AndroidManager : MonoBehaviour
{
    [SerializeField]
    Player _player;
    [SerializeField]
    GameObject _desktopHud;
    [SerializeField]
    GameObject _androidHud;

    [SerializeField]
    TMP_Text _redButtonText;
    [SerializeField]
    TMP_Text _greenButtonText;
    [SerializeField]
    TMP_Text _blueButtonText;
    [SerializeField]
    TMP_Text _scoreText;

    Vector2 _startTouchPosition;
    Vector2 _endTouchPosition;

    private void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            _desktopHud.SetActive(true);
        }
        else
        {
            _androidHud.SetActive(true);
        }
    }
    public void Transform(int form)
    {
        switch (form)
        {
            case 0:
                _player.Transform(Form.Red);
                break;
            case 1:
                _player.Transform(Form.Green);
                break;
            case 2:
                _player.Transform(Form.Blue);
                break;
        }
    }

    void Update()
    {
        if (Game.IsGameOver || Game.IsPaused)
            return;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            _startTouchPosition = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            _endTouchPosition = Input.GetTouch(0).position;

            if (_endTouchPosition.x < _startTouchPosition.x - 150)
            {
                _player.Lane--;
            }
            else if (_startTouchPosition.x + 150 < _endTouchPosition.x)
            {
                _player.Lane++;
            }
        }

        _redButtonText.text = $"{_player.RedEnergy}/5";
        _greenButtonText.text = $"{_player.GreenEnergy}/5";
        _blueButtonText.text = $"{_player.BlueEnergy}/5";
        _scoreText.text = $"Score: {Player.Score}";
    }
}
