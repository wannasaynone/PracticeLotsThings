using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManage : Manager {

    private static List<Actor> m_actors = new List<Actor>();

    public ActorManage()
    {
        List<View> _views = GetViews<Actor>();
        for(int i = 0; i < _views.Count; i++)
        {
            m_actors.Add(((Actor)_views[i]));
        }
    }

	public static Actor GetActor(int id)
    {
        return m_actors.Find(x => x.ID == id);
    }

}
