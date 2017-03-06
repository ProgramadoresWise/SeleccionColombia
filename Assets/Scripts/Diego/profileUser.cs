using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using MaterialUI;
using System.Collections.Generic;
using ImageVideoContactPicker;
using System;
using UnityEngine.Events;
using System.Configuration;
using System.IO;


public class profileUser : MonoBehaviour {

	[HideInInspector]
	public Text _name;
	[HideInInspector]
	public InputField nick, desc, nickComplete, descComplete;
	[HideInInspector]
	public InputField name, lastname, email, indc, cel, pass, confirmpass;
	[HideInInspector]
	public MaterialDropdown day, month, year;
	[HideInInspector]
	public Dropdown country, city;
	[HideInInspector]
	public Toggle m, f;

	public Image photoUser, photoUserView , photouserNav;
	public UnityEvent editUser;
	string gender;

	public carnet mycarnet;

	delegate void Delegado(WWW www);

	public bool changePhoto;

	string photoPathUrl;

	public Sprite defaultUser;


	void Start () {
		reloadInfoUser();
//		photoUser.sprite = ImgLoadManager.main.UsersImg(photoUser , DataApp.main.GetMyID().ToString(), false);

		mycarnet.loadCarnet( photoUser.sprite , User.main.GetMyName() + " " + User.main.GetMyLastName(), User.main.GetMyFechaIncial(), User.main.GetMyFechaFinal() , int.Parse(User.main.GetMyNumH()));
		//
		
	}

	public void MyloadCarnet( ){
		mycarnet.loadCarnet( photoUser.sprite , User.main.GetMyName() + " " + User.main.GetMyLastName(), User.main.GetMyFechaIncial(), User.main.GetMyFechaFinal() , int.Parse(User.main.GetMyNumH()));
	}

	void reloadInfoUser(){
		if( DataApp.main.IsRegistered()){

				nick.text = User.main.GetMyNick();
				desc.text = User.main.GetMyDesc();
				nickComplete.text = User.main.GetMyNick();
				descComplete.text = User.main.GetMyDesc();
				_name.text = User.main._name;
				name.text = User.main._name;
				lastname.text = User.main._lastname;
				email.text = User.main._email;
				indc.text = User.main._ind_cel;
				cel.text = User.main._cel;
				pass.text = User.main._pass;
				confirmpass.text = User.main._pass;
				day.currentlySelected = User.main.GetMyDayId();
				month.currentlySelected = User.main.GetMyMonthId();
				year.currentlySelected = User.main.GetMyYearId();
				country.value = User.main.GetMyCountry();
				city.value = User.main.GetMyCity();
					if(User.main.GetMyGender() == "Masculino"){
						m.isOn = true;
						f.isOn = false;
						gender= "Masculino";
					}else{
						f.isOn = true;
						m.isOn = false;
						gender = "Femenino";
					}
				disableCity();
		}else{
			ClearInputfields();
		}
		DesactivarBotones();
	}



	public void UpdateInfoU( ){
		DataApp.main.EnableLoading();
		StartCoroutine( DataApp.main.CheckInternet( internet =>{
			if( internet ){
				if( confirmpass.text == pass.text  ) {
						if(  pass.text.Length < 6)
							DataApp.main.popUpInformative(true, "Contraseña no Valida", "La contraseña debe tener minimo 6 digitos.");
						else
							updateInfoUser();
				}else{
					DataApp.main.popUpInformative( true, "Contraseña Incorrecta", "La contraseña no coinciden");
				}
			}else{
				DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
			}
		}));
	}

	public void UpdateInfoUV(  ){
		DataApp.main.EnableLoading();
		StartCoroutine( DataApp.main.CheckInternet( internet =>{
			if( internet ){
				updateInfoUserView( );
			}else{
				DataApp.main.popUpInformative(true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
			}

		}));

		NavigatorManager.main.disablePopUpInfo();
	}

	public void ChangeGender( Toggle g){
		if(g.isOn ){
			gender = g.name;
		}
	}


	public void disableCity( ){
		if( country.value > 0) {
			city.gameObject.SetActive(false);
		}else{
			city.gameObject.SetActive(true);
		}
	} 


	int parseMonth( string month ){
		int rMonth = 0;
		rMonth = ( month == "Enero" )? 1: ( month == "Febrero" )? 2:( month == "Marzo" )? 3:  ( month == "Abril" )? 4: ( month == "Mayo" )? 5: ( month == "Junio" )? 6: ( month == "Julio" )? 7: ( month == "Agosto" )? 8: ( month == "Septiembre" )? 9: ( month == "Octubre" )? 10: ( month == "Noviembre" )? 11: 12;
		return rMonth;
	}


	void updateInfoUser( ){

		int _month = parseMonth(month.buttonTextContent.text);

		StartCoroutine( DataApp.main.Consult( DataApp.main.host + "Registro/registerandlogin.php", new Dictionary<string,string>{
			{"indata","updateUser"}, 
			{"iduser",DataApp.main.GetMyID().ToString()},
			{"nombre",name.text},
			{"apellido",lastname.text},
			{"correo",email.text},
			{"pass",pass.text},
			{"indcelular",indc.text},
			{"celular",cel.text},
			{"paisid",country.value.ToString()},
			{"pais",country.captionText.text},
			{"ciudadid",city.value.ToString()},
			{"ciudad",city.captionText.text},
			{"fechaNacimiento",year.buttonTextContent.text+"-"+_month+"-"+day.buttonTextContent.text},
			{"genero",gender}
		}, result => {
			Debug.Log("PUTO R: "+ result);
			if( !string.IsNullOrEmpty( result ) && result == "ActualizacionExitosa"){
				User.main.reSaveDataInUser(email.text,
					name.text,
					lastname.text,
					pass.text,gender,
					indc.text,
					cel.text,
					country.value,
					city.value,
					year.buttonTextContent.text+"-"+_month+"-"+day.buttonTextContent.text,
					User.main.GetMyDesc(),
					User.main.GetMyNick(),
					User.main.GetMyEmailVerif(),
					User.main.GetMyFechaIncial(),
					User.main.GetMyNumH(),
					"update"
				);
//				User.main.SetMyBirthday(year.buttonTextContent.text+"-"+month.buttonTextContent.text+"-"+day.buttonTextContent.text);
				// GUARDANDO IDES DE FECHA DE NACIEMIENTO
				User.main.SetMyYearId(year.currentlySelected);
				User.main.SetMyMonthId(month.currentlySelected);
				User.main.SetMyDayId(day.currentlySelected);
//				ToastManager.Show("Actualización Exitosa",1f,null);
			}else{
				ToastManager.Show("Error en la actualización, Intentalo de nuevo.",1f,null);
			}
			DataApp.main.DisableLoading();
		} ));
	}

	void updateInfoUserView(  ){
		DataApp.main.EnableLoading();
//		if( changePhoto ){
//			SaveAndUoploadPhoto(changePhoto);
//		}

		StartCoroutine( DataApp.main.Consult( DataApp.main.host + "Registro/registerandlogin.php", new Dictionary<string,string>{
			{"indata","updateViewUser"}, 
			{"iduser",DataApp.main.GetMyID().ToString()},
			{"nick",nick.text},
			{"desc",desc.text}
		}, result => {
			Debug.Log("PUTO R: "+ desc.text);
			if( !string.IsNullOrEmpty( result ) && result == "ActualizacionExitosa"){
				User.main.SetMyDesc(desc.text);
				User.main.SetMyNick(nick.text);
				nickComplete.text = User.main.GetMyNick();
				descComplete.text = User.main.GetMyDesc();
				ToastManager.Show("Actualización Exitosa",1f,null);
			}else{
				ToastManager.Show("Error en la actualización, Intentalo de nuevo.",1f,null);
			}
			DataApp.main.DisableLoading();
		} ));
	}


	public void ChangePhotoProfile ( ) {  ///  abre galeria y cambia la imagen de perfil
		DataApp.main.EnableLoading();
//		changePhoto = true;
		PickerEventListener.onImageSelect += OnImageSelect;
		PickerEventListener.onError += RespuestaError;
		PickerEventListener.onCancel += OnCancelSelect;
		#if UNITY_ANDROID
		AndroidPicker.BrowseImage(true);
		#elif UNITY_IOS 
		IOSPicker.BrowseImage(true); 
		#endif

//		editUser.Invoke();
//		this.GetComponent<Panel>()._enablePopUpInfoPanel(0);
	}


	void OnImageSelect( string rutaImagen ) { // Selecciona la ruta de la imagen en el dispositivo
		photoPathUrl = rutaImagen;
		StartCoroutine( TraerContenidoImg( "file://" + photoPathUrl , AsignarImagen ) );
		DataApp.main.DisableLoading();
	}

	IEnumerator TraerContenidoImg ( string enlaceRecibido, Delegado delegado ) { 
		WWW www = new WWW (enlaceRecibido);
		yield return www;
		if(delegado != null){   
			delegado(www);   
		}
	}
		
	void AsignarImagen( WWW wwwRecibido ){  // asigan la imagen Descargada del movil
		Texture2D imgPhoto = new Texture2D(4, 4, TextureFormat.ARGB32, false);
		wwwRecibido.LoadImageIntoTexture( imgPhoto );
		Texture2D texturaWeb = wwwRecibido.texture;
		Sprite spriteWeb = Sprite.Create(texturaWeb,new Rect(0,0,texturaWeb.width,texturaWeb.height),new Vector2(0.5f,0.5f));
		photoUserView.sprite = spriteWeb;
		photoUser.sprite = spriteWeb;
		PickerEventListener.onImageSelect -= OnImageSelect;
		PickerEventListener.onError -= RespuestaError ;

		StartCoroutine( UploadPhotoUser(photoPathUrl, DataApp.main.host + "Users/uploadPhoto.php"));

	}


	void SaveAndUoploadPhoto (bool save ){
		if(save)
			StartCoroutine( UploadPhotoUser(photoPathUrl, DataApp.main.host + "Users/uploadPhoto.php"));
		else
			photoUserView.sprite = defaultUser;
	}
		

	IEnumerator UploadPhotoUser (string localFileName, string uploadURL) {
		DataApp.main.EnableLoading();
		WWW localFile = new WWW("file://" + localFileName);
		yield return localFile;
		if (localFile.error == null){
		} else {
			yield break;  	
		}
			WWWForm postForm = new WWWForm( );
			string nameImg = DataApp.main.GetMyID()+".jpg";
			postForm.AddBinaryData("theFile",localFile.bytes,nameImg,"text/plain");
			WWW upload = new WWW(uploadURL,postForm);
			yield return upload;
			if (upload.error == null){
				yield return new WaitForEndOfFrame();
				File.Delete(Application.persistentDataPath + "/PhotoUser/"+ nameImg +".jpg");
				photoUser.sprite = photoUserView.sprite;
				photouserNav.sprite = photoUserView.sprite;
				File.WriteAllBytes(Application.persistentDataPath + "/PhotoUser/"+ nameImg +".jpg", upload.bytes);
				ToastManager.Show("Actualización Exitosa",4f,null);
			} else {
				//StartCoroutine(DownloadImageProfile());
				ToastManager.Show("Hubo problemas en la actualización de tu foto,\nIntentalo de nuevo",4f,null);
			}
		DataApp.main.DisableLoading();
	}

	void OnCancelSelect( ) {
		photoPathUrl = null;
		DataApp.main.DisableLoading();
		changePhoto = false;

	}


	void RespuestaError( string errorMsg ) {
		photoPathUrl = null;
		DataApp.main.DisableLoading();
		changePhoto = false;
	}

	void ClearInputfields( ){
		name.text ="";
		lastname.text="";
		email.text="";
		indc.text="";
		cel.text="";
		pass.text="";
		confirmpass.text="";
		nick.text="";
		desc.text="";
		day.currentlySelected=0;
		month.currentlySelected=0;
		year.currentlySelected=0;
		country.value =0;
		city.value =0;
	}

	[HeaderAttribute("Registro")]
	public GameObject btnGuardar;
	public GameObject btnCancelar;

	public void ActivarBotones()
	{
		btnGuardar.SetActive(true);
		btnCancelar.SetActive(true);
	}
	public void DesactivarBotones()
	{
		btnGuardar.SetActive(false);
		btnCancelar.SetActive(false);
	}
}
