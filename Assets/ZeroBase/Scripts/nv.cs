using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class nv : MonoBehaviour
{
    private Transform targetTr;
    private NavMeshAgent _nvAgent;
    // Start is called before the first frame update
    void Start()
    {
        targetTr = GameObject.Find("Portal").transform;
    }
    private void OnEnable()
    {
        _nvAgent = GetComponent<NavMeshAgent>();
        _nvAgent.SetDestination(targetTr.position);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
