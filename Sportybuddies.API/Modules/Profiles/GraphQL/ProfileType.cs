#nullable enable
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Authorization;
using HotChocolate.Data;
using Sportybuddies.API.Modules.Profiles.ValueObjects;

namespace Sportybuddies.API.Modules.Profiles.GraphQL;

/// <summary>
/// GraphQL type for Profile entity
/// </summary>
public class ProfileType : ObjectType<ProfileDto>
{
    protected override void Configure(IObjectTypeDescriptor<ProfileDto> descriptor)
    {
        descriptor.Name("Profile");
        descriptor.Description("Represents a user profile with personal information and sports preferences");
        
        descriptor.Field(p => p.Id)
            .Type<NonNullType<UuidType>>()
            .Description("The unique identifier of the profile");
            
        descriptor.Field(p => p.Name)
            .Type<NonNullType<StringType>>()
            .Description("The user's display name");
            
        descriptor.Field(p => p.Description)
            .Type<StringType>()
            .Description("The user's profile description");
            
        descriptor.Field(p => p.MainPhotoUrl)
            .Type<StringType>()
            .Description("URL to the user's main profile photo");
            
        descriptor.Field(p => p.CreatedOn)
            .Type<NonNullType<DateTimeType>>()
            .Description("When the profile was created");
            
        descriptor.Field(p => p.Gender)
            .Type<NonNullType<GenderType>>()
            .Description("The user's gender");
            
        descriptor.Field(p => p.DateOfBirth)
            .Type<NonNullType<DateTimeType>>()
            .Description("The user's date of birth");
            
        descriptor.Field(p => p.Preferences)
            .Type<PreferencesType>()
            .Description("User's preferences for matching");
            
        descriptor.Field(p => p.Location)
            .Type<LocationType>()
            .Description("User's location information");
            
        descriptor.Field(p => p.Sports)
            .Type<NonNullType<ListType<NonNullType<SportType>>>>()
            .Description("Sports that the user is interested in");
    }
}

/// <summary>
/// GraphQL type for Gender enum
/// </summary>
public class GenderType : EnumType<Gender>
{
    protected override void Configure(IEnumTypeDescriptor<Gender> descriptor)
    {
        descriptor.Name("Gender");
        descriptor.Description("Gender options");
    }
}

/// <summary>
/// GraphQL type for Preferences value object
/// </summary>
public class PreferencesType : ObjectType<Preferences>
{
    protected override void Configure(IObjectTypeDescriptor<Preferences> descriptor)
    {
        descriptor.Name("Preferences");
        descriptor.Description("User preferences for matching and discovery");
    }
}

/// <summary>
/// GraphQL type for Location value object
/// </summary>
public class LocationType : ObjectType<ValueObjects.Location>
{
    protected override void Configure(IObjectTypeDescriptor<ValueObjects.Location> descriptor)
    {
        descriptor.Name("Location");
        descriptor.Description("User's geographic location");
    }
}

/// <summary>
/// GraphQL type for Sport entity
/// </summary>
public class SportType : ObjectType<SportDto>
{
    protected override void Configure(IObjectTypeDescriptor<SportDto> descriptor)
    {
        descriptor.Name("Sport");
        descriptor.Description("A sport that users can be interested in");
        
        descriptor.Field(s => s.Id)
            .Type<NonNullType<UuidType>>()
            .Description("The unique identifier of the sport");
            
        descriptor.Field(s => s.Name)
            .Type<NonNullType<StringType>>()
            .Description("The name of the sport");
            
        descriptor.Field(s => s.Description)
            .Type<StringType>()
            .Description("A description of the sport");
    }
}