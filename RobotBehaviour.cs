using UnityEngine;
using System.Collections;

public class RobotBehaviour : MonoBehaviour {

	public Transform[] wheels;
	public float motorPower = 1500.0f;
	public float maxTurn = 25.0f;

	float instantePower = 0.0f;
	float brake = 0.0f;
	float wheelTurn = 0.0f;
	int count = 0;

	Vector3 addv = new Vector3 (0.55f, 0.0f, 0.0f); 
	Rigidbody myRigidbody;

	// Use this for initialization
	void Start () {
		myRigidbody = this.gameObject.GetComponent<Rigidbody>();
		myRigidbody.centerOfMass = new Vector3(0, 0.0f, 0.0f);
	}

	// Update is called once per frame
	void FixedUpdate() {
		instantePower = motorPower * Time.deltaTime;

		Vector3 fwd = this.transform.TransformDirection (Vector3.forward) * 10;
		Vector3 origl = transform.position - addv;
		Vector3 origc = transform.position; 
		Vector3 origr = transform.position + addv; 

		Debug.DrawRay (origl, fwd, Color.green);
		Debug.DrawRay (origc, fwd, Color.blue);
		Debug.DrawRay (origr, fwd, Color.red);

		//if obstacle is detected 
		if ((Physics.Raycast (origl, fwd, 10) || Physics.Raycast (origc, fwd, 10) || Physics.Raycast (origr, fwd, 10))&&count<3)
		{ 
			Debug.Log ("Something in front of car");
			wheelTurn = maxTurn;
			brake = myRigidbody.mass * 0.1f;

			//turn collider
			getCollider(0).steerAngle = wheelTurn;
			getCollider(1).steerAngle = wheelTurn;
			count++;
		} 
		else 
		{
			brake = 0.0f;
			instantePower = motorPower * Time.deltaTime;
			count = 0;
			Debug.Log ("Can go");

			//Do not turn wheel collider
			getCollider(0).steerAngle = 0.0f;
			getCollider(1).steerAngle = 0.0f;
		}


		//turn wheels
		wheels[0].localEulerAngles = new Vector3(wheels[0].localEulerAngles.x,
			getCollider(0).steerAngle - wheels[0].localEulerAngles.z + 90,
			wheels[0].localEulerAngles.z);
		wheels[1].localEulerAngles = new Vector3(wheels[1].localEulerAngles.x,
			getCollider(1).steerAngle - wheels[1].localEulerAngles.z + 90,
			wheels[1].localEulerAngles.z);

		//spin wheels
		wheels[0].Rotate(0, -getCollider(0).rpm / 60 * 360 * Time.deltaTime, 0);
		wheels[1].Rotate(0, -getCollider(1).rpm / 60 * 360 * Time.deltaTime, 0);
		wheels[2].Rotate(0, -getCollider(2).rpm / 60 * 360 * Time.deltaTime, 0);
		wheels[3].Rotate(0, -getCollider(3).rpm / 60 * 360 * Time.deltaTime, 0);

		//brakes
		if (brake > 0.0f)
		{
			getCollider(0).brakeTorque = brake;
			getCollider(1).brakeTorque = brake;
			getCollider(2).brakeTorque = brake;
			getCollider(3).brakeTorque = brake;
			getCollider(2).motorTorque = 0.0f;
			getCollider(3).motorTorque = 0.0f;
		}
		else
		{
			getCollider(0).brakeTorque = 0.0f;
			getCollider(1).brakeTorque = 0.0f;
			getCollider(2).brakeTorque = 0.0f;
			getCollider(3).brakeTorque = 0.0f;
			getCollider(2).motorTorque = instantePower;
			getCollider(3).motorTorque = instantePower;
		}
	}

	WheelCollider getCollider(int n)
	{
		return wheels[n].gameObject.GetComponent<WheelCollider>();
	}
}