﻿using System;
using System.Threading.Tasks;
using Silent.Tetris.Contracts;
using Silent.Tetris.Contracts.Core;
using Silent.Tetris.Contracts.Rendering;
using Silent.Tetris.Core.Engine;
using Silent.Tetris.Renderers;
using Silent.Tetris.Views;

namespace Silent.Tetris
{
    internal class Tetris
    {
        private static void Main()
        {
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedException;
            Console.Title = "Tetris";

            IDependencyResolver gameServiceLocator = BuildServiceLocator();
            IObserveAsync<ICommand> commandObserver = gameServiceLocator.Resolve<IObserveAsync<ICommand>>();
            IDisposable commandObserverDisposable = commandObserver.ObserveAsync();
            INavigationService navigationService = gameServiceLocator.Resolve<INavigationService>();
            navigationService.Navigate(new HomeView(gameServiceLocator));

            using (commandObserverDisposable)
            {
                while (navigationService.CurrentView != null)
                {
                    navigationService.CurrentView.Render();
                    Task.Delay(50).Wait();
                }
            }
        }

        private static IDependencyResolver BuildServiceLocator()
        {
            IDependencyResolver gameContainer = new DependencyResolver();
            gameContainer.Register<INavigationService>(new NavigationService());
            gameContainer.Register<ISpriteRenderer>(new SpriteRenderer());
            gameContainer.Register<IFactory<IFigure>>(new FigureFactory());
            gameContainer.Register<IRandomGenerator<IFigure>>(new FigureRandomGenerator(gameContainer.Resolve<IFactory<IFigure>>()));
            gameContainer.Register<IGameEngine>(new GameEngine(gameContainer.Resolve<IRandomGenerator<IFigure>>()));
            gameContainer.Register<IRepository<ScoreRecord>>(new ScoreRecordRepository("highscores.json"));
            gameContainer.Register<IObserveAsync<ICommand>>(new ConsoleCommandsObserveAsync());
            return gameContainer;
        }

        private static void TaskSchedulerOnUnobservedException(object sender, UnobservedTaskExceptionEventArgs eventArgs)
        {
            eventArgs.SetObserved();
            Console.WriteLine(eventArgs.Exception.Flatten());
        }
    }
}
