namespace Allors.Database.Population;

using Meta;

public record Handle(
    RoleType RoleType, 
    string Name, 
    object Value);
