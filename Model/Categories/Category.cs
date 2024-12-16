using System.ComponentModel.DataAnnotations;

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
    [Required]
    public string Title
    {
        get => title;
        set => title = value;
    }
    [Required]
    public string Description
    {
        get => description;
        set => description = value;
    }
    [Required]
    public string Icon
    {
        get => icon;
        set => icon = value;
    }
}
