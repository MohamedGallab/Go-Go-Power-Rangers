using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> _soundEffects = new();
    AudioSource _audioSource;
    int _lane = 0;
    public int Lane
    {
        get { return _lane; }
        set
        {
            _lane = value;
            if (_lane > 1) { _lane = 1; }
            if (_lane < -1) { _lane = -1; }
            transform.position = new Vector3(_lane * 3, 0.5f, 0);
        }
    }

    [SerializeField]
    TileSpawner tileSpawner;
    bool _hasShield = false;
    public bool HasShield
    {
        get { return _hasShield; }
        private set
        {
            _hasShield = value;
            transform.GetChild(0).gameObject.SetActive(value);
        }
    }
    public bool HasMultiplier { get; private set; } = false;
    Form _form = Form.White;
    public Form Form
    {
        get { return _form; }
        private set
        {
            switch (value)
            {
                case Form.Red:
                    RedEnergy--;
                    HasShield = false;
                    HasMultiplier = false;
                    GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    break;

                case Form.Green:
                    GreenEnergy--;
                    HasShield = false;
                    GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                    break;

                case Form.Blue:
                    BlueEnergy--;
                    HasMultiplier = false;
                    GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
                    break;

                case Form.White:
                    HasShield = false;
                    HasMultiplier = false;
                    GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    break;
            }
            _form = value;
            _audioSource.PlayOneShot(_soundEffects[1]);
        }
    }
    [SerializeField]
    TMP_Text _scoreText;
    public static int Score { get; private set; } = 0;

    [SerializeField]
    TMP_Text _redEnergyText;
    int _redEnergy = 0;
    public int RedEnergy
    {
        get { return _redEnergy; }
        private set
        {
            _redEnergy = value;
            if (_redEnergy > 5) _redEnergy = 5;
            if (_redEnergy < 0) _redEnergy = 0;
        }
    }

    [SerializeField]
    TMP_Text _greenEnergyText;
    int _greenEnergy = 0;
    public int GreenEnergy
    {
        get { return _greenEnergy; }
        set
        {
            _greenEnergy = value;
            if (_greenEnergy > 5) _greenEnergy = 5;
            if (_greenEnergy < 0) _greenEnergy = 0;
        }
    }

    [SerializeField]
    TMP_Text _blueEnergyText;
    int _blueEnergy = 0;
    public int BlueEnergy
    {
        get { return _blueEnergy; }
        set
        {
            _blueEnergy = value;
            if (_blueEnergy > 5) _blueEnergy = 5;
            if (_blueEnergy < 0) _blueEnergy = 0;
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.mute = Convert.ToBoolean(PlayerPrefs.GetInt("IsMuted"));
        Score = 0;
    }

    void Update()
    {
        if (Game.IsGameOver || Game.IsPaused)
            return;

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            MoveLeft();

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            MoveRight();

        if (Input.GetKeyDown(KeyCode.J))
            Transform(Form.Red);

        if (Input.GetKeyDown(KeyCode.K))
            Transform(Form.Green);

        if (Input.GetKeyDown(KeyCode.L))
            Transform(Form.Blue);

        if (Input.GetKeyDown(KeyCode.Space))
            UsePower();

        if ((Form == Form.Red && RedEnergy == 0) ||
            (Form == Form.Green && GreenEnergy == 0) ||
            (Form == Form.Blue && BlueEnergy == 0))
        {
            Form = Form.White;
        }

        _scoreText.text = $"Score: {Score}";
        _redEnergyText.text = $"Red Energy: {RedEnergy}";
        _greenEnergyText.text = $"Green Energy: {GreenEnergy}";
        _blueEnergyText.text = $"Blue Energy: {BlueEnergy}";
    }

    private void OnTriggerEnter(Collider other)
    {
        Score++;
        switch (other.tag)
        {
            case "Orb/Red":
                if (Form == Form.Red)
                    Score++;
                else
                    RedEnergy++;
                if (HasMultiplier)
                {
                    Score += 4;
                    RedEnergy++;
                }
                _audioSource.PlayOneShot(_soundEffects[0]);
                other.gameObject.SetActive(false);
                break;

            case "Orb/Green":
                if (Form == Form.Green)
                    Score++;
                else
                    GreenEnergy++;
                if (HasMultiplier)
                    Score += 8;
                _audioSource.PlayOneShot(_soundEffects[0]);
                other.gameObject.SetActive(false);
                break;

            case "Orb/Blue":
                if (Form == Form.Blue)
                    Score++;
                else
                    BlueEnergy++;
                if (HasMultiplier)
                {
                    Score += 4;
                    BlueEnergy++;
                }
                _audioSource.PlayOneShot(_soundEffects[0]);
                other.gameObject.SetActive(false);
                break;

            case "Obstacle":
                _audioSource.PlayOneShot(_soundEffects[3]);
                Score--;
                if (HasShield)
                    HasShield = false;
                else if (Form == Form.White)
                {
                    Game.IsGameOver = true;
                }
                else
                    Form = Form.White;
                break;
        }
        HasMultiplier = false;
        tileSpawner.DisableItems();
    }

    void MoveLeft()
    {
        Lane--;
    }

    void MoveRight()
    {
        Lane++;
    }

    public void Transform(Form form)
    {
        switch (form)
        {
            case Form.Red when RedEnergy == 5:
                Form = Form.Red;
                break;

            case Form.Green when GreenEnergy == 5:
                Form = Form.Green;
                break;

            case Form.Blue when BlueEnergy == 5:
                Form = Form.Blue;
                break;

            default:
                _audioSource.PlayOneShot(_soundEffects[4]);
                break;
        }
    }

    public void UsePower()
    {
        switch (Form)
        {
            case Form.Red:
                RedEnergy--;
                tileSpawner.RemoveAllObstacles();
                _audioSource.PlayOneShot(_soundEffects[2]);
                break;

            case Form.Green when !HasMultiplier:
                GreenEnergy--;
                HasMultiplier = true;
                _audioSource.PlayOneShot(_soundEffects[2]);
                break;

            case Form.Blue when !HasShield:
                BlueEnergy--;
                HasShield = true;
                _audioSource.PlayOneShot(_soundEffects[2]);
                break;

            default:
                _audioSource.PlayOneShot(_soundEffects[4]);
                break;
        }
    }

}
