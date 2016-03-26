namespace Silent.Tetris.Contracts.Views
{
    public interface IView
    {
        INavigationService NavigationService { get; }

        void Initialize(INavigationService navigationService);

        void Render();
    }

    public interface IView<out TPresenter> : IView
    {
        TPresenter Presenter { get; }
    }
}