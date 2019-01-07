using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class drawPhysicsLine : MonoBehaviour {

    // INPUT : icon position -> (x, y)

    // temporary location
    private Vector3 position;
    // the Icon Object
    public RectTransform icon;

    // gameobject for line-draw
    public GameObject linePrefab;
    // gameobject-Parent
    private GameObject parent;
    // gameobject-Parent List
    private List<GameObject> parentList = new List<GameObject>();

    // line length
    public float lineLength = 0.2f;
    public float lineWidth = 0.1f;

    private Vector3 touchPos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        drawLine();
	}

    void drawLine()
    {

        if (Input.GetMouseButtonDown(0))
        {
            // get the icon coord with Vector3
            position = icon.position;
            // repair z-axis
            position.z = 8f;
            // convert icon coord to world coord
            touchPos = Camera.main.ScreenToWorldPoint(position);
            touchPos.z = 0;
            // generate parentObject
            parent = new GameObject();
            // set the tag-name to the parent
            parent.tag = "Parent";
            // add the parentObject to the List
            parentList.Add(parent);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 startPos = touchPos;
            // get the icon coord with Vector3
            position = icon.position;
            // repair z-axis
            position.z = 8f;
            Vector3 endPos = Camera.main.ScreenToWorldPoint(position);
            
            // if the condition is true, the object is created.
            if ((endPos - startPos).magnitude > lineLength)
            {
                GameObject obj = Instantiate(linePrefab, transform.position, transform.rotation) as GameObject;
                obj.transform.position = (startPos + endPos) / 2;
                obj.transform.right = (endPos - startPos).normalized;

                obj.transform.localScale = new Vector3((endPos - startPos).magnitude * 1.2f, lineWidth, lineWidth);

                obj.transform.parent = this.transform;

                touchPos = endPos;
                // attach the child-obj to parent-obj
                obj.transform.parent = parent.transform;
            }
        }

        // If the mouse cursor is up, finish.
        if (Input.GetMouseButtonUp(0))
        {
            Finish();
        }

        return;
    }

    void Finish()
    {
        // get the list length
        int index = parentList.Count;
        if (index != 0)
        {
            // Access to the parentObj from the List.
            GameObject temporaryParent = parentList[index - 1];
            // check that parent has child
            if (temporaryParent.transform.childCount != 0)
            {
                // add component rigidbody to the parentObj
                Rigidbody parentRigidProp = temporaryParent.AddComponent<Rigidbody>();
                // set the parent axis constraint
                parentRigidProp.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            }
        }
        return;
    }
}
