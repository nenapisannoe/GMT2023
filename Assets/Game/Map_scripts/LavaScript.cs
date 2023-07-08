using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public class LavaScript : MonoBehaviour
    {
        public float frequency = 2;
        public float lavadDamage = 2;
        private void OnTriggerEnter2D(Collider2D other) {
            var target = other.gameObject.GetComponent<Character>();
            if (target != null){
                StartCoroutine(DamageCharacter(target));
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var target = other.gameObject.GetComponent<Character>();
            if (target != null){
                StopAllCoroutines();
            }
        }

        private IEnumerator DamageCharacter(Character target) {
            while (true) {
                target.attakMe(new Damage {
                    Type = DamageType.Fire,
                    Value = lavadDamage
                });
                yield return new WaitForSeconds(frequency);
            }
        }
    }
}

