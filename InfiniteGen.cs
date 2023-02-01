using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random=UnityEngine.Random;
using UnityEngine.Events;

/// <summary>
/// This class is used to generate the main road while the player is playing the game (procedural generation).
/// </summary>
public class InfiniteGen : MonoBehaviour {

    [SerializeField] private GameObject tileMap;
    [SerializeField] private GameObject[] roadTiles;
    [SerializeField] private GameObject[] sceneryProps;
    private Vector3[] roadTileSizes;
    public float tileZ {get; private set;}
    public float zGap {get; private set;}
    public float xGap {get; private set;}
    public float yGap {get; private set;}
    public float tileLength {get; private set;}
    public int numTiles {get; private set;}
    public int numRoads {get; private set;}
    [SerializeField] private Transform playerTransform;
    public Transform playerTransformInstance {get {return playerTransform;}}
    private LinkedList<GameObject> gameplayTiles = new LinkedList<GameObject>();
    private Stack<GameObject> roadStack = new Stack<GameObject>();

    //Dictionary for indices in the roadTiles array and their corresponding spawn functions.
    private Dictionary<int, Action> roadSpawnDict = new Dictionary<int, Action>();
    [SerializeField] private GameObject vehicleBody;
    [SerializeField] private CarController carControl;
    [SerializeField] private Vector3 triggerPos;

    ///<summary>
    ///Enumerated values for road type indices as defined in the main array.
    ///</summary>
    private enum RoadType {
        Straight = 0, RightCurve = 4, Bridge = 1, UpRamp = 5, DownRamp = 6, TunnelEntrance = 2, TunnelPiece = 3,
        Straight2 = 7, LiftRoad = 8, CheckPoint = 9, LeftCurve = 7}

    ///<summary>
    ///Spawns a tile map at the specified index.
    ///</summary>
    void Start() {
    roadSpawnDict.Add((int)RoadType.Straight, spawnStraightRoads);
    roadSpawnDict.Add((int)RoadType.Bridge, spawnBridge);
    roadSpawnDict.Add((int)RoadType.TunnelEntrance, spawnTunnelEntrance);
    roadSpawnDict.Add((int)RoadType.TunnelPiece, spawnTunnelPiece);
    roadSpawnDict.Add((int)RoadType.RightCurve, spawnRightRoadCurve);
    roadSpawnDict.Add((int)RoadType.LeftCurve, spawnLeftRoadCurve);
    roadSpawnDict.Add((int)RoadType.UpRamp, spawnUpRamp);
    roadSpawnDict.Add((int)RoadType.DownRamp, spawnDownRamp);
    roadSpawnDict.Add((int)RoadType.LiftRoad, spawnLiftRoad);

        tileZ = 0;
        zGap = 0;
        xGap = 0;
        yGap = 0;
        tileLength = tileMap.GetComponentsInChildren<Renderer>()[0].bounds.size.x;
        numTiles = 1;
        numRoads = 1;
        initializeRoadTileSizes();
        for(int i = 0; i < 4; i++) {
           if (i != 0) {
                spawnTileMap();
            }
        }
    }
    
    ///<summary>
    ///Spawns a tile map at the specified index. This is done via a switch statement given the index.
    ///</summary>
    ///<param name="index">Index of the tile map to spawn.</param>
    public void pickRoadToSpawn() {
        if (playerTransform.position.z - 4000 < (zGap * -0.5f) + (numRoads * roadTileSizes[(int)RoadType.Straight].z * -1)) {
            int index = Random.Range(0, roadTiles.Length - 2);
            if(index == ((int)RoadType.TunnelPiece)) {
                index = (int)RoadType.Straight;
            }
            roadSpawnDict[index]();
            numRoads++;
        }
    }

    ///<summary>
    ///Find the length of all road tiles in the array. This is done by finding the z bounds of a road tile's
    ///Mesh Renderer component. If it is nested in an empty GameObject, it will find the z bounds of the first
    ///Mesh Renderer component in the children of the empty GameObject.
    ///</summary>
    ///<catches>Exception if the road tile is nested in an empty GameObject.</catches>
    public void initializeRoadTileSizes() {
    roadTileSizes = new Vector3[roadTiles.Length];
        for(int i = 0; i<roadTiles.Length; i++){
            try {
    roadTileSizes[i] = roadTiles[i].GetComponent<Renderer>().bounds.size;
         }  
         catch(Exception e) {
        roadTileSizes[i] = roadTiles[i].GetComponentsInChildren<Renderer>()[0].bounds.size;
            }
        }
    }

    ///<summary>
    ///Checks if the stack peek contains a curved road (right or left). Used in conjunction with spawning road types.
    ///</summary>
   public bool curveCheck() {
        var rightCurveRoad = GameObject.Find("ROAD CURVE(RIGHT)(Clone)");
        var leftCurveRoad = GameObject.Find("ROAD CURVE(LEFT)(Clone)");
        return (roadStack.Peek() == rightCurveRoad || roadStack.Peek() == leftCurveRoad && roadStack.Peek() != null);
    }

    ///<summary>
    ///Checks if the stack peek contains a bridge. Used in conjunction with spawning road types.
    ///</summary>
    public bool bridgeCheck() {
        var bridgeRoad = GameObject.Find("Highway Bridge 1(Clone)");
        return (roadStack.Peek() == bridgeRoad && roadStack.Peek() != null);
    }

    ///<summary>
    ///Checks if the stack peek contains an upward-sloping ramp. Used in conjunction with spawning road types.
    ///</summary>
    public bool upRampCheck() {
        var upRampRoad = GameObject.Find("Highway Bridge 1(Clone)");
        return (roadStack.Peek() == upRampRoad && roadStack.Peek() != null);
    }

    ///<summary>
    ///Checks if the stack peek contains a lifted road. Used in conjunction with spawning road types.
    ///</summary>
    public bool roadLiftCheck() {
        var roadLift = GameObject.Find("Road Lift Highway(Clone)");
        return (roadStack.Peek() == roadLift && roadStack.Peek() != null);
    }

    ///<summary>
    ///Checks if the stack peek contains a downward-sloping ramp. Used in conjunction with spawning road types.
    ///</summary>
    public bool downRampCheck() {
        var downRamp = GameObject.Find("DOWN RAMP ROAD(Clone)");
        return (roadStack.Peek() == downRamp && roadStack.Peek() != null);
    }

    ///<summary>
    ///Spawns a tile map at the specified index.
    ///</summary>
    ///<param name="index">Index of the tile map to spawn.</param>
    private void spawnTileMap() {
        GameObject tile = Instantiate(tileMap, (transform.up * 3.6f) + (transform.right) *
         tileMap.GetComponentsInChildren<Renderer>()[0].bounds.size.x * numTiles, Quaternion.Euler(new Vector3(-90, 0, 90)));
        tileZ += tileLength  * 5;
        gameplayTiles.AddLast(tile);
        numTiles++;
    }

    ///<summary>
    ///Spawns a straight road piece at the specified index.
    ///</summary>
    ///<param name="index">Index of the straight road piece to spawn.</param>
    private void spawnRoadMap(int index = (int)RoadType.Straight) {
        if(yGap == 0) {
        GameObject road = Instantiate(roadTiles[index], (transform.up * (yGap)) + (transform.forward *
         (xGap)) + (transform.right) * zGap, Quaternion.identity);
            if(yGap < 0) {
                yGap = 0;
            }
        zGap += roadTileSizes[(int)RoadType.Straight].z;
        roadStack.Push(road);
        }
    }
    
    ///<summary>
    ///Spawns a bridge at the specified index.
    ///</summary>
    ///<param name="index">Index of the bridge to spawn.</param>
    public void spawnBridge() {
    if(yGap > 0 && !bridgeCheck()) {
        if(roadLiftCheck()) {
        zGap += 3.5f;
        }
        GameObject bridge = Instantiate(roadTiles[(int)RoadType.Bridge], (transform.up * (yGap)) + (transform.forward * 
        (11.3f + xGap)) + (transform.right) * zGap, Quaternion.Euler(Vector3.zero));
        zGap +=roadTileSizes[(int)RoadType.Bridge].z;
        roadStack.Push(bridge);
        }
    }

    ///<summary>
    ///Spawns a tunnel entrance at the specified index.
    ///</summary>
    ///<param name="index">Index of the tunnel entrance to spawn.</param>
    public void spawnTunnelEntrance() {
        if (yGap <= 0 && !curveCheck()) {
            GameObject tunnelEntrance = Instantiate(roadTiles[(int)RoadType.TunnelEntrance], (transform.up * (yGap + 3.75f)) + (transform.forward * 
            (xGap)) + (transform.right) * zGap, Quaternion.Euler(Vector3.zero));
            zGap +=roadTileSizes[(int)RoadType.TunnelEntrance].z;
            roadStack.Push(tunnelEntrance);
            for(int i = 0; i<Random.Range(12, 25); i++) {
                spawnTunnelPiece();
            }
        }
    }

    ///<summary>
    ///Spawns a tunnel piece at the specified index.
    ///</summary>
    ///<param name="index">Index of the tunnel piece to spawn.</param>
    public void spawnTunnelPiece() {
        GameObject tunnelPiece = Instantiate(roadTiles[(int)RoadType.TunnelPiece], (transform.up * (yGap)) + (transform.forward * 
        (xGap)) + (transform.right) * zGap, Quaternion.Euler(Vector3.zero));
        zGap += roadTileSizes[(int)RoadType.TunnelPiece].z;
        roadStack.Push(tunnelPiece);
    }

    ///<summary>
    ///Spawns a right curve road piece at the specified index.
    ///</summary>
    ///<param name="index">Index of the right curve road piece to spawn.</param>
    public void spawnRightRoadCurve() {
        if(yGap != roadTileSizes[(int)RoadType.UpRamp].y) {
        GameObject rightRoadCurve = Instantiate(roadTiles[(int)RoadType.RightCurve], (transform.up * (yGap)) + (transform.forward * 
        (xGap)) + (transform.right) * zGap, Quaternion.Euler(Vector3.zero));
        zGap += roadTileSizes[(int)RoadType.RightCurve].z - 7;
        xGap -= 42.4f;
        roadStack.Push(rightRoadCurve);
        }
    }

    ///<summary>
    ///Spawns an upward sloping ramp at the specified index.
    ///</summary>
    ///<param name="index">Index of the upward sloping ramp to spawn.</param>
    public void spawnUpRamp() {
        if(yGap != roadTileSizes[(int)RoadType.UpRamp].y) {
        GameObject upRamp = Instantiate(roadTiles[(int)RoadType.UpRamp], (transform.up * (yGap)) + (transform.forward * 
        (xGap)) + (transform.right) * zGap, Quaternion.Euler(Vector3.zero));
        zGap +=roadTileSizes[(int)RoadType.UpRamp].z;
        yGap += roadTileSizes[(int)RoadType.UpRamp].y;
        roadStack.Push(upRamp);
        }
    }

    ///<summary>
    ///Spawns a left curve road piece at the specified index.
    ///</summary>
    ///<param name="index">Index of the left curve road piece to spawn.</param>
    public void spawnLeftRoadCurve() {
        if (yGap != roadTileSizes[(int)RoadType.UpRamp].y) {
        GameObject leftRoadCurve = Instantiate(roadTiles[(int)RoadType.LeftCurve], (transform.up * (yGap)) + (transform.forward * 
        (xGap)) + (transform.right) * zGap, Quaternion.Euler(Vector3.zero));
        zGap += roadTileSizes[(int)RoadType.LeftCurve].z + 186f;
        xGap += 107.3f;
        roadStack.Push(leftRoadCurve);
        }
    }

    ///<summary>
    ///Spawns a lifted road piece at the specified index.
    ///</summary>
    ///<param name="index">The index of the road tile to spawn.</param>
    public void spawnLiftRoad() {
        if(yGap > 0) {
        GameObject liftRoad = Instantiate(roadTiles[(int)RoadType.LiftRoad], (transform.up * (-11.25f + yGap)) + (transform.forward * 
        (11.3f + xGap)) + (transform.right) * zGap, Quaternion.Euler(Vector3.zero));
        zGap +=roadTileSizes[(int)RoadType.LiftRoad].z;
        roadStack.Push(liftRoad);
        }
    }

    ///<summary>
    ///Spawns a downward sloping ramp at the specified index.
    ///</summary>
    ///<param name="index">The index of the road tile to spawn.</param>
    public void spawnDownRamp() {
        if (yGap > 0 && !roadLiftCheck()) {
        yGap -= roadTileSizes[(int)RoadType.UpRamp].y;
        GameObject downRamp = Instantiate(roadTiles[(int)RoadType.DownRamp], (transform.up * (yGap)) + (transform.forward * 
        (xGap)) + (transform.right) * (zGap), Quaternion.Euler(Vector3.zero));
        zGap +=roadTileSizes[(int)RoadType.DownRamp].z;
        roadStack.Push(downRamp);
            }
        }

    ///<summary>
    ///Helper method to spawn 2-6 straight road pieces and a checkpoint.
    ///</summary>
    public void spawnStraightRoads() {
        for(int i = 0; i<Random.Range(2, 6); i++) {
        spawnRoadMap();
        }
        spawnRoadMap((int)RoadType.CheckPoint);
    }

    ///<summary>
    ///Move the background objects in accordance with the vehicle's Z position and velocity.
    ///If velocity is negative or zero, the objects will not move.
    ///</summary>
    public void moveScenery() {
        for(int i = 0; i<sceneryProps.Length; i++) {
            if(vehicleBody.GetComponent<Rigidbody>().velocity.z > 0) {
            sceneryProps[i].transform.position = new Vector3(sceneryProps[i].transform.position.x, sceneryProps[i].transform.position.y, 
            sceneryProps[i].transform.position.z - Mathf.Abs(vehicleBody.GetComponent<Rigidbody>().velocity.z) * 0.004f);
        }
     }
    }

    ///<summary>
    ///Spawn road pieces in accordance with frame updates.
    ///</summary>
    private void FixedUpdate() {
        if (playerTransform.position.z > (tileZ * -0.01f - (numTiles * 0.5f * tileLength * 1))) {
            spawnTileMap();
        }
        pickRoadToSpawn();
        moveScenery();
    }

    ///<summary>
    ///Delete road pieces in accordance with frame updates.
    ///</summary>
    private void Delete() {
        var temp = gameplayTiles.First.Value;
        gameplayTiles.RemoveFirst();
        Destroy(temp);
    }
}