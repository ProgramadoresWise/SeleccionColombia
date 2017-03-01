using UnityEngine;
using System.Collections;
using System;

public class User : Singleton<User>  {


//	public static User main;
	[SerializeField]
	private DataRowList userDataList;

	[SerializeField]
	private string name = "";
	public string _name {get{ return name; } set{ name = value; }}
	[SerializeField]
	private string lastname;
	public string _lastname {get{ return lastname; } set{ lastname = value; }}
	[SerializeField]
	private string email;
	public string _email {get{ return email; } set{ email = value; }}
	[SerializeField]
	private string ind_cel;
	public string _ind_cel {get{ return ind_cel; } set{ ind_cel = value; }}
	[SerializeField]
	private string cel;
	public string _cel {get{ return cel; } set{ cel = value; }}
	[SerializeField]
	private string pass;
	public string _pass {get{ return pass; } set{ pass = value; }}
//	[SerializeField]
	private int country;
	public int _country {get{ return country; } set{ country = value; }}
//	[SerializeField]
	private int city;
	public int _city {get{ return city; } set{ city = value; }}
	[SerializeField]
	private string gender;
	public string _gender {get{ return gender; } set{ gender = value; }}
	[SerializeField]
	private string nickname;
	public string _nickname {get{ return nickname; } set{ nickname = value; }}
	[SerializeField]
	private string desc;
	public string _desc {get{ return desc; } set{ desc = value; }}
	[SerializeField]
	private int emailV;
	public int _emailV {get{ return emailV; } set{ emailV = value; }}

	[SerializeField]
	private string fechaInicial;
	public string _fechaInicial {get{ return fechaInicial; } set{ fechaInicial = value; }}

//	[SerializeField]
	private string fechaFinal;
	public string _fechaFinal {get{ return fechaFinal; } set{ fechaFinal = value; }}

	[SerializeField]
	private string numeracionHincha;
	public string _numeracionHincha {get{ return numeracionHincha; } set{ numeracionHincha = value; }}

	public string fechaNaciemiento;

	public bool MayorDeEdad;

	void Start(){
		if( DataApp.main.IsRegistered() )
			reloadDataInUser();
	}

	public void reloadDataInUser( ){
		_name = GetMyName();
		_lastname = GetMyLastName();
		_email = GetMyEmail();
		_ind_cel = GetMyIndcel();
		_cel = GetMyCel();
		_pass = GetMyPass();
		_country = GetMyCountry();
		_city = GetMyCity();
		_gender = GetMyGender();
		_nickname = GetMyNick();
		_desc = GetMyDesc();
		_fechaInicial = GetMyFechaIncial();
		_fechaFinal = GetMyFechaFinal();
		_numeracionHincha = GetMyNumH();
		_emailV = GetMyEmailVerif();
		MayorDeEdad = IsAdult();
		_numeracionHincha = GetMyNumH();
		fechaNaciemiento = GetMyBirthday();
	}

	public void reSaveDataInUser( string mail, string r_name, string r_lastname, string r_pass, string r_gender, string r_indcel, string r_cel, int r_countryid ,int r_cityid, string r_fechanacimiento, string r_desc, string r_nick, int emailv, string _fechainicial, string numh, string option){
	
		SetMyEmail(mail);
		SetMyName (r_name);	
		SetMyLastName(r_lastname);
		SetMyIndcel(r_indcel);
		SetMyCel(r_cel);
		SetMyPass(r_pass);
		SetMyCountry(r_countryid);
		SetMyCity(r_cityid);
		SetMyGender(r_gender);
		SetMyNick(r_nick);
		SetMyDesc(r_desc);
		SetMyBirthday( r_fechanacimiento );
		SetMyEmailVerif(emailv);
		SetMyFechaIncial(_fechainicial);
		SetMyNumH( numh);

		reloadDataInUser();


		if( option== "login" ){
			DataApp.main.popUpInformative( true, "Login Exitoso" , "Bienvenido,\n " + r_name +" "+r_lastname);
			NavigatorManager.main.HistorialClear( );
			NavigatorManager.main.saveIndex(0,0, "inicio");
		}else if(option == "update"){
			DataApp.main.popUpInformative( true, "Actualizacion Exitosa" , "");
		}else if( option== "registro" ){
//			DataApp.main.popUpInformative( true, "Registro Exitoso" , "Bienvenido,\n " + r_name +" "+r_lastname);
			NavigatorManager.main.HistorialClear( );
			NavigatorManager.main.saveIndex(0,0, "inicio");
		}
	}


	public IEnumerator getDatasUser( string url ,string opcData){
		
		print(url);
		yield return StartCoroutine( GetJsonDataScript.getJson.GetPhpData( url));
		if( GetJsonDataScript.getJson._state == "Successful" ){
			userDataList  = GetJsonDataScript.getJson.GetData( userDataList , "mail", "name", "lastname", "password", "gender",  "indcel","cel" , "countryid","cityid","fechaNacimiento", "description", "nick", "emailVerificado", "fechaVencimiento", "numeroHincha", "fechainicial");
			reSaveDataInUser(
				userDataList.dataList[0].GetValueToKey("mail"),
				userDataList.dataList[0].GetValueToKey("name"),
				userDataList.dataList[0].GetValueToKey("lastname"),
				userDataList.dataList[0].GetValueToKey("password"),
				userDataList.dataList[0].GetValueToKey("gender"),
				userDataList.dataList[0].GetValueToKey("indcel"),
				userDataList.dataList[0].GetValueToKey("cel"),
				int.Parse(userDataList.dataList[0].GetValueToKey("countryid")),
				int.Parse(userDataList.dataList[0].GetValueToKey("cityid")),
				userDataList.dataList[0].GetValueToKey("fechaNacimiento"),
				userDataList.dataList[0].GetValueToKey("description"),
				userDataList.dataList[0].GetValueToKey("nick"),
				int.Parse(userDataList.dataList[0].GetValueToKey("emailVerificado")),
				userDataList.dataList[0].GetValueToKey("fechainicial"),
				userDataList.dataList[0].GetValueToKey("numeroHincha"),
				opcData
			);
		}else if( GetJsonDataScript.getJson._state == "Warning_01" ){
			print("w1");
		}else if( GetJsonDataScript.getJson._state == "Warning_02" ){
			print("w2");
		}else if (GetJsonDataScript.getJson._state == "Warning_03"){
			StartCoroutine(getDatasUser ( url ,opcData ));
		}

//		userDataList.dataList[0].GetValueToKey("name");

	}


	public void DeleteDataUser( ){
		PlayerPrefs.DeleteKey ("MyName");
		PlayerPrefs.DeleteKey ("MylastName");
		PlayerPrefs.DeleteKey ("MyEmail");
		PlayerPrefs.DeleteKey ("MyCel");
		PlayerPrefs.DeleteKey ("MyCountry");
		PlayerPrefs.DeleteKey ("MyCity");
		PlayerPrefs.DeleteKey ("MyGender");
		PlayerPrefs.DeleteKey ("MyNick");
		PlayerPrefs.DeleteKey ("MyDesc");
		PlayerPrefs.DeleteKey ("MyDayId");
		PlayerPrefs.DeleteKey ("MyMonthId");
		PlayerPrefs.DeleteKey ("MyYearId");
		PlayerPrefs.DeleteKey ("MyEmailV");
		PlayerPrefs.DeleteKey ("MyDateInit");
		PlayerPrefs.DeleteKey ("MyDateFinal");
		PlayerPrefs.DeleteKey ("MyAge");
		PlayerPrefs.DeleteKey ("MyToken");
		PlayerPrefs.DeleteKey ("MyDateFinal");
		PlayerPrefs.DeleteKey ("MyNumH");
		PlayerPrefs.DeleteKey ("MyID");


		Debug.Log(DataApp.main.IsRegistered());
	}


	bool AgeValidate(string date){
		string [] f = date.Split('-');
		int y = int.Parse(f[0]);
		int m = int.Parse(f[1]);
		int d = int.Parse(f[2]); 

		bool isM = false;

		DateTime n = DateTime.Now;
		DateTime age = new DateTime(y,m,d);
		int year = n.Year - age.Year;
		year = ((Math.Abs(n.Month - age.Month)) > 0 )? year-1: year;
		isM =(year >= 18)? true: false;
		return isM;
	}


//	void SeparateBDate(string date ){
//		string temp= null;
//		int indexDate = 0;
//		for(int i = 0;  i < date.Length; i++){
//			if( date [i] == '-'){
//				if( indexDate == 0)
//					SetMyYear(int.Parse(temp));
//				if( indexDate == 1)
//					SetMyMonth(int.Parse(temp));
//				if( indexDate == 2)
//					SetMyDay(int.Parse(temp));
//
//				temp = "";
//			}else{
//				temp += date [i];
//			}
//		}
//
//
//
//
//	}

	/// 
	///     USER PLAYER PREFSSSS
 	/// 
	/// 


	#region NAME
	public void SetMyName (string name){
		PlayerPrefs.SetString ("MyName", name);
	}

	public string GetMyName (){
		return PlayerPrefs.HasKey("MyName") ? PlayerPrefs.GetString ("MyName"): "";
	}

	public bool IsNameRegister(){
		return PlayerPrefs.HasKey("MyName");
	}
	#endregion


	#region LASTNAME
	public void SetMyLastName (string lname){
		PlayerPrefs.SetString ("MylastName", lname);
	}

	public string GetMyLastName (){
		return PlayerPrefs.HasKey("MylastName") ? PlayerPrefs.GetString ("MylastName"): "";
	}

	public bool IsLastNameRegister(){
		return PlayerPrefs.HasKey("MylastName");
	}
	#endregion


	#region EMAIL
	public void SetMyEmail (string email){
		PlayerPrefs.SetString ("MyEmail", email);
	}

	public string GetMyEmail (){
		return PlayerPrefs.HasKey("MyEmail") ? PlayerPrefs.GetString ("MyEmail"): "";
	}

	public bool IsEmailRegister(){
		return PlayerPrefs.HasKey("MyEmail");
	}
	#endregion


	#region INDCEL
	public void SetMyIndcel (string indCel){
		PlayerPrefs.SetString ("MyIndCel", indCel);
	}

	public string GetMyIndcel (){
		return PlayerPrefs.HasKey("MyIndCel") ? PlayerPrefs.GetString ("MyIndCel"): "57";
	}

	public bool IsIndcelRegister(){
		return PlayerPrefs.HasKey("MyIndCel");
	}
	#endregion


	#region CELULAR
	public void SetMyCel (string cel){
		PlayerPrefs.SetString ("MyCel", cel);
	}

	public string GetMyCel (){
		return PlayerPrefs.HasKey("MyCel") ? PlayerPrefs.GetString ("MyCel"): "99999999999";
	}

	public bool IsCelRegister(){
		return PlayerPrefs.HasKey("MyCel");
	}
	#endregion


	#region PASSWORD
	public void SetMyPass (string pass){
		PlayerPrefs.SetString ("MyPass", pass);
	}

	public string GetMyPass (){
		return PlayerPrefs.HasKey("MyPass") ? PlayerPrefs.GetString ("MyPass"): "";
	}

	public bool IsPassRegister (){
		return PlayerPrefs.HasKey("MyPass");
	}
	#endregion


	#region COUNTRY
	public void SetMyCountry (int country){
		PlayerPrefs.SetInt ("MyCountry", country);
	}

	public int GetMyCountry (){
		return PlayerPrefs.HasKey("MyCountry") ? PlayerPrefs.GetInt ("MyCountry"): 0;
	}

	public bool IsCountryRegister (){
		return PlayerPrefs.HasKey("MyCountry");
	}
	#endregion


	#region CITY
	public void SetMyCity (int city){
		PlayerPrefs.SetInt ("MyCity", city);
	}

	public int GetMyCity (){
		return PlayerPrefs.HasKey("MyCity") ? PlayerPrefs.GetInt ("MyCity"): 0;
	}

	public bool IsCityRegister (){
		return PlayerPrefs.HasKey("MyCity");
	}
	#endregion


	#region GENERO
	public void SetMyGender (string gender ){
		PlayerPrefs.SetString ("MyGender", gender);
	}

	public string GetMyGender (){
		return PlayerPrefs.HasKey("MyGender") ? PlayerPrefs.GetString ("MyGender"): "Masculino";
	}

	public bool IsGenderRegister (){
		return PlayerPrefs.HasKey("MyGender");
	}
	#endregion


	#region NIKCNAME
	public void SetMyNick (string nick ){
		PlayerPrefs.SetString ("MyNick", nick);
	}

	public string GetMyNick (){
		return PlayerPrefs.HasKey("MyNick") ? PlayerPrefs.GetString ("MyNick"): "";
	}

	public bool IsNickResgister (){
		return PlayerPrefs.HasKey("MyNick");
	}
	#endregion


	#region DESC
	public void SetMyDesc (string desc ){
		PlayerPrefs.SetString ("MyDesc", desc);
	}

	public string GetMyDesc (){
		return PlayerPrefs.HasKey("MyDesc") ? PlayerPrefs.GetString ("MyDesc"): "";
	}

	public bool IsDescResgister (){
		return PlayerPrefs.HasKey("MyDesc");
	}
	#endregion


	#region DAY ID
	public void SetMyDayId ( int day ){
		PlayerPrefs.SetInt ("MyDayId", day);
	}

	public int GetMyDayId (){
		return PlayerPrefs.HasKey("MyDayId") ? PlayerPrefs.GetInt ("MyDayId"): 0;
	}

	public bool IsDayIdRegister (){
		return PlayerPrefs.HasKey("MyDayId");
	}
	#endregion


	#region MONTH ID
	public void SetMyMonthId ( int month ){
		PlayerPrefs.SetInt ("MyMonthId", month);
	}

	public int GetMyMonthId (){
		return PlayerPrefs.HasKey("MyMonthId") ? PlayerPrefs.GetInt ("MyMonthId"): 0;
	}

	public bool IsMonthIdRegister (){
		return PlayerPrefs.HasKey("MyMonthId");
	}
	#endregion


	#region YEAR ID
	public void SetMyYearId ( int year ){
		PlayerPrefs.SetInt ("MyYearId", year);
	}

	public int GetMyYearId ( ){
		return PlayerPrefs.HasKey("MyYearId") ? PlayerPrefs.GetInt ("MyYearId"): 0;
	}

	public bool IsYearIdRegister ( ){
		return PlayerPrefs.HasKey("MyYearId");
	}
	#endregion


	#region EMAILVERIFICADO
	public void SetMyEmailVerif ( int v ){
		PlayerPrefs.SetInt ("MyEmailV", v);
	}

	public int GetMyEmailVerif ( ){
		return PlayerPrefs.HasKey("MyEmailV") ? PlayerPrefs.GetInt ("MyEmailV"): 0;
	}

	public bool IsEmailVerifRegister ( ){
		return PlayerPrefs.HasKey("MyEmailV");
	}
	#endregion


	#region FECHA INICIAL
	public void SetMyFechaIncial (string date){
		PlayerPrefs.SetString ("MyDateInit", date);
	}

	public string GetMyFechaIncial (){
		return PlayerPrefs.HasKey("MyDateInit") ? PlayerPrefs.GetString ("MyDateInit"): "";
	}

	public bool IsFechaIncialRegister(){
		return PlayerPrefs.HasKey("MyDateInit");
	}
	#endregion


	#region FECHA FINAL
	public void SetMyFechaFinal (string date){
		PlayerPrefs.SetString ("MyDateFinal", date);
	}

	public string GetMyFechaFinal (){
		return PlayerPrefs.HasKey("MyDateFinal") ? PlayerPrefs.GetString ("MyDateFinal"): "";
	}

	public bool IsFechaFinalRegister(){
		return PlayerPrefs.HasKey("MyDateFinal");
	}
	#endregion


	#region MAYOR DE EDAD
	public void SetMyBirthday (string date){
		PlayerPrefs.SetString ("MyAge", date);
	}

	public string GetMyBirthday (){
		return PlayerPrefs.HasKey("MyAge") ? PlayerPrefs.GetString ("MyAge"): "";
	}

	public bool IsAdult(){
		return AgeValidate(PlayerPrefs.GetString ("MyAge"));
	}
	#endregion

	#region TOKEN
	public void SetMyToken (string tk){
		PlayerPrefs.SetString ("MyToken", tk);
	}

	public string GetMyToken (){
		return PlayerPrefs.HasKey("MyToken") ? PlayerPrefs.GetString ("MyToken"): "";
	}

	public bool IsTokenRegister(){
		return PlayerPrefs.HasKey("MyToken");
	}
	#endregion

	#region HINCHA OFICILA NUMERACION

	public void SetMyNumH (string name){
		PlayerPrefs.SetString ("MyNumH", name);
	}

	public string GetMyNumH (){
		return PlayerPrefs.HasKey("MyNumH") ? PlayerPrefs.GetString ("MyNumH"): "";
	}

	public bool IsNumHRegister(){
		return PlayerPrefs.HasKey("MyNumH");
	}
	#endregion
}
