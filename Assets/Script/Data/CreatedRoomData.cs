using System;

public class CreatedRoomData {

	public string CreatedRoomName { get; private set; }
    public CreatedRoomData(string roomName)
    {
        CreatedRoomName = roomName;
    }

}
