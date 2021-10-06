using System;
namespace Amolenk.GameATron4000.Engine.Scenes
{
    public interface ISceneHost
    {
        ValueTask RegisterSceneAsync<TScene>(TScene scene) where TScene : Scene;

        ValueTask StartAsync();

        ValueTask SwitchToSceneAsync(string key);
    }
}

