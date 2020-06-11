using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevTalk.TestAttackMultiplier
{
    public class EffectsAttackMultiplier : MonoBehaviour
    {
        [SerializeField] private TestAttackMultiplier attackMultiplier;
        [SerializeField] private Image enemy;

        private void OnEnable()
        {
            attackMultiplier.OnStartAttack += StartAttack;
            attackMultiplier.OnAttack += EndAttack;
        }

        private void OnDisable()
        {
            attackMultiplier.OnStartAttack -= StartAttack;
            attackMultiplier.OnAttack -= EndAttack;
        }

        private void StartAttack()
        {
            enemy.color = new Color(1,0,0);
        }

        private void EndAttack(float attack = 0)
        {
            enemy.color = new Color(1, 1, 1);
        }
    }
}