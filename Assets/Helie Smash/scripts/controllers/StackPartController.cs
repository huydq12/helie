﻿using System.Collections;
using UnityEngine;

public class StackPartController : MonoBehaviour
{
    private Rigidbody rigid = null;
    private MeshRenderer meshRenderer = null;

    public bool IsDeadlyPart { private set; get; }
    public StackController StackControllerParent { private set; get; }



    private Vector3 originalLocalPos = Vector3.zero;
    private Vector3 originalLocalAngles = Vector3.zero;
    private void OnEnable()
    {
        if (meshRenderer == null)
            meshRenderer = GetComponent<MeshRenderer>();
        if (rigid == null)
            rigid = GetComponent<Rigidbody>();
        if (StackControllerParent == null)
            StackControllerParent = transform.parent.GetComponent<StackController>();

        originalLocalPos = transform.localPosition;
        originalLocalAngles = transform.localEulerAngles;
    }

    private void OnDisable()
    {
        rigid.isKinematic = true;
        transform.localPosition = originalLocalPos;
        transform.localEulerAngles = originalLocalAngles;
    }


    /// <summary>
    /// Loại bỏ tất cả đối tương con ra khỏi Stack 
    /// </summary>
    public void RemoveAllChilds()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).SetParent(null);
            i--;
        }
    }

    /// <summary>
    /// Đặt phần chết Stack 
    /// </summary>
    /// <param name="isDeadlyPart"></param>
    public void SetDeadlyPart(bool isDeadlyPart)
    {
        IsDeadlyPart = isDeadlyPart;
    }


    /// <summary>
    /// Đặt màu cho Stack
    /// </summary>
    /// <param name="material"></param>
    public void SetMaterial(Material material)
    {
        meshRenderer.material = material;
    }


    /// <summary>
    ///Làm vờ phần ngăn xếp này 
    /// </summary>
    public void Shatter()
    {
        rigid.isKinematic = false;
        StartCoroutine(CRHandleShattering());
    }
    private IEnumerator CRHandleShattering()
    {
        Vector3 forcePoint = transform.parent.position;
        float parentXPos = transform.parent.position.x;
        float xPos = meshRenderer.bounds.center.x;
        Vector3 subDir = (parentXPos - xPos < 0) ? Vector3.right : Vector3.left;
        Vector3 dir = (Vector3.up * 1.5f + subDir).normalized;
        yield return new WaitForFixedUpdate();
        float force = Random.Range(30f, 45f);
        float torque = Random.Range(110f, 180f);
        rigid.AddForceAtPosition(dir * force, forcePoint, ForceMode.Impulse);
        rigid.AddTorque(Vector3.left * torque);
        rigid.velocity = Vector3.down * 15f;
    }




    /// <summary>
    /// Get vị trí trung tâm Mesh 
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCenterMeshPosition()
    {
        return meshRenderer.bounds.center;
    }
}
