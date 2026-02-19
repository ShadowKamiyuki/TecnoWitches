using System.Threading.Tasks;

public interface ISceneLoader
{
    bool IsLoading { get; }
    float Progress { get; }

    Task ProcessRequest(LoadingRequest request);
}
