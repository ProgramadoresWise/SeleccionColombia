using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.Networking;

public enum typejugador{
	redondos,
	cuadrados
}



public class ImgLoadManager : MonoBehaviour {


	public static ImgLoadManager main;


	// Noticias

	public Sprite NewsImg( Image imgRec, string nameImg , bool write ){
		Sprite imgPhoto = imgRec.sprite;
		StartCoroutine( NewsImg_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}

	public IEnumerator NewsImg_d( Image imgRec, string nameImg ,System.Action <Sprite> downImg, bool write ){
		Sprite imgPhoto = imgRec.sprite;
		byte[] imgBt = null;
		Texture2D tex = null;

		if(write){
			File.Delete(Application.persistentDataPath + "/Noticias/"+ nameImg +".jpg");
		}

		if( File.Exists(Application.persistentDataPath + "/Noticias/"+ nameImg + ".jpg")){
			
			imgBt = File.ReadAllBytes(Application.persistentDataPath+ "/Noticias/"+ nameImg + ".jpg") ;
			tex = new Texture2D(imgRec.mainTexture.width, imgRec.mainTexture.height);
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create( tex , new Rect(0,0, tex.width, tex.height), Vector2.zero);
		}else{
			

			imgRec.GetComponent<LoadImageIndicator>().LoadInd(true);

//			UnityWebRequest wr = new UnityWebRequest( DataApp.main.host + "Imagenes/Noticias/"+nameImg+".jpg" );
//			DownloadHandlerTexture text = new DownloadHandlerTexture(true);
//			wr.downloadHandler = text;
//			yield return wr.Send();
//			if(  string.IsNullOrEmpty( wr.error ) ){
//				tex = text.texture;
//				imgBt = tex.EncodeToJPG( );
//				imgRec.sprite = Sprite.Create(tex, new Rect(0,0, tex.width, tex.height), Vector2.zero);
//				yield return new WaitForEndOfFrame();
//				File.WriteAllBytes(Application.persistentDataPath + "/Noticias/"+ nameImg +".jpg", imgBt);
//			}
			WWW newImg = new WWW( DataApp.main.host + "Imagenes/Noticias/"+nameImg+".jpg");
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgRec.sprite  = Sprite.Create( newImg.texture, new Rect(0,0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath + "/Noticias/"+ nameImg +".jpg", imgBt);

				yield return new WaitForEndOfFrame();

			}
			imgRec.GetComponent<LoadImageIndicator>().LoadInd(false);
		}
		downImg(imgPhoto);
	}




	// Patrocinadores

	public Sprite sponsorImg( Image imgRec, string nameImg , bool write ){
		Sprite imgPhoto = imgRec.sprite;
		StartCoroutine( SponsorImg_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}

	IEnumerator SponsorImg_d( Image imgRec, string nameImg ,System.Action <Sprite> downImg, bool write ){
		Sprite imgPhoto = imgRec.sprite;
		byte[] imgBt = null;
		Texture2D tex = null;

		if(write){
			File.Delete(Application.persistentDataPath + "/Patrocinadores/"+ nameImg +".jpg");
		}

		if( File.Exists(Application.persistentDataPath + "/Patrocinadores/"+ nameImg + ".jpg")){
			imgBt = File.ReadAllBytes(Application.persistentDataPath+ "/Patrocinadores/"+ nameImg + ".jpg") ;
			tex = new Texture2D(imgRec.mainTexture.width, imgRec.mainTexture.height);
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create( tex , new Rect(0,0, tex.width, tex.height), Vector2.zero);
		}else{
			
			imgRec.GetComponent<LoadImageIndicator>().LoadInd(true);
			WWW newImg = new WWW( DataApp.main.host + "Imagenes/Patrocinadores/"+nameImg+".jpg");
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgRec.sprite  = Sprite.Create( newImg.texture, new Rect(0,0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath + "/Patrocinadores/"+ nameImg +".jpg", imgBt);
				yield return new WaitForEndOfFrame();
			}
			imgRec.GetComponent<LoadImageIndicator>().LoadInd(false);
		}
		downImg(imgPhoto);
	}



	// tu elijes

	public Sprite TuElijesImg( Image imgRec, string nameImg , bool write ){
		Sprite imgPhoto = imgRec.sprite;
		StartCoroutine( TuElijesImg_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}

	IEnumerator TuElijesImg_d( Image imgRec, string nameImg ,System.Action <Sprite> downImg, bool write ){
		Sprite imgPhoto = imgRec.sprite;
		byte[] imgBt = null;
		Texture2D tex = null;
		if(write){
			File.Delete(Application.persistentDataPath + "/TuElijes/"+ nameImg +".jpg");
		}
	
		if( File.Exists(Application.persistentDataPath + "/TuElijes/"+ nameImg + ".jpg")){
			imgBt = File.ReadAllBytes(Application.persistentDataPath+ "/TuElijes/"+ nameImg + ".jpg") ;
			tex = new Texture2D(imgRec.mainTexture.width, imgRec.mainTexture.height);
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create( tex , new Rect(0,0, tex.width, tex.height), Vector2.zero);
		}else{
			imgRec.GetComponent<LoadImageIndicator>().LoadInd(true);
			WWW newImg = new WWW( DataApp.main.host + "/Imagenes/TuElijes/"+nameImg+".jpg");
		
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgRec.sprite  = Sprite.Create( newImg.texture, new Rect(0,0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath + "/TuElijes/"+ nameImg +".jpg", imgBt);
				yield return new WaitForEndOfFrame();
			}
			imgRec.GetComponent<LoadImageIndicator>().LoadInd(false);
		}
		downImg(imgPhoto);
	}




	// ESCUDOS

	public Sprite teamImg( Image imgRec, string nameImg , bool write ){
		Sprite imgPhoto = imgRec.sprite;
		StartCoroutine( TeamImg_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}

	IEnumerator TeamImg_d( Image imgRec, string nameImg ,System.Action <Sprite> downImg, bool write ){
		Sprite imgPhoto = imgRec.sprite;
		byte[] imgBt = null;
		Texture2D tex = null;

		if(write){
			File.Delete(Application.persistentDataPath + "/Equipos/"+ nameImg +".jpg");
		}
		if( File.Exists(Application.persistentDataPath + "/Equipos/"+ nameImg + ".jpg")){
			
			imgBt = File.ReadAllBytes(Application.persistentDataPath+ "/Equipos/"+ nameImg + ".jpg") ;
			tex = new Texture2D(imgRec.mainTexture.width, imgRec.mainTexture.height);
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create( tex , new Rect(0,0, tex.width, tex.height), Vector2.zero);
		}else{
			
			imgRec.GetComponent<LoadImageIndicator>().LoadInd(true);
			WWW newImg = new WWW( DataApp.main.host + "Imagenes/Equipos/"+nameImg+".jpg");
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgRec.sprite  = Sprite.Create( newImg.texture, new Rect(0,0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath + "/Equipos/"+ nameImg +".jpg", imgBt);
				yield return new WaitForEndOfFrame();
			}
			imgRec.GetComponent<LoadImageIndicator>().LoadInd(false);
		}
		downImg(imgPhoto);
	}


	// JUGADORES 

	public Sprite PlayerImg( Image imgRec ,string nameImg , bool write ){

		Sprite imgPhoto = imgRec.sprite;
		StartCoroutine( PlayerImg_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}

	public IEnumerator PlayerImg_d( Image imgRec, string nameImg ,System.Action <Sprite> downImg, bool write ){
		Sprite imgPhoto = imgRec.sprite;
		byte[] imgBt = null;
		Texture2D tex = null;

//		string typeJ = ( tp == typejugador.cuadrados)? "Cuadradas": "Redondas";
		string typeImg =".png";

		if(write){
			File.Delete(Application.persistentDataPath  + "/Redondas/"+ nameImg +typeImg);
		}

		if( File.Exists(Application.persistentDataPath + "/Redondas/"+ nameImg +typeImg)){
			
			imgBt = File.ReadAllBytes(Application.persistentDataPath + "/Redondas/"+ nameImg + typeImg) ;
			tex = new Texture2D(imgRec.mainTexture.width, imgRec.mainTexture.height);
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create( tex , new Rect(0,0, tex.width, tex.height), Vector2.zero);
		}else{
			
//			imgRec.GetComponent<LoadImageIndicator>().LoadInd(true);
			WWW newImg = new WWW( DataApp.main.host  + "Imagenes/Jugadores/Redondas/"+ nameImg+typeImg);
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgRec.sprite  = Sprite.Create( newImg.texture, new Rect(0,0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToPNG( );
				File.WriteAllBytes(Application.persistentDataPath  + "/Redondas/"+ nameImg + typeImg, imgBt);
				yield return new WaitForEndOfFrame();
			}
//			imgRec.GetComponent<LoadImageIndicator>().LoadInd(false);
		}
		downImg(imgPhoto);
	}




	public IEnumerator PlayerImg_dAlineacionPolla( Sprite img, string nameImg ,System.Action <Sprite> downImg, bool write ){
		Sprite imgPhoto = img;
		byte[] imgBt = null;
		Texture2D tex = null;

		string typeImg =".png";

		if(write){
			File.Delete(Application.persistentDataPath  + "/Redondas/"+ nameImg +typeImg);
		}

		if( File.Exists(Application.persistentDataPath + "/Redondas/"+ nameImg +typeImg)){

			imgBt = File.ReadAllBytes(Application.persistentDataPath + "/Redondas/"+ nameImg + typeImg) ;
		
			tex = new Texture2D(256, 256);	
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create( tex , new Rect(0,0, tex.width, tex.height), Vector2.zero);
		}else{

			//			imgRec.GetComponent<LoadImageIndicator>().LoadInd(true);
			WWW newImg = new WWW( DataApp.main.host  + "Imagenes/Jugadores/Redondas/"+ nameImg+typeImg);
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgPhoto  = Sprite.Create( newImg.texture, new Rect(0,0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToPNG( );
				File.WriteAllBytes(Application.persistentDataPath  + "/Redondas/"+ nameImg + typeImg, imgBt);
				yield return new WaitForEndOfFrame();
			}
			//			imgRec.GetComponent<LoadImageIndicator>().LoadInd(false);
		}
		downImg(imgPhoto);
	}


	// JUGADORES CONVOCADOS 

	public Sprite PlayerConvocadosImg( Image imgRec,string nameImg , bool write ){
		Sprite imgPhoto = imgRec.sprite;
		StartCoroutine( PlayerConvocadosImg_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}

	public IEnumerator PlayerConvocadosImg_d( Image imgRec, string nameImg ,System.Action <Sprite> downImg, bool write ){
		Sprite imgPhoto = imgRec.sprite;
		byte[] imgBt = null;
		Texture2D tex = null;

		if(write){
			File.Delete(Application.persistentDataPath  + "/JugadoresConvocados/"+ nameImg +".jpg");
		}

		if( File.Exists(Application.persistentDataPath + "/JugadoresConvocados/"+ nameImg + ".jpg")){

			imgBt = File.ReadAllBytes(Application.persistentDataPath + "/JugadoresConvocados/"+ nameImg + ".jpg") ;
			tex = new Texture2D(imgRec.mainTexture.width, imgRec.mainTexture.height);
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create( tex , new Rect(0,0, tex.width, tex.height), Vector2.zero);
		}else{

//			imgRec.GetComponent<LoadImageIndicator>().LoadInd(true);
			WWW newImg = new WWW( DataApp.main.host  + "Imagenes/JugadoresConvocados/"+ nameImg+".jpg");
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgRec.sprite  = Sprite.Create( newImg.texture, new Rect(0,0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath  + "/JugadoresConvocados/"+ nameImg +".jpg", imgBt);
				yield return new WaitForEndOfFrame();
			}
//			imgRec.GetComponent<LoadImageIndicator>().LoadInd(false);
		}
		downImg(imgPhoto);
	}




	// JUGADORES PERFIL JUGADOR INFORMACION

	public Sprite PlayerInfo( Image imgRec,string nameImg , bool write ){
		Sprite imgPhoto = imgRec.sprite;
		StartCoroutine( PlayerInfo_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}

	public IEnumerator PlayerInfo_d( Image imgRec, string nameImg ,System.Action <Sprite> downImg, bool write ){
		Sprite imgPhoto = imgRec.sprite;
		byte[] imgBt = null;
		Texture2D tex = null;

		if(write){
			File.Delete(Application.persistentDataPath  + "/PerfilJugador/"+ nameImg +".jpg");
		}

		if( File.Exists(Application.persistentDataPath + "/PerfilJugador/"+ nameImg + ".jpg")){

			imgBt = File.ReadAllBytes(Application.persistentDataPath + "/PerfilJugador/"+ nameImg + ".jpg") ;
			tex = new Texture2D(imgRec.mainTexture.width, imgRec.mainTexture.height);
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create( tex , new Rect(0,0, tex.width, tex.height), Vector2.zero);
		}else{

			imgRec.GetComponent<LoadImageIndicator>().LoadInd(true);
			WWW newImg = new WWW( DataApp.main.host  + "Imagenes/PerfilJugador/Informacion/"+ nameImg+".jpg");
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgRec.sprite  = Sprite.Create( newImg.texture, new Rect(0,0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath  + "/PerfilJugador/"+ nameImg +".jpg", imgBt);
				yield return new WaitForEndOfFrame();
			}
			imgRec.GetComponent<LoadImageIndicator>().LoadInd(false);
		}
		downImg(imgPhoto);
	}


	// JUGADORES PERFIL JUGADOR ESTADISTICAS

	public Sprite PlayerStats( Image imgRec,string nameImg , bool write ){
		Sprite imgPhoto = imgRec.sprite;
		StartCoroutine( PlayerStats_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}

	public IEnumerator PlayerStats_d( Image imgRec, string nameImg ,System.Action <Sprite> downImg, bool write ){
		Sprite imgPhoto = imgRec.sprite;
		byte[] imgBt = null;
		Texture2D tex = null;

		if(write){
			File.Delete(Application.persistentDataPath  + "/PerfilJugador/"+ nameImg +".jpg");
		}

		if( File.Exists(Application.persistentDataPath + "/PerfilJugador/"+ nameImg + ".jpg")){

			imgBt = File.ReadAllBytes(Application.persistentDataPath + "/PerfilJugador/"+ nameImg + ".jpg") ;
			tex = new Texture2D(imgRec.mainTexture.width, imgRec.mainTexture.height);
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create( tex , new Rect(0,0, tex.width, tex.height), Vector2.zero);
		}else{

			imgRec.GetComponent<LoadImageIndicator>().LoadInd(true);
			WWW newImg = new WWW( DataApp.main.host  + "Imagenes/PerfilJugador/Estadisticas/"+ nameImg+".jpg");
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgRec.sprite  = Sprite.Create( newImg.texture, new Rect(0,0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath  + "/PerfilJugador/"+ nameImg +".jpg", imgBt);
				yield return new WaitForEndOfFrame();
			}
			imgRec.GetComponent<LoadImageIndicator>().LoadInd(false);
		}
		downImg(imgPhoto);
	}


	// JUGADORES PERFIL JUGADOR ULTIMO PARTIDO

	public Sprite PlayerFinalMatch( Image imgRec,string nameImg , bool write ){
		Sprite imgPhoto = imgRec.sprite;
		StartCoroutine( PlayerFinalMatch_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}

	public IEnumerator PlayerFinalMatch_d( Image imgRec, string nameImg ,System.Action <Sprite> downImg, bool write ){
		Sprite imgPhoto = imgRec.sprite;
		byte[] imgBt = null;
		Texture2D tex = null;

		if(write){
			File.Delete(Application.persistentDataPath  + "/PerfilJugador/"+ nameImg +".jpg");
		}

		if( File.Exists(Application.persistentDataPath + "/PerfilJugador/"+ nameImg + ".jpg")){

			imgBt = File.ReadAllBytes(Application.persistentDataPath + "/PerfilJugador/"+ nameImg + ".jpg") ;
			tex = new Texture2D(imgRec.mainTexture.width, imgRec.mainTexture.height);
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create( tex , new Rect(0,0, tex.width, tex.height), Vector2.zero);
		}else{

//			imgRec.GetComponent<LoadImageIndicator>().LoadInd(true);
			WWW newImg = new WWW( DataApp.main.host  + "Imagenes/PerfilJugador/UltimoPartido/"+ nameImg+".jpg");
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgRec.sprite  = Sprite.Create( newImg.texture, new Rect(0,0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath  + "/PerfilJugador/"+ nameImg +".jpg", imgBt);
				yield return new WaitForEndOfFrame();
			}
//			imgRec.GetComponent<LoadImageIndicator>().LoadInd(false);
		}
		downImg(imgPhoto);
	}


	// USUSARIO
	public Sprite UsersImg( Image imgRec, string nameImg , bool write ){
		Sprite imgPhoto = imgRec.sprite;
		StartCoroutine( UsersImg_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}

	IEnumerator UsersImg_d( Image imgRec, string nameImg ,System.Action <Sprite> downImg, bool write ){
		Sprite imgPhoto = imgRec.sprite;
		byte[] imgBt = null;
		Texture2D tex = null;

		if(write){
			File.Delete(Application.persistentDataPath + "/PhotoUser/"+ nameImg +".jpg");
		}

		if( File.Exists(Application.persistentDataPath + "/PhotoUser/"+ nameImg + ".jpg")){
			
			imgBt = File.ReadAllBytes(Application.persistentDataPath+ "/PhotoUser/"+ nameImg + ".jpg") ;
			tex = new Texture2D(imgRec.mainTexture.width, imgRec.mainTexture.height);
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create( tex , new Rect(0,0, tex.width, tex.height), Vector2.zero);
		}else{
			
			imgRec.GetComponent<LoadImageIndicator>().LoadInd(true);
			WWW newImg = new WWW( DataApp.main.host + "Users/Photos/"+nameImg+".jpg");
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgRec.sprite  = Sprite.Create( newImg.texture, new Rect(0,0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath + "/PhotoUser/"+ nameImg +".jpg", imgBt);
				yield return new WaitForEndOfFrame();
			}
			imgRec.GetComponent<LoadImageIndicator>().LoadInd(false);
		}
		downImg(imgPhoto);
	}




	//     EN MAIL  ----
	public Texture MailImg( RawImage imgRec, string nameImg , bool write ){
		Texture imgPhoto = imgRec.texture;
		StartCoroutine( MailImg_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}


	IEnumerator MailImg_d( RawImage imgRec, string nameImg ,System.Action <Texture> downImg, bool write ){
		Texture	imgPhoto = imgRec.texture;
		Texture2D imgTemp = null;
		byte[] imgBt = null;

		if(write){
			File.Delete(Application.persistentDataPath +"/Mail/"+ nameImg +".jpg");
		}

		if( File.Exists(Application.persistentDataPath +"/Mail/"+ nameImg + ".jpg")){

			imgBt = File.ReadAllBytes(Application.persistentDataPath +"/Mail/"+ nameImg + ".jpg");
			imgTemp = new Texture2D(imgRec.texture.width , imgRec.texture.height);
			imgTemp.LoadImage(imgBt);
			imgPhoto = imgTemp as Texture;
		}else{
			
			WWW newImg = new WWW( DataApp.main.host +"Imagenes/Mail/"+ nameImg+".jpg");
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgBt = newImg.texture.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath +"/Mail/"+ nameImg +".jpg", imgBt);
				yield return new WaitForEndOfFrame();
				imgRec.texture = newImg.texture;
				imgPhoto = newImg.texture;
			}
		}
		downImg(imgPhoto);
	}


	// Load escusdos de tabla de posciones 
	public Texture ShieldTableTexture( RawImage imgRec, string nameImg , bool write ){
		Texture imgPhoto = imgRec.texture;
		StartCoroutine( ShieldTableTexture_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}


	IEnumerator ShieldTableTexture_d( RawImage imgRec, string nameImg ,System.Action <Texture> downImg, bool write ){
		Texture	imgPhoto = imgRec.texture;
		Texture2D imgTemp = null;
		byte[] imgBt = null;

		if(write){
			File.Delete(Application.persistentDataPath +"/EquipoTabla/"+ nameImg +".jpg");
		}

		if( File.Exists(Application.persistentDataPath +"/EquipoTabla/"+ nameImg + ".jpg")){

			imgBt = File.ReadAllBytes(Application.persistentDataPath +"/EquipoTabla/"+ nameImg + ".jpg");
			imgTemp = new Texture2D(imgRec.texture.width , imgRec.texture.height);
			imgTemp.LoadImage(imgBt);
			imgPhoto = imgTemp as Texture;
		}else{
			
			WWW newImg = new WWW( DataApp.main.host +"Imagenes/tablaPos/"+ nameImg+".jpg");
			yield return newImg;
			if( string.IsNullOrEmpty( newImg.error)) { 
				imgBt = newImg.texture.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath +"/EquipoTabla/"+ nameImg +".jpg", imgBt);
				yield return new WaitForEndOfFrame();
				imgRec.texture = newImg.texture;
				imgPhoto = newImg.texture;
			}
		}
		downImg(imgPhoto);
	}



	public Texture ShieldCalendarTexture(RawImage imgRec, string nameImg, bool write)
	{
		Texture imgPhoto = imgRec.texture;
		StartCoroutine(ShielCalendarTexture_d(imgRec, nameImg, resultImg => {
			imgPhoto = resultImg;
		}, write));
		return imgPhoto;
	}


	IEnumerator ShielCalendarTexture_d(RawImage imgRec, string nameImg, System.Action<Texture> downImg, bool write)
	{
		Texture imgPhoto = imgRec.texture;
		Texture2D imgTemp = null;
		byte[] imgBt = null;

		if (write)
		{
			File.Delete(Application.persistentDataPath + "/Equipos/" + nameImg + ".jpg");
		}

		if (File.Exists(Application.persistentDataPath + "/Equipos/" + nameImg + ".jpg"))
		{

			imgBt = File.ReadAllBytes(Application.persistentDataPath + "/Equipos/" + nameImg + ".jpg");
			imgTemp = new Texture2D(imgRec.texture.width, imgRec.texture.height);
			imgTemp.LoadImage(imgBt);
			imgPhoto = imgTemp as Texture;
		}
		else
		{
//			print(nameImg);
			WWW newImg = new WWW(DataApp.main.host + "Imagenes/Equipos/" + nameImg + ".jpg");
			yield return newImg;
			if (string.IsNullOrEmpty(newImg.error))
			{
				imgBt = newImg.texture.EncodeToJPG();
				File.WriteAllBytes(Application.persistentDataPath + "/Equipos/" + nameImg + ".jpg", imgBt);
				yield return new WaitForEndOfFrame();
				imgRec.texture = newImg.texture;
				imgPhoto = newImg.texture;
			}
		}
		downImg(imgPhoto);
	}


	// Calendario

	public Sprite CalendarImg(Image imgRec, string nameImg, bool write)
	{
		Sprite imgPhoto = imgRec.sprite;
		StartCoroutine(CalendarImg_d(imgRec, nameImg, resultImg => {
			imgPhoto = resultImg;
		}, write));
		return imgPhoto;
	}

	public IEnumerator CalendarImg_d(Image imgRec, string nameImg, System.Action<Sprite> downImg, bool write)
	{
		Sprite imgPhoto = imgRec.sprite;
		byte[] imgBt = null;
		Texture2D tex = null;

		if (write)
		{
			File.Delete(Application.persistentDataPath + "/Calendario/" + nameImg + ".jpg");
		}

		if (File.Exists(Application.persistentDataPath + "/Calendario/" + nameImg + ".jpg"))
		{

			imgBt = File.ReadAllBytes(Application.persistentDataPath + "/Calendario/" + nameImg + ".jpg");
			tex = new Texture2D(imgRec.mainTexture.width, imgRec.mainTexture.height);
			tex.LoadImage(imgBt);
			imgPhoto = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
		}
		else
		{
			WWW newImg = new WWW(DataApp.main.host + "Imagenes/Calendario/" + nameImg + ".jpg");
			yield return newImg;

			if (string.IsNullOrEmpty(newImg.error))
			{
				imgRec.sprite = Sprite.Create(newImg.texture, new Rect(0, 0, newImg.texture.width, newImg.texture.height), Vector2.zero);
				imgBt = newImg.texture.EncodeToJPG();
				File.WriteAllBytes(Application.persistentDataPath + "/Calendario/" + nameImg + ".jpg", imgBt);
				yield return new WaitForEndOfFrame();
			}
		}
		downImg(imgPhoto);
	}


	//     SPLASH    ----
	public Texture SplashImg( RawImage imgRec, string nameImg , bool write ){
		Texture imgPhoto = imgRec.texture;
		StartCoroutine( SplashImg_d (imgRec, nameImg, resultImg =>{
			imgPhoto = resultImg;  }, write ));
		return imgPhoto;
	}


	public IEnumerator SplashImg_d( RawImage imgRec, string nameImg ,System.Action <Texture> downImg, bool write ){
		Texture	imgPhoto = imgRec.texture;
		Texture2D imgTemp = null;
		byte[] imgBt = null;

		if(write){
			File.Delete(Application.persistentDataPath +"/Splash/"+ nameImg +".jpg");
		}

		if( File.Exists(Application.persistentDataPath +"/Splash/"+ nameImg + ".jpg")){
			Debug.Log("TOMANDO FOTOS");
			imgBt = File.ReadAllBytes(Application.persistentDataPath +"/Splash/"+ nameImg + ".jpg");
			imgTemp = new Texture2D(imgRec.texture.width , imgRec.texture.height);
			imgTemp.LoadImage(imgBt);
			imgPhoto = imgTemp as Texture;
		}else{
			Debug.Log("DESCARGANDO FOTOS");
			UnityWebRequest wr = new UnityWebRequest(DataApp.main.host +"Imagenes/inicial/"+ nameImg+".jpg");
			DownloadHandlerTexture text = new DownloadHandlerTexture(true);
			wr.downloadHandler = text;
			yield return wr.Send();

			if( string.IsNullOrEmpty( wr.error)) { 
				imgTemp = text.texture;
				imgBt = imgTemp.EncodeToJPG( );
				File.WriteAllBytes(Application.persistentDataPath +"/Splash/"+ nameImg +".jpg", imgBt);
				imgRec.texture = text.texture;
				imgPhoto = text.texture;
			}
				//				imgBt = newImg.texture.EncodeToJPG( );
//			WWW newImg = new WWW( DataApp.main.host +"Imagenes/inicial/"+ nameImg+".jpg");
//			yield return newImg;
//			if( string.IsNullOrEmpty( newImg.error)) { 
//				imgBt = newImg.texture.EncodeToJPG( );
//				File.WriteAllBytes(Application.persistentDataPath +"/Splash/"+ nameImg +".jpg", imgBt);
//				yield return new WaitForEndOfFrame();
//				imgRec.texture = newImg.texture;
//				imgPhoto = newImg.texture;
//			}
		}
		downImg(imgPhoto);
	}




}
