using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Threading;
using Facebook.Unity;
using FFmpeg.AutoGen;
using MaterialUI;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;


///  -------------------- arreglos para mañana

/// ARREGLAR EL PANEL TU ELIGES

/// PROBAR CARGAS ANTES DE SPLASH PARA AHORRAR LATENCIAS
/// 
/// VALIDAR NAVEGACION COMPLETA DE TODA LA APP
/// 
/// CAMBIAR EL INAPPBROWSER O MIRAR SI SE PUEDE CERRA PARA EL BOTON VERIFICAR EMAIL 

/// CREAR LAS DOS PAGINAS DE JUEGOS 
/// HABILITAR EL BOTON DE IR A GOL TRICOLOR
/// CAMBIAR ARTE DE POLLA TRICOLOR
/// ARREGLAR NAVEGACION DE PERFIL CUANDO GAURDO UNA NUEVA FOTO O LA CANCELO
/// </summary>

enum optionSendEmail
{
    nosend = 0,
        send = 1,
        error = 2,
        cancel = 3
}
public class RegisterLoginManager : MonoBehaviour
{

    public static RegisterLoginManager main;

    string urlRegisterAndoLogin;
    [HideInInspector]
    public InputField t_name, t_lastname, t_email, t_indcel, t_cel, t_pass, t_confirmpass;
    [HideInInspector]
    public MaterialDropdown d_day, d_month, d_year;
    public Toggle t_terminos;
    //	[HideInInspector]
    public Dropdown d_country, d_city;
    [HideInInspector]
    public string gender;

    public GameObject panelRegister, PanelLogin;

    public InputField login_email, login_pass;
    public Button LoginBtn;
    public CanvasGroup verificarButton;

    Texture[] mailtextures = new Texture[3];
    optionSendEmail optMail = optionSendEmail.nosend;

    RegexUtilities utilmail = new RegexUtilities ();
    public RawImage maiLRaw;

    private string getUrlPos = "Analytics/PostInstalacionesDiarias.php";
    private int userID = 0;

    void Awake ()
    {
        urlRegisterAndoLogin = DataApp.main.host + "Registro/registerandlogin.php";

        if (!FB.IsInitialized)
        {
            FB.Init (InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp ();
        }
    }

    void Start ()
    {
        LoginBtn.onClick.AddListener (() => LoginUser (login_email, login_pass));
        downloadImgMailing ();
    }

    #region FB METHODS
    private void InitCallback ()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp ();
        }
        else
        {

        }
    }

    private void OnHideUnity (bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void CallFBLogin ()
    {
        List<string> perms = new List<string> (new string[] { "public_profile", "email", "user_friends" });
        FB.LogInWithReadPermissions (perms, HandleResult);
        DataApp.main.EnableLoading ();
    }

    private void FetchFBProfile ()
    {
        FB.API ("/me?fields=first_name,last_name,email", HttpMethod.GET, FetchProfileCallback);
    }

    private void HandleResult (ILoginResult result)
    {
        Debug.Log ("logeado? :  " + FB.IsLoggedIn);
        if (FB.IsLoggedIn)
        {
            FetchFBProfile ();
        }
        else
        {
            DataApp.main.DisableLoading ();
            //  return;
        }
    }

    private void FetchProfileCallback (IResult result)
    {

        string mailResponse = "";
        string nameResponse = "";
        string lnameResponse = "";

        if (result.Error == null)
        {
            Debug.Log ("Obtain: " + result.RawResult);
            Dictionary<string, object> FBUserDetails = (Dictionary<string, object> ) result.ResultDictionary;
            mailResponse = FBUserDetails["email"].ToString ();
            nameResponse = FBUserDetails["first_name"].ToString ();
            lnameResponse = FBUserDetails["last_name"].ToString ();
            StartCoroutine (confirmDatasAndMailFb (true, mailResponse, nameResponse, lnameResponse));
        }
        else
        {
            Debug.Log ("Error FetchProfileCallback");
        }
    }
    #endregion

    IEnumerator PostNewUser ()
    {

        int plataform = 0;

#if UNITY_ANDROID
        plataform = 1;
#elif UNITY_IOS
        plataform = 2;
#endif

        DateTime nt = DateTime.Now;
        string fechaInit = nt.Year + "-" + nt.Month + "-" + nt.Day;

        string tk = TokenGenerator ();

        int month = parseMonth (d_month.buttonTextContent.text);

        StartCoroutine (DataApp.main.Consult (urlRegisterAndoLogin, new Dictionary<string, string>
        { { "indata", "nuevoUsuario" },
            { "nombre", t_name.text },
            { "apellido", t_lastname.text },
            { "correo", t_email.text },
            { "pass", t_pass.text },
            { "indcelular", t_indcel.text },
            { "celular", t_cel.text },
            { "paisid", d_country.value.ToString () },
            { "pais", d_country.captionText.text },
            { "ciudadid", d_city.value.ToString () },
            { "ciudad", d_city.captionText.text },
            { "fechaNacimiento", d_year.buttonTextContent.text + "-" + month + "-" + d_day.buttonTextContent.text },
            { "plataforma", 1. ToString () },
            { "app", 1. ToString () },
            { "genero", gender },
            { "version", DataApp.main.versionApp.ToString () },
            { "token", tk },
            { "fechainit", fechaInit }
        }, result =>
        {
            Debug.Log (urlRegisterAndoLogin + "indata=nuevoUsuario&nombre=" + t_name.text + "&apellido=" + t_lastname.text + "&correo=" + t_email.text + "&pass=" + t_pass.text + "&indcelular=" + t_indcel.text + "&celular=" + t_cel.text + "&paisid=" + d_country.value + "&pais=" + d_country.captionText.text + "&ciudadid=" + d_city.value + "&ciudad=" + d_city.captionText.text + "&fechaNacimiento=" + d_year.buttonTextContent.text + "/" + d_month.buttonTextContent.text + "/" + d_day.buttonTextContent.text + "&plataforma=" + 1 + "&app=" + 1 + "&genero=" + gender + "&version=" + DataApp.main.versionApp.ToString () + "&token=" + tk + "&fechainit=" + fechaInit);
            int number;
            bool result_ = Int32.TryParse (result, out number);
            if (!string.IsNullOrEmpty (result) && result_)
            {
                DataApp.main.SetMyID (result);
                User.main.SetMyFechaIncial (fechaInit);
                User.main.SetMyBirthday (d_year.buttonTextContent.text + "-" + month + "-" + d_day.buttonTextContent.text);
                //				SendEmailto( t_email.text , tk, false);
                mailSent = true;
                StartCoroutine (OpenOptionEmailVerification (mailSent));
                SetDatesUser (t_name.text, t_lastname.text, t_email.text, t_pass.text, t_indcel.text, t_cel.text, d_country.value, d_city.value, d_day.currentlySelected, d_month.currentlySelected, d_year.currentlySelected, plataform, 1, gender, result);
                User.main.SetMyToken (tk);
            }
            else
            {
                //				ToastManager.Show("Error al Registrarte, Intentalo de nuevo.",5f,null);
                DataApp.main.DisableLoading ();
            }
        }));
        yield return new WaitForEndOfFrame ();
    }

    int parseMonth (string month)
    {
        int rMonth = 0;
        rMonth = (month == "Enero") ? 1 : (month == "Febrero") ? 2 : (month == "Marzo") ? 3 : (month == "Abril") ? 4 : (month == "Mayo") ? 5 : (month == "Junio") ? 6 : (month == "Julio") ? 7 : (month == "Agosto") ? 8 : (month == "Septiembre") ? 9 : (month == "Octubre") ? 10 : (month == "Noviembre") ? 11 : 12;
        return rMonth;
    }

    string saveFechaInicial ()
    {
        DateTime dt = new DateTime ();
        dt = DateTime.Now;
        string date = (dt.Month == 1) ? "ENE" : (dt.Month == 2) ? "FEB" : (dt.Month == 3) ? "MAR" : (dt.Month == 4) ? "ABRIL" : (dt.Month == 5) ? "MAY" : (dt.Month == 6) ? "JUN" : (dt.Month == 7) ? "JUL" : (dt.Month == 8) ? "AGOS" : (dt.Month == 9) ? "SEPT" : (dt.Month == 10) ? "OCT" : (dt.Month == 11) ? "NOV" : "DIC";
        return date + " " + dt.Year;
    }

    void SetDatesUser (string name, string lname, string email, string pass, string indcel, string cel, int country, int city, int day, int month, int year, int plataform, int app, string gender, string hincha_n)
    {

        User.main.SetMyName (name);
        User.main.SetMyLastName (lname);
        User.main.SetMyEmail (email);
        User.main.SetMyPass (pass);
        User.main.SetMyIndcel (indcel);
        User.main.SetMyCel (cel);
        User.main.SetMyCountry (country);
        User.main.SetMyCity (city);
        User.main.SetMyDayId (day);
        User.main.SetMyMonthId (month);
        User.main.SetMyYearId (year);
        User.main.SetMyGender (gender);
        User.main.SetMyNumH (hincha_n);

        User.main.reloadDataInUser ();

        StartCoroutine (analyticsInstalacionesDiarias (name, email));
    }

    private IEnumerator analyticsInstalacionesDiarias (string name, string email)
    {
        userID = DataApp.main.GetMyID ();
        WWWForm form = new WWWForm ();
        form.AddField ("userID", userID);
        form.AddField ("correo", email);
        form.AddField ("name", name);
        form.AddField ("version", DataApp.main.versionApp);
        WWW www = new WWW (DataApp.main.host + getUrlPos, form);
        yield return www;
    }

    public void SendEmailto (string emailreceiver, string tk, bool rv)
    {
        string sender = "programacion.wise@wiseinmedia.com";
        string smtpPassword = "wise1234";
        string smtpHost = "smtp.gmail.com";

        //		string pathHeader = Application.persistentDataPath + "/Mail/header.jpg";
        string[] attachmentPath = new string[1];

        attachmentPath[0] = Application.persistentDataPath + "/Mail/body.jpg";
        //		attachmentPath[1]  = Application.persistentDataPath + "/Mail/body.jpg";

        MailMessage mail = new MailMessage ();
        mail.IsBodyHtml = true;
        mail.AlternateViews.Add (getImageBody (null, attachmentPath, tk));
        mail.From = new MailAddress (sender);
        mail.Subject = "Selección Colombia App";
        mail.Body = "Welcomo to Selección Colombia Oficial";
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.To.Add (emailreceiver);

        var smtpServer = new SmtpClient ()
        {
            Host = "smtp.gmail.com",
                EnableSsl = true,
                Port = 25,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = (ICredentialsByHost) new NetworkCredential (sender, smtpPassword)
        };
        smtpServer.SendCompleted += new SendCompletedEventHandler (SendCompletedCallback);
        //		ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        string userState = "..." + t_email.text;
        smtpServer.SendAsync (mail, userState);
        mail.Dispose ();

        StartCoroutine (asynFailEmail ());
        Debug.Log ("boleano en " + rv);
        StartCoroutine (OpenOptionEmailVerification (rv));
    }

    void downloadImgMailing ()
    {
        //		mailtextures[0]  = ImgLoadManager.main.MailImg(maiLRaw,"header",false);
        mailtextures[0] = ImgLoadManager.main.MailImg (maiLRaw, "body", false);
        //		mailtextures[2]  = ImgLoadManager.main.MailImg(maiLRaw,"2body",false);
    }

    bool mailSent = false;
    private void SendCompletedCallback (object sender, AsyncCompletedEventArgs e)
    {
        String token = (string) e.UserState;
        if (e.Cancelled) { }
        optMail = optionSendEmail.cancel;
        if (e.Error != null)
        {
            optMail = optionSendEmail.error;
        }
        else
        {
            optMail = optionSendEmail.send;
        }
        mailSent = true;
    }

    IEnumerator asynFailEmail ()
    {
        for (int i = 0; i < 10; i++)
        {
            Debug.Log (": " + mailSent + " en seg:  " + i);
            if (mailSent)
                break;
            if (i == 6)
            {
                SnackbarManager.Show ("El registro esta demorando más de lo normal, Por favor Espere", 5f, "Continuar", () =>
                {
                    SnackbarManager.OnSnackbarCompleted ();
                });
            }
            yield return new WaitForSecondsRealtime (1);
        }

        if (!mailSent)
        {
            DataApp.main.popUpInformative (true, "Error al registrar su correo", "Intenta más tarde re-enviar su correo de activación.");
            yield return new WaitForSecondsRealtime (1);
            mailSent = true;
        }

    }

    IEnumerator OpenOptionEmailVerification (bool reenviar)
    {
        yield return new WaitUntil (() => mailSent); // ARREGLAR PAR QUE SE PUEDA ENVIAR EL CORREO
        Debug.Log ("enviado el puto email con resultado en " + mailSent);
        if (DataApp.main.IsRegistered ())
        {
            if (reenviar)
            {
                StartCoroutine (User.main.getDatasUser (DataApp.main.host + "Registro/userDatas.php?indata=userDatas&idUser=" + DataApp.main.GetMyID (), "registro"));
            }

            if (optMail == optionSendEmail.error)
                ToastManager.Show ("Se ha producido un Error al enviar el correo electronico", 5f, null);
            else if (optMail == optionSendEmail.send && !reenviar)
                DataApp.main.ActivePopUpRegistroInfo (true);
            ToastManager.Show ("Registro éxitoso. Bienvenido", 5f, null);
            //				SnackbarManager.Show("Correo enviado,\nDeseas verificar tu correo electronico?",8f, "Verificar", () => {
            //					OpenMail();
            //					SnackbarManager.OnSnackbarCompleted();
            ////					ToastManager.Show("Aqui Abro el navegador",15f,null);
            ////					Application.OpenURL("www.gmail.com");
            //				});
            StartCoroutine (EnablBtnVerif ());

        }
        else
        {
            DataApp.main.popUpInformative (true, "Error al registrarse", "Intenta registrarte de nuevo.");
        }
        //		mailSent = false;
        DataApp.main.DisableLoading ();
    }

    IEnumerator EnablBtnVerif ()
    {
        yield return new WaitForSecondsRealtime (6f);
        for (float i = 0.0f; i <= 1.0f; i += 0.01f)
        {
            yield return new WaitForEndOfFrame ();
            verificarButton.alpha = i;
        }
    }

    //	public void OpenMail( ){
    //
    //		InAppBrowser.DisplayOptions dis = new InAppBrowser.DisplayOptions();
    //		dis.displayURLAsPageTitle = false;
    //		dis.backButtonText = "Volver a S.C";
    //		dis.pageTitle = "Verificacion de E-mail";
    //		dis.barBackgroundColor = "#FBB100";
    //		dis.textColor = "#000000";
    //
    //		bool isSave = false;
    //		string stmp=null;
    //		string mail =  User.main.GetMyEmail();
    //		for(int i = 0; i < mail.Length;i++){
    //			if(isSave == true)
    //				stmp += mail[i];
    //			if(mail[i] == '@')
    //				isSave = true;
    //		}
    //		InAppBrowser.OpenURL("https://www."+stmp, dis);
    //	}

    public void OpenMail ()
    {
        bool isSave = false;
        string stmp = null;
        string mail = User.main.GetMyEmail ();
        for (int i = 0; i < mail.Length; i++)
        {
            if (isSave == true)
                stmp += mail[i];
            if (mail[i] == '@')
                isSave = true;
        }
        Application.OpenURL ("https://www." + stmp);
        Debug.Log ("EMAIL DE : " + stmp);
    }

    private AlternateView getImageBody (string pathHeader, string[] filePath, string token)
    {
        List<LinkedResource> inlines = new List<LinkedResource> (0);
        LinkedResource inline;
        //BODY INIT

        string linkValid = DataApp.main.host + "Registro/registerandlogin.php?indata=changeMail&iduser=" + DataApp.main.GetMyID () + "&token=" + token;

        string htmlBody = null;
        if (!string.IsNullOrEmpty (pathHeader))
        {
            inline = new LinkedResource (pathHeader);
            inline.ContentId = Guid.NewGuid ().ToString ();
            inlines.Add (inline);
            htmlBody += @"<body style=""background-color:white;""</body> <center > < a href = "" //www.fcf.com.co/""><img  src='cid:" + inline.ContentId + @"'/></a></center><center > < br > < font face = ""verdana"" color = ""black"" > < h2 > Bienvenido, < b > "+ t_name.text+" "+t_lastname.text  +@" < /b> </font > < br / > < /h2></center >";
        }
        else
        {
            htmlBody += @"<body style=""background-color:white;""</body> <center > < br > < font face = ""verdana"" color = ""black"" > < h2 > Bienvenido, < b > "+ t_name.text+" "+t_lastname.text  + @" < /b> </font > < br / > < /h2></center > ";
        }

        foreach (string path in filePath)
        {
            inline = new LinkedResource (path);
            inline.ContentId = Guid.NewGuid ().ToString ();
            inlines.Add (inline);
        }

        int countInLines = 0;
        foreach (LinkedResource inl in inlines)
        {
            countInLines++;
            if ((!string.IsNullOrEmpty (pathHeader) && countInLines == 1))
            { }
            else
            {
                htmlBody += @"<center><a href=" + linkValid + @"><img  src='cid:" + inl.ContentId + @"'/></a></center>";
                htmlBody += @"<center><p><h5> </h5></p></center>";
            }
        }

        htmlBody += @"<br><br/> <
            center > < p > < h5 > < font face = ""verdana"" color = ""gray"" > < a href = "" //www.fcf.com.co/""> Políticas de Privacidad </a> </h5></p></center>
            <
            center > < p > < h5 > < font face = ""verdana"" color = ""gray"" > Federación Colombiana de Futbol, Inc.·Bogotá, Colombia < /font></h
        5 > < /p></center > ";

        AlternateView alternateView = AlternateView.CreateAlternateViewFromString (htmlBody, null, MediaTypeNames.Text.Html);

        //		<center><b> <h2>Para terminar tu proceso de registro, Haz click<a href=+ linkValid +@> AQUI </a></h2></b></center>
        foreach (LinkedResource inl in inlines)
        {
            alternateView.LinkedResources.Add (inl);
        }
        return alternateView;
    }

    public void PostUser ()
    {
        DataApp.main.EnableLoading ();
        _validDatas ();
    }

    void _validDatas ()
    {
        bool validDatas = false;

        if (string.IsNullOrEmpty (t_name.text))
        {
            DataApp.main.popUpInformative (true, "Nombre no Valido", "Ingresa tu nombre.");
        }
        else if (string.IsNullOrEmpty (t_lastname.text))
        {
            DataApp.main.popUpInformative (true, "Apellido no Valido", "Ingresa tu Apellido.");
        }
        else if (!utilmail.IsValidEmail (t_email.text))
        {
            DataApp.main.popUpInformative (true, "Correo no Valido", "Tu correo electronico no existe o no es correcto.");
        }
        else if (string.IsNullOrEmpty (t_pass.text))
        {
            DataApp.main.popUpInformative (true, "Contraseña no Valida", "Ingresa tu Contraseña.");
        }
        else if (t_pass.text.Length < 6)
        {
            DataApp.main.popUpInformative (true, "Contraseña no Valida", "La contraseña debe tener minimo 6 digitos.");
        }
        else if (string.IsNullOrEmpty (t_confirmpass.text) || t_confirmpass.text != t_pass.text)
        {
            DataApp.main.popUpInformative (true, "Contraseña no Valida", "La Contraseña no coincide.");
        }
        else if (string.IsNullOrEmpty (t_indcel.text))
        {
            DataApp.main.popUpInformative (true, "Indicador no Valido", "Ingresa el indicador de tu país.");
        }
        else if (string.IsNullOrEmpty (t_cel.text))
        {
            DataApp.main.popUpInformative (true, "Celular no Valido", "Ingresa tu número de celular.");
        }
        else if (d_country.value < 0)
        {
            DataApp.main.popUpInformative (true, "País no Valido", "Selecciona tu país de nacimiento.");
        }
        else if (d_city.value < 0)
        {
            DataApp.main.popUpInformative (true, "Ciudad no Valida", "Selecciona tu ciudad de nacimiento.");
        }
        else if (d_day.currentlySelected == -1)
        {
            DataApp.main.popUpInformative (true, "Día no Valido", "Ingresa el día de tu nacimiento.");
        }
        else if (d_month.currentlySelected == -1)
        {
            DataApp.main.popUpInformative (true, "Mes no Valido", "Ingresa el mes de tu nacimiento.");
        }
        else if (d_year.currentlySelected == -1)
        {
            DataApp.main.popUpInformative (true, "Año no Valido", "Ingresa el año de tu nacimiento.");
        }
        else if (string.IsNullOrEmpty (gender))
        {
            DataApp.main.popUpInformative (true, "Genero no Valido", "Selecciona tu genero.");
        }
        else if(!t_terminos.isOn)
        {
            DataApp.main.popUpInformative (true, "Políticas no valido", "Selecciona la casilla de politicas de privacidad.");
        }
        else
        {
            validDatas = true;
            StartCoroutine (DataApp.main.CheckInternet (internet =>
            {
                if (internet)
                    StartCoroutine (confirmDatasAndMail (validDatas));
                else
                    DataApp.main.popUpInformative (true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
            }));
        }
    }

    public void ChangeGender (Toggle g)
    {
        if (g.isOn)
        {
            gender = g.name;
        }
    }

    public void LoginUser (InputField mail, InputField pass)
    {
        DataApp.main.EnableLoading ();
        StartCoroutine (DataApp.main.CheckInternet (internet =>
        {
            if (internet)
            {
                if (utilmail.IsValidEmail (mail.text))
                    StartCoroutine (_loginUser (mail.text, pass.text));
                else
                    DataApp.main.popUpInformative (true, "Correo no Valido", "Tu correo electronico no es correcto.");
            }
            else
            {
                DataApp.main.popUpInformative (true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
            }

        }));
    }

    IEnumerator _loginUser (string mail, string pass)
    {
        WWW getId = new WWW (urlRegisterAndoLogin + "?indata=login&correo=" + mail + "&pass=" + pass);
        yield return getId;
        print (getId.text);
        string answerMail = getId.text.Trim ();
        if (string.IsNullOrEmpty (getId.error))
        {
            if (answerMail.Equals ("notExist"))
            {
                DataApp.main.popUpInformative (true, "Correo no registrado", mail + ", no esta registrado.");
            }
            else if (answerMail.Equals ("failpass"))
            {
                DataApp.main.popUpInformative (true, "Contraseña Incorrecta", " Vuelve a escribir tu contraseña.");
            }
            else
            {
                yield return StartCoroutine (User.main.getDatasUser (DataApp.main.host + "Registro/userDatas.php?indata=userDatas&idUser=" + answerMail, "login"));
                DataApp.main.SetMyID (answerMail);
            }
        }
        else
        {
            DataApp.main.popUpInformative (true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
        }
        //DataApp.main.DisableLoading();
    }

    private IEnumerator confirmDatasAndMailFb (bool validDts, string mail, string name, string lname)
    {
        if (validDts)
        {
            DataApp.main.EnableLoading ();
            WWW getMail = new WWW (urlRegisterAndoLogin + "?indata=validarMail&correo=" + mail);
            yield return getMail;
            print (getMail.text);
            string answerMail = getMail.text.Trim ();
            //si no existe el mail guarde los datos en la base de datos
            if (string.IsNullOrEmpty (getMail.error))
            {
                if (answerMail.Equals ("") == true)
                {
                    t_email.text = mail;
                    t_name.text = name;
                    t_lastname.text = lname;
                    SnackbarManager.Show ("Ingresa los datos faltantes en el registro.");
                    //						ActivatePanelLogin(true);
                }
                else
                {
                    login_email.text = mail;
                    NavigatorManager.main.saveIndex (14, 1, "");
                    NavigatorManager.main.panelsPrincipales[NavigatorManager.main.actualPanel]._enablePopUpInfoPanel (0);
                    //						SnackbarManager.Show("Ingresa los datos faltantes en el registro.");
                }
            }
            else
            {
                DataApp.main.popUpInformative (true, "Fallo en la conexíon.", "Revisa tu conexión a internet.");
            }
            DataApp.main.DisableLoading ();
        }
    }

    public void ContinueApp ()
    {
        SnackbarManager.OnSnackbarCompleted ();
        NavigatorManager.main.HistorialClear ();
        NavigatorManager.main.saveIndex (0, 0, "Inicio");
    }

    private IEnumerator confirmDatasAndMail (bool validDts)
    {
        if (validDts)
        {

            DataApp.main.EnableLoading ();
            string userMail = t_email.text;
            //				Debug.Log(userMail);
            WWW getMail = new WWW (urlRegisterAndoLogin + "?indata=validarMail&correo=" + userMail);
            yield return getMail;
            print (getMail.text);
            string answerMail = getMail.text.Trim ();
            //si no existe el mail guarde los datos en la base de datos
            if (answerMail.Equals ("") == true)
            {
                StartCoroutine (PostNewUser ());
            }
            //si existe el mail notifiquecelo al usuario
            else
            {
                DataApp.main.popUpInformative (true, "Este correo ya esta registrado", "Intenta registrarte con otro correo.");
                t_email.text = "";
            }
        }
    }

    IEnumerator validEmailVerification ()
    {
        if (DataApp.main.IsRegistered () && User.main.GetMyEmailVerif () == 0)
        {
            string userMail = t_email.text;
            WWW emailV = new WWW (urlRegisterAndoLogin + "?indata=verificarMail&iduser=" + DataApp.main.GetMyID ());
            yield return emailV;
            if (string.IsNullOrEmpty (emailV.error))
            {
                if (!string.IsNullOrEmpty (emailV.text) && emailV.text != "fail")
                {
                    User.main.SetMyEmailVerif (int.Parse (emailV.text));
                    User.main._emailV = User.main.GetMyEmailVerif ();
                    if (User.main.GetMyEmailVerif () == 1)
                    {
                        // DESACTIVAR LOS BOTONES(VERIFICAR Y CONTINUAR) CUANDO REGRESA A FOCO; 
                        ToastManager.Show ("Tu cuenta ha sido verificado exitosamente\nBienvenido a Selección Colombia Oficial");
                        yield return new WaitForSecondsRealtime (3);
                        DataApp.main.EnableLoading ();
                        DataApp.main.ActivePopUpRegistroInfo (false);
                        yield return new WaitForSecondsRealtime (2);
                        ContinueApp ();
                        DataApp.main.DisableLoading ();
                    }
                }
            }
        }
    }

    public string TokenGenerator ()
    {
        string tk = "";
        for (int i = 0; i < 9; i++)
        {
            int type = UnityEngine.Random.Range (0, 2);
            if (type == 1)
            {
                tk += UnityEngine.Random.Range (1, 9);
            }
            else
            {
                char c = (char) UnityEngine.Random.Range (65, 90);
                tk += c;
            }
        }
        return DateTime.Now.Year + "-" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Minute + "-" + tk;
    }

    public void disableCity ()
    {
        if (d_country.value > 0)
        {
            d_city.gameObject.SetActive (false);
        }
        else
        {
            d_city.gameObject.SetActive (true);
        }
    }

    void OnApplicationPause (bool pauseStatus)
    {
        if (!pauseStatus && DataApp.main.IsRegistered () && User.main.GetMyEmailVerif () == 0)
        {
            // HACER EL LLAMADO PARA VERIFICAR SI VALIDO LA CUENTA O NO;
            StartCoroutine (validEmailVerification ());
        }
        else
        { }
    }

    /// CAMBIAR PAGINAS DE REGISTRO 
    public TabView homeView;
    public void nextPage ()
    {
        homeView.SetPage (homeView.currentPage + 1);
    }

}
