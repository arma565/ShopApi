using System.ComponentModel.DataAnnotations;

public class Gallery
{
    int id = 0;
    string img = "";

    public int Id { get => id; set => id = value; }
    
    [Required]
    public string Img
    {
        get => img;
        set => img = value;
    }

}
