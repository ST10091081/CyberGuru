using System;
using System.ComponentModel.DataAnnotations;

namespace CyberGamify.Models
{
public class GameProgress
{
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }
    public int Points { get; set; }
    public string Badge { get; set; }
    public DateTime LastPlayed { get; set; }
}
}