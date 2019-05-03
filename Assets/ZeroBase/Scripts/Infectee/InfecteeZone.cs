using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum stage { STAGE_1, STAGE_2, STAGE_3, STAGE_4, STAGE_5 };

public class InfecteeZone : MonoBehaviour
{
    public float rangeX, rangeZ;
    public stage spawnStage; 
    
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(rangeX, 0.1f, rangeZ);
    }
}
