using System;

[Serializable]
public class RoomData {

	public string RoomName { get; private set; }
    public bool IsMasterClient { get; private set; }
    public RoomData() { }

}
