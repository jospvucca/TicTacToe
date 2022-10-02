using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    public int r;
    public int c;
    public int _player;
    public bool _isEmpty = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (_player == 0)
        {
            _isEmpty = false;
            GameManager._gameManager.ClickSquare(this);
        }
    }

}
