public class Config
{
    public bool LogAvatars { get; set; } = true;
    public bool LogOwnAvatars { get; set; } = true;
    public bool LogFriendsAvatars { get; set; } = true;
    public bool LogToConsole { get; set; } = true;
    public bool SendToAPI { get; set; } = false;
    public string Username { get; set; } = "Default";
}