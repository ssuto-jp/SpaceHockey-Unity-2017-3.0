using UnityEngine;

namespace SpaceHockey.Gimmicks
{
    public class Meteor : MonoBehaviour
    {
        [SerializeField] private GameObject explosionParticle;

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                explosionParticle.transform.parent = null;

                explosionParticle.SetActive(true);
                Destroy(gameObject);
                Destroy(explosionParticle, 1f);
            }
        }
    }
}

