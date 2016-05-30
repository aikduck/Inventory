using UnityEngine;
using System.Collections;
using LitJson;
using System.IO;
using System.Collections.Generic;

public class Stat
{
	public int Power { get; set; }
	public int Defence { get; set; }
	public int Vitality { get; set; }
}

//Class to define our item
public class ItemData
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public int Value { get; set; }
	public Stat Stats { get; set; }
	public bool Stackable { get; set; }
	public string Slug { get; set; }
	public Sprite sprite { get; set; }
	public GameObject gameObject { get; set; }

	public ItemData(JsonData data)
	{
		Id = (int)data ["id"];
		Title = data ["title"].ToString ();
		Description = data ["description"].ToString ();
		Value = (int)data ["value"];
		Stats = new Stat ();
		Stats.Power = (int)data ["stats"] ["power"];
		Stats.Defence = (int)data ["stats"] ["defence"];
		Stats.Vitality = (int)data ["stats"] ["vitality"];
		Stackable = (bool)data ["stackable"];
		Slug = data ["slug"].ToString ();
		sprite = Resources.Load<Sprite> ("Sprites/Items/" + Slug);
	}
}


public class ItemDatabase : MonoBehaviour {

	private List<ItemData> database = new List<ItemData> ();
	//Holds JSON data we pull in from the scene
	private JsonData itemData;

	// Use this for initialization
	void Start () 
	{
		//Read in from JSON file
		string jsonFilePath = Application.dataPath + "/StreamingAssets/items.json";
		string jsonData = File.ReadAllText (jsonFilePath);
			
		//Convert JSON data to object
		itemData = JsonMapper.ToObject(jsonData);

		//Construct item database
		ConstructItemDatabase ();
	}

	void ConstructItemDatabase()
	{
		//Loop through all items from JSON data
		for (int i = 0; i < itemData.Count; i++) 
		{
			database.Add (new ItemData (itemData[i]));
		}

		//Add each to database list
	}

	public ItemData GetItemById(int id)
	{
		for (int i = 0; i < itemData.Count; i++) 
		{
			if (database [i].Id == id) 
			{
				return database [i];
			}
		}

		print("invalid item id");
		return null;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
