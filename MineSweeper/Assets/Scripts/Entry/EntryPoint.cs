using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

class EntryPoint : IInitializable
{
    private readonly IAssetManager _assetManager;
    private readonly IBoardController _boardController;
    private readonly ILevelDataLoader _levelDataLoader;
    private readonly IUserDataController _userDataController;

    public EntryPoint(
        IAssetManager assetManager,
        IBoardController boardController,
        ILevelDataLoader levelDataLoader,
        IUserDataController userDataController)
    {
        _assetManager = assetManager;
        _boardController = boardController;
        _levelDataLoader = levelDataLoader;
        _userDataController = userDataController;
    }

    public void Initialize()
    {
        //Debug.Log($"screen W: {Screen.width}");
        //Debug.Log($"screen H: {Screen.height}");
        //Debug.Log($"safe x: {Screen.safeArea.x}");
        //Debug.Log($"safe y: {Screen.safeArea.y}");
        //Debug.Log($"safe W: {Screen.safeArea.width}");
        //Debug.Log($"safe H: {Screen.safeArea.height}");
        //Debug.Log($"safe yMax: {Screen.safeArea.yMax}");
        //Debug.Log($"safe yMin: {Screen.safeArea.yMin}");
        //Debug.Log($"safe xMax: {Screen.safeArea.xMax}");
        //Debug.Log($"safe xMin: {Screen.safeArea.xMin}");
        InitializeAsync().Forget();
    }

    public async UniTaskVoid InitializeAsync()
    {
        _assetManager.LoadAssets();

        await _userDataController.LoadUserAsync();

        await _levelDataLoader.LoadCurrentLevelAsync(_userDataController.GetPreviousLevel());

        _boardController.CreateBoard();
    }
}
