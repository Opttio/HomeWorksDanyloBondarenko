using System;

namespace _Project.Scripts.Core.EventBus
{
    public static class GameUIBus
    {
        public static event Action<int> OnViewIdChanged;

        public static void ChangeViewId(int newViewId)
        {
            OnViewIdChanged?.Invoke(newViewId);
        }
    }
}