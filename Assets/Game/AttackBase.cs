using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Game {
	
	public class AttackBase : MonoBehaviour {

		[SerializeField] private SpriteRenderer m_Sprite;

		public Vector2 CheckPosition(Vector2 mousePos, Vector2 characterPos)
		{
			var newPos = new Vector3(Mathf.Clamp(mousePos.x, characterPos.x - 1f, characterPos.x + 1f), Mathf.Clamp(mousePos.y, characterPos.y - 1f, characterPos.y + 1f), 0f); 
			// проверяем позицию и корректируем в зависимости от скилла
			return newPos;
		}

		public async UniTask Run() {
			var sequence = DOTween.Sequence();
			sequence.Insert(0f, m_Sprite.DOColor(Color.red, 0.5f));
			sequence.Insert(0.5f, m_Sprite.DOColor(Color.white, 0.5f));
			await sequence.ToUniTask();
		}

		private void AttackTrigger() {
			//проверяем коллайдер, не попали ли в него враги
			
		}
		
	}
	
}