using System;
using UnityEngine;

namespace _Project.Scripts.Core.EventBus
{
    public static class GameEventBus
    {
        public static event Action<int> OnDistanceChanged;
        public static event Action<int> OnAttemptChanged;
        public static event Action<float> OnGameSpeed;
        public static event Action<Vector2> OnPositionChanged;
        public static event Action<float> OnHighestYChanged;

        public static void ChangeDistance(int newDistance) => OnDistanceChanged?.Invoke(newDistance);
        public static void ChangeAttempt(int newAttempt) => OnAttemptChanged?.Invoke(newAttempt);
        public static void ChangeGameSpeed(float newGameSpeed) => OnGameSpeed?.Invoke(newGameSpeed);
        public static void ChangePosition(Vector2 newPosition) => OnPositionChanged?.Invoke(newPosition);
        public static void ChangeHighestY(float newHighestY) => OnHighestYChanged?.Invoke(newHighestY);
    }
}