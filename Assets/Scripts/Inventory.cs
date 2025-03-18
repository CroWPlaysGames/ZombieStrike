using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private GameObject selectedItem;
    [Header("UI Management")]
    [SerializeField] private Text collectionTitle;
    [SerializeField] private Transform grid;
    [SerializeField] private Button color1;
    [SerializeField] private Button color2;
    [SerializeField] private Button color3;
    [Header("Rarity Management")]
    [SerializeField] private Color standard;
    [SerializeField] private Color common;
    [SerializeField] private Color uncommon;
    [SerializeField] private Color rare;
    [SerializeField] private Color scarce;
    [SerializeField] private Color collectible;
    [Header("Collection Management")]
    [SerializeField] private GameObject collectionPrefab;
    [SerializeField] private CollectionItem[] hair;
    [SerializeField] private CollectionItem[] face;
    [SerializeField] private CollectionItem[] shirts;
    [SerializeField] private CollectionItem[] jackets;
    [SerializeField] private CollectionItem[] backpacks;
    [SerializeField] private CollectionItem[] pants;
    [SerializeField] private CollectionItem[] shoes;
    [SerializeField] private CollectionItem[] starterWeapons;
    [SerializeField] private CollectionItem[] recoveredWeapons;

    public void OpenCollection(string set)
    {
        CollectionItem[] collectionSet = null;

        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
                
        switch(set)
        {
            case "hair":
                collectionSet = hair; 
                collectionTitle.text = "Hair Slot";                
                break;
            case "face":
                collectionSet = face;
                collectionTitle.text = "Face Slot";
                break;
            case "shirt":
                collectionSet = shirts;
                collectionTitle.text = "Shirt Slot";
                break;
            case "jacket":
                collectionSet = jackets;
                collectionTitle.text = "Jacket Slot";
                break;
            case "backpack":
                collectionSet = backpacks;
                collectionTitle.text = "Backpack Slot";
                break;
            case "pants":
                collectionSet = pants;
                collectionTitle.text = "Pants Slot";
                break;
            case "shoes":
                collectionSet = shoes;
                collectionTitle.text = "Shoes Slot";
                break;
            case "starterweapon":
                collectionSet = starterWeapons;
                collectionTitle.text = "Starter Weapon Slot";
                break;
            case "recoveredweapon":
                collectionSet = recoveredWeapons;
                collectionTitle.text = "Recovered Weapon Slot";
                break;
        }

        foreach (CollectionItem collectionItem in collectionSet)
        {
            GameObject item = Instantiate(collectionPrefab);            
            AssignData(item, collectionItem);
            item.transform.SetParent(grid);
            item.transform.localScale = Vector3.one;
        }

        grid.GetComponent<GridManager>().SortCollection();
    }

    public void CollectionItemSelect()
    {
        color1.enabled = false;
        color2.enabled = false;
        color3.enabled = false;

        switch (selectedItem.GetComponent<Collection>().materials.Length)
        {
            case 0:
                break;
            case 1:
                color1.enabled = true;
                color1.transform.position = Vector3.zero;
                color1.transform.Find("Value").GetComponent<Image>().color = selectedItem.GetComponent<Collection>().materials[0].color;
                break;
            case 2:
                color1.enabled = true;
                color2.enabled = true;
                color1.transform.position = new Vector3(-75, 0, 0);
                color2.transform.position = new Vector3(75, 0, 0);
                color1.transform.Find("Value").GetComponent<Image>().color = selectedItem.GetComponent<Collection>().materials[0].color;
                color2.transform.Find("Value").GetComponent<Image>().color = selectedItem.GetComponent<Collection>().materials[1].color;
                break;
            case 3:
                color1.enabled = true;
                color2.enabled = true;
                color3.enabled = true;
                color1.transform.position = new Vector3(-150, 0, 0);
                color2.transform.position = Vector3.zero;
                color3.transform.position = new Vector3(150, 0, 0);
                color1.transform.Find("Value").GetComponent<Image>().color = selectedItem.GetComponent<Collection>().materials[0].color;
                color2.transform.Find("Value").GetComponent<Image>().color = selectedItem.GetComponent<Collection>().materials[1].color;
                color3.transform.Find("Value").GetComponent<Image>().color = selectedItem.GetComponent<Collection>().materials[2].color;
                break;
        }
    }

    private void AssignData(GameObject item, CollectionItem data)
    {
        item.name = data.GetName();
        item.GetComponentInChildren<Text>().text = data.GetName();
        item.GetComponent<Collection>().name = data.GetName();
        item.GetComponent<Collection>().model = data.GetModel();
        item.GetComponent<Collection>().materials = data.GetMaterials();
        switch(data.GetRarity().ToString())
        {
            case "standard": 
                item.GetComponent<Collection>().rarity = standard;
                item.transform.Find("Rarity").GetComponent<Image>().color = standard; 
                break;
            case "common": 
                item.GetComponent<Collection>().rarity = common;
                item.transform.Find("Rarity").GetComponent<Image>().color = common;
                break;
            case "uncommon": 
                item.GetComponent<Collection>().rarity = uncommon;
                item.transform.Find("Rarity").GetComponent<Image>().color = uncommon;
                break;
            case "rare": 
                item.GetComponent<Collection>().rarity = rare;
                item.transform.Find("Rarity").GetComponent<Image>().color = rare;
                break;
            case "scarce": 
                item.GetComponent<Collection>().rarity = scarce;
                item.transform.Find("Rarity").GetComponent<Image>().color = scarce;
                break;
            case "collectible": 
                item.GetComponent<Collection>().rarity = collectible;
                item.transform.Find("Rarity").GetComponent<Image>().color = collectible;
                break;
        }
    }
}

[System.Serializable]
public class CollectionItem
{
    [Header("General Info")]
    [SerializeField] private string name;
    [SerializeField] private Mesh model;
    [SerializeField] private Rarity rarity;
    public enum Rarity { standard, common, uncommon, rare, scarce, collectible }
    [Header("Materials")]
    [SerializeField] private bool customizable;
    [SerializeField] private Material[] defaultMaterials;

    public string GetName()
    {
        return name;
    }

    public Mesh GetModel()
    {
        return model;
    }

    public Rarity GetRarity()
    {
        return rarity;
    }

    public Material[] GetMaterials()
    {
        return defaultMaterials;
    }
}