﻿using System.Collections.Generic;
using Model.Runtime.Projectiles;
using UnityEngine;

namespace UnitBrains.Player
{
    public class SecondUnitBrain : DefaultPlayerUnitBrain
    {
        public override string TargetUnitName => "Cobra Commando";
        private const float OverheatTemperature = 3f;
        private const float OverheatCooldown = 2f;
        private float _temperature = 0f;
        private float _cooldownTime = 0f;
        private bool _overheated;
        
        protected override void GenerateProjectiles(Vector2Int forTarget, List<BaseProjectile> intoList)
        {
            float overheatTemperature = OverheatTemperature;
            ///////////////////////////////////////
            // Homework 1.3 (1st block, 3rd module)
            ///////////////////////////////////////        

            // Implement Temperature Overheating
            int curentTemperature = GetTemperature();
            if (curentTemperature >= overheatTemperature)
            {
                return;
            }
            IncreaseTemperature();

            // Implement Projectile Boost
            for (int projectileBoostIndex = 0; projectileBoostIndex <= curentTemperature; projectileBoostIndex++)
            {
                var projectile = CreateProjectile(forTarget);
                AddProjectileToList(projectile, intoList);
            }

            ///////////////////////////////////////
        }

        public override Vector2Int GetNextStep()
        {
            return base.GetNextStep();
        }

        protected override List<Vector2Int> SelectTargets()
        {
            ///////////////////////////////////////
            // Homework 1.4 (1st block, 4rd module)
            ///////////////////////////////////////
            List<Vector2Int> result = GetReachableTargets();

            // sometimes it happens that the number of available targets is zero =(
            // or there is only one avaible target
            // returning result in this case
            if (result.Count > 1)
            {
                Vector2Int nearestTarget = result[0];
                float nearestDistance = DistanceToOwnBase(nearestTarget);
                foreach (var target in result)
                {
                    float distance = DistanceToOwnBase(target);
                    if (nearestDistance > distance)
                    {
                        nearestTarget = target;
                        nearestDistance = distance;
                    }
                }
                result.Clear();
                result.Add(nearestTarget);
                return result;
            }
            else
            {
                return result;
            }
            ///////////////////////////////////////
        }

        public override void Update(float deltaTime, float time)
        {
            if (_overheated)
            {              
                _cooldownTime += Time.deltaTime;
                float t = _cooldownTime / (OverheatCooldown/10);
                _temperature = Mathf.Lerp(OverheatTemperature, 0, t);
                if (t >= 1)
                {
                    _cooldownTime = 0;
                    _overheated = false;
                }
            }
        }

        private int GetTemperature()
        {
            if(_overheated) return (int) OverheatTemperature;
            else return (int)_temperature;
        }

        private void IncreaseTemperature()
        {
            _temperature += 1f;
            if (_temperature >= OverheatTemperature) _overheated = true;
        }
    }
}