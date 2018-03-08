using SpaceHockey.Balls;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceHockey.GameManagers
{
    public class BattleManager : Photon.MonoBehaviour
    {
        private PhotonView photon;
        private Ball ballScript;
        public GameObject ball;
        [SerializeField] private GameObject[] goal = new GameObject[2];
        [SerializeField] private GameObject respawnPoint;
        [SerializeField] private GameObject battlePanel;
        [SerializeField] private Text scoreText;

        public IntReactiveProperty[] _score;
        public int finalScore { get; private set; } = 6;

        private BoolReactiveProperty _isBallSet = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsBallSet
        {
            get { return _isBallSet; }
        }

        private FloatReactiveProperty _currentTime = new FloatReactiveProperty(0);
        public IReadOnlyReactiveProperty<float> CurrentTime
        {
            get { return _currentTime; }
        }

        private void Start()
        {
            photon = GetComponent<PhotonView>();
            ballScript = ball.GetComponent<Ball>();
            var r = respawnPoint.transform.position;

            _score = new IntReactiveProperty[2];
            for (var i = 0; i < 2; i++)
            {
                _score[i] = new IntReactiveProperty(0);
            }

            //プレイ開始時の処理
            IsBallSet
                .Where(b => b == true)
                .Subscribe(_ =>
                {
                    ballScript.ShootBall();
                    _isBallSet.Value = false;
                });

            //ゴールに入れた時の処理  
            this.goal[0].OnTriggerEnterAsObservable()
                .Where(collider => collider.tag == "Ball")
                .Subscribe(_ =>
                {
                    _score[1].Value++;
                    _currentTime.Value = 0;
                    ballScript.SetBall(r);
                    _isBallSet.Value = true;
                });

            this.goal[1].OnTriggerEnterAsObservable()
                 .Where(collider => collider.tag == "Ball")
                 .Subscribe(_ =>
                 {
                     _score[0].Value++;
                     _currentTime.Value = 0;
                     ballScript.SetBall(r);
                     _isBallSet.Value = true;
                 });

            //得点を表示
            _score[0].Merge(_score[1])
                 .Subscribe(_ => scoreText.text = $"{_score[0]} - {_score[1]}");

            this.UpdateAsObservable()
                .Where(_ => IsBallSet.Value == false)
                .Subscribe(_ =>
                {
                    _currentTime.Value += Time.deltaTime;
                });
        }

        //初期化
        public void StartBattle()
        {
            _currentTime.Value = 0;
            _isBallSet.Value = true;
            photon.RPC("DisplayBattlePanel", PhotonTargets.All);
        }

        [PunRPC]
        private void DisplayBattlePanel()
        {
            battlePanel.SetActive(true);
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
                _isBallSet.Value = (bool)stream.ReceiveNext();
                _score[0].Value = (int)stream.ReceiveNext();
                _score[1].Value = (int)stream.ReceiveNext();
            }
        }
    }
}

