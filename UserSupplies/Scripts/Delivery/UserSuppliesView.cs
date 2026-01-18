using System.Collections;
using TMPro;
using UnityEngine;
using Xenocode.Features.UserSupplies.Scripts.Domain.Model;

namespace Xenocode.Features.UserSupplies.Scripts.Delivery
{
    public class UserSuppliesView : MonoBehaviour, IUserSuppliesView
    {
        [SerializeField] private TextMeshProUGUI _notificationText;
        [SerializeField] private TextMeshProUGUI _messageWarningText;
        [SerializeField] private TextMeshProUGUI _goldText;
        [SerializeField] private TextMeshProUGUI _killRewardText;
        private Coroutine _activeCoroutine;
        public void UpdateGoldText(int amount)
        {
            _notificationText.text = string.Empty;
            _goldText.text = $"Gold: {amount}";
        }

        public void UpdateNotificationText(int amount)
        {
            _notificationText.text = string.Empty;
            _notificationText.text = $"Gold income: +{amount}";
            _ = StartCoroutine(DisplayTextAndFadeout(_notificationText));
        }

        public void NotifyInsufficientFounds()
        {
            _messageWarningText.text = $"Not enough gold for build...";
            if (_activeCoroutine != null) StopCoroutine(_activeCoroutine);
            _activeCoroutine = StartCoroutine(DisplayTextAndFadeout(_messageWarningText));
        }
        
        IEnumerator DisplayTextAndFadeout(TextMeshProUGUI textComponent)
        {
            Color c = textComponent.color;
            c.a = 1f;
            textComponent.color = c;

            float duration = 5f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float newAlpha = Mathf.Lerp(1f, 0f, elapsed / duration);
                c.a = newAlpha;
                textComponent.color = c;
                yield return null;
            }
            c.a = 0f;
            textComponent.color = c;
        }

        public void NotifyKillReward(int reward)
        {
            _killRewardText.text = $"Kill: +{reward}";
            if (_activeCoroutine != null) StopCoroutine(_activeCoroutine);
            _activeCoroutine = StartCoroutine(DisplayTextAndFadeout(_killRewardText));
        }
    }
}

