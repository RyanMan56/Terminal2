using UnityEngine;
using System.Collections;

public class GUICrosshair : MonoBehaviour {
    private Texture2D crosshair, outerCrosshair;
    private Vector2 lastWindowSize;
    private Rect crosshairRect;

	// Use this for initialization
	void Start () {
        crosshair = new Texture2D(2, 2);
        
        lastWindowSize = new Vector2(Screen.width, Screen.height);
        CalculateRect();
	}
	
	// Update is called once per frame
	void Update () {
        if (lastWindowSize.x != Screen.width || Screen.height != Screen.height)
        {
            CalculateRect();
        }
    }

    void CalculateRect() {
        lastWindowSize = new Vector2(Screen.width, Screen.height);
        crosshairRect = new Rect((lastWindowSize.x - crosshair.width) / 2.0f, (lastWindowSize.y - crosshair.height) / 2.0f, crosshair.width, crosshair.height);
    }

    void OnGUI() {
        GUI.DrawTexture(crosshairRect, crosshair);
    }
}
