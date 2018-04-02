using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    //  [RequireComponent(typeof (AudioSource))]
    public class WheelEffects : MonoBehaviour
    {
        public Transform SkidTrailPrefab;
        public static Transform skidTrailsDetachedParent;
        private ParticleSystem skidParticles;
        public bool skidding { get; private set; }
        public bool PlayingAudio { get; private set; }
        public float slipLimit = 5;
        private CarAudio m_CarAudio;

        private Transform m_SkidTrail;
        private WheelCollider m_WheelCollider;

        private void Start()
        {

            skidParticles = transform.root.GetComponentInChildren<ParticleSystem>();

            if (skidParticles == null)
            {
                Debug.LogWarning(" no particle system found on car to generate smoke particles");
            }
            else
            {
                Debug.LogWarning(" XXX system found on car to generate smoke particles");
                skidParticles.Stop();
            }

            m_WheelCollider = GetComponent<WheelCollider>();

            PlayingAudio = false;

            if (skidTrailsDetachedParent == null)
            {
                skidTrailsDetachedParent = new GameObject("Skid Trails - Detached").transform;
            }
            // hubs is the parent, car is the grandparent

            m_CarAudio = GetComponentInParent<Transform>().GetComponentInParent<CarAudio>();
        }


        public void EmitTyreSmoke()
        {
            skidParticles.transform.position = transform.position - transform.up * m_WheelCollider.radius;
            skidParticles.Emit(1);
            if (!skidding)
            {
                m_CarAudio.m_Skid.volume = 30;
                StartCoroutine(StartSkidTrail());
            }
        }

        public void PlayAudio()
        {

            m_CarAudio.m_Skid.Play();
            PlayingAudio = true;
        }


        public void StopAudio()
        {
            m_CarAudio.m_Skid.Stop();
            PlayingAudio = false;
        }


        public IEnumerator StartSkidTrail()
        {
            skidding = true;
            m_SkidTrail = Instantiate(SkidTrailPrefab);
            while (m_SkidTrail == null)
            {
                yield return null;
            }
            m_SkidTrail.parent = transform;
            m_SkidTrail.localPosition = -Vector3.up * m_WheelCollider.radius;
        }


        public void EndSkidTrail()
        {
            if (!skidding)
            {
                return;
            }
            skidding = false;
            m_SkidTrail.parent = skidTrailsDetachedParent;
            Destroy(m_SkidTrail.gameObject, 10);
        }
    }
}
