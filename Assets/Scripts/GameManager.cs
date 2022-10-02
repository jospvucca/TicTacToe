using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _gameManager;

    public GameObject _xPiece;
    public GameObject _oPiece;
    public int _currentPlayer;

    public Text _prompt;

    public Square[] _aSquares;
    private Square[,] _aGrid;

    bool _gameOver;
    private int _moves = 0;

    private List<Square> aWinOportunities;
    private List<Square> aBlockOportunities;

    public float _threshold;
/* 
    private static float _playerFirst;
    private static int _playerFirstInt;
    public static int _gamesPlayed = 0;
    public static int _player1Score = 0;
    public static int _player2Score = 0;
    */

    // Start is called before the first frame update
    void Start()
    {
        if(_gameManager == null)
        {
            _gameManager = this;
        }

        /*if (_gamesPlayed == 0)
        {
            _playerFirst = Random.Range(0.0f, 1.0f);
            Debug.Log("PlayerFirst: " + _playerFirst.ToString());
            if (_playerFirst < 0.5f) 
            {
                _playerFirstInt = 0;
            }
            else
            {
                _playerFirstInt = 1;
            }
            _currentPlayer = _playerFirstInt + 1;
        }
        else
        {
            if (_playerFirstInt == 1)
            {
                _currentPlayer = 1;
                _playerFirstInt = 0;
            }
            else if (_playerFirstInt == 0)
            {
                _currentPlayer = 2;
                _playerFirstInt = 1;
            }
            else
                Debug.Log("Error logic.");
        }*/_currentPlayer = 1;//kom kasnije
        ShowPlayerPrompt();

        _aSquares = FindObjectsOfType(typeof(Square)) as Square[];
        _aGrid = new Square[3, 3];
        Square theSquare;

        for (int i = 0; i < _aSquares.Length; i++) 
        {
            theSquare = _aSquares[i];
            _aGrid[theSquare.r, theSquare.c] = theSquare;
        }

        /*_gamesPlayed++;*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClickSquare(Square other)
    {
        //izmjenjeno u pdfu ovako
        StartCoroutine(PlayGame(other));
       /* if (_gameOver)
            return;

        if(_currentPlayer == 1)
        {
            PlacePiece(_xPiece, other);
        }
        else
        {
            ComputerTakeTurn();
        }*/
    }

    void PlacePiece(GameObject piece, Square other)
    {
        _moves++;
        Instantiate(piece, other.transform.position, Quaternion.identity);
        other._player = _currentPlayer;

        if (CheckForWin(other) && _moves > 3) 
        {
            _gameOver = true;
            StartCoroutine(ShowWinnerPrompt());
            return;
        }
        else if (_moves >= 9) 
        {
            _gameOver = true;
            StartCoroutine(ShowTiePrompt());
            return;
        }

        _currentPlayer++;
        if (_currentPlayer > 2) 
        {
            _currentPlayer = 1;
        }
        ShowPlayerPrompt();
    }

    void ShowPlayerPrompt()
    {
        if(_currentPlayer == 1)
        {
            _prompt.text = "Player 1, place an X";
        }
        else
        {
            _prompt.text = "Player 2, place an O";
        }
    }

    int GetPlayer(int r,int c)
    {
        return _aGrid[r, c]._player;
    }

    bool CheckForWin(Square other)
    {
        bool _notEmpty = false;
        _notEmpty = CheckEmpty(_aGrid[other.r, 0], _aGrid[other.r, 1], _aGrid[other.r, 2]);
        if (
            (GetPlayer(other.r, 0) == GetPlayer(other.r, 2)) &&
            (GetPlayer(other.r, 1) == GetPlayer(other.r, 2)) &&
            _notEmpty == true
            ) 
        {
            
            Debug.Log("Ver1");
            return true;
        }

        _notEmpty = CheckEmpty(_aGrid[0, other.c], _aGrid[1, other.c], _aGrid[other.c, 2]);
        if (
            (GetPlayer(0, other.c) == GetPlayer(2, other.c)) &&
            (GetPlayer(1, other.c) == GetPlayer(2, other.c)) &&
            _notEmpty == true
            )
        {
            Debug.Log("Ver2");
            return true;
        }

        _notEmpty = CheckEmpty(_aGrid[0, 0], _aGrid[1, 1], _aGrid[2, 2]);
        if (
          (GetPlayer(0, 0) == GetPlayer(2, 2)) &&
          (GetPlayer(1, 1) == GetPlayer(2, 2)) &&
          _notEmpty == true
          )
        {
            Debug.Log("Ver3");
            return true;
        }

        _notEmpty = CheckEmpty(_aGrid[0, 2], _aGrid[1, 1], _aGrid[2, 0]);
        if (
          (GetPlayer(0, 2) == GetPlayer(2, 0)) &&
          (GetPlayer(1, 1) == GetPlayer(2, 0)) &&
          _notEmpty == true
          )
        {
            Debug.Log("Ver4");
            return true;
        }


        return false;
    }

    IEnumerator ShowWinnerPrompt()
    { 
        
        if (_currentPlayer == 1)
        {
            /*_player1Score++;
            _prompt.text = "Player 1 wins!\nCurrentScore: " + _player1Score + " : " + _player2Score;*/

        }
        else
        {
           /* _player2Score++;
            _prompt.text = "Player 2 wins!\nCurrentScore: " + _player1Score + " : " + _player2Score;*/
        }

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
        
    }

    IEnumerator ShowTiePrompt()
    {
        _prompt.text = "It's a tie. Neither player wins.";
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(0);
    }

    bool CheckEmpty(Square first, Square second, Square third)
    {
        if (first._isEmpty == false && second._isEmpty == false && third._isEmpty == false)
            return true;
        else
            return false;
    }

    //AI
    void ComputerTakeTurn()
    {
        /* Square theSquare;
         List<Square> _aEmptySquares = new List<Square>();

         for (int i = 0; i < _aSquares.Length; i++) 
         {
             theSquare = _aSquares[i];
             if (theSquare._player == 0 && theSquare._isEmpty == true)
             {
                 _aEmptySquares.Add(theSquare);
             }
         }

         theSquare = _aEmptySquares[Random.Range(0, _aEmptySquares.Count)];
         PlacePiece(_oPiece, theSquare);*/

        Square theSquare = null;

        if (Random.value > _threshold)
        {
            theSquare = WinORBlock();
        }

        if (theSquare == null && Random.value > _threshold)
        {
            theSquare = GetCentre();
        }

        if (theSquare == null && Random.value > _threshold)
        {
            theSquare = GetEmptyCorner();
        }

        if (theSquare == null && Random.value > _threshold)
        {
            theSquare = GetEmptySide();
        }

        if (theSquare == null && Random.value > _threshold)
        {
            theSquare = GetRandomEmptySquare();
        }

        PlacePiece(_oPiece, theSquare);
        /*

        theSquare = WinORBlock();

        if (theSquare == null)
        {
            theSquare = CreateORPreventTrap();
        }

        if(theSquare == null)
        {
            theSquare = GetCentre();
        }

        if(theSquare == null)
        {
            theSquare = GetEmptyCorner();
        }

        if(theSquare == null)
        {
            theSquare = GetEmptySide();
        }

        if(theSquare == null)
        {
            theSquare = GetRandomEmptySquare();
        }

        PlacePiece(_oPiece, theSquare);*/
    }

    IEnumerator PlayGame(Square other)
    {
        if (_gameOver || _currentPlayer == 2)
            yield break;

        PlacePiece(_xPiece, other);

        if(!_gameOver)
        {
            yield return new WaitForSeconds(2);
            ComputerTakeTurn();
        }
    }

    Square GetRandomEmptySquare()
    {
        Square theSquare;
        List<Square> aEmptySquares = new List<Square>();

        for (int i = 0; i < _aSquares.Length; i++)  
        {
            theSquare = _aSquares[i];
            if(theSquare._player == 0)
            {
                aEmptySquares.Add(theSquare);
            }

        }

        theSquare = aEmptySquares[Random.Range(0, aEmptySquares.Count)];

        return theSquare;
    }

    Square GetCentre()
    {
        if (GetPlayer(1, 1) == 0)
        {
            return _aGrid[1, 1];
        }

        return null;
    }

    Square GetEmptyCorner()
    {
        List<Square> aEmptyCorners = new List<Square>();

        if(GetPlayer(0,0) == 0)
        {
            aEmptyCorners.Add(_aGrid[0, 0]);
        }

        if (GetPlayer(0, 2) == 0)
        {
            aEmptyCorners.Add(_aGrid[0, 2]);
        }

        if (GetPlayer(2, 0) == 0)
        {
            aEmptyCorners.Add(_aGrid[2, 0]);
        }

        if (GetPlayer(2, 2) == 0)
        {
            aEmptyCorners.Add(_aGrid[2, 2]);
        }

        if(aEmptyCorners.Count > 0)
        {
            return aEmptyCorners[Random.Range(0, aEmptyCorners.Count)];
        }

        return null;
    }

    Square GetEmptySide()
    {
        List<Square> aEmptySides = new List<Square>();

        if (GetPlayer(0, 1) == 0)   
        {
            aEmptySides.Add(_aGrid[0, 1]);
        }

        if (GetPlayer(1, 0) == 0)
        {
            aEmptySides.Add(_aGrid[1, 0]);
        }

        if (GetPlayer(1, 2) == 0)
        {
            aEmptySides.Add(_aGrid[1, 2]);
        }

        if (GetPlayer(2, 1) == 0)
        {
            aEmptySides.Add(_aGrid[2, 1]);
        }

        if (aEmptySides.Count > 0)
        {
            return aEmptySides[Random.Range(0, aEmptySides.Count)];
        }

        return null;
    }

    void CheckForTwo(Vector2[] coords)
    {
        int p1Count = 0;
        int p2Count = 0;
        Square theSquare = null;
        Vector2 coord;

        for (int i = 0; i < coords.Length; i++)
        {
            coord = coords[i];

            if (GetPlayer((int)coord.x, (int)coord.y) == 1)
            {
                p1Count++;
            }
            else if (GetPlayer((int)coord.x, (int)coord.y) == 2)
            {
                p2Count++;
            }
            else
            {
                theSquare = _aGrid[(int)coord.x, (int)coord.y];
            }
        }

        if (theSquare != null) 
        {
            if (p1Count == 2)
            {
                aBlockOportunities.Add(theSquare);
            }

            if (p2Count == 2) 
            {
                aWinOportunities.Add(theSquare);
            }
        }
    }

    Square WinORBlock()
    {
        aWinOportunities = new List<Square>();
        aBlockOportunities = new List<Square>();

        CheckForTwo(new Vector2[ ] {
            new Vector2 (0 , 0 ) ,
            new Vector2 (0 , 1 ) ,
            new Vector2(0, 2) } ) ;

        CheckForTwo(new Vector2[ ] {
            new Vector2 (1 , 0 ) ,
            new Vector2 (1 , 1 ) ,
            new Vector2(1, 2) } ) ;

        CheckForTwo(new Vector2[ ] {
            new Vector2 (2 , 0 ) ,
            new Vector2 (2 , 1 ) ,
            new Vector2(2, 2) } ) ;

        CheckForTwo(new Vector2[ ] {
            new Vector2 (0 , 0 ) ,
            new Vector2 (1 , 0 ) ,
            new Vector2(2, 0) } ) ;

        CheckForTwo(new Vector2[ ] {
            new Vector2 (0 , 1 ) ,
            new Vector2 (1 , 1 ) ,
            new Vector2(2, 1) } ) ;

        CheckForTwo(new Vector2[ ] {
            new Vector2 (0 , 2 ) ,
            new Vector2 (1 , 2 ) ,
            new Vector2(2, 2) } ) ;

        CheckForTwo(new Vector2[ ] {
            new Vector2 (0 , 0 ) ,
            new Vector2 (1 , 1 ) ,
            new Vector2(2, 2) } ) ;

        CheckForTwo(new Vector2[ ] {
            new Vector2 (0 , 2 ) ,
            new Vector2 (1 , 1 ) ,
            new Vector2(2, 0) } ) ;

        if(aWinOportunities.Count > 0)
        {
            return aWinOportunities[Random.Range(0, aWinOportunities.Count)];
        }

        if (aBlockOportunities.Count > 0)
        {
            return aBlockOportunities[Random.Range(0, aBlockOportunities.Count)];
        }

        return null;
    }

    Square CreateORPreventTrap()
    {
        int p1Corners = 0;
        int p2Corners = 0;

        Square[] Corners = new Square[] { _aGrid[0, 0], _aGrid[0, 2], _aGrid[2, 0], _aGrid[2, 2] };

        foreach(Square S in Corners)
        {
            if(S._player == 1)
            {
                p1Corners++;
            }

            if(S._player == 2)
            {
                p2Corners++;
            }
        }

        if (p1Corners == 2) 
        {
            return GetEmptySide();
        }

        if(p2Corners == 2)
        {
            return GetEmptyCorner();
            //ili centre
        }

        return null;
    }
}
