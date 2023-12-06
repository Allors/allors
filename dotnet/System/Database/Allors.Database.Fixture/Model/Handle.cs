namespace Allors.Database.Fixture;

using Meta;

public record Handle(
    IRoleType RoleType, 
    string Name, 
    object Value);
