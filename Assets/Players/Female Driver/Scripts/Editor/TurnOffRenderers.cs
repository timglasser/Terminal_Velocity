using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
 
public class TurnOffRenderers : ScriptableObject
{
     [MenuItem ("Window/TurnOffRenderers")]
    static void MenuDumpModels()
    {
        GameObject go = Selection.activeGameObject;
		go.AddComponent<Outfitter>();
		go.AddComponent<CharacterDemoController>();

		Transform[] tms = go.transform.GetComponentsInChildren<Transform>();
		
		foreach(Transform tm in tms)
		{
			if(tm.gameObject.GetComponent<Renderer>()!=null)
			{
				tm.gameObject.GetComponent<Renderer>().enabled = false;				
			}
		}

		Outfitter o = go.transform.GetComponent<Outfitter>();
		o.weapons = new List<WeaponSlot>();
		WeaponSlot weap = new WeaponSlot();
		for(int i = 0;i<9;i++)
		{
			weap = new WeaponSlot();
			o.weapons.Add(weap);
			o.weapons[i].models = new List<Renderer>();
		}

		foreach(Transform tm in tms)
		{
			if(tm.gameObject.name.ToLower() == "gladius02")
			{

				o.weapons[1].models.Add(tm.gameObject.GetComponent<Renderer>());
			}
			if(tm.gameObject.name.ToLower() == "longsword")
			{
				o.weapons[2].models.Add(tm.gameObject.GetComponent<Renderer>());
			}
			if(tm.gameObject.name.ToLower() == "bow")
			{
				o.weapons[3].models.Add(tm.gameObject.GetComponent<Renderer>());
			}
			if(tm.gameObject.name.ToLower() == "gladius02")
			{
				o.weapons[4].models.Add(tm.gameObject.GetComponent<Renderer>());
			}
			if(tm.gameObject.name.Contains("Pistol"))
			{
				o.weapons[5].models.Add(tm.gameObject.GetComponent<Renderer>());
			}
			if(tm.gameObject.name.Contains("SpaceRifle"))
			{
				o.weapons[6].models.Add(tm.gameObject.GetComponent<Renderer>());
			}
			if(tm.gameObject.name.ToLower() == "spear")
			{
				o.weapons[7].models.Add(tm.gameObject.GetComponent<Renderer>());
			}
			if(tm.gameObject.name.ToLower() == "broadsword")
			{
				o.weapons[8].models.Add(tm.gameObject.GetComponent<Renderer>());
			}
		}
		foreach(Transform tm in tms)
		{
			if(tm.gameObject.name.ToLower() == "gladius")
			{
				o.weapons[4].models.Add(tm.gameObject.GetComponent<Renderer>());
			}
			if(tm.gameObject.name.ToLower() == "arrow")
			{
				o.weapons[3].models.Add(tm.gameObject.GetComponent<Renderer>());
			}
			if(tm.gameObject.name.ToLower() == "shield01")
			{
				o.weapons[8].models.Add(tm.gameObject.GetComponent<Renderer>());
			}
		}
		CharacterDemoController cdc = go.transform.GetComponent<CharacterDemoController>();
		cdc.floorPlane = GameObject.Find("Plane");
		GameObject goct= GameObject.Find("CameraTarget");
		CamTarget ct = goct.GetComponent<CamTarget>();
		ct.target = go.transform;
    } 
}