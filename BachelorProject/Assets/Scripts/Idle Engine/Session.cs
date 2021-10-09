using System.Linq;
using UnityEngine;
using System;

namespace IdleEngine.Session
{
    [CreateAssetMenu(fileName = "Session", menuName = "Idle/ Session", order = 0)]
    public class Session : ScriptableObject
    {

        #region vars
        public double Money;
        public double BaseIncome;

        public double ProductionTimeInSeconds;
        public double CollectedTimeInSeconds;

        public long LastTicks;
        #endregion


        public void Tick(double deltaTime)
        {
            Money += CalculatedProgression(deltaTime);
        }


        public void CalculateOfflineTick()
        {
            if (LastTicks <= 0)
            {
                return;
            }

            var _deltaTime = (DateTime.UtcNow.Ticks - LastTicks)/ TimeSpan.TicksPerSecond;

            var _moneyBefore = Money;

            Tick(_deltaTime);

            Debug.Log($"Calculated Money = {Money - _moneyBefore}");
        }

        public void SaveTicks()
        {
            LastTicks = DateTime.UtcNow.Ticks;
        }

        private double CalculatedProgression(double deltaTime)
        {
            CollectedTimeInSeconds += deltaTime;

            double calculatedSum = 0;

            while (CollectedTimeInSeconds >= ProductionTimeInSeconds)
            {
                calculatedSum += BaseIncome;
                CollectedTimeInSeconds -= ProductionTimeInSeconds;
            }

            return calculatedSum;
        }

    }
}