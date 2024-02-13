using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Zenject;

class NextLevelButtonView : IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly IViewport _viewport;
    private readonly IUiRoot _uiRoot;
    private readonly GameContext _gameContext;

    private readonly GameObject _wrapper;
    private readonly TextMeshPro _textMeshPro;

    public NextLevelButtonView(
        SignalBus signalBus,
        IViewport viewport,
        IUiRoot uiRoot,
        GameContext gameContext
    ){
        _signalBus = signalBus;
        _viewport = viewport;
        _uiRoot = uiRoot;
        _gameContext = gameContext;

        _wrapper = new GameObject(nameof(NextLevelButtonView));
        _wrapper.transform.localScale = new Vector3(0,0);

        _uiRoot.AddObject(_wrapper);

        _textMeshPro = _wrapper.AddComponent<TextMeshPro>();
        _textMeshPro.fontSize = 480;
        _textMeshPro.rectTransform.pivot = new Vector2(0,1);
        _textMeshPro.rectTransform.localPosition = new Vector3(0, -1810);
        _textMeshPro.rectTransform.sizeDelta = new Vector2(_viewport.Width, 100);
        _textMeshPro.alignment = TextAlignmentOptions.Center;
        _textMeshPro.color = Color.white;
        _textMeshPro.text = "NEXT LEVEL";

        var boxCollider = _wrapper.AddComponent<BoxCollider2D>();
        boxCollider.offset = new Vector2(540, -50);
        boxCollider.size = new Vector2(_viewport.Width, 100);

        var inputHandler = _wrapper.AddComponent<NextLevelButtonListener>();

        SubscribeSignals();
    }

    private void SubscribeSignals()
    {
        _signalBus.Subscribe<VictorySignal>(UpdateText);
    }

    private void UnsubscribeSignals()
    {
        _signalBus.Unsubscribe<VictorySignal>(UpdateText);
    }

    public void UpdateText()
    {
        _wrapper.transform.DOScale(1, 0.2f).SetDelay(0.2f);
    }

    public void Dispose()
    {
        UnsubscribeSignals();
    }
}
