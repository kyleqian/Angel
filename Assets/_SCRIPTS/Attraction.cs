using UnityEngine;

public class Attraction : MonoBehaviour
{
    [SerializeField] Transform attractor_;
    [SerializeField] float thrust_ = 100;

    Rigidbody rigidBody_;

    void Awake()
    {
        rigidBody_ = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rigidBody_.AddForce(thrust_ * (attractor_.position - transform.position));
    }
}
