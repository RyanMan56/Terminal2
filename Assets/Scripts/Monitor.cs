using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour
{
    public int width = 88;
    public int height = 24;
    public Vector2 CurrentPosition;
    public int CurrentPositionInCmd = 0;

    private GameObject _screenTextObj;
    private Text _screenText;
    private GameObject _player;
    private PlayerController _playerController;
    private Collider _screenCollider;
    private ScreenCursor _cursorScript;

    private string _currentCmd = "";    
    private List<List<char>> _textArray, _lastTextArray;

    // Use this for initialization
    void Start()
    {
        _screenTextObj = transform.Find("Object/Screen/Canvas/ScreenText").gameObject;
        _screenText = _screenTextObj.GetComponent<Text>();
        _player = GameObject.Find("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _screenCollider = GameObject.Find("Object/Screen").GetComponent<BoxCollider>();
        _cursorScript = GetComponentInChildren<ScreenCursor>();

        _textArray = new List<List<char>>();
        _lastTextArray = new List<List<char>>();
        CurrentPosition = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        CheckTouch();
        if (!_playerController.canMove)
        {
            TextInput();
        }
        UpdateScreen();
    }

    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && ((e.keyCode == KeyCode.LeftArrow) || e.keyCode == KeyCode.RightArrow))
        {
            ArrowPressed(e.keyCode);
        }
    }

    void CheckTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2((Screen.width - 1) / 2, (Screen.height - 1) / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                if (hit.collider.Equals(_screenCollider)) _playerController.canMove = !_playerController.canMove;
            }
        }
    }

    void TextInput()
    {
        string inputString = Input.inputString;
        if (inputString != "")
        {
            foreach (KeyCode c in inputString)
            {
                _cursorScript.ResetTimer();
                switch (c)
                {
                    case KeyCode.Return:
                        ReturnPressed();
                        break;
                    case KeyCode.Backspace:
                        BackspacePressed();
                        break;
                    default:                        
                        AddTextValue((char)c);
                        break;
                }
            }
        }
    }

    void UpdateScreen()
    {
        if (!CompareLists(_lastTextArray, _textArray))
        {
            _screenText.text = GridToString(_textArray);
            _lastTextArray = _textArray.ConvertAll(a => new List<char>(a));
        }
    }

    string GridToString(List<List<char>> chars2d)
    {
        string text = "";
        foreach (List<char> chars in chars2d)
        {

            text += new string(chars.ToArray());
            if (chars.Count < width) text += '\n';
        }
        return text;
    }

    void AddTextValue(char val)
    {
        Debug.Log("Current: " + _currentCmd + "_");

        if (_textArray.Count.Equals(0))
        {
            _textArray.Add(new List<char>() { val });
            CurrentPosition.x++;            
        }
        else
        {
            _textArray[(int)CurrentPosition.y].Add(val);
            if (_textArray[(int)CurrentPosition.y].Count.Equals(width - 1)) // TODO Needs rewriting for cursor not being at end
            {
                CurrentPosition = new Vector2(0, CurrentPosition.y + 1);
                if (_textArray.Count <= CurrentPosition.y)
                {
                    _textArray.Add(new List<char>());
                }
            }
            else
            {
                CurrentPosition.x++;
            }
        }
        _currentCmd += val;
        CurrentPositionInCmd++;
    }

    void ReturnPressed()
    {
        ExecuteCommand();
        _currentCmd = "";
        CurrentPosition = new Vector2(0, CurrentPosition.y + 1);
        CurrentPositionInCmd = 0;

        _textArray.Add(new List<char>());
    }

    void BackspacePressed()
    {
        Debug.Log("Current: " + _currentCmd);
        if (_currentCmd.Length > 0)
        {
            _currentCmd = _currentCmd.Substring(0, _currentCmd.Length - 1);
            if (CurrentPosition.x > 0)
            {
                _textArray[(int)CurrentPosition.y].RemoveAt((int)CurrentPosition.x - 1);
                CurrentPosition.x--;                
            }
            else
            {
                _textArray.RemoveAt((int)CurrentPosition.y);
                CurrentPosition.y = CurrentPosition.y - 1;
                CurrentPosition.x = _textArray[(int)CurrentPosition.y].Count - 1;
                _textArray[(int)CurrentPosition.y].RemoveAt((int)CurrentPosition.x);
            }
            CurrentPositionInCmd--;            
        }
    }

    void ArrowPressed(KeyCode arrow)
    {
        _cursorScript.ResetTimer();
        if (arrow == KeyCode.RightArrow)
        {
            if (CurrentPositionInCmd < _currentCmd.Length)
            {
                CurrentPositionInCmd++;
                if (CurrentPosition.x < width - 2)
                {
                    CurrentPosition.x++;
                }
                else
                {
                    CurrentPosition.y++;
                    CurrentPosition.x = 0;
                }
            }
        }
        else if (arrow == KeyCode.LeftArrow)
        {
            if (CurrentPositionInCmd > 0)
            {
                CurrentPositionInCmd--;
                if (CurrentPosition.x > 0)
                {
                    CurrentPosition.x--;
                }
                else
                {
                    CurrentPosition.y--;
                    CurrentPosition.x = _textArray[(int)CurrentPosition.y].Count - 1;
                }
            }
        }
        Debug.Log("CurrentPos: " + CurrentPosition + ", Pos: " + CurrentPositionInCmd + ", Length: " + _currentCmd.Length);
    }

    void ExecuteCommand()
    {
        Debug.Log("Returned value: " + _currentCmd);
    }

    bool CompareLists<T>(List<List<T>> A, List<List<T>> B)
    {
        if (A.SequenceEqual(B)) return true; // so it doesn't initially keep checking forever when they're empty
        if (!A.Count.Equals(B.Count)) return false;

        int count = A.Count < B.Count ? B.Count : A.Count;

        for (int i = 0; i < count; i++)
        {
            if (!A[i].SequenceEqual(B[i])) return false;
        }
        return true;
    }
}
