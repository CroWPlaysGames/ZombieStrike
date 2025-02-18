using UnityEngine;
using UnityEditor;

public class Equipment : MonoBehaviour
{
    [SerializeField] private enum EquipmentItem{Medkit, Stimulant}
    [SerializeField] EquipmentItem equipmentItem;
    [SerializeField] private float healthRestore;
    [SerializeField] private AudioClip useEquipment;
    [SerializeField] private float useEquipmentVolume;

    public void UseEquipment()
    {
        switch (equipmentItem.ToString())
        {
            case "Medkit":
                break;
            case "Stimulant":
                break;            
        }
        FindAnyObjectByType<PlayerController>().currentHealth += healthRestore;
        //FindAnyObjectByType<AudioManager>().Play(heal, healVolume);
    }
}
