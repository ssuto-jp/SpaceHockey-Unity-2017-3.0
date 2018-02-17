using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceHockey.Balls
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallCore : MonoBehaviour
    {
        private Rigidbody rb;
        private float initialSpeed = 10f;
        private float accele = 25;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();

            this.OnCollisionEnterAsObservable()
                .Subscribe(_ =>
                {
                    rb.velocity = rb.velocity.normalized * accele;                 
                });
        }

        public void ShootBall()
        {           
            rb.velocity = (Vector3.forward + Vector3.left) * initialSpeed;
        }

        public void SetBall(Vector3 respawn)
        {
            rb.velocity = Vector3.zero;
            transform.position = respawn;
        }
    }
}

