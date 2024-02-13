using UnityEngine;
using Zenject;

class MouseLeftButtonHandler : ITickable
{
    private readonly Camera _camera;

    private readonly ISwitcher _switcher;

    private const float HOLD_DURATION = 0.3f;

    private bool _down = false;

    private float _downTime;

    private Vector2 _downPosition;

    private IListener _downElement = null;

    public MouseLeftButtonHandler(
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
        if(Input.GetMouseButtonDown(0))
        {
            _down = true;
            _downTime = Time.unscaledTime;
            _downPosition = Input.mousePosition;

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

            var pos = Input.mousePosition;
            var duration = Time.unscaledTime - _downTime;

            if(pos.y - Constants.boxSize > _downPosition.y)
            {
                (_downElement as IListener<MouseLeftSwipeUp>)?.OnAction();
                Clear();
            }
            else if(pos.y < _downPosition.y - Constants.boxSize)
            {
                (_downElement as IListener<MouseLeftSwipeDown>)?.OnAction();
                Clear();
            }
            else if(Input.GetMouseButtonUp(0) && duration <= HOLD_DURATION)
            {
                (_downElement as IListener<MouseLeftClick>)?.OnAction();
                Clear();
            }
            else if(Input.GetMouseButtonUp(0) && duration > HOLD_DURATION)
            {
                (_downElement as IListener<MouseLeftHoldClick>)?.OnAction();
                Clear();
            }
            
        }
    }
}