using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceHockey.Balls
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonRigidbodyView))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float initialSpeed;
        [SerializeField] private float accele;
        [SerializeField] private GameObject generationParticle;
        [SerializeField] private GameObject extinctionParticle;
        [SerializeField] private AudioClip collisionAudio;
        [SerializeField] private AudioClip generationAudio;
        private Rigidbody rb;
        private AudioSource audioSource;
        private PhotonView photonView;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            audioSource = GetComponent<AudioSource>();
            photonView = GetComponent<PhotonView>();
        }

        private void Start()
        {
            this.OnCollisionEnterAsObservable()
                .Subscribe(collision =>
                {
                    rb.velocity = rb.velocity.normalized * accele;
                    audioSource.PlayOneShot(collisionAudio);
                });

            this.OnTriggerEnterAsObservable()
                .Where(other => other.CompareTag("Goal"))
                .Subscribe(_ =>
                {
                    if (photonView.isMine)
                    {
                        photonView.RPC("DestroyBall", PhotonTargets.All, null);
                    }
                });
        }

        [PunRPC]
        private void DestroyBall()
        {
            extinctionParticle.transform.parent = null;
            extinctionParticle.SetActive(true);
            Destroy(gameObject);
            Destroy(extinctionParticle, 3f);
        }

        [PunRPC]
        public void ShootBall()
        {
            StartCoroutine(ShootBallCoroutine());
        }

        private IEnumerator ShootBallCoroutine()
        {
            generationParticle.SetActive(true);
            audioSource.PlayOneShot(generationAudio);

            yield return new WaitForSeconds(2);

            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            rb.velocity = (Vector3.forward + Vector3.left) * initialSpeed;
            Destroy(generationParticle);
        }
    }
}


