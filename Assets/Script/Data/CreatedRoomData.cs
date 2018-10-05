using System;

namespace PracticeLotsThings.Data
{
    [Serializable]
    public class CreatedRoomData
    {

        public string CreatedRoomName { get; private set; }
        public CreatedRoomData(string roomName)
        {
            CreatedRoomName = roomName;
        }
    }
}

