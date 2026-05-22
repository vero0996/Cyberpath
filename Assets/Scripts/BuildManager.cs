using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private GameObject[] DefensaPrefabs;
    
    private int defensaSelected = 0;


    private void Awake()
    {
        main = this;
    }

    public GameObject GetSelectedDefensa()
    {
        return DefensaPrefabs[defensaSelected];
    }
}