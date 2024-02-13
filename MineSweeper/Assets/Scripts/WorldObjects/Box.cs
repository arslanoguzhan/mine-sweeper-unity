using DG.Tweening;
using UnityEngine;
using Zenject;

class Box : MonoBehaviour
{
    private SignalBus _signalBus;

    private IAssetManager _assetManager;

    private ISwitcher _switcher;

    private IBoardController _boardController;

    private SpriteRenderer _boxSpriteRenderer;

    private SpriteRenderer _btnSpriteRenderer;

    public GameObject ButtonObject {get; private set;}

    public int Row {get; private set;}
    public int Col {get; private set;}

    public BoxContent Content {get; private set;} = BoxContent.Zero;
    public BoxState State {get; private set;} = BoxState.Button;

    [Inject]
    public void Inject(
        SignalBus signalBus,
        IAssetManager assetManager,
        ISwitcher switcher,
        IBoardController boardController)
    {
        _signalBus ??= signalBus;
        _switcher ??= switcher;
        _boardController ??= boardController;
        _assetManager ??= assetManager;
    }

    public void Setup(int row, int col)
    {
        Content = BoxContent.Zero;
        State = BoxState.Button;
        Row = row;
        Col = col;

        ButtonObject = transform.GetChild(0).gameObject;
        ButtonObject.transform.localScale = new Vector3(0,0);

        _boxSpriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _btnSpriteRenderer = ButtonObject.GetComponent<SpriteRenderer>();
        _btnSpriteRenderer.sprite = _assetManager.GetSprite(SpriteIndex.Button);
        _boxSpriteRenderer.sprite = _assetManager.GetSprite(SpriteIndex.Zero);
    }

    public void UpdateContent(BoxContent newType)
    {
        Content = newType;
        _boxSpriteRenderer.sprite = _assetManager.GetSprite((int)Content);
    }

    public void UpdateButtonSprite()
    {
        var spriteIndex = SpriteIndex.Button.ToInt() + (int)State;
        _btnSpriteRenderer.sprite = _assetManager.GetSprite(spriteIndex);
    }

    public void UpdateState(BoxState newState)
    {
        State = newState;
    }

    public void OnOpen()
    {
        _switcher.DisableWorld();

        UpdateState(BoxState.Open);

        ButtonObject.transform.DOScale(new Vector3(0, 0), Constants.btnHideAnimDuration).OnComplete(() => 
        {
            if(Content == BoxContent.Bomb)
            {
                _signalBus.Fire(new BombBoxOpenedSignal());
            }
            else
            {
                _signalBus.Fire(new SafeBoxOpenedSignal());

                if(Content == BoxContent.Zero)
                {
                    var nearBoxes = _boardController.GetNearBoxes(Row, Col);

                    foreach(var nbox in nearBoxes)
                    {
                        nbox?.OnPoke();
                    }
                }
            }

            _switcher.EnableWorld();
        });
    }

    public void OnPoke()
    {
        if(State == BoxState.Open)
            return;

        if(Content == BoxContent.Bomb)
            return;

        _switcher.DisableWorld();

        UpdateState(BoxState.Open);

        ButtonObject
        .transform
        .DOScale(new Vector3(0, 0), Constants.btnHideAnimDuration)
        .OnComplete(() => 
        {
            _signalBus.Fire(new SafeBoxOpenedSignal());

            if(Content == BoxContent.Zero)
            {
                var nearBoxes = _boardController.GetNearBoxes(Row, Col);

                foreach(var nb in nearBoxes)
                {
                    nb?.OnPoke();
                }
            }

            _switcher.EnableWorld();
        });
    }

    public void OnFlag()
    {
        if(State != BoxState.Button)
            return;

        if(Content == BoxContent.Bomb)
        {
            _signalBus.Fire(new BombBoxFlaggedSignal());
        }
        else
        {
            _signalBus.Fire(new SafeBoxFlaggedSignal());
        }

        UpdateState(BoxState.Flag);

        UpdateButtonSprite();
    }

    public void OnUnFlag()
    {
        if(State != BoxState.Flag)
            return;

        if(Content == BoxContent.Bomb)
        {
            _signalBus.Fire(new BombBoxUnFlaggedSignal());
        }
        else
        {
            _signalBus.Fire(new SafeBoxUnFlaggedSignal());
        }

        UpdateState(BoxState.Button);

        UpdateButtonSprite();
    }
}
