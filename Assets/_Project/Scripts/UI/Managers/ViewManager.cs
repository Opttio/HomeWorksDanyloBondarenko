using _Project.Scripts.Core.EventBus;
using _Project.Scripts.Runtime.Interfaces;
using UnityEngine;

namespace _Project.Scripts.UI.Managers
{
    public class ViewManager : MonoBehaviour
    {
        [SerializeField] private Canvas[] _views;

        private void OnEnable()
        {
            SubscribeToEvents();
        }

        private void Start()
        {
            ActivateView(0);
        }

        private void OnDisable()
        {
            UnSubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            GameUIBus.OnViewIdChanged += ActivateView;
        }

        private void UnSubscribeToEvents()
        {
            GameUIBus.OnViewIdChanged -= ActivateView;
        }

        public void ActivateView(int id)
        {
            if (id >= _views.Length)
                return;
            foreach (var view in _views)
                view.enabled = false;
            _views[id].enabled = true;
            var viewComponent = _views[id].GetComponent<IView>();
            viewComponent?.OnViewActivated();
        }
    }
}