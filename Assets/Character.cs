using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private GameObject selectedItem;
    [Header("UI Management")]
    [SerializeField] private Text collectionTitle;
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
    [SerializeField] private CollectionItem[] weapons;

    public void HairCollection()
    {
        collectionTitle.text = "Hair Slot";
        List<GameObject> items = new();
        foreach (CollectionItem collectionItem in hair)
        {
            GameObject newItem = Instantiate(collectionPrefab);
            AssignData(newItem, collectionItem);
            items.Add(newItem);
        }
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
        item.GetComponent<Collection>().name = data.GetName();
        item.GetComponent<Collection>().model = data.GetModel();
        item.GetComponent<Collection>().materials = data.GetMaterials();
        switch(data.GetRarity().ToString())
        {
            case "standard": item.GetComponent<Collection>().rarity = standard; break;
            case "common": item.GetComponent<Collection>().rarity = common; break;
            case "uncommon": item.GetComponent<Collection>().rarity = uncommon; break;
            case "rare": item.GetComponent<Collection>().rarity = rare; break;
            case "scarce": item.GetComponent<Collection>().rarity = scarce; break;
            case "collectible": item.GetComponent<Collection>().rarity = collectible; break;
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