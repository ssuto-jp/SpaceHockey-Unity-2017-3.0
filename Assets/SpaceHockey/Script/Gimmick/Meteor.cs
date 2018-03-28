using System.Collections;
using System.Collections.Generic;
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

                Destroy(gameObject);
                explosionParticle.SetActive(true);
                Destroy(explosionParticle, 1f);
            }
        }
    }
}

