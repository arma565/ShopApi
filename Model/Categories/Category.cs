public class Category
{
    private int id = 0;
    private string title = "";
    private string description = "";
    private string icon = "";

    public int Id
    {
        get => id;
        set => id = value;
    }
    public string Title
    {
        get => title;
        set => title = value;
    }
    public string Description
    {
        get => description;
        set => description = value;
    }
    public string Icon
    {
        get => icon;
        set => icon = value;
    }
}
