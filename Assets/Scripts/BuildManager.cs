using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class BuildManager : MonoBehaviour
{
    public static BuildManager main;

    [Header("References")]
    [SerializeField] private Torres[] torre; 

    private int defensaSelected = 0;


    private void Awake()
    {
        main = this;
    }

    public Torres GetSelectedDefensa()
    {
        return torre[defensaSelected];
    }

    public void SetSelectedDefensa(int indexTorre)
    {
        defensaSelected = indexTorre;
    }
}