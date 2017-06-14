using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Returned value: " + _currentCmd);
            _currentCmd = "";
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {

        }
        else if (Input.inputString != "")
        {
            Debug.Log(Input.inputString);
            _currentCmd += Input.inputString;
            AddTextValue(Input.inputString[0]);
        }
    }
    
    void UpdateScreen()
    {
        if (!_lastTextArray.Equals(_textArray)) {
            Debug.Log("Helooooooooo");
            _screenText.text = GridToString(_textArray);
            _lastTextArray = _textArray;
        }
    }

    string GridToString(List<List<char>> chars2d)
    {
        string text = "";
        foreach (List<char> chars in chars2d)
        {
            text += chars;
            if (chars.Count < width) text += '\n';
        }
        return text;
    }

    void AddTextValue(char val)
    {
        if (_textArray.Count.Equals(0) || _textArray[_textArray.Count - 1].Count.Equals(width - 1))
        {
            _textArray.Add(new List<char>() { val });
        }
        else
        {
            _textArray[_textArray.Count - 1].Add(val);
        }
    }    
}
