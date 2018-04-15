using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceHockey.Players
{
    public class PlayerMover : BasePlayerComponent
    {
        private const float racketSpeed = 20f;      
        private Rigidbody rb;

        public enum PlayerType
        {
            player1, player2
        }
        public PlayerType type;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        protected override void OnInitialize()
        {
            switch (PlayerId.Instance.Player_Id)
            {
                case 1:
                    this.FixedUpdateAsObservable()
                        .Where(_ => type == PlayerType.player1)
                        .Subscribe(_ => rb.velocity = new Vector3(-Input.GetAxis("Horizontal"), 0, 0) * racketSpeed);
                    break;
                case 2:
                    this.FixedUpdateAsObservable()
                        .Where(_ => type == PlayerType.player2)
                        .Subscribe(_ => rb.velocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0) * racketSpeed);
                    break;
                default:
                    break;
            }
        }
    }
}
