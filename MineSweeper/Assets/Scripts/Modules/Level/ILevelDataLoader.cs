using System.Threading.Tasks;

interface ILevelDataLoader
{
    Task LoadCurrentLevelAsync(Level previousLevel);
}