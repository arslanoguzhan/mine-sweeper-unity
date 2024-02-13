using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

class LevelDataController : ILevelDataController, ILevelDataLoader
{
    private readonly IFileManager _fileManager;

    private readonly IJsonSerializer _jsonSerializer;

    private string LevelFileName(int level) => $"level_{level}.json";

    private const int LEVEL_LOAD_BATCH_SIZE = 1;

    private Level _level;

    public LevelDataController(IFileManager fileManager, IJsonSerializer jsonSerializer)
    {
        _fileManager = fileManager;
        _jsonSerializer = jsonSerializer;
    }

    public Level GetCurrentLevel()
    {
        return _level;
    }

    async Task ILevelDataLoader.LoadCurrentLevelAsync(Level previousLevel)
    {
        await Task.Yield();

        if(previousLevel.Number == 0)
        {
            _level = new Level()
            {
                Number = 1,
                RowCount = 4,
                ColCount = 4,
                BombCount = 1
            };
        }
        else if ((previousLevel.BombCount+1) * 4 < previousLevel.RowCount * previousLevel.ColCount)
        {
            _level = new Level()
            {
                Number = previousLevel.Number+1,
                RowCount = previousLevel.RowCount,
                ColCount = previousLevel.ColCount,
                BombCount = previousLevel.BombCount+1
            };
        }
        else
        {
            _level = new Level()
            {
                Number = previousLevel.Number+1
            };

            if(previousLevel.RowCount == previousLevel.ColCount)
            {
                _level.RowCount = previousLevel.RowCount+1;
                _level.ColCount = previousLevel.ColCount;
            }
            else if(!(previousLevel.RowCount == Constants.rowCountMax && previousLevel.ColCount == Constants.colCountMax))
            {
                _level.RowCount = previousLevel.RowCount;
                _level.ColCount = previousLevel.ColCount+1;
            }

            _level.BombCount = (_level.RowCount * _level.ColCount) / 6;
        }

        PersistLevelAsync(_level).Forget();
    }

    private async UniTask PersistLevelAsync(Level level)
    {
        await Task.Yield();

        var levelFileName = LevelFileName(level.Number);
        var levelJson = _jsonSerializer.Serialize(level);

        await _fileManager.FileWriteAsync(levelFileName, levelJson);
    }
}
