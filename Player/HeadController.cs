using UnityEngine;
using System.Collections;

/*
 * This class tracks Head movements and react appropriatly 
 */
public class HeadController : MonoBehaviour
{
	private GameObject head;
	private float minShowAngle = 80;
 	private float maxShowAngle = 100;
	private float maxHideAngle = 50;
	private float diffAngle = 60;
	private float oldRotationgAngleY;
	private GameObject menuSpawn;

	private static HeadController _instance;
	public static HeadController Instance { get { return _instance; } }

	void Awake ()
	{
		_instance = this;
		head = gameObject;
		menuSpawn = GameObject.FindGameObjectsWithTag (Constants.spInteriorMenuSpawn) [0] as GameObject;
		oldRotationgAngleY = head.transform.eulerAngles.y;
	}

	// Use this for initialization
	void Start ()
	{
	}
	
	// FixedUpdate is called once per frame
	void FixedUpdate ()
	{
		bool isAngleForShowingFloorMenu = head.transform.eulerAngles.x >= minShowAngle && head.transform.eulerAngles.x <= maxShowAngle;
		bool isAngleForHidingFloorMenu = head.transform.eulerAngles.x <= maxHideAngle;

		if (isAngleForShowingFloorMenu && !InteriorMenuController.Instance.isActiveInteriorMenu ()) {
			InteriorMenuController.Instance.showHideMenu (true, menuSpawn.transform.position);
		} else if (isAngleForHidingFloorMenu && InteriorMenuController.Instance.isActiveInteriorMenu ()) {
			InteriorMenuController.Instance.showHideMenu (false, menuSpawn.transform.position);
		}

		if (InteriorMenuController.Instance.isActiveInteriorMenu () && isRotatingByY ()) {
			InteriorMenuController.Instance.followMenuByHeadRotation (menuSpawn.transform.position);
		}
	}

	/// <summary>
	/// Check if rotating by asix Y.
	/// </summary>
	/// <returns><c>true</c>, if rotating by y was ised, <c>false</c> otherwise.</returns>
	private bool isRotatingByY ()
	{
		float diff = head.transform.eulerAngles.y - oldRotationgAngleY;
		if (diff < 0) {
			diff = diff * (-1);
		}
		if (diff > diffAngle) {
			oldRotationgAngleY = head.transform.eulerAngles.y;
			return true;
		}
		return false;
	}

}
