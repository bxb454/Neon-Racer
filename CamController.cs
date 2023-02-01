using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script is used to control the camera in the game.
/// </summary>
public class CamController : MonoBehaviour {

    [SerializeField] private Transform followObj;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float followSpeed;
    [SerializeField] private float lookSpeed;
    
    /// <summary>
    /// This function is used to look at the target object.
    /// </summary>
    public void LookAtTarget() {
        Vector3 lookDirection = followObj.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, lookSpeed * Time.deltaTime);
    }

    /// <summary>
    /// This function is used to move the camera to the target object.
    /// </summary>
    public void moveToTarget() {
        Vector3 targetPos = followObj.position + followObj.forward * offset.z + followObj.right * offset.x + followObj.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }

    void FixedUpdate() {
        LookAtTarget();
        moveToTarget();
    }
}
