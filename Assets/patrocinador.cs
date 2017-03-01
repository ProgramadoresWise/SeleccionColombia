using UnityEngine;
using UnityEngine.UI;

public class patrocinador : MonoBehaviour 
{
	[HeaderAttribute("Google Analytics")]
	public GoogleAnalyticsV4 gav4;

	[HeaderAttribute("Patrocinador")]
	public Image imgPatrocinador;
	public string nombrePatrocinador;
	[SerializeField]
	public string url;

	public void OpenUrl( )
	{
		gav4.LogEvent("Patrocinadores", "Click", nombrePatrocinador, 0);

		if(!string.IsNullOrEmpty(url))
			Application.OpenURL(url);
	}

}
