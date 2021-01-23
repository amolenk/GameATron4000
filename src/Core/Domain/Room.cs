using GameATron4000.Core.Services;

namespace GameATron4000.Core.Domain
{
    public class Room : IScene
    {
        public Room()
        {
        }

        public void Create()
        {
            throw new System.NotImplementedException();
        }

        public void Update()
        {
            throw new System.NotImplementedException();
        }

        public static Room Enter()
        {
            return new Room();
        }
    }
}