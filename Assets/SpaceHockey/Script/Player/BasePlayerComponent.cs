using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace SpaceHockey.Players
{
    public abstract class BasePlayerComponent : MonoBehaviour
    {
        private void Start()
        {
            PlayerId.Instance.OnInitializeAsync
                .Subscribe(_ => OnInitialize());
        }

        protected abstract void OnInitialize();
    }
}
