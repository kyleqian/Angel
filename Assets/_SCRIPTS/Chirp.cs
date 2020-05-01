using UnityEngine;

public class Chirp : MonoBehaviour
{
    [SerializeField] Transform origin_;
    [SerializeField] bool inversePitch_;
    [SerializeField] int octaveModifier_;

    ChuckSubInstance chuck_;

    void Awake()
    {
        chuck_ = GetComponent<ChuckSubInstance>();
    }

    void OnCollisionEnter(Collision collision)
    {
        int tempNormalizedPitch = Mathf.RoundToInt(Mathf.InverseLerp(0, 0.5f, (origin_.position - transform.position).magnitude) * 7);
        float normalizedGain = Mathf.InverseLerp(0, 4, collision.relativeVelocity.magnitude);
        //Debug.LogError(collision.relativeVelocity.magnitude);
        PlayChuck(normalizedGain, tempNormalizedPitch);
    }

    void PlayChuck(float normalizedGain, int tempNormalizedPitch)
    {
        chuck_.RunCode(string.Format(@"
            SinOsc s => ADSR e => dac;

            // set a, d, s, and r
            e.set( 10::ms, 8::ms, .5, 500::ms );
            // set gain
            {0} => s.gain;

            [60, 62, 64, 65, 67, 69, 71, 72] @=> int CMajor[];

            // choose freq
            //Math.random2( 20, 120 ) => Std.mtof => s.freq;
            CMajor[{1}] + {2} => Std.mtof => s.freq;

            // key on - start attack
            e.keyOn();
            // advance time by 800 ms
            500::ms => now;
            // key off - start release
            e.keyOff();
            // advance time by 800 ms
            800::ms => now;
		", normalizedGain, inversePitch_ ? (7 - tempNormalizedPitch) : tempNormalizedPitch, octaveModifier_ * 12));
    }
}
