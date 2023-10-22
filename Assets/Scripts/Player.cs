using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    int _lane = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _lane--;
            if (_lane < -1) { _lane = 0; }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _lane++;
            if (_lane > 1) { _lane = 1; }
        }

    }
}
