using UnityEngine;
using Zenject;

class TouchHandler : ITickable
{
    private readonly Camera _camera;

    private readonly ISwitcher _switcher;

    private const float HOLD_DURATION = 0.3f;

    private bool _down = false;

    private float _downTime;

    private Vector2 _downPosition;

    private IListener _downElement = null;

    public TouchHandler(
        Camera camera,
        ISwitcher switcher
    ){
        _camera = camera;
        _switcher = switcher;
    }

    private void Clear()
    {
        _down = false;
        _downTime = 0;
        _downPosition = default;
        _downElement = null;
    }

    public void Tick()
    {
        if(Input.touchCount == 0)
            return;

        var t = Input.GetTouch(0);
        if (t.phase == TouchPhase.Began)
        {
            _down = true;
            _downTime = Time.unscaledTime;
            _downPosition = t.position;

            RaycastHit2D hit = Physics2D.Raycast(_camera.ScreenToWorldPoint(_downPosition), Vector2.zero);

            if(hit.collider != null)
            {
                if(hit.collider.TryGetComponent(out IListener component))
                {
                    _downElement = component;
                }
            }
        }
        else
        {
            if(!_down)
                return;

            if(_switcher.IsWorldDisabled && _downElement is not IUiElement)
            {
                Clear();
                return;
            }

            var duration = Time.unscaledTime - _downTime;
            var pos = t.position;

            if(pos.y - Constants.boxSize/2 > _downPosition.y)
            {
                (_downElement as IListener<TouchSwipeUp>)?.OnAction();
                Clear();
            }
            else if(pos.y < _downPosition.y - Constants.boxSize/2)
            {
                (_downElement as IListener<TouchSwipeDown>)?.OnAction();
                Clear();
            }
            else if(t.phase == TouchPhase.Ended && duration <= HOLD_DURATION)
            {
                (_downElement as IListener<TouchTap>)?.OnAction();
                Clear();
            }
            else if(duration > HOLD_DURATION)
            {
                (_downElement as IListener<TouchHold>)?.OnAction();
                Clear();
            }
        }
    }
}
