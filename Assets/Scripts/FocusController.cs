using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Focus
{
    public class FocusController : MonoBehaviour
    {
        [SerializeField] private float _focusTimeDuration;
        [SerializeField] private TextMeshProUGUI _focusCountText;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _resetButton;
        [SerializeField] private List<Image> _progressBars;

        private float _focusStartTime;
        private int _focusCount;
        private int _currentBar;
        
        private void Awake()
        {
            Assert.IsNotNull(_focusCountText);
            Assert.IsNotNull(_addButton);
            Assert.IsNotNull(_resetButton);
            Assert.IsNotNull(_progressBars);
            Assert.IsTrue(_progressBars.Count > 0);
            Assert.IsTrue(_focusTimeDuration > 0f);
            OnReset();
        }

        private void OnEnable()
        {
            _addButton.onClick.AddListener(OnAdd);
            _resetButton.onClick.AddListener(OnReset);
        }

        private void OnDisable()
        {
            _addButton.onClick.RemoveListener(OnAdd);
            _resetButton.onClick.RemoveListener(OnReset);
        }

        private void OnReset()
        {
            _currentBar = 0;
            _progressBars.ForEach(pb => pb.fillAmount = 0f);
            _addButton.gameObject.SetActive(false);
            _focusStartTime = Time.time;
        }

        private void OnAdd()
        {
            _focusCount++;
            _focusCountText.text = _focusCount.ToString();
            
            if (_currentBar >= _progressBars.Count)
            {
                _currentBar = _progressBars.Count - 1;
                _focusStartTime = Time.time;
                _progressBars[_currentBar - 1].fillAmount = 1f;
                _progressBars[_currentBar].fillAmount = 0f;
            }
            else
            {
                _progressBars[_currentBar - 1].fillAmount = _progressBars[_currentBar].fillAmount;
                _progressBars[_currentBar].fillAmount = 0f;
                _currentBar--;
            }
            
            if (_currentBar == 0) { _addButton.gameObject.SetActive(false); }
        }

        private void FixedUpdate()
        {
            if (_currentBar >= _progressBars.Count) { return; }
            float currentProgress = (Time.time - _focusStartTime) / _focusTimeDuration;

            if (currentProgress > 1f)
            {
                _progressBars[_currentBar].fillAmount = 1f;
                _currentBar++;
                _addButton.gameObject.SetActive(true);
                _focusStartTime = Time.time;
            }
            else
            {
                _progressBars[_currentBar].fillAmount = currentProgress;
            }
        }
    }
}