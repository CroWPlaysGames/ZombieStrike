using UnityEngine;

public class Collection : MonoBehaviour
{
    [HideInInspector] public new string name;
    [HideInInspector] public Mesh model;
    [HideInInspector] public Material[] materials;
    [HideInInspector] public Rarity rarity;
    [HideInInspector] public enum Rarity { standard, common, uncommon, rare, scarce, collectible }
    [HideInInspector] public Color color;
}