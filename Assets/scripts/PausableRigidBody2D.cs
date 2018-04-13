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
        if (body == null) return;
        savedVelocity = body.velocity;
        savedAngularVelocity = body.angularVelocity;
        body.isKinematic = true;
    }

    public void Resume()
    {
        if (body == null) return;
        body.isKinematic = false;
        body.velocity = savedVelocity;
        body.angularVelocity = savedAngularVelocity;
        body.WakeUp();
    }

    public void Reactivate()
    {
        if (body == null) return;
        body.isKinematic = false;
        body.WakeUp();
    }
}