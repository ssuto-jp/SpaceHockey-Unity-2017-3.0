using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceHockey.Balls
{
    [RequireComponent(typeof(Rigidbody))]
    public class Ball : MonoBehaviour
    {      
        private Rigidbody rb;
        [SerializeField] private float initialSpeed;
        [SerializeField] private float accele;

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
        }
             
        [PunRPC]
        public void ShootBall()
        {          
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>();
            }
            rb.velocity = (Vector3.forward + Vector3.left) * initialSpeed;
        }
    }
}


