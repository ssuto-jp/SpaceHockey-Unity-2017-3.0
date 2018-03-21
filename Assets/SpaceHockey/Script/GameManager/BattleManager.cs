using System.Collections;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace SpaceHockey.GameManagers
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] goal = new GameObject[2];
        [SerializeField] private Transform respawnPoint;
        [SerializeField] private GameObject battlePanel;
        [SerializeField] private Text scoreText;

        private GameObject ball;
        private bool isEnteringGoal;

        public int MaxScore { get; } = 3;
        public IntReactiveProperty[] _score = new IntReactiveProperty[2];

        private BoolReactiveProperty _isWinner = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsWinner
        {
            get { return _isWinner; }
        }

        private void Awake()
        {
            for (var i = 0; i < 2; i++)
            {
                _score[i] = new IntReactiveProperty(0);
            }
        }

        private void Start()
        {
            //ゴールに入ったら得点を増やす
            this.goal[0].OnTriggerEnterAsObservable()
                .Where(other => other.tag == "Ball")
                .Subscribe(other =>
                {
                    isEnteringGoal = true;
                    ++_score[1].Value;
                });

            this.goal[1].OnTriggerEnterAsObservable()
                .Where(other => other.tag == "Ball")
                .Subscribe(other =>
                {
                    isEnteringGoal = true;
                    ++_score[0].Value;
                });

            //得点を表示
            _score[0].Merge(_score[1])
                .TakeWhile(score => score <= MaxScore)
                .Subscribe(score =>
                {
                    scoreText.text = $"{_score[0]} - {_score[1]}";
                    if (score == MaxScore)
                    {
                        _isWinner.Value = true;
                    }
                });
        }

        public IEnumerator BattleCoroutine()
        {
            yield return StartCoroutine(RoundStarting());

            yield return StartCoroutine(RoundPlaying());

            yield return StartCoroutine(RoundEnding());

            if (!IsWinner.Value)
            {
                StartCoroutine(BattleCoroutine());
            }
        }

        private IEnumerator RoundStarting()
        {
            //

            yield return new WaitForSeconds(1);
        }

        private IEnumerator RoundPlaying()
        {
            ball = PhotonNetwork.Instantiate("Ball", respawnPoint.transform.position, Quaternion.identity, 0);
            ball.GetComponent<PhotonView>().RPC("ShootBall", PhotonTargets.AllViaServer);

            while (!isEnteringGoal)
            {
                yield return null;
            }
        }


        private IEnumerator RoundEnding()
        {
            if (isEnteringGoal)
            {
                PhotonNetwork.Destroy(ball);
                isEnteringGoal = false;
            }

            yield return new WaitForSeconds(1);
        }

        [PunRPC]
        public void DisplayBattlePanel()
        {
            battlePanel.SetActive(true);
        }

        private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.isWriting)
            {
                stream.SendNext(_score[0].Value);
                stream.SendNext(_score[1].Value);
            }
            else
            {
                _score[0].Value = (int)stream.ReceiveNext();
                _score[1].Value = (int)stream.ReceiveNext();
            }
        }
    }
}

