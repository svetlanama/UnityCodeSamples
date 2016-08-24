using System;
using UnityEngine;
using System.Collections;


public enum MenuSize
{
	Small,
	Wide,
	High
};

/*
 * This class is responsible for positioned,
 * rotation and displaying context menu
 */
[RequireComponent (typeof(Collider))]
public class ArtContextController : MonoBehaviour
{
	public enum Positions
	{
		Top,
		Bottom,
		Left,
		Right
	};

	public Positions PopupPosition;
	public MenuSize ContextMenuSize;
	public float OffsetBotttom = 0;
	public float OffsetTop = 0;
	public float OffsetLeft = 0;
	public float OffsetRight = 0;
	public float RotationAngleX = 0;
	public float RotationAngleY = 0;
	public float RotationAngleZ = 0;
	public float OffsetOverlayX = 0;
	public float OffsetOverlayZ = 0;
	public float OffsetOverlayY = 0;

	private const String CanvasNameSmall = Constants.canvasNameSmall;
	private const String CanvasNameWide = Constants.canvasNameWide;
	private const String CanvasNameHigh = Constants.canvasNameHigh;
	private GameObject _pictureBorder;
	private Bounds _bounds;
	private Bounds _meshBounds;
	private Bounds _colliderBounds;
	private GameObject _menuCanvas;
	private Vector3 _basePosition;
	private Quaternion _baseRotation;
	private float _menuWidth;
	private float _menuHeight;

 
	// Use this for initialization
	void Start ()
	{
		DefineMenuCanvas ();
		if (_menuCanvas == null)
			return;

		_basePosition = _menuCanvas.transform.position;
		_baseRotation = _menuCanvas.transform.rotation;

		RectTransform rt = _menuCanvas.GetComponent<RectTransform> ();
		_menuHeight = rt.rect.height * _menuCanvas.transform.localScale.y;
		_menuWidth = rt.rect.width * _menuCanvas.transform.localScale.x;

		_pictureBorder = transform.GetChild (0).gameObject;
		if (_pictureBorder == null)
			return;

		Renderer planeMesh = _pictureBorder.GetComponent<Renderer> ();
		BoxCollider boxCollider = transform.GetComponent<BoxCollider> ();
		Mesh mesh = _pictureBorder.GetComponent<MeshFilter> ().mesh;

		_bounds = planeMesh.bounds;
		_meshBounds = mesh.bounds;
		_colliderBounds = boxCollider.bounds;
	}

	/// <summary>
	/// Define menu canvas type.
	/// </summary>
	void DefineMenuCanvas ()
	{
		_menuCanvas = GameObject.Find (CanvasNameSmall);
		switch (ContextMenuSize) {

		case MenuSize.Small:
			_menuCanvas = GameObject.Find (CanvasNameSmall);
			break;

		case MenuSize.Wide:
			_menuCanvas = GameObject.Find (CanvasNameWide);
			break;

		case MenuSize.High:
			_menuCanvas = GameObject.Find (CanvasNameHigh);
			break;

		default:
			_menuCanvas = GameObject.Find (CanvasNameSmall);
			break;		
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	/// <summary>
	/// Rotate context menu to the correct direction
	/// </summary>
	private void RotatePopup ()
	{
		Vector3 rotation = _pictureBorder.transform.rotation.eulerAngles;
		rotation = new Vector3 (-rotation.x, rotation.y + 180, -rotation.z);
		_menuCanvas.transform.rotation = Quaternion.Euler (rotation);
	}

	/// <summary>
	/// Show canvas at one of the four sides on the picture
	/// </summary>
	private void ShowPopup ()
	{
		if (!IsAllowShowPopup ()) {
			return;
		}

		RotatePopup ();
		switch (PopupPosition) {
		case Positions.Top:
			Vector3 topPos = _pictureBorder.transform.TransformPoint (
				                            new Vector3 (_meshBounds.center.x, _meshBounds.extents.y, _meshBounds.center.z));
			_menuCanvas.transform.position = topPos - (_menuHeight / 2 - OffsetTop) * -_menuCanvas.transform.up;
			break;

		case Positions.Bottom:

			Vector3 bottomPos = _pictureBorder.transform.TransformPoint (
				                    new Vector3 (_meshBounds.center.x, -_meshBounds.extents.y, _meshBounds.center.z)); 
			_menuCanvas.transform.position = bottomPos - (_menuHeight / 2 - OffsetBotttom) * _menuCanvas.transform.up;
			break;

		case Positions.Left:
			Vector3 leftPos = _pictureBorder.transform.TransformPoint (
				                             new Vector3 (_meshBounds.extents.x, _meshBounds.center.y, _meshBounds.extents.z));
			_menuCanvas.transform.position = leftPos - (_menuWidth / 2 - OffsetLeft) * _menuCanvas.transform.right;
			break;

		case Positions.Right:
			Vector3 rightPos = _pictureBorder.transform.TransformPoint (
				                              new Vector3 (-_meshBounds.extents.x, _meshBounds.center.y, -_meshBounds.extents.z));
			_menuCanvas.transform.position = rightPos - (_menuWidth / 2 - OffsetRight) * -_menuCanvas.transform.right;
			break;
		}

		IndividualRotation ();
		IndividualInFrontOfOffset ();
	}

	/// <summary>
	/// Individual the rotation of the context menu.
	/// </summary>
	private void IndividualRotation ()
	{
		if (RotationAngleY != 0) {
			_menuCanvas.transform.Rotate (Vector3.up, RotationAngleY);
		}
		if (RotationAngleX != 0) {
			_menuCanvas.transform.Rotate (Vector3.left, RotationAngleX);
		}
	}

	/// <summary>
	/// Individuals the in front of offset.
	/// </summary>
	private void IndividualInFrontOfOffset ()
	{

		Vector3 pos = _menuCanvas.transform.position;
		_menuCanvas.transform.position = new Vector3 (pos.x + OffsetOverlayX, pos.y + OffsetOverlayY, pos.z + OffsetOverlayZ);

	}

	/// <summary>
	/// Determines whether this instance is allow show popup.
	/// </summary>
	/// <returns><c>true</c> if this instance is allow show popup; otherwise, <c>false</c>.</returns>
	private bool IsAllowShowPopup ()
	{
		if (Vector3.Distance (transform.position, Camera.main.transform.position) < 20) {
			return true;
		}
		return false;
	}

}
