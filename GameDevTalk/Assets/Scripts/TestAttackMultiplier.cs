using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTalk.TestAttackMultiplier
{
    public class TestAttackMultiplier : MonoBehaviour
    {
        public event System.Action<float> OnAttack;
        public event System.Action OnStartAttack;

        private bool isAttack;
        private float timerAttack;

        private void Update()
        {
            if(isAttack)
            {
                timerAttack += Time.deltaTime;
            }
        }

        public void StartAttack()
        {
            OnStartAttack?.Invoke();
            isAttack = true;
        }

        public void EndAttack()
        {
            isAttack = false;
            OnAttack?.Invoke(timerAttack);
            timerAttack = 0;
        }
    }
}