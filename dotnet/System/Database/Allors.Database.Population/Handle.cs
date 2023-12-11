namespace Allors.Database.Population;

using Meta;

public record Handle(
    IRoleType RoleType, 
    string Name, 
    object Value);
