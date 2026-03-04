using System.Threading.Tasks;

public interface ITransitionService
{
    Task FadeIn();
    Task FadeOut();
}