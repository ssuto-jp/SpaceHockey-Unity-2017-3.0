using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceHockey.Balls
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PhotonView))]
    [RequireComponent(typeof(PhotonTransformView))]
    [RequireComponent(typeof(PhotonRigidbodyView))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private float initialSpeed;
        [SerializeField] private float accele;
        [SerializeField] private GameObject generationParticle;
        [SerializeField] private GameObject extinctionParticle;
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            this.OnCollisionEnterAsObservable()
                .Subscribe(collision =>
                {
                    rb.velocity = rb.velocity.normalized * accele;
                });

            this.OnTriggerEnterAsObservable()
                .Where(other => other.CompareTag("Goal"))
                .Subscribe(_ =>
                {
                    extinctionParticle.transform.parent = null;

                    Destroy(gameObject);
                    extinctionParticle.SetActive(true);
                    Destroy(extinctionParticle, 3f);
                });
        }

        [PunRPC]
        public void ShootBall()
        {
            StartCoroutine(ShootBallCoroutine());
        }

        private IEnumerator ShootBallCoroutine()
        {
            generationParticle.SetActive(true);

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


