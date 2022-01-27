using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pillar : MonoBehaviour
{
    [SerializeField] private Transform bottomPillarTrans = null;

    void Update()
    {
        bottomPillarTrans.eulerAngles += Vector3.up * 30f * Time.deltaTime;
    }
}
