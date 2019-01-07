using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveIcon : MonoBehaviour {

    // icon speed px/sec
    public float iconSpeed = Screen.width;
    // icon size
    private RectTransform rect;
    // icon offset
    private Vector2 offset;

	// Use this for initialization
	void Start () {
        rect = GetComponent<RectTransform>();
        // set the offset to half of the icon
        offset = new Vector2(rect.sizeDelta.x / 2f, rect.sizeDelta.y / 2f);
	}
	
	// Update is called once per frame
	void Update () {
	    // if no touch the key, no action
        if (Input.GetAxis("Horizontal") == 0f && Input.GetAxis("Vertical") == 0f)
        {
            return;
        }
        // calculate the next position
        var pos = rect.anchoredPosition + new Vector2(Input.GetAxis("Horizontal") * iconSpeed, Input.GetAxis("Vertical") * iconSpeed) * Time.deltaTime;

        // make the icon not to get out of the screen
        pos.x = Mathf.Clamp(pos.x, -Screen.width * 0.5f + offset.x, Screen.width * 0.5f - offset.x);
        pos.y = Mathf.Clamp(pos.y, -Screen.height * 0.5f + offset.y, Screen.height * 0.5f - offset.y);
        // set the icon location
        rect.anchoredPosition = pos;
	}
}
