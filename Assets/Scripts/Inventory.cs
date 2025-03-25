using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [Header("UI Management")]
    [SerializeField] private Transform grid;
    [SerializeField] private Dropdown sort;
    [SerializeField] private GameObject collectionPrefab;
    [SerializeField] private GameObject color1;
    [SerializeField] private GameObject color2;
    [SerializeField] private GameObject color3;
    [Header("Rarity Management")]
    [SerializeField] private Color standard;
    [SerializeField] private Color common;
    [SerializeField] private Color uncommon;
    [SerializeField] private Color rare;
    [SerializeField] private Color scarce;
    [SerializeField] private Color collectible;
    [Header("Collection Management")]
    [SerializeField] private CollectionItem[] hats;
    [SerializeField] private CollectionItem[] face;
    [SerializeField] private CollectionItem[] shirts;
    [SerializeField] private CollectionItem[] jackets;
    [SerializeField] private CollectionItem[] gloves;
    [SerializeField] private CollectionItem[] backpacks;
    [SerializeField] private CollectionItem[] pants;
    [SerializeField] private CollectionItem[] shoes;
    [SerializeField] private CollectionItem[] starterWeapons;
    [SerializeField] private CollectionItem[] recoveredWeapons;
    [SerializeField] private CollectionItem[] meleeWeapons;


    public void OpenCollection(string category)
    {
        ClearGrid();
        CollectionItem[][] allClothes = { hats, face, shirts, jackets, gloves, backpacks, pants, shoes };
        CollectionItem[][] allWeapons = { starterWeapons, recoveredWeapons, meleeWeapons };

        switch (category)
        {
            case "Hats": AddToGrid(hats); break;
            case "Face": AddToGrid(face); break;
            case "Shirt": AddToGrid(shirts); break;
            case "Jacket": AddToGrid(jackets); break;
            case "Gloves": AddToGrid(gloves); break;
            case "Backpack": AddToGrid(backpacks); break;
            case "Pants": AddToGrid(pants); break;
            case "Shoes": AddToGrid(shoes); break;
            case "Starter Weapon": AddToGrid(starterWeapons); break;
            case "Recovered Weapon": AddToGrid(recoveredWeapons); break;
            case "Melee Weapon": AddToGrid(meleeWeapons); break;
            case "All Clothing": foreach (CollectionItem[] collections in allClothes) { AddToGrid(collections);  } break;
            case "All Weapons": foreach (CollectionItem[] collections in allWeapons) { AddToGrid(collections); } break;
        }
    }

    public void SelectItem(GameObject selectedItem)
    {
        color1.SetActive(false);
        color2.SetActive(false);
        color3.SetActive(false);

        switch (selectedItem.GetComponent<Collection>().materials.Length)
        {
            case 0:
                break;
            case 1:
                color1.SetActive(true);
                color1.transform.localPosition = Vector3.zero;
                color1.transform.Find("Color").Find("Value").GetComponentInChildren<Image>().color = selectedItem.GetComponent<Collection>().materials[0].color;
                break;
            case 2:
                color1.SetActive(true);
                color2.SetActive(true);
                color1.transform.localPosition = new Vector3(-75, 0, 0);
                color2.transform.localPosition = new Vector3(75, 0, 0);
                color1.transform.Find("Color").Find("Value").GetComponent<Image>().color = selectedItem.GetComponent<Collection>().materials[0].color;
                color2.transform.Find("Color").Find("Value").GetComponentInChildren<Image>().color = selectedItem.GetComponent<Collection>().materials[1].color;
                break;
            case 3:
                color1.SetActive(true);
                color2.SetActive(true);
                color3.SetActive(true);
                color1.transform.localPosition = new Vector3(-150, 0, 0);
                color2.transform.localPosition = Vector3.zero;
                color3.transform.localPosition = new Vector3(150, 0, 0);
                color1.transform.Find("Color").Find("Value").GetComponentInChildren<Image>().color = selectedItem.GetComponent<Collection>().materials[0].color;
                color2.transform.Find("Color").Find("Value").GetComponentInChildren<Image>().color = selectedItem.GetComponent<Collection>().materials[1].color;
                color3.transform.Find("Color").Find("Value").GetComponentInChildren<Image>().color = selectedItem.GetComponent<Collection>().materials[2].color;
                break;
        }

        //GameObject playerDisplay = GameObject.FindWithTag("Player").transform.Find(category).gameObject;
        //playerDisplay.GetComponent<MeshFilter>().mesh = selectedItem.GetComponent<Collection>().model;
        //playerDisplay.GetComponent<MeshRenderer>().materials = selectedItem.GetComponent<Collection>().materials;
    }

    private void AssignData(GameObject item, CollectionItem data)
    {
        Image rarity = item.transform.Find("Rarity").GetComponent<Image>();
        Text text = item.transform.Find("Name").gameObject.GetComponent<Text>();
        GameObject model = item.transform.Find("Model").gameObject;

        item.name = data.GetName();
        text.text = data.GetName();
        model.GetComponent<Transform>().SetLocalPositionAndRotation(data.GetPosition(), data.GetRotation());
        model.GetComponent<Transform>().localScale = data.GetScale() * 1000;
        model.GetComponent<MeshFilter>().mesh = data.GetModel();
        model.GetComponent<MeshRenderer>().materials = data.GetMaterials();
        

        switch (data.GetRarity().ToString())
        {
            case "standard": rarity.color = standard;  break;
            case "common": rarity.color = common; break;
            case "uncommon": rarity.color = uncommon; break;
            case "rare": rarity.color = rare; break;
            case "scarce": rarity.color = scarce; break;
            case "collectible": rarity.color = collectible; break;
        }

        item.GetComponent<Collection>().name = data.GetName();
        item.GetComponent<Collection>().model = data.GetModel();
        item.GetComponent<Collection>().materials = data.GetMaterials();
        item.GetComponent<Collection>().rarity = (Collection.Rarity)data.GetRarity();
        item.GetComponent<Collection>().color = rarity.color;
    }

    private void AddToGrid(CollectionItem[] collection)
    {
        foreach (CollectionItem collectionItem in collection)
        {
            GameObject item = Instantiate(collectionPrefab);
            AssignData(item, collectionItem);
            item.transform.SetParent(grid);
            item.transform.localScale = Vector3.one;
            item.GetComponent<Button>().onClick.AddListener(delegate { SelectItem(item); });
        }

        grid.GetComponent<GridManager>().SortCollection();
    }

    private void ClearGrid()
    {
        foreach (Transform child in grid)
        {
            Destroy(child.gameObject);
        }
    }
}

[System.Serializable]
public class CollectionItem
{
    [Header("General Info")]
    [SerializeField] private string name;
    [SerializeField] private Mesh model;
    [SerializeField] private Vector3 modelPosition;
    [SerializeField] private Vector3 modelRotation;
    [SerializeField] private float modelScale;
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

    public Vector3 GetPosition()
    {
        return modelPosition;
    }

    public Quaternion GetRotation()
    {
        return Quaternion.Euler(modelRotation);
    }

    public Vector3 GetScale()
    {
        return new Vector3(modelScale, modelScale, modelScale);
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