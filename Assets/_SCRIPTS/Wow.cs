using UnityEngine;

public class Wow : MonoBehaviour
{
    [SerializeField] Transform me_;
    [SerializeField] float leakFactor_ = 0.8f;

    ChuckSubInstance chuck_;
    float integrator_;
    Vector3 prevPos_;
    bool firstPosRecorded_;

    void Awake()
    {
        chuck_ = GetComponent<ChuckSubInstance>();
        chuck_.RunFile("bowing.ck", true);
    }
    

    void Update()
    {
        Vector3 pos = transform.position;

        if (!firstPosRecorded_)
        {
            prevPos_ = pos;
            firstPosRecorded_ = true;
            return;
        }

        float distanceFromMe = Vector3.Distance(pos, me_.position);
        Vector3 diff = pos - prevPos_;

        // Put magnitude into leaky integrator
        integrator_ += diff.magnitude;

        // Leak!
        integrator_ *= leakFactor_;

        prevPos_ = pos;

        chuck_.SetFloat("bowIntensity", integrator_);
        chuck_.SetFloat("thePitch", 24 + 24 * distanceFromMe);
    }
}
