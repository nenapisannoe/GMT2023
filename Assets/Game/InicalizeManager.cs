using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public static class InicalizeManager
    {
        public static List<HitableObject> allObjects = new List<HitableObject>();

        public static void disableAll(){
            foreach(HitableObject obj in allObjects){
                obj.gameObject.SetActive(false);
            }
        }

        public static void enableAll(){
            foreach(HitableObject obj in allObjects){
                obj.gameObject.SetActive(true);
            }
        }
    }
}
