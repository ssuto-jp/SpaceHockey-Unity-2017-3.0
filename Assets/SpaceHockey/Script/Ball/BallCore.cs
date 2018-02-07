using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceHockey.Balls
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallCore : MonoBehaviour
    {
        private Rigidbody rb;
        [SerializeField] private float ballSpeed;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void ShootBall()
        {           
            rb.velocity = (Vector3.forward + Vector3.left) * ballSpeed;
        }
    }
}

