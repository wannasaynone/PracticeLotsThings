
public abstract class SubManager {

    public bool IsEnable = true;
    private Page m_page;

    public SubManager(Page page)
    {
        m_page = page;
    }

    public abstract void Update();
}
