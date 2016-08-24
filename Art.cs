using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * This class contains all necessary properties and functions 
 * to build and display gameObject of the Art 
 */
public class Art : MonoBehaviour
{

	public GameObject pictureObject;
	public int id = 0;
	public string title;
	public string artist;
	public string artist2;
	public string year;
	public string medium;
	public string description;
	public string file;
	public string audioFile;
	public string hallName;
	private ArtContextController artContextController;

	void Awake ()
	{
		artContextController = this.GetComponent<ArtContextController> (); 
	}

	/// <summary>
	/// Gets the art.
	/// Use memoization not to find the Art every time if we already get it.
	/// <returns>The art.</returns>
	Dictionary<int, Art> _art = new Dictionary<int, Art>();
	public Art getArt() {

		Art lookup;
		if (_art.TryGetValue (id, out lookup)) {
			return lookup;
		}
			
		lookup = DataManager.Instance.GetArtById (this.id) as Art;
		_art[id] = lookup;
		return lookup;
	}

	/// <summary>
	/// Gets the size of the picture avg. 
	/// Use memoization not to calculate the size of Art every time if we already have this.
	/// </summary>
	/// <returns>The picture average size.</returns>
	Dictionary<int, float> _size = new Dictionary<int, float>();
	public float getPictureAvgSize()
	{
		float lookup;
		if (_size.TryGetValue (id, out lookup)) {
			return lookup;
		}

		GameObject pictureBorder = this.transform.GetChild(0).gameObject;
		if (pictureBorder == null) return -1;

		Renderer planeMesh = pictureBorder.GetComponent<Renderer>();
		float avg = (planeMesh.transform.localScale.x + planeMesh.transform.localScale.y) / 2;
		lookup = avg;

		_size[id] = lookup;
		return lookup;
	}

	/// <summary>
	/// Track if gaze pointed/removed on/from Art.
	/// </summary>
	/// <param name="gazedAt">If set to <c>true</c> gazed at.</param>
	public void SetGazedAt (bool gazedAt)
	{
		if (gazedAt == true) {
	         
			Art jsonSelectedArt = getArt ();
			switch (artContextController.ContextMenuSize) {

			case MenuSize.Wide:
				WideArtContextDataController.Instance.DisplayContextText (jsonSelectedArt);
				break;
					
			case MenuSize.High:
				HighArtContextDataController.Instance.DisplayContextText (jsonSelectedArt);
				break;

			case MenuSize.Small:
				SmallArtContextDataController.Instance.DisplayContextText (jsonSelectedArt);
				break;

			default:
				SmallArtContextDataController.Instance.DisplayContextText (jsonSelectedArt);
				break;
			}
		} 
	}

	/// <summary>
	/// Download the image of Art form local storage and apply as a new Texture.
	/// </summary>
	/// <param name="art">Art.</param>
	public void DownloadFormLocalStorage (Art art)
	{
		string extension = System.IO.Path.GetExtension (art.file);
		string result = art.file.Substring (0, art.file.Length - extension.Length);
		string artPath = "Arts/" + art.hallName + "/" + result;
		Texture texture = Resources.Load (artPath) as Texture;
		pictureObject.GetComponent<MeshRenderer> ().material.mainTexture = texture; 
	}

	/// <summary>
	///  Download the image of Art form server and apply as a new Texture.
	/// </summary>
	/// <returns>The image.</returns>
	/// <param name="art">Art.</param>
	public IEnumerator DownloadImage (Art art)
	{
		string artPath = "www.sample.com";
		WWW www = new WWW (artPath);
		yield return www;

		if (!string.IsNullOrEmpty (www.error)) {
			www.Dispose ();
			www = null;
			DownloadFormLocalStorage (art);

		} else if (www.isDone) {
			pictureObject.GetComponent<MeshRenderer> ().material.mainTexture = www.texture;
			www.Dispose ();
			www = null;
		} else {
			www.Dispose ();
			www = null;
			DownloadFormLocalStorage (art);
		}
	}

	/// <summary>
	/// Scale the image.
	/// </summary>
	/// <param name="scale">Scale.</param>
	public void scaleImage (float scale)
	{
		transform.localScale = new Vector3 (scale, scale, scale);
		pictureObject.transform.localScale = new Vector3 (scale, scale, scale);
	}
}
