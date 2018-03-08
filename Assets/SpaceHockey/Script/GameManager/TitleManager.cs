using UniRx;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace SpaceHockey.GameManagers
{
    public class TitleManager : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private void Start()
        {
            startButton.OnClickAsObservable()
                .First()
                .Subscribe(_ => SceneManager.LoadScene("Main"));
        }
    }
}


