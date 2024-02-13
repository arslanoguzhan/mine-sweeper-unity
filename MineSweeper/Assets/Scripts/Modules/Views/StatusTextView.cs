using System;
using TMPro;
using UnityEngine;
using Zenject;
using DG.Tweening;

class StatusTextView : IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly IViewport _viewport;
    private readonly IUiRoot _uiRoot;

    private readonly GameObject _wrapper;
    private readonly TextMeshPro _textMeshPro;

    public StatusTextView(
        SignalBus signalBus,
        IViewport viewport,
        IUiRoot uiRoot
    ){
        _signalBus = signalBus;
        _viewport = viewport;
        _uiRoot = uiRoot;

        _wrapper = new GameObject(nameof(StatusTextView));
        _wrapper.transform.localScale = new Vector3(0,0);

        _uiRoot.AddObject(_wrapper);

        _textMeshPro = _wrapper.AddComponent<TextMeshPro>();
        _textMeshPro.fontSize = 480;
        _textMeshPro.color = Color.white;
        _textMeshPro.rectTransform.pivot = new Vector2(0,1);
        _textMeshPro.rectTransform.localPosition = new Vector3(0, Constants.statusPositionY);
        _textMeshPro.rectTransform.sizeDelta = new Vector2(_viewport.Width, 100);
        _textMeshPro.alignment = TextAlignmentOptions.Center;

        SubscribeSignals();
    }

    private void SubscribeSignals()
    {
        _signalBus.Subscribe<VictorySignal>(OnVictory);
        _signalBus.Subscribe<DefeatSignal>(OnDefeat);
    }

    private void UnsubscribeSignals()
    {
        _signalBus.Unsubscribe<VictorySignal>(OnVictory);
        _signalBus.Unsubscribe<DefeatSignal>(OnDefeat);
    }

    public void OnVictory()
    {
        _textMeshPro.color = Color.green;
        _textMeshPro.text = "SUCCESS!";
        _wrapper.transform.DOScale(1, 0.2f);
    }

    public void OnDefeat()
    {
        _textMeshPro.color = Color.red;
        _textMeshPro.text = "FAILED!";
        _wrapper.transform.DOScale(1, 0.2f);
    }

    public void Dispose()
    {
        UnsubscribeSignals();
    }
}