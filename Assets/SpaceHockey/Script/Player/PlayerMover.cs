using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceHockey.Players
{
    public class PlayerMover : BasePlayerComponent
    {
        private const float racketSpeed = 0.3f;
        private const float racketMaxPos = 8f;

        public enum PlayerType
        {
            player1, player2
        }
        public PlayerType type;

        protected override void OnInitialize()
        {
            switch (PlayerId.Instance.Player_Id)
            {
                case 1:
                    this.UpdateAsObservable()
                   .Where(_ => type == PlayerType.player1)
                   .Where(_ => Input.GetKey(KeyCode.LeftArrow))
                   .Where(_ => racketMaxPos > transform.position.x)
                   .Subscribe(_ => transform.Translate(Vector3.right * racketSpeed));

                    this.UpdateAsObservable()
                        .Where(_ => type == PlayerType.player1)
                        .Where(_ => Input.GetKey(KeyCode.RightArrow))
                        .Where(_ => transform.position.x > -racketMaxPos)
                        .Subscribe(_ => transform.Translate(Vector3.left * racketSpeed));
                    break;
                case 2:
                    this.UpdateAsObservable()
                    .Where(_ => type == PlayerType.player2)
                    .Where(_ => Input.GetKey(KeyCode.LeftArrow))
                    .Where(_ => -racketMaxPos < transform.position.x)
                    .Subscribe(_ => transform.Translate(Vector3.left * racketSpeed));

                    this.UpdateAsObservable()
                        .Where(_ => type == PlayerType.player2)
                        .Where(_ => Input.GetKey(KeyCode.RightArrow))
                        .Where(_ => transform.position.x < racketMaxPos)
                        .Subscribe(_ => transform.Translate(Vector3.right * racketSpeed));
                    break;
                default:
                    break;
            }
        }
    }
}
