using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game {
    public static class ChestsStorage
    {
        public static List<ChestContriller> active_chests = new List<ChestContriller>();
    }
    
    public static class BarrelsStorage
    {
        public static List<ExplosiveBarrelController> active_barrels = new List<ExplosiveBarrelController>();
    }
}
