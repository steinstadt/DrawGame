using UnityEngine;
using System.Collections;

public class SphereLifeSpan : MonoBehaviour {

    // sphere's life time
    public float lifetime;

	// Use this for initialization
	void Start () {
        Destroy(gameObject, lifetime);
	}
}
