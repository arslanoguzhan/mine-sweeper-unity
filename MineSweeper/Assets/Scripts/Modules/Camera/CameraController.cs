using UnityEngine;
using Zenject;

class CameraController : ICameraController, ITickable, IViewport
{
    private readonly Camera _camera;

    private readonly int _width = Constants.ViewportVirtualWidth;

    int IViewport.Width => _width;

    private Vector2 _resolution;

    public CameraController(Camera camera)
    {
        _camera = camera;
        _camera.orthographic = true;
    }

    public void Tick()
    {
        var csize = new Vector2(_camera.pixelWidth, _camera.pixelHeight);
        if( csize != _resolution)
        {
            _resolution = new Vector2(_camera.pixelWidth, _camera.pixelHeight);
            _camera.orthographicSize = _width * _resolution.y / _resolution.x / 2;
        }
    }
}