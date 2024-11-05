using CircleOfLife.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleOfLife
{
    public abstract class BuildBase : MonoBehaviour, IBattleEntity
    {
        public BattleStats.Stats Attribute;
        public BattleStats Stats { get; set; }
        public FactionType FactionType => factionType;
        [SerializeField]
        private FactionType factionType;

        public BattleRange BattleRange;
        public GameObject RangeObj;
        protected bool TimerFinish
        {
            get
            {
                if (timer + Stats.Current.AttackInterval <= Time.time)
                {
                    timer = Time.time;
                    return true;
                }
                return false;
            }
        }

        private float timer = 0;


        public abstract void HurtAction(BattleStats battleStats);

        public void ShowRange()
        {
            RangeObj.SetActive(true);
            RangeObj.transform.localScale = new Vector3(BattleRange.Range.radius, BattleRange.Range.radius, 1);
        }

    }
}
