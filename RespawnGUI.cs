using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// This class is used as the logic behind the respawn GUI.
/// </summary>
public class RespawnGUI : MonoBehaviour {
[SerializeField] private InfiniteGen infGen;
[SerializeField] private Transform vehicleTransform;
[SerializeField] private Rigidbody rb;

private float respawnX = 0;
private float respawnY = 0;
private float respawnZ = 0;
 private const float respawnOffsetY = 3.2f;
 private const float respawnOffsetZ = 5.0f;
// Time delay before respawn availability
private const float timeDelay = 4.0f;

/// <summary>
/// Helper method that sets the vehicle's position and rotation.
/// </summary>
private void SetBodyTransform(Rigidbody rb, Transform bodyTransform, float x, float y, float z) {
        Vector3 testVec = new Vector3(x, y, z);
        bodyTransform.position = testVec;
        bodyTransform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        rb.position = bodyTransform.position;
    }
    
/// <summary>
/// Coroutine that sets the respawn coordinates.
/// </summary>
public IEnumerator respawnCoords() {
    yield return new WaitForSeconds(timeDelay);
    respawnX = infGen.xGap;
    respawnY = infGen.yGap + respawnOffsetY;
    respawnZ = (infGen.zGap * -1) + respawnOffsetZ;
}

/// <summary>
///Respawn the vehicle via the SetBodyTransform helper method.
/// </summary>
public void respawnVehicle() {
    SetBodyTransform(rb, vehicleTransform, respawnX, respawnY, respawnZ);
}

/// <summary>
/// Start the coroutine.
/// </summary>
void Start() {
    StartCoroutine(respawnCoords());
    }
}
