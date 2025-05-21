using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float force;


    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.name);
    }

    public void MovePlayer()
    {
        if (Input.GetKey(KeyCode.A)) 
            rigidbody.AddForce(-1f * force, 0, 0);

        if (Input.GetKey(KeyCode.D)) 
            rigidbody.AddForce(Vector3.right * force);
        
        if (Input.GetKey(KeyCode.W))
            rigidbody.AddForce(Vector3.forward * force);
        
        if (Input.GetKey(KeyCode.S))
            rigidbody.AddForce(Vector3.back * force);
    }
}
