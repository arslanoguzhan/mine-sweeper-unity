using System;
using TMPro;
using UnityEngine;
using Zenject;

class LevelTextView : IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly IViewport _viewport;
    private readonly IUiRoot _uiRoot;
    private readonly ILevelDataController _levelDataController;

    private readonly GameObject _wrapper;
    private readonly TextMeshPro _textMeshPro;

    public LevelTextView(
        SignalBus signalBus,
        IViewport viewport,
        IUiRoot uiRoot,
        ILevelDataController levelDataController
    ){
        _signalBus = signalBus;
        _viewport = viewport;
        _uiRoot = uiRoot;
        _levelDataController = levelDataController;

        _wrapper = new GameObject(nameof(LevelTextView));
        _uiRoot.AddObject(_wrapper);

        _textMeshPro = _wrapper.AddComponent<TextMeshPro>();
        _textMeshPro.fontSize = 640;
        _textMeshPro.fontWeight = FontWeight.Bold;
        _textMeshPro.rectTransform.pivot = new Vector2(0,1);
        _textMeshPro.rectTransform.localPosition = new Vector3(0, 0);
        _textMeshPro.rectTransform.sizeDelta = new Vector2(_viewport.Width, 100);
        _textMeshPro.alignment = TextAlignmentOptions.Center;
        _textMeshPro.color = Color.white;

        SubscribeSignals();
    }

    private void SubscribeSignals()
    {
        _signalBus.Subscribe<BoardCreatedSignal>(UpdateText);
    }

    private void UnsubscribeSignals()
    {
        _signalBus.Unsubscribe<BoardCreatedSignal>(UpdateText);
    }
    public void UpdateText()
    {
        var level = _levelDataController.GetCurrentLevel().Number;
        _textMeshPro.text = $"LEVEL {level}";
    }

    public void Dispose()
    {
        UnsubscribeSignals();
    }
}