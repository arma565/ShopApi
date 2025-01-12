using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

public class NewProduct
{
    private int id = 0;
    private string title = "";
    private string icon = "";
    private string link = "";

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
    [SwaggerSchema(Description = "New product icon", ReadOnly = true)]
    public string Icon
    {
        get => icon;
        set => icon = value;
    }
    public string Link
    {
        get => link;
        set => link = value;
    }
}
