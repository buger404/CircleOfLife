using System;
using CircleOfLife.Battle;
using CircleOfLife.Buff;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Milutools.Recycle;

namespace CircleOfLife
{
    public class ElectricFencing : BuildBase
    {
        public GameObject EffectPrefab;
        public override List<LevelUpDirection> LevelUpDirections => new()
        {
            
        };
        
        public override void HurtAction(BattleContext context)
        {
            if (context.AttackerData!=null)
            {
                DamageManagement.BuffDamage(context.AttackerData, Stats.Current.Attack);
                ShowEffectAnimation(context.AttackerData.Transform.position);
            }
            if (Stats.Current.Hp <= 0) RecyclePool.ReturnToPool(gameObject);
           
        }

        private void Awake()
        {
            Level = 1;
            NowType = BuildSkillType.ElectricFencing;
            Stats = Attribute[0].Build(gameObject, HurtAction);
            RecyclePool.EnsurePrefabRegistered(BuildEffects.Thunder, EffectPrefab, 20);
        }

        protected override void LevelUpFunc()
        {

        }

      

        private void ShowEffectAnimation(Vector3 position)
        {
            RecyclePool.Request(BuildEffects.Thunder, (c) =>
            {
                c.Transform.position = position;
                c.GameObject.SetActive(true);
            });
        }

        private void Skill(Collision2D collision)
        {
            BattleStats stats = collision.collider.GetBattleStats();
            if (stats == null) return;
            if (stats.BattleEntity.FactionType == FactionType.Enemy)
            {
                DamageManagement.Damage(new BattleContext(PhysicsLayer,Stats,stats));
                stats.BattleEntity.Stats.ApplyBuff(BuffUtils.ToBuff(UniversalBuff.SlowDown, 5f));
                ShowEffectAnimation(stats.Transform.position);
            }
        }

        public override void FixedUpdateFunc()
        {
           
        }

        public override void OnColliderEnterFunc(Collision2D collision)
        {
            Skill(collision);
        }

        public override void OnColliderTriggerFunc(Collision2D collision)
        {
            if (!TimerFinish) return;
            Skill(collision);
        }

        public override void OnEnableFunc()
        {
          
            NowType = BuildSkillType.ElectricFencing;
       
        }

        public override void OnDisableFunc()
        {
            
        }
    }
}
