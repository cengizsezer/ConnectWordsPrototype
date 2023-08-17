using System.Threading.Tasks;
using MyProject.Core.Manager;

public class GeneratorController
{
    public BoardGenerator BoardGenerator = null;
    public WordTableGenerator WordGenerator = null;
    public GeneratorController() : this(null, null, null, null)
    {

    }

    public GeneratorController(BoardGeneratorSceneReferences boardReferences, BoardGeneratorSettings boardSettings
        , WordTableGeneratorReferences wordTableReferences, WordTableGeneratorSettings wordTableSettings)
    {
       
        BoardGenerator = new BoardGenerator(boardSettings, boardReferences);
        WordGenerator = new WordTableGenerator(wordTableSettings, wordTableReferences);
        BoardGenerator.Controller = this;
        WordGenerator.Controller = this;
    }


    public async Task Initialized()
    {
        
        await PoolHandler.I.IsLoading.Task;
        await GenerateChildsAsync();
        OnInit();
        await Task.Yield();
    }

    private async Task GenerateChildsAsync()
    {
        
        await WordGenerator.CreateBoardAsync();
        await BoardGenerator.CreateBoardAsync();
    }

    private async void OnInit()
    {
        GameManager.I.isRunning = true;
        await Task.Yield();
    }

}
