using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Survinya.Stat.Mono
{
    public class APlayer : Actor
    {
        public override ActorType actorType => ActorType.Player;

        protected override void InitializeState()
        {
            base.InitializeState();
            state.CurrentHealth = 150;
            state.MaxHealth = 150;
            state.CurrentLevel = 1;
            state.MaxLevel = 10;
            state.CurrentExperience = 0;
            state.MaxExperience = 100;
        }

        protected override void OnEnable()
        {
            actor.Register(this);
        }

        protected override void OnDisable()
        {
            actor.Unregister(this);
        }

        protected override void Update()
        {
            base.Update();
        }

        public override void TakeDamage(int damage)
        {
            // 如果正在死亡或無敵，直接返回不處理傷害
            if (IsDead || IsInvincible) return;

            base.TakeDamage(damage);

            IsInvincible = true;

            // 使用 UniRx 的 Observable.Timer 來處理延遲
            Observable.Timer(TimeSpan.FromSeconds(1f))
                .Subscribe(_ =>
                {
                    IsInvincible = false;
                })
                .AddTo(this); // 確保物件被銷毀時，訂閱也會被清理

            // TODO: Implement player death
        }

        protected override void OnDeath()
        {
            base.OnDeath();
            IsDead = true;
            Debug.Log("Player is dead");
            // TODO: Implement after player death
        }

        protected override void GainExperience(int experience)
        {
            base.GainExperience(experience);
        }

        protected override void LevelUp()
        {
            base.LevelUp();
        }

        protected override void OnMaxLevel()
        {
            base.OnMaxLevel();
        }

        protected override IActor GetPlayer()
        {
            return null; // Don't need to find the player actor
        }

        protected override IActor GetNearestEnemy()
        {
            return actor.GetNearestActor(this);
        }

        protected override List<IActor> GetNearbyEnemies()
        {
            return actor.GetNearbyActors(this, 1f); // Unused for now
        }
    }
}