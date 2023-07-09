using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public class LavaScript : MonoBehaviour
    {
        public float frequency = 0.1f;
        public float lavadDamage = 40;
        private void OnTriggerEnter2D(Collider2D other) {
            var target = other.gameObject.GetComponent<HitableObject>();
            if (target != null){
                StartCoroutine(DamageCharacter(target));
            }
        }

        private void OnTriggerExit2D(Collider2D other) {
            var target = other.gameObject.GetComponent<HitableObject>();
            if (target != null){
                StopAllCoroutines();
            }
        }

        private IEnumerator DamageCharacter(HitableObject target) {
            while (true) {
                target.attakMe(new Damage {
                    Type = DamageType.Magma,
                    Value = lavadDamage
                });
                yield return new WaitForSeconds(frequency);
            }
        }
    }
}

