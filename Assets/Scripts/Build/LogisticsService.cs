using System;
using CircleOfLife.Battle;
using CircleOfLife.Buff;
using Milutools.Recycle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CircleOfLife
{
    
    public class LogisticsService : BuildBase
    {
        public enum LogisticsServiceBuffType
        {
            加速=1,
            加护甲=2,
            加攻击力=4
        }
        public override List<LevelUpDirection> LevelUpDirections
        {
            get
            {
                if (Level == 1)
                {
                    return new()
                    {
                        new LevelUpDirection()
                        {
                            Title="加速",
                            Value=LogisticsServiceBuffType.加速,
                            NeedLevel=2,
                        },
                        new LevelUpDirection()
                        {
                            Title="加护甲",
                            Value=LogisticsServiceBuffType.加护甲,
                            NeedLevel=2,
                        },
                        new LevelUpDirection()
                        {
                            Title="加攻击力",
                            Value=LogisticsServiceBuffType.加攻击力,
                            NeedLevel=2,
                        },
                    };
                    
                }else if (Level == 2)
                {
                    var result = new List<LevelUpDirection>()
                    {
                        new LevelUpDirection()
                        {
                            Title="加速",
                            Value=LogisticsServiceBuffType.加速,
                            NeedLevel=3,
                        },
                        new LevelUpDirection()
                        {
                            Title="加护甲",
                            Value=LogisticsServiceBuffType.加护甲,
                            NeedLevel=3,
                        },
                        new LevelUpDirection()
                        {
                            Title="加攻击力",
                            Value=LogisticsServiceBuffType.加攻击力,
                            NeedLevel=3,
                        },
                    };
                    result.RemoveAll(x => x.Value.Equals(NowType));
                    return result;

                }

                return new();
              
            }
        }

  
        
        public override void HurtAction(BattleContext context)
        {
            if (context.HitData.Current.Hp <= 0)
            {
                RecyclePool.ReturnToPool(gameObject);
            }
        }

        private void Awake()
        {
            Level = 1;
            NowType = BuildSkillType.LogisticsService;
            Stats = Attribute[0].Build(gameObject, HurtAction);
        }
        private void OnEnable()
        {
            Level = 1;
            NowType = BuildSkillType.LogisticsService;
            allType = new();
            ReplaceStats(Attribute[0], true);

        }
        public float SpeedUpValue, ArmorUpValue, AttackUpValue;

        private void SpeedUp(BattleStats stats, BuffContext buff)
        {

            stats.Current.Velocity += SpeedUpValue;
        }

        private void ArmorUp(BattleStats stats, BuffContext buff)
        {

            stats.Current.Armor += ArmorUpValue;
        }
        private void AttackUp(BattleStats stats, BuffContext buff)
        {

            stats.Current.Attack += AttackUpValue;
        }
        private void FixedUpdate()
        {
            RecoveryHP();


            if (TimerFinish)
            {
                var list = BattleRange.GetAllFriendInRange(PhysicsLayer, FactionType.Friend);
                foreach(var item in list)
                {

                    if (allType.Contains(LogisticsServiceBuffType.加速))
                    {
                        Stats.ApplyBuff(BuffUtils.ToBuff(SpeedUp, 5f));
                    }
                    if (allType.Contains(LogisticsServiceBuffType.加护甲))
                    {
                        Stats.ApplyBuff(BuffUtils.ToBuff(ArmorUp, 5f));

                    }
                    if (allType.Contains(LogisticsServiceBuffType.加攻击力))
                    {
                        Stats.ApplyBuff(BuffUtils.ToBuff(AttackUp, 5f));
                    }



                }


            }
        }

        public void OnRoundFinish()
        {
            Debug.Log($"获得资源:{Stats.Current.Attack}");
        }

        protected override void LevelUpFunc()
        {
           
        }
    }
}
