using System;
using CircleOfLife.Battle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleOfLife
{
    public abstract class BuildBase : MonoBehaviour, IBattleEntity
    {
        public List<BattleStats.Stats> Attribute;
        public BattleStats Stats { get; set; }
        public FactionType FactionType => factionType;
        [SerializeField]
        private FactionType factionType;

        public BattleRange BattleRange;
        public GameObject RangeObj;

        public LayerMask PhysicsLayer;
        private float needResetTime = 0;
        private bool needReset=false;
        protected bool TimerFinish
        {
            get
            {
                if(needReset&& needResetTime<Time.time) timer = Time.time;
                if (timer + Stats.Current.AttackInterval <= Time.time)
                {       
                    needReset = true;
                    return true;
                }
                needReset = false;
                return false;
            }
        }

        private float timer = 0;

        public class LevelUpDirection
        {
            public string Title;
            public Enum Value;
            public int NeedLevel;
        }
        public abstract List<LevelUpDirection> LevelUpDirections { get; }
        public int Level { get; protected set; }

        public Enum NowType { get; protected set; }

        protected List<Enum> allType=new();

        public bool WhetherSelectDirection;

        protected abstract void LevelUpFunc();
         
        public void LevelUp(Enum direction = null)
        {
            Level++;
            NowType = direction;
            if (direction != null&&!allType.Contains(direction)) allType.Add(direction);
            ReplaceStats(Attribute[Level - 1]);
            LevelUpFunc();
        }
        public void ChangeDirection(Enum direction)
        {
            NowType = direction;
        }

        public abstract void HurtAction(BattleContext context);

        public void ShowRange()
        {
            RangeObj.SetActive(true);
            UpdateRange();
        }
        public void CloseRange()
        {
            RangeObj.SetActive(false);
        }
        public void OnReset()
        {
            Stats.Reset();
            
        }

        public void ReplaceStats(BattleStats.Stats stats,bool isReset=false)
        {
            float befoHP = Stats.Current.Hp;
            Stats.ReplaceBaseStat(stats);
            if (!isReset) Stats.Current.Hp =Mathf.Min(befoHP, Stats.Current.Hp);
        }


        private bool NeedRecovery
        {
            get
            {
                if (recoveryTimer + 1 <= Time.time)
                {
                    recoveryTimer = Time.time;
                    return true;
                }
                return false;

            }
        }

        private float recoveryTimer=0;

        public void RecoveryHP()
        {
            if (NeedRecovery)
            {
                var list = BattleRange.GetAllFriendInRange(PhysicsLayer, factionType);
                Stats.Current.Hp += 5 * list.Count;
            }
        }

        public void UpdateRange()
        {
            BattleRange.Range.radius = Stats.Current.EffectRange;
            RangeObj.transform.localScale = new Vector3(BattleRange.Range.radius, BattleRange.Range.radius, 1);
        }

  

    }
}
