using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum EBlockType{ Office, OfficeOld, Park, Appartment };

[ExecuteInEditMode]
public class CityBlockGeneration : MonoBehaviour
{
    public EBlockType blockType;
    [SerializeField] float[] possibleRotations;

    [SerializeField] Transform centrePosition;

    [Header("List of Available Prefabs")]
    [SerializeField] GameObject[] officePrefabs;
    [SerializeField] GameObject[] officeOldPrefabs;
    [SerializeField] GameObject[] parkPrefabs;
    [SerializeField] GameObject[] appartmentPrefabs;
    


    public void SpawnMyBlock()
    {
        //Spawn my centre piece.
        Instantiate(PickMyCentre(), centrePosition.position, Quaternion.Euler(0, possibleRotations[Random.Range(0,possibleRotations.Length - 1)] ,0), centrePosition);
    }

    private GameObject PickMyCentre()
    {
        GameObject myCentre = null;

        //Based on my block type, set by the world generator, pick one of the available prefabs to spawn.
        if (blockType == EBlockType.Office)
            myCentre = officePrefabs[Random.Range(0, (officePrefabs.Length - 1))];
        else if (blockType == EBlockType.OfficeOld)
            myCentre = officeOldPrefabs[Random.Range(0, (officeOldPrefabs.Length - 1))];
        else if (blockType == EBlockType.Park)
            myCentre = parkPrefabs[Random.Range(0, (parkPrefabs.Length - 1))];
        else if (blockType == EBlockType.Appartment)
            myCentre = appartmentPrefabs[Random.Range(0, (appartmentPrefabs.Length - 1))];
        else
            Debug.Log("[ERROR] Tile: " + this.name + ", has no city block type and will not spawn!");

       //Return the chosen prefab.
        return myCentre;
    }
}