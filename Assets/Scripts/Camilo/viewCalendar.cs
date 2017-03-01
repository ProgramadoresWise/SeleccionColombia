using UnityEngine;
using System.Collections;
using UnityEngine.UI;



//[System.Serializable]


public class viewCalendar : MonoBehaviour {
	
	public calendarData managerCalendar;

	public int posId;
    public string nameTeam;
	public Text titleFinalmatchs;
    public Text nameTeam1;
    public Text nameTeam2;
    public string inicTeam;
    public Text score;
	public Text type;
	public Text dateMatch;
	public Text timeMatch;

    public Image imageCalendar;
    public RawImage imageBandera1;
    public RawImage imageBandera2;

    public Text title;
    public Text description;

    public int localVisitante;
    public int actualizar;

    public GameObject EspacioAbajo;


    float h;
    [SerializeField]
    public float sizeheight;
    public float sizewidth;
    [SerializeField]
    public float _w, _h;

	void Start( ){
//		this.GetComponent<LayoutElement>().preferredWidth = Screen.width;
//		if(Screen.width == 1440){
//			this.GetComponent<LayoutElement>().preferredWidth = 720;
//		}
	}


    public IEnumerator LoadImageInit()
    {
        if (posId != 1)
        { 
            EspacioAbajo.SetActive(false);
        }
        else
        {
            EspacioAbajo.SetActive(true);
        }

//        yield return StartCoroutine(ImgLoadManager.main.CalendarImg_d(imageCalendar, posId.ToString(), resultSprite => {
//            _w = imageCalendar.sprite.texture.width;
//            _h = imageCalendar.sprite.texture.height;
//            Rect dim = new Rect(0, 0, imageCalendar.sprite.texture.width, imageCalendar.sprite.texture.height);
//            imageCalendar.sprite = Sprite.Create(imageCalendar.sprite.texture, dim, Vector2.zero);
//            h = this.GetComponent<RectTransform>().sizeDelta.x;
//            sizewidth = RecalculateSize((int)_h, "w");
//            sizeheight = RecalculateSize((int)_h, "h");
//        }, false));

        if (actualizar == 1)
        {
			imageBandera1.texture = Resources.Load("Equipos/"+inicTeam) as Texture;
			imageBandera2.texture = Resources.Load("Equipos/"+inicTeam) as Texture;

//            imageBandera1.texture = ImgLoadManager.main.ShieldCalendarTexture(imageBandera1, inicTeam, true);
//            imageBandera2.texture = ImgLoadManager.main.ShieldCalendarTexture(imageBandera2, inicTeam, true);
            imageCalendar.sprite = ImgLoadManager.main.CalendarImg(imageCalendar, posId.ToString(), true);
        }
        else
        {
			imageBandera1.texture = Resources.Load("Equipos/"+inicTeam) as Texture;
			imageBandera2.texture = Resources.Load("Equipos/"+inicTeam) as Texture;

//            imageBandera1.texture = ImgLoadManager.main.ShieldCalendarTexture(imageBandera1, inicTeam, false);
//            imageBandera2.texture = ImgLoadManager.main.ShieldCalendarTexture(imageBandera2, inicTeam, false);
            imageCalendar.sprite = ImgLoadManager.main.CalendarImg(imageCalendar, posId.ToString(), false);
        }

        _w = imageCalendar.sprite.texture.width;
        _h = imageCalendar.sprite.texture.height;
        Rect newdim = new Rect(0, 0, imageCalendar.sprite.texture.width, imageCalendar.sprite.texture.height);
        imageCalendar.sprite = Sprite.Create(imageCalendar.sprite.texture, newdim, Vector2.zero);
        h = this.GetComponent<RectTransform>().sizeDelta.x;
        sizewidth = RecalculateSize((int)_h, "w");
        sizeheight = RecalculateSize((int)_h, "h");

        if (localVisitante == 1)
        {
            imageBandera2.enabled = true;
            imageBandera1.enabled = false;

            nameTeam2.text = nameTeam;
            nameTeam1.text = "COLOMBIA";
        }
        else
        {
            imageBandera2.enabled = false;
            imageBandera1.enabled = true;

            nameTeam1.text = nameTeam;
            nameTeam2.text = "COLOMBIA";
        }


        managerCalendar.canCreate = true;



        yield return new WaitForEndOfFrame();

    }

    public float RecalculateHeigth(int oldH)
    {
        float newH = 1024f;

        switch (oldH)
        {
            case 512:
                newH = (h == 1125) ? newH = 410 : (h == 1152) ? newH = 400 : ((int)h == 1079) ? newH = 400 : (h == 1080) ? newH = 400 : (h == 1200) ? newH = 430 : newH = oldH;
                sizeheight = 400;
                break;
            case 1024:
                newH = (h == 1125) ? newH = 800 : (h == 1152) ? newH = 830 : ((int)h == 1079) ? newH = 768 : (h == 1080) ? newH = 768 : (h == 1200) ? newH = 860 : newH = oldH;
                sizeheight = 562;
                break;
            case 1536:
                newH = (h == 1125) ? newH = 1200 : (h == 1152) ? newH = 1245 : ((int)h == 1079) ? newH = 1152 : (h == 1080) ? newH = 1152 : (h == 1200) ? newH = 1290 : newH = oldH;
                sizeheight = 800;
                break;
        }
        //		tituloNew.text = oldH + " x " + newH + " = "+ h;
        return newH / 2;
    }


    public float RecalculateSize(int oldH, string changeCalculate)
    {
        float newW = _w; // (float)Screen.width;
        float newH = _h; //(float)Screen.height;

        switch (Screen.width)
        {
            case 480:
                if (oldH == 512)
                {
                    newW = 480;
                    newH = 380;
                }
                else if (oldH == 1024)
                {
                    newW = 480;
                    newH = 565;
                }
                else if (oldH == 1536)
                {
                    newW = 480;
                    newH = 805;
                }
                break;
            case 600:
                if (oldH == 512)
                {
                    newW = 600;
                    newH = 460;
                }
                else if (oldH == 1024)
                {
                    newW = 600;
                    newH = 705;
                }
                else if (oldH == 1536)
                {
                    newW = 600;
                    newH = 1002;
                }
                break;
            case 800:
                if (oldH == 512)
                {
                    newW = 800;
                    newH = 600;
                }
                else if (oldH == 1024)
                {
                    newW = 800;
                    newH = 935;
                }
                else if (oldH == 1536)
                {
                    newW = 800;
                    newH = 1295;
                }
                break;
            case 1440:
                if (oldH == 512)
                {
                    newW = 1440;
                    newH = 1140;
                }
                else if (oldH == 1024)
                {
                    newW = 1440;
                    newH = 1700;
                }
                else if (oldH == 1536)
                {
                    newW = 1440;
                    newH = 2420;
                }
                break;
            case 720:
                if (oldH == 512)
                {
                    newW = 720;
                    newH = 570;
                }
                else if (oldH == 1024)
                {
                    newW = 720;
                    newH = 850;
                }
                else if (oldH == 1536)
                {
                    newW = 720;
                    newH = 1210;
                }
                break;
        }

        if (changeCalculate == "w")
            return newW;
        else
            return newH;
    }
    


    public void viewNewBig( )  {
		DataApp.main.EnableLoading();
		StartCoroutine  ( managerCalendar.bigView.BigCalendar( this ));
	}

}