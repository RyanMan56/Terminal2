using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour {
	public int width = 88;
	public int height = 24;
    public Vector2 CurrentPosition;

    private GameObject _screenTextObj;
    private Text _screenText;
    private GameObject _player;
    private PlayerController _playerController;
    private Collider _screenCollider;

    private string _currentCmd = "";
    private List<List<char>> _textArray, _lastTextArray;    

	// Use this for initialization
	void Start () {
        _screenTextObj = transform.Find("Object/Screen/Canvas/ScreenText").gameObject;
        _screenText = _screenTextObj.GetComponent<Text>();
        _player = GameObject.Find("Player");
        _playerController = _player.GetComponent<PlayerController>();
        _screenCollider = GameObject.Find("Object/Screen").GetComponent<BoxCollider>();

        _textArray = new List<List<char>>();
        _lastTextArray = new List<List<char>>();
        CurrentPosition = new Vector2(0, 0);
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(CurrentPosition);
        CheckTouch();
        if (!_playerController.canMove)
        {
            TextInput();
        }
        UpdateScreen();
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
                if (c.Equals(KeyCode.Return))
                {
                    ReturnPressed();
                }
                else if (c.Equals(KeyCode.Backspace))
                {
                    BackspacePressed();
                }
                else
                {
                    _currentCmd += Input.inputString;
                    AddTextValue((char)c);
                }                
            }
        }
    }
    
    void UpdateScreen()
    {
        if (!CompareLists(_lastTextArray, _textArray)) {
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
        Debug.Log("Current: " + _currentCmd);

        if (_textArray.Count.Equals(0))
        {            
            _textArray.Add(new List<char>() { val });
            CurrentPosition.x++;
        }
        else
        {
            _textArray[(int)CurrentPosition.y].Add(val);
            if (_textArray[(int)CurrentPosition.y].Count.Equals(width - 1))
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
    }    

    void ReturnPressed()
    {        
        ExecuteCommand();
        _currentCmd = "";
        CurrentPosition = new Vector2(0, CurrentPosition.y + 1);

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
        }
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
