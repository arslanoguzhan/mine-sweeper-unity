using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Zenject;

class BoardController : IBoardController
{
    private readonly DiContainer _diContainer;
    private readonly SignalBus _signalBus;
    private readonly IViewport _viewport;
    private readonly IAssetManager _assetManager;
    private readonly IWorldRoot _worldRoot;
    private readonly ISwitcher _switcher;
    private readonly ILevelDataController _levelDataController;

    private readonly (int, int)[] _nearCoords = new (int, int)[]
    {
        (-1, -1),
        (-1,  0),
        (-1, +1),
        ( 0, -1),
        ( 0, +1),
        (+1, -1),
        (+1,  0),
        (+1, +1)
    };

    private Box[,] _boxTable;
    private Level _level;

    public BoardController(
        DiContainer diContainer,
        SignalBus signalBus,
        IViewport viewport,
        IAssetManager assetManager,
        IWorldRoot worldRoot,
        ISwitcher switcher,
        ILevelDataController levelDataController)
    {
        _diContainer = diContainer;
        _signalBus = signalBus;
        _viewport = viewport;
        _assetManager = assetManager;
        _worldRoot = worldRoot;
        _switcher = switcher;
        _levelDataController = levelDataController;
    }

    public void CreateBoard()
    {
        _level = _levelDataController.GetCurrentLevel();

        var posX = (_viewport.Width - (_level.ColCount * Constants.boxVolume))/2;
        var posY = Constants.boardPositionY;
        var posXOffset = Constants.boxVolume/2;
        var posYOffset = Constants.boxVolume/2;

        posX += posXOffset;
        posY -= posYOffset;

        var board = new GameObject("board");
        _worldRoot.AddObject(board);
        board.transform.localPosition = new Vector3(posX, posY);

        _boxTable = new Box[_level.RowCount,_level.ColCount];

        _switcher.DisableUi();
        _switcher.DisableWorld();

        Sequence sequence = DOTween.Sequence();

        for(var r = 0; r < _level.RowCount; r++)
        {
            for(var c = 0; c < _level.ColCount; c++)
            {
                var box = _boxTable[r,c] = CreateBox(board, r, c);

                sequence.Join(box.ButtonObject.transform.DOScale(new Vector3(1, 1), Constants.btnCreateAnimDuration));
            }
        }

        var remainingBombCount = _level.BombCount;

        while(remainingBombCount > 0)
        {
            var r = UnityEngine.Random.Range(0,_level.RowCount);
            var c = UnityEngine.Random.Range(0,_level.ColCount);

            if ((r == 0 && c == 0)
            ||  (r == _level.RowCount-1 && c == 0)
            ||  (r == 0 && c == _level.ColCount-1)
            ||  (r == _level.RowCount-1 && c == _level.ColCount-1))
                continue;

            if(_boxTable[r, c].Content == BoxContent.Bomb)
                continue;

            _boxTable[r, c].UpdateContent(BoxContent.Bomb);
            Increment(r-1, c-1);
            Increment(r-1, c);
            Increment(r-1, c+1);
            Increment(r, c-1);
            Increment(r, c+1);
            Increment(r+1, c-1);
            Increment(r+1, c);
            Increment(r+1, c+1);

            remainingBombCount -= 1;
        }

        sequence.OnComplete(() => 
        {
            _switcher.EnableUi();
            _switcher.EnableWorld();
        });

        sequence.Play();

        _signalBus.Fire(new BoardCreatedSignal(_level));
    }

    private void Increment(int r, int c)
    {
        if(r < 0 || c < 0 || r == _level.RowCount || c == _level.ColCount || _boxTable[r,c].Content == BoxContent.Bomb)
            return;

        var box = _boxTable[r,c];
        box.UpdateContent((BoxContent)((int)box.Content + 1));
    }

    private Box CreateBox(GameObject grid, int r, int c)
    {
        var boxGameObject = new GameObject($"box-{r.ToString().PadLeft(2, '0')}-{c.ToString().PadLeft(2, '0')}");
        boxGameObject.transform.parent = grid.transform;
        boxGameObject.transform.localPosition = new Vector3(c * Constants.boxVolume, r * -Constants.boxVolume);

        var btnGameObject = new GameObject($"button-{r.ToString().PadLeft(2, '0')}-{c.ToString().PadLeft(2, '0')}");
        btnGameObject.transform.parent = boxGameObject.transform;
        btnGameObject.transform.localPosition = new Vector3(0, 0);
        btnGameObject.transform.localScale = new Vector3(0, 0, 1);

        var btnSpriteRenderer = btnGameObject.AddComponent<SpriteRenderer>();
        btnSpriteRenderer.sprite = _assetManager.GetSprite(SpriteIndex.Button);
        btnSpriteRenderer.sortingOrder = 1;

        var boxComponent = _diContainer.InstantiateComponent<Box>(boxGameObject); 
        var boxListener = boxGameObject.AddComponent<BoxListener>();
        var boxSpriteRenderer = boxGameObject.AddComponent<SpriteRenderer>();
        var boxCollider = boxGameObject.AddComponent<BoxCollider2D>();
        //boxCollider.offset = new Vector2(Constants.boxSize/2, -Constants.boxSize/2);
        boxCollider.offset = new Vector2(0, 0);
        boxCollider.size = new Vector2(Constants.boxSize, Constants.boxSize);

        boxListener.Setup(boxComponent);
        boxComponent.Setup(r, c);

        return boxComponent;
    }

    public List<Box> GetNearBoxes(int r, int c)
    {
        var boxList = new List<Box>();

        foreach(var coord in _nearCoords)
        {
            var newr = r+coord.Item1;
            var newc = c+coord.Item2;
            
            if(!(newr < 0 || newc < 0 || newr == _level.RowCount || newc == _level.ColCount))
            {
                var box = _boxTable[newr, newc];
                if(box.State != BoxState.Open && box.Content != BoxContent.Bomb)
                {
                    boxList.Add(_boxTable[newr, newc]);
                }
            }
        }

        return boxList;
    }
}