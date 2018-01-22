using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceHockey.Players
{
    public class PlayerCamera : BasePlayerComponent
    {
        [SerializeField] private GameObject player1Cam;
        [SerializeField] private GameObject player2Cam;

        protected override void OnInitialize()
        {

            switch (PlayerId.Instance.Player_Id)
            {
                case 1:
                    player1Cam.SetActive(true);
                    break;
                case 2:
                    player2Cam.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }
}

