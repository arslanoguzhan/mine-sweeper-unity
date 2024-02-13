interface ISwitcher
{
    void EnableUi();
    void EnableWorld();

    void DisableUi();
    void DisableWorld();

    bool IsUiEnabled {get;}
    bool IsWorldEnabled {get;}

    bool IsUiDisabled {get;}
    bool IsWorldDisabled {get;}
}