namespace Lemon.ShadowFiend.Models;

public struct WindowsIdentityModel
{
    public WindowsIdentityModel(string userName, string password, string machineName)
    {
        UserName = userName;
        Password = password;
        MachineName = machineName;
    }

    public string UserName
    {
        get;
    }

    public string Password
    {
        get;
    }

    public string MachineName
    {
        get;
    }

    public override bool Equals(object? obj)
    {
        if (obj is WindowsIdentityModel other)
        {
            return UserName.Equals(other.UserName) 
                   && Password.Equals(other.Password)
                   && MachineName.Equals(other.MachineName);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return (UserName, Password).GetHashCode();
    }
}