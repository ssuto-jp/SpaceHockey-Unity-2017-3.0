using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace SpaceHockey.Players
{
    public class PlayerMover : BasePlayerComponent
    {

        private float racket_Speed = 0.3f;//ラケットの移動速度
        private float racket_MaxPos = 5f;//ラケットの最大座標

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
                   .Where(_ => racket_MaxPos > transform.position.x)
                   .Subscribe(_ => transform.Translate(Vector3.right * racket_Speed));

                    this.UpdateAsObservable()
                        .Where(_ => type == PlayerType.player1)
                        .Where(_ => Input.GetKey(KeyCode.RightArrow))
                        .Where(_ => transform.position.x > -racket_MaxPos)
                        .Subscribe(_ => transform.Translate(Vector3.left * racket_Speed));
                    break;
                case 2:
                    this.UpdateAsObservable()
                    .Where(_ => type == PlayerType.player2)
                    .Where(_ => Input.GetKey(KeyCode.LeftArrow))
                    .Where(_ => -racket_MaxPos < transform.position.x)
                    .Subscribe(_ => transform.Translate(Vector3.left * racket_Speed));

                    this.UpdateAsObservable()
                        .Where(_ => type == PlayerType.player2)
                        .Where(_ => Input.GetKey(KeyCode.RightArrow))
                        .Where(_ => transform.position.x < racket_MaxPos)
                        .Subscribe(_ => transform.Translate(Vector3.right * racket_Speed));
                    break;
                default:
                    break;
            }
        }
    }
}
