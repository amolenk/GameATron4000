using System;
using System.Threading.Tasks;
using GameATron4000.Core.Messages;

namespace GameATron4000.Core.Services
{
    public delegate void ChangeRoomCallback(string roomId);
    public delegate void PutItemCallback(IItemState item, int x, int y, string roomId, bool addCameraOffset);

    /// <summary>
    /// Classes that implement this method can run custom game script logic.
    /// </summary>
    public interface IGameScriptInterpreter
    {
        IWorldState World { get; }

        event ChangeRoomCallback ChangeRoomCallback;

        event PutItemCallback PutItemCallback;

        void StartGame();

        void BeforeEnterRoom(string roomId);

        void AfterEnterRoom(string roomId);

        void Close();

        void Give();

        void LookAt();

        void Open();

        void PickUp();

        void Pull();

        void Push();

        void TalkTo();

        void Use();
    }
}
