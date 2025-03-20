using UnityEngine;

public class Collection : MonoBehaviour
{
    [HideInInspector] public new string name;
    [HideInInspector] public Mesh model;
    [HideInInspector] public Material[] materials;
    [HideInInspector] public Color rarity;
}