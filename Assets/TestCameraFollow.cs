using UnityEngine;

public class TestCameraFollow : MonoBehaviour {

    [SerializeField] GameObject player;

	// Update is called once per frame
	void Update () {

        transform.position = player.transform.position + new Vector3(0, 0.5f, -5f);
		
	}
}
