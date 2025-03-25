using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    [Header("Achievements")]
    [SerializeField] private Transform grid;
    [SerializeField] private GameObject achievementPrefab;
    [SerializeField] private Achievement[] achievements;


    public void OpenAchievements()
    {
        foreach (Achievement achievement in achievements)
        {
            GameObject item = Instantiate(achievementPrefab);
            AssignData(item.transform, achievement);
            item.transform.SetParent(grid);
            item.transform.localScale = Vector3.one;
        }

        grid.GetComponent<GridManager>().SortCollection();
    }

    private void AssignData(Transform item, Achievement achievement)
    {
        item.transform.GetChild(0).GetComponent<Image>().sprite = achievement.GetIcon();
        item.transform.GetChild(1).GetComponent<Text>().text = achievement.GetTitle();
        item.transform.GetChild(2).GetComponent<Text>().text = achievement.GetInfo();
    }    
}

[System.Serializable]
public class Achievement
{
    [SerializeField] private string name;
    [SerializeField] private string info;
    [SerializeField] private Sprite icon;

    public Sprite GetIcon()
    {
        return icon;
    }

    public string GetTitle()
    {
        return name;
    }

    public string GetInfo()
    {
        return info;
    }
}