using UnityEngine;
using System.Collections;

/*
 * Look at me script
 */
public class LookAtMe: MonoBehaviour {

	// Update is called once per frame
	void Update () {
		 transform.LookAt(Camera.main.transform);
		 transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
	     transform.rotation = Quaternion.Euler(90.0f,  transform.rotation.eulerAngles.y , transform.rotation.eulerAngles.z);
        }
}
