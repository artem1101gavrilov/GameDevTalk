using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameDevTalk.TestAttackMultiplier
{
    public class DisplayAttackMultiplier : MonoBehaviour
    {
        [SerializeField] private TestAttackMultiplier attackMultiplier;
        [SerializeField] private Text textAttackMultiplier;

        private void OnEnable()
        {
            attackMultiplier.OnAttack += SetTextAttack;
        }

        private void OnDisable()
        {
            attackMultiplier.OnAttack -= SetTextAttack;
        }

        private void SetTextAttack(float attack)
        {
            textAttackMultiplier.text = attack.ToString();
        }
    }
}