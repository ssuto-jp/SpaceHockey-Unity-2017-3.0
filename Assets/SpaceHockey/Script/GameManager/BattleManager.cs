using System.Collections;
using SpaceHockey.Gimmicks;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using TMPro;

namespace SpaceHockey.GameManagers
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private GameObject stageObject;
        [SerializeField] private GameObject[] goal = new GameObject[2];
        [SerializeField] private Transform ballSpawnPos;
        [SerializeField] private GameObject battlePanel;
        [SerializeField] private TextMeshProUGUI scoreText;

        private StageManager stageManager;
        private GameObject ball;
        private bool isEnteringGoal;

        public int MaxScore { get; } = 2;
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
            stageManager = stageObject.GetComponent<StageManager>();

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
                        stageManager.ResetStage();
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
            stageManager.ResetStage();

            yield return new WaitForSeconds(2);
        }

        private IEnumerator RoundPlaying()
        {
            ball = PhotonNetwork.Instantiate("Ball", ballSpawnPos.transform.position, Quaternion.identity, 0);
            ball.GetComponent<PhotonView>().RPC("ShootBall", PhotonTargets.AllViaServer);

            yield return new WaitForSeconds(Random.Range(5, 10));

            yield return stageManager.ChangeStage(0);

            while (!isEnteringGoal)
            {
                yield return null;
            }
        }

        private IEnumerator RoundEnding()
        {
            if (isEnteringGoal)
            {
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

