using UnityEngine;

public class PausableRigidBody2D : MonoBehaviour
{

    private Rigidbody2D body;
    private Vector3 savedVelocity;
    private float savedAngularVelocity;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public void Pause()
    {
        savedVelocity = body.velocity;
        savedAngularVelocity = body.angularVelocity;
        body.isKinematic = true;
    }

    public void Resume()
    {
        body.isKinematic = false;
        body.velocity = savedVelocity;
        body.angularVelocity = savedAngularVelocity;
        body.WakeUp();
    }

    public void Reactivate()
    {
        body.isKinematic = false;
        body.WakeUp();
    }
}