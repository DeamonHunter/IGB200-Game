using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Enemies {
    /* This is the way waves should be spawned.
     * The first array is the enemy type to be spawned.
     * The second is the spawner to spawn it at.
     * 
     * E.G.
     * 0 0 0 1
     * 1 2 3 4
     * Will spawn 1 enemy 0 at spawner 1. 1 enemy 0 at spawner 2. 1 enemy 0 at spawner 3. 1 enemy 1 at spawner 4.
     */
    [Serializable]
    public class Wave {
        public int[] Enemies;
        public int[] Spawner;
        public float[] HealthMultipliers;
        public float SecondsBetweenEnemies;
        public bool IsWaveDark;
    }
}
