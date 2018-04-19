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
        private PhotonView photonView;
        private GameObject ball;
        private bool isEnteringGoal;
        public int MaxScore { get; } = 6;
        public IntReactiveProperty[] _score = new IntReactiveProperty[2];

        private BoolReactiveProperty _isWinner = new BoolReactiveProperty(false);
        public IReadOnlyReactiveProperty<bool> IsWinner
        {
            get { return _isWinner; }
        }

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();

            for (var i = 0; i < 2; i++)
            {
                _score[i] = new IntReactiveProperty(0);
            }
        }

        private void Start()
        {
            stageManager = stageObject.GetComponent<StageManager>();

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

        public void StartBattle()
        {
            photonView.RPC("InitializePlayer", PhotonTargets.AllViaServer, null);
            photonView.RPC("DisplayBattlePanel", PhotonTargets.AllViaServer, null);
            SetGoalCollider();
            StartCoroutine(BattleCoroutine());
        }

        public IEnumerator BattleCoroutine()
        {
            yield return StartCoroutine(RoundStarting());

            yield return StartCoroutine(RoundEnding());

            if (!IsWinner.Value)
            {
                StartCoroutine(BattleCoroutine());
            }
        }

        private IEnumerator RoundStarting()
        {
            yield return new WaitForSeconds(1);

            ball = PhotonNetwork.Instantiate("Ball", ballSpawnPos.transform.position, Quaternion.identity, 0);
            ball.GetComponent<PhotonView>().RPC("ShootBall", PhotonTargets.AllViaServer);

            yield return new WaitForSeconds(Random.Range(5, 10));

            if (!isEnteringGoal)
            {
                yield return stageManager.ChangeStage(Random.Range(0, 3));
            }

            while (!isEnteringGoal)
            {
                yield return null;
            }
        }

        private IEnumerator RoundEnding()
        {
            if (isEnteringGoal)
            {
                stageManager.ResetStage();
                isEnteringGoal = false;
            }

            yield return new WaitForSeconds(2);
        }

        [PunRPC]
        public void DisplayBattlePanel()
        {
            battlePanel.SetActive(true);
        }

        private void SetGoalCollider()
        {
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

