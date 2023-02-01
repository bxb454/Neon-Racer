using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for the coin object
/// </summary>
public class Coin : MonoBehaviour {

    [SerializeField] private Transform coinTransform;
    private const float spinAmt = 1;
    [SerializeField] GameObject coin;
    [SerializeField] private SaveData saveData;
    private float randX;
    private float randZ;
    private float tempGap;
    public InfiniteGen infGen;
    [SerializeField] private CarController carController;
    private float waitTime;
    private List<GameObject> activeCoins = new List<GameObject>();
    [SerializeField] private int lowerRandBound;
    [SerializeField] private int upperRandBound;
    [SerializeField] private int lowerWaitBound;
    [SerializeField] private int upperWaitBound;
    [SerializeField] private int scoreIncrement;

    ///<summary>
    /// Define random values for the coin and start the coroutine
    ///</summary>
    void Start() {
        tempGap = 0;
        randX = Random.Range(lowerRandBound, upperRandBound);
         waitTime = Random.Range(lowerWaitBound, upperWaitBound);
        StartCoroutine(CoinSpawn());
    }

    ///<summary>
    /// Get the coin transform
    ///</summary>
    ///<param name="coinTransform">The coin transform</param>
    ///<param name="spinAmt">The spin amount</param>
    public void GetCoinTransform(Transform coinTransform, float spinAmt) => this.coinTransform.rotation = coinTransform.rotation * Quaternion.Euler(new Vector3(spinAmt, 0, 0));
    
    ///<summary>
    /// Update the coin transform
    ///</summary>
    public void UpdateCoinTransform() {
        GetCoinTransform(coinTransform, spinAmt);
    }

    ///<summary>
    /// Spawn a coin via coroutine to spawn a coin every 2-5 seconds (randomly chosen)
    ///</summary>
   public IEnumerator CoinSpawn() {
        yield return new WaitForSeconds(waitTime);
        if (carController.groundHit && carController.rbInstance.velocity.z < 0.0f) {
            if(infGen.yGap > 0) {
                tempGap = 2.5f;
            }
            else if (infGen.yGap == 0) {
                tempGap = 5.2f;
            }
            GameObject co = Instantiate(coin, new Vector3(randX + infGen.xGap, infGen.yGap + tempGap, infGen.zGap * -1),
                Quaternion.Euler(new Vector3(0, 90, 90)));
            activeCoins.Add(co);
            co.transform.rotation = co.transform.rotation * Quaternion.Euler(new Vector3(spinAmt, 0, 0));
        }
    }

    ///<summary>
    /// Destroy the coin when the player collides with it
    ///</summary>
    ///<param name="other">The collider of the coin</param>
    void OnTriggerEnter(Collider other) {
        GameObject.Destroy(coin);
        saveData.data.score += scoreIncrement;
    }

    private void FixedUpdate() {
       UpdateCoinTransform();
    }
    
    ///<summary>
    /// Delete the coin when the player passes it
    ///</summary>
    public void DeleteCoin() {
        Destroy(activeCoins[0]);
        activeCoins.RemoveAt(0);
    }
}
