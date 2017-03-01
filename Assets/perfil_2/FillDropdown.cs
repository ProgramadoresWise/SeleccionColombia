using UnityEngine;
using UnityEngine.	UI;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

public class FillDropdown : MonoBehaviour {

	Dropdown DropCountry;
	// Use this for initialization
	void Start () {
		DropCountry = this.GetComponent<Dropdown>();
		//CountryList();
	}


	public  List<string> CountryList( ){
		List<string> countrys = new List<string>( );
		CultureInfo [] getCulterInfo = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
		foreach (CultureInfo culture  in getCulterInfo){
			RegionInfo getRegionInfo = new RegionInfo(culture.Name);
			if( !(countrys.Contains(getRegionInfo.EnglishName))){
				string newC = getRegionInfo.EnglishName;
				Debug.Log(newC);
				countrys.Add(getRegionInfo.EnglishName);
			}
		}
		DropCountry.AddOptions(countrys);
		countrys.Sort();
//
//		for(int i = 0; i <countrys.Count; i++){
//			CultureInfo transCountry = new CultureInfo("es-ES");
//			countrys[i] = transCountry.EnglishName;
//			Debug.Log(countrys[i]);
//		}
//
		return countrys;
	}
}
