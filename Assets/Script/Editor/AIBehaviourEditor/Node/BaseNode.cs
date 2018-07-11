using UnityEngine;

public abstract class BaseNode {

    public Vector3 EnterPoint { get { return new Vector3(windowRect.x, windowRect.y + windowRect.height / 2f); } }
    public Vector3 OutPoint { get { return new Vector3(windowRect.x + windowRect.width, windowRect.y + windowRect.height / 2f); } }

    public abstract string Title { get; set; }
    public Rect windowRect = default(Rect);
    public abstract void DrawWindow();

}
