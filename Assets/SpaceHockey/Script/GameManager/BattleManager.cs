using System;
using System.Collections;
using System.Collections.Generic;
using SpaceHockey.Balls;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceHockey.GameManagers
{
    public class BattleManager : Photon.MonoBehaviour
    {
        private BallCore ballCore;
        public GameObject ball;
        [SerializeField] private GameObject[] goal = new GameObject[2];
        [SerializeField] private GameObject respawnPoint;
        [SerializeField] private Text scoreText;

        private BoolReactiveProperty IsBallSet = new BoolReactiveProperty(false);
        private IntReactiveProperty[] _score;

        private void Start()
        {
            ballCore = ball.GetComponent<BallCore>();
            var r = respawnPoint.transform.position;

            _score = new IntReactiveProperty[2];
            for (var i = 0; i < 2; i++)
            {
                _score[i] = new IntReactiveProperty(0);
            }

            IsBallSet
                .Where(b => b == true)
                .Delay(TimeSpan.FromSeconds(3))
                .Subscribe(_ =>
                {
                    ballCore.ShootBall();
                    IsBallSet.Value = false;
                });


            this.goal[0].OnTriggerEnterAsObservable()
                .Where(collider => collider.tag == "Ball")
                .Subscribe(_ =>
                {
                    Debug.Log("goal1");
                    _score[1].Value++;
                    ballCore.SetBall(r);
                    IsBallSet.Value = true;
                });


            this.goal[1].OnTriggerEnterAsObservable()
                 .Where(collider => collider.tag == "Ball")
                 .Subscribe(_ =>
                 {
                     Debug.Log("goal2");
                     _score[0].Value++;
                     ballCore.SetBall(r);
                     IsBallSet.Value = true;
                 });

            _score[0].Merge(_score[1])
                 .Subscribe(_ => scoreText.text = $"{_score[0]} - {_score[1]}");
        }

        public void StartBattle()
        {
            IsBallSet.Value = true;
        }

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(IsBallSet.Value);
                stream.SendNext(_score[0].Value);
                stream.SendNext(_score[1].Value);
            }
            else
            {
                IsBallSet.Value = (bool)stream.ReceiveNext();
                _score[0].Value = (int)stream.ReceiveNext();
                _score[1].Value = (int)stream.ReceiveNext();
            }
        }
    }
}

