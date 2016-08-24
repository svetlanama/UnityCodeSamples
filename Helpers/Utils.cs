using UnityEngine;
using System.Collections;
using System.Net;

/*
 * Utils class which is responsible 
 * for providing additional helpfull methods 
 */
static class Utils
{

	/// <summary>
	/// Checks the network avaliable.
	/// </summary>
	/// <returns><c>true</c>, if network avaliable was checked, <c>false</c> otherwise.</returns>
	public static bool IsNetworkAvaliable ()
	{
		bool internetPossiblyAvailable = false;
		switch (Application.internetReachability) {

		case NetworkReachability.ReachableViaLocalAreaNetwork:
			internetPossiblyAvailable = true;
			break;
		case NetworkReachability.ReachableViaCarrierDataNetwork:
			internetPossiblyAvailable = true;
			break;
		default:
			internetPossiblyAvailable = false;
			break;
		}

		if (!CheckConnection ()) {
			internetPossiblyAvailable = false;
		}

		return internetPossiblyAvailable;
	}

	/// <summary>
	/// Checks the ability to connect via HttpWebRequest.
	/// </summary>
	/// <returns><c>true</c>, if connection was checked, <c>false</c> otherwise.</returns>
	public static bool CheckConnection ()
	{
		string URL = "http://www.google.com";
		bool flag = false;
		HttpWebRequest request = (HttpWebRequest)WebRequest.Create (URL);
		request.KeepAlive = false;
		request.Timeout = 5000;
		request.Proxy = null;

		request.ServicePoint.MaxIdleTime = 5000;
		try {
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse ()) {
				if (response.StatusCode == HttpStatusCode.OK) {
					flag = true;
				}   
				response.Close ();
			}
		} catch (WebException ex) {
			flag = false;
		} finally {
			request.Abort ();
			request = null; 
		}
		return flag;
	}
}
