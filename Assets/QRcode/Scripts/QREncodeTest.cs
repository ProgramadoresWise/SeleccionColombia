/// <summary>

/// </summary>
/// 
/// 

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QREncodeTest : MonoBehaviour 
{
	public QRCodeEncodeController e_qrController;
	public RawImage qrCodeImage;
	//public InputField m_inputfield;

	//public string num;

	//public Text infoText;
	// Use this for initialization
	void Start () 
	{
		qrCodeImage.enabled = false;
		if (e_qrController != null) 
		{
			e_qrController.onQREncodeFinished += qrEncodeFinished;//Add Finished Event
		}

		StartCoroutine(codeBar());
	}

	IEnumerator codeBar() 
	{
		yield return new WaitForEndOfFrame();
		generateCode(PlayerPrefs.GetString("codigoBarrasNum"));
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void qrEncodeFinished(Texture2D tex)
	{
		if (tex != null && tex != null) 
		{
			int width = tex.width;
			int height = tex.height;
			float aspect = width * 1.0f / height;
			qrCodeImage.GetComponent<RectTransform> ().sizeDelta = new Vector2 (170, 170.0f / aspect);
			qrCodeImage.texture = tex;
			qrCodeImage.enabled = true;
		} 
		else 
		{

		}
	}

	public void generateCode(string str)
	{
		if (e_qrController != null) 
		{
			string valueStr =  str;
			int errorlog = e_qrController.Encode(valueStr);
		}
	}


/*	public void Encode()
	{
		if (e_qrController != null) 
		{
			//string valueStr = m_inputfield.text;
			string valueStr =  num;
			int errorlog = e_qrController.Encode(valueStr);
		}
	}*/

	/*public void ClearCode()
	{
		qrCodeImage.texture = null;
		m_inputfield.text = "";
	
	}*/

}
