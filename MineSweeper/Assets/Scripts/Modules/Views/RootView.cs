using UnityEngine;

class RootView : IWorldRoot, IUiRoot
{
    private readonly IViewport _viewport;

    private readonly GameObject _worldRoot;

    private readonly GameObject _uiRoot;

    public RootView(IViewport viewport)
    {
        _viewport = viewport;

        var virtualHeight = _viewport.Width * Screen.height / Screen.width;

        var unsafeTopMarginReal = Screen.height - Screen.safeArea.yMax;
        var unsafeTopMarginVirtual = unsafeTopMarginReal * _viewport.Width / Screen.width;

        var startX = -_viewport.Width / 2;
        var startY = virtualHeight / 2 - unsafeTopMarginVirtual;

        _worldRoot = new GameObject("safeWorldRoot");
        _worldRoot.transform.localPosition = new Vector3(startX, startY);

        _uiRoot = new GameObject("safeUiRoot");
        _uiRoot.transform.localPosition = new Vector3(startX, startY);
    }

    void IWorldRoot.AddObject(GameObject childObject)
    {
        childObject.transform.parent = _worldRoot.transform;
    }

    void IUiRoot.AddObject(GameObject childObject)
    {
        childObject.transform.parent = _uiRoot.transform;
    }
}
