using UnityEngine;
using System.Collections;


/*
 * This class is responsible for autowalking of player
 */
public class Autowalk : MonoBehaviour
{

	public Camera camera;

	void Update ()
	{
		RaycastHit hit;
		Vector3 forward = Camera.main.transform.forward;
	
		if (Physics.Raycast (transform.position, forward, out hit, Constants.Distance)) { 

			Debug.DrawLine (transform.position, hit.point, Color.blue);

			if (isAllowToMove (hit.transform.gameObject)) {
				if (PositioningPlayerManager.Instance.isMovedFarFromPermissibleDistance (hit.transform.gameObject)) {
					PaintingsAudioController.Instance.stopSelectedAudio();
				}
				if (isAcceptableDistance (hit.transform.gameObject)) {
					float step = Constants.speedMoving * Time.deltaTime;
					Vector3 newPlayerPos = new Vector3 (hit.transform.position.x, transform.position.y, hit.transform.position.z);
					transform.position = Vector3.MoveTowards (transform.position, newPlayerPos, step);

					if (!isAcceptableDistance (hit.transform.gameObject)) {
						PositioningPlayerManager.Instance.positioningPlayerInFrontOfPainting (hit.transform.gameObject);
					}
				} else {
					PositioningPlayerManager.Instance.positioningPlayerInFrontOfPainting (hit.transform.gameObject);
				}
			}
		} 
	}

	/// <summary>
	/// Check is allow to move. Means that player pointed directly forward not floor or ceil
	/// </summary>
	/// <returns><c>true</c>, if allow to move was ised, <c>false</c> otherwise.</returns>
	/// <param name="hittedObject">Hitted object.</param>
	private bool isAllowToMove (GameObject hittedObject)
	{
		if (hittedObject.tag != Constants.Ceiling && hittedObject.tag != Constants.Floor) {
			// If no hit ceiling or floor let's go
			return true;
		}

		return false;
	}

 
	/// <summary>
	/// Check if the distance is acceptable for moving.
	/// </summary>
	/// <returns><c>true</c>, if acceptable distance, <c>false</c> otherwise.</returns>
	/// <param name="hittedObject">Hitted object.</param>
	private bool isAcceptableDistance (GameObject hittedObject)
	{
		float distance = Vector3.Distance (transform.position, hittedObject.transform.position);
		if (distance > Settings.acceptableDistance) {
			return true;
		}
		return false;
	}
}
