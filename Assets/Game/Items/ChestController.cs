using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public class ChestContriller :HitableObject
    {
        public Sprite ContaminatedSprite;
        public Sprite BaseSprite;
        public void Start(){
            ChestsStorage.active_chests.Add(this);
        }

        public bool isDanger = false;

        public void Awake(){
            base.Awake();
        }

        public void OnEnable(){ 
            base.OnEnable();
            isDanger = false;
            gameObject.GetComponent<SpriteRenderer>().sprite = BaseSprite;
        	ChestsStorage.active_chests.Add(this);         
    	}

        public override void attakMe(Damage attackDamage){
            if (attackDamage.Type is DamageType.BossAbility2){
                isDanger = true;
                gameObject.GetComponent<SpriteRenderer>().sprite = ContaminatedSprite;
            }
        }

        public void open() {
            ChestsStorage.active_chests.Remove(this);
            gameObject.SetActive(false);
        }
    }
}
