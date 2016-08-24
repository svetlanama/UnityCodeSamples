using UnityEngine;
using System.Collections;

/*
* This class is responsible for auto-positioning and rotationg player
* directly infront of the GameObject, which he is going
*/
public class PositioningPlayerManager : MonoBehaviour {

	private GvrHead head; 
	private GameObject player;
	private float permissibleValue = 1.0f;
	private GameObject targetToPositioned;

	private static PositioningPlayerManager _instance;
	public static PositioningPlayerManager Instance { get { return _instance; } }

	void Awake() {
		_instance = this;
		player = GameObject.FindGameObjectsWithTag (Constants.goPlayer) [0] as GameObject;
		head = Camera.main.GetComponent<StereoController>().Head;
	}

	/// <summary>
	/// Positioning player in front of the gameObject.
	/// </summary>
	/// <param name="hitObject">Hit object.</param>
	public void positioningPlayerInFrontOfPainting (GameObject hitObject)
	{
		if (hitObject != null && hitObject.tag == Constants.goArt) {

			        if (isPermissibleDistance(hitObject.transform.gameObject) && isSameTarget(hitObject)) {
					// Already positioned positioning
					return;
			        }

				GameObject _pictureBorder = hitObject.transform.GetChild (0).gameObject;
				if (_pictureBorder == null)
					return;

				Mesh mesh = _pictureBorder.GetComponent<MeshFilter> ().mesh;
				Bounds _meshBounds = mesh.bounds;

				Vector3 hitObjectPos = _pictureBorder.transform.position;
				Vector3 hitObjectDirection = _pictureBorder.transform.forward;
				float spawnDistance = Settings.acceptableDistance;

				// Move player in front of object
				Vector3 spawnPos = hitObjectPos + hitObjectDirection * spawnDistance;
				spawnPos = new Vector3 (spawnPos.x, transform.position.y, spawnPos.z); 
				player.transform.position = spawnPos;

				// Make precise rotation, such as player should look at the object directly
				var headRotation = GvrViewer.Instance.HeadPose.Orientation;
 				Quaternion lookAt = Quaternion.LookRotation (hitObject.transform.position - transform.position);

				Vector3 rot = new Vector3 (0, lookAt.eulerAngles.y - headRotation.eulerAngles.y, 0);
 			        player.transform.rotation = Quaternion.Euler (rot);

			        // Set up current selected object
			        targetToPositioned = hitObject;
		} 
	}

	/// <summary>
	/// Ises the permissible distance not to posotioning player in front of painting to avoid to often player positioning
	/// </summary>
	/// <returns><c>true</c>, if permissible distance was ised, <c>false</c> otherwise.</returns>
	/// <param name="hittedObject">Hitted object.</param>
	private bool isPermissibleDistance (GameObject hittedObject)
	{
		float distance = Vector3.Distance (player.transform.position, hittedObject.transform.position);
		if (distance > Settings.acceptableDistance - permissibleValue && distance < Settings.acceptableDistance + permissibleValue ) {
			return true;
		}
		return false;
	}

        /// <summary>
        /// Check if played hit tha same target as it was before
        /// <summary>
        /// <returns><c>true</c>, if yes, <c>false</c> otherwise.</returns>
	private bool isHitSameTarget(GameObject hittedArt)
	{
		if (targetToPositioned == null) {
			return false;
		}

		if (hittedArt == targetToPositioned) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// Check If player moved far from permissible distance. 
	/// In other words how far does player move from gameObject
	/// to reset isPlayerPositioned to false
	/// </summary>
	/// <returns><c>true</c>, if moved far from permissible distance was ised, <c>false</c> otherwise.</returns>
	/// <param name="hittedObject">Hitted object.</param>
	public bool isMovedFarFromPermissibleDistance (GameObject hittedObject)
	{
		float distance = Vector3.Distance (player.transform.position, hittedObject.transform.position);
		if (distance > Settings.acceptableDistance + permissibleValue) {
			return true;
		}
		return false;
	}
}
