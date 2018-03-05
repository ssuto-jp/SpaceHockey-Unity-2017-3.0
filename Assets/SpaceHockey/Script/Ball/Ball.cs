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
        private float initialSpeed = 10f;
        private float accele = 40;
        private Color changedColor;
        private Vector3 originalScale;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            originalScale = transform.localScale;
            
            this.OnCollisionEnterAsObservable()
                .Subscribe(collision =>
                {
                    rb.velocity = rb.velocity.normalized * accele;

                    if (collision.gameObject.CompareTag("Wall"))
                    {
                        ChangeBallState(changedColor);
                    }
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

        [PunRPC]
        public void FetchChangedColor(Color color)
        {
            changedColor = color;
        }

        [PunRPC]
        public void ChangeBallState(Color color)
        {
            if (color == Color.white)
            {
                transform.localScale = originalScale;
            }
            else if (color == Color.red)
            {
                AccelerateBall();
            }
            else if (color == Color.blue)
            {
                ChangeBallScale();
            }
            else if (color == Color.yellow)
            {
                StartCoroutine(BlinkCoroutine());
            }
            else
            {
                Debug.Log("error");
            }
        }

        //ボールを加速させる
        private void AccelerateBall()
        {
            rb.velocity = rb.velocity.normalized * 60;
        }

        //ボールを点滅させる
        private IEnumerator BlinkCoroutine()
        {
            var renderer = GetComponent<Renderer>();
            for (int i = 0; i < 5; i++)
            {
                renderer.enabled = !renderer.enabled;

                yield return new WaitForSeconds(0.5f);
            }
        }

        //ボールの大きさをランダムに変える
        private void ChangeBallScale()
        {
            var random = Random.Range(1, 6);
            transform.localScale = new Vector3(random, transform.localScale.y, random);
        }
    }
}


