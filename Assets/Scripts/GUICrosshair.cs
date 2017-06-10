using UnityEngine;
using System.Collections;

public class GUICrosshair : MonoBehaviour {
    private Texture2D crosshair, outerCrosshair;
    private Vector2 lastWindowSize;
    private Rect crosshairRect;//, outerCrosshairRect;

	// Use this for initialization
	void Start () {
        crosshair = new Texture2D(2, 2);
        //outerCrosshair = new Texture2D(3, 3);

        //for (int i = 0; i < outerCrosshair.width; i++)
        //    for (int j = 0; j < outerCrosshair.height; j++)
        //        outerCrosshair.SetPixel(i, j, Color.black);
        
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
        //outerCrosshairRect = new Rect((lastWindowSize.x - outerCrosshair.width) / 2.0f, (lastWindowSize.y - outerCrosshair.height) / 2.0f, outerCrosshair.width, outerCrosshair.height);
    }

    void OnGUI() {
        GUI.DrawTexture(crosshairRect, crosshair);
        //GUI.DrawTexture(new Rect(Input.mousePosition, new Vector2(2, 2)), crosshair);
        //GUI.DrawTexture(outerCrosshairRect, outerCrosshair);
    }
}
