using UnityEngine;

class BoxListener : MonoBehaviour,
    IListener<TouchTap>,
    IListener<TouchSwipeUp>,
    IListener<TouchSwipeDown>,
    IListener<MouseLeftClick>,
    IListener<MouseRightClick>,
    IListener<MouseLeftSwipeUp>,
    IListener<MouseLeftSwipeDown>
{
    private Box _box;

    public void Setup(Box box)
    {
        _box = box;
    }

    void IListener<MouseLeftClick>.OnAction()
    {
        if(_box.State != BoxState.Open)
        {
            _box.OnOpen();
        }
    }

    void IListener<MouseRightClick>.OnAction()
    {
        if(_box.State == BoxState.Button)
        {
            _box.OnFlag();
        }
        else if(_box.State == BoxState.Flag)
        {
            _box.OnUnFlag();
        }
    }

    void IListener<TouchTap>.OnAction()
    {
        if(_box.State != BoxState.Open)
        {
            _box.OnOpen();
        }
    }

    void IListener<TouchSwipeUp>.OnAction()
    {
        if(_box.State == BoxState.Button)
        {
            _box.OnFlag();
        }
    }

    void IListener<TouchSwipeDown>.OnAction()
    {
        if(_box.State == BoxState.Flag)
        {
            _box.OnUnFlag();
        }
    }

    void IListener<MouseLeftSwipeUp>.OnAction()
    {
        if(_box.State == BoxState.Button)
        {
            _box.OnFlag();
        }
    }

    void IListener<MouseLeftSwipeDown>.OnAction()
    {
        if(_box.State == BoxState.Flag)
        {
            _box.OnUnFlag();
        }
    }
}