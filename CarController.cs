using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class CarController : MonoBehaviour {
    [SerializeField] private float horizontalInput;
    [SerializeField] private float verticalInput;
    public float steerAngle { get; private set;}
    [SerializeField] private float motorForce;
    [SerializeField] private WheelCollider fl, fr;
    [SerializeField] private WheelCollider rl, rr;
    [SerializeField] private Transform flTransform, frTransform;
    [SerializeField] private Transform rlTransform, rrTransform;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private Rigidbody rb;
    public Rigidbody rbInstance { get{ return rb; }}
    [SerializeField] private Vector3 centerOfMass;
    public Transform carTransform;
    [SerializeField] private float maxSpeed;
    public bool accelAvailable { get; private set;}
    [SerializeField] private float downForce;
    public bool groundHit = false;
    [SerializeField] private InfiniteGen infGen;

    ///<summary>
    ///This method allows downforce to be applied onto the car, ensuring stability on the tracks and no
    ///unwanted flips / jerky movements. Downforce value is set in the inspector.
    ///</summary>
    public void groundGlue() {
        RaycastHit hitGround;
        if (Physics.Raycast(carTransform.position + new Vector3(0, 0, 0f), new Vector3(0, -0.2f, 0f), out hitGround)) {
            groundHit = true;
        }
        else {
            groundHit = false;
        }
        float mapHeight = hitGround.point.y;
        rb.AddForce((transform.up * -1) * downForce);
        //Debug.DrawRay(carTransform.position + new Vector3(0, 0, 0f), new Vector3(0, -0.2f, 0f), Color.white);
    }

    ///<summary>
    ///This method gets the input from the player and stores it in the variables horizontalInput and verticalInput,
    ///which are then used in the Steer() and Accelerate() methods.
    ///</summary>
    private void GetInput() {
        horizontalInput = SimpleInput.GetAxis("Horizontal");
        verticalInput = SimpleInput.GetAxis("Vertical");
    }

    ///<summary>
    ///This method is used to set the position of the wheels on the car, so that they rotate and move with the car.
    ///</summary>
    private void Steer() {
        steerAngle = maxSteerAngle * horizontalInput;
        fl.steerAngle = steerAngle;
        fr.steerAngle = steerAngle;
    }

    ///<summary>
    ///This method is used to allow the car to accelerate, and is called in the Update() method.
    ///</summary>
    private void Accelerate() {
        if (maxSpeed > rb.velocity.sqrMagnitude) {
            fl.motorTorque = verticalInput * motorForce;
            fr.motorTorque = verticalInput * motorForce;
            rl.motorTorque = verticalInput * motorForce;
            rr.motorTorque = verticalInput * motorForce;
        }
    }

    ///<summary>
    ///This method is used to check whether the car is drifting or not, and is called in the Update() method.
    ///</summary>
    private void driftCheck() {
        float driftVal = Vector3.Dot(rb.velocity.normalized, transform.forward.normalized);
        float driftAngle = Mathf.Acos(driftVal * 0.2f) * Mathf.Rad2Deg;
        if (driftAngle > maxSteerAngle & (rb.velocity.sqrMagnitude > maxSpeed/1.5f)) {
            accelAvailable = false;
        }
        else {
            accelAvailable = true;
        }
    }
    
    ///<summary>
    ///This method is used to apply force to the rear wheels of the vehicle, ensuring that the car will drift when conditions are necessary.
    ///</summary>
    private void applyWheelForce() {
        float currentSpeed = rb.velocity.magnitude;
        if(currentSpeed < maxSpeed) {
            return;
        }
        if(!accelAvailable) {
            return;
        }
        foreach(WheelCollider wheel in new WheelCollider[] {rl, rr}) {
            wheel.motorTorque = 1.5f * verticalInput * motorForce;
        }
    }

    ///<summary>
    ///This method is used to update the position of the wheels on the car, so that they rotate and move with the car.
    ///</summary>
    private void UpdateWheelPoses() {
        UpdateWheelPose(fl, flTransform);
        UpdateWheelPose(fr, frTransform);
        UpdateWheelPose(rl, rlTransform);
        UpdateWheelPose(rr, rrTransform);
    }

    ///<summary>
    ///This method is used to check whether the car (player) is going the wrong way, and is called in the Update() method.
    ///</summary>
    public string isWrongWay() {
        if (Vector3.Dot(transform.forward, rb.velocity) < 0.05f && (rb.rotation.eulerAngles.y > 270 || rb.rotation.eulerAngles.y < 60)) {
            return "Wrong Way!";
        }
        else {
            return "";
        }
    }

    ///<summary>
    ///Helper method to UpdateWheelPoses(). Technical work behind updating wheel positions and rotations.
    ///</summary>
    ///<param name="_collider">The wheel collider of the wheel.</param>
    ///<param name="_transform">The transform of the wheel.</param>
    private void UpdateWheelPose(WheelCollider collider, Transform transform) {
        Vector3 pos = transform.position;
        Quaternion quat = transform.rotation;
        collider.GetWorldPose(out pos, out quat);
        transform.position = pos;
        transform.rotation = quat * Quaternion.Euler(Vector3.zero);
    }

    ///<summary>
    ///This method is used to set the center of mass of the car, so that it is more stable and less likely to flip.
    ///</summary>
    void Start() {
        rb.centerOfMass = centerOfMass;
        accelAvailable = true;
    }

    ///<summary>
    ///This method is used to update the car's position and rotation, and is called in the FixedUpdate() method.
    ///</summary>
    void FixedUpdate() {
        Steer();
        Accelerate();
        UpdateWheelPoses();
        GetInput();
        groundGlue();
        driftCheck();
    }
}
