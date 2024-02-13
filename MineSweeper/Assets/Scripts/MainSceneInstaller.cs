using TMPro;
using UnityEngine;
using Zenject;

class MainSceneInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        InstallEntryPoint();
        InstallMonoBehaviors();
        InstallEndpoints();
        InstallHelpers();
        InstallInputs();
        InstallControllers();
        InstallViews();

        DeclareSignals();
    }

    private void InstallEntryPoint()
    {
        Container.BindInterfacesTo<EntryPoint>().AsSingle().NonLazy();
        Container.BindExecutionOrder<EntryPoint>(1);
        //Container.BindInitializableExecutionOrder<Startup>(1);
        //Container.BindInterfacesTo<CameraController>();

        Container.Bind<IAssetManager>().To<AssetManager>().AsSingle().NonLazy();
    }

    private void InstallViews()
    {
        Container.BindInterfacesTo<RootView>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<LevelTextView>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<BombCountTextView>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<StatusTextView>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<RestartButtonView>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<NextLevelButtonView>().AsSingle().NonLazy();
    }

    private void InstallMonoBehaviors()
    {
        Container.BindInstance(Camera.main).AsSingle().NonLazy();
        Container.Bind<Box>().To<Box>().AsTransient();
    }

    private void InstallEndpoints()
    {
    }

    private void InstallHelpers()
    {
        Container.Bind<IFileManager>().To<FileManager>().AsSingle().NonLazy();
        Container.Bind<IJsonSerializer>().To<JsonSerializer>().AsSingle().NonLazy();

        Container.Bind<SignalLogger>().AsSingle().NonLazy();
    }

    private void InstallInputs()
    {
        Container.BindInterfacesAndSelfTo<TouchHandler>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<MouseLeftButtonHandler>().AsSingle().NonLazy();
    }

    private void InstallControllers()
    {
        Container.BindInterfacesAndSelfTo<LevelDataController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<LevelEndController>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<CameraController>().AsSingle().NonLazy();

        Container.Bind<IBoardController>().To<BoardController>().AsSingle().NonLazy();
        Container.Bind<IUserDataController>().To<UserDataController>().AsSingle().NonLazy();
        Container.Bind<ISwitcher>().To<Switcher>().AsSingle().NonLazy();

        Container.Bind<GameContext>().AsSingle().NonLazy();
    }

    private void DeclareSignals()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<BoardCreatedSignal>().OptionalSubscriber();

        Container.DeclareSignal<SafeBoxOpenedSignal>().OptionalSubscriber();
        Container.DeclareSignal<SafeBoxFlaggedSignal>().OptionalSubscriber();
        Container.DeclareSignal<SafeBoxUnFlaggedSignal>().OptionalSubscriber();

        Container.DeclareSignal<BombBoxOpenedSignal>().OptionalSubscriber();
        Container.DeclareSignal<BombBoxFlaggedSignal>().OptionalSubscriber();
        Container.DeclareSignal<BombBoxUnFlaggedSignal>().OptionalSubscriber();

        Container.DeclareSignal<VictorySignal>().OptionalSubscriber();
        Container.DeclareSignal<DefeatSignal>().OptionalSubscriber();
    }
}