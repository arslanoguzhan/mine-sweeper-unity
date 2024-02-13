using System;
using UnityEngine;

class Switcher : ISwitcher
{
    private int _layerUi = 0;
    private int _layerWorld = 0;

    public bool IsUiEnabled => _layerUi == 0;

    public bool IsUiDisabled => _layerUi > 0;

    public bool IsWorldEnabled => _layerWorld == 0;

    public bool IsWorldDisabled => _layerWorld > 0;

    public void EnableUi()
    {
        _layerUi = Math.Max(0, _layerUi-1);
    }

    public void DisableUi()
    {
        _layerUi++;
    }

    public void EnableWorld()
    {
        _layerWorld = Math.Max(0, _layerWorld-1);
    }

    public void DisableWorld()
    {
        _layerWorld++;
    }
}