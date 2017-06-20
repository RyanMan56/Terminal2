using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Monitor : MonoBehaviour {
	public int width = 88;
	public int height = 24;

    private GameObject _screenTextObj;
    private Text _screenText;
    private GameObject _player;
    private PlayerController _playerController;
    private Collider _screenCollider;

    private string _currentCmd;
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
	}
	
	// Update is called once per frame
	void Update () {
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
        if (Input.GetKeyDown(KeyCode.Return))
        {                      
            ReturnPressed();            
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {

        }
        else if (inputString != "")
        {
            Debug.Log(Input.inputString);
            _currentCmd += Input.inputString;

            foreach (char c in inputString)
            {
                AddTextValue(c);
            }
        }
    }
    
    void UpdateScreen()
    {
        if (!CompareLists(_lastTextArray, _textArray)) {
            Debug.Log("Screen refreshed");
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
        //if (!_textArray.Count.Equals(0))
        //{
        //    Debug.Log("COUNT: " + _textArray[_textArray.Count - 1].Count);
        //}
        if (_textArray.Count.Equals(0) || _textArray[_textArray.Count - 1].Count.Equals(width - 1))
        {
            _textArray.Add(new List<char>() { val });
        }
        else
        {
            _textArray[_textArray.Count - 1].Add(val);
        }
    }    

    void ReturnPressed()
    {        
        ExecuteCommand();
        _currentCmd = "";

        _textArray.Add(new List<char>());
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
