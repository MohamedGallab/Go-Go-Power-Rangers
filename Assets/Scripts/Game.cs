using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Game : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _gameOverMenu;
    [SerializeField] GameObject _hud;
    [SerializeField] GameObject _androidHud;
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] List<AudioClip> _soundTracks = new();
    AudioSource _audioSource;
    public static bool IsPaused { get; private set; }
    public static bool IsGameOver { get; set; }

    private void Start()
    {
        IsPaused = false;
        IsGameOver = false;
        Time.timeScale = 1;
        _audioSource = GetComponent<AudioSource>();
        _audioSource.mute = Convert.ToBoolean(PlayerPrefs.GetInt("IsMuted"));
        _audioSource.clip = _soundTracks[0];
        _audioSource.Play();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsGameOver)
        {
            if (IsPaused)
                Resume();
            else
                Pause();
        }

        if (IsGameOver)
        {
            Time.timeScale = 0;
            _gameOverMenu.SetActive(true);
            _hud.SetActive(false);
            _androidHud.SetActive(false);
            _scoreText.text = $"Score: {Player.Score}";
            _audioSource.Pause();
            _audioSource.clip = _soundTracks[1];
            _audioSource.Play();
        }
    }
    public void Pause()
    {
        _pauseMenu.SetActive(true);
        Time.timeScale = 0;
        IsPaused = true;
        _audioSource.Pause();
        _audioSource.clip = _soundTracks[1];
        _audioSource.Play();
    }

    public void Resume()
    {
        _pauseMenu.SetActive(false);
        Time.timeScale = 1;
        IsPaused = false;
        _audioSource.Pause();
        _audioSource.clip = _soundTracks[0];
        _audioSource.Play();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
