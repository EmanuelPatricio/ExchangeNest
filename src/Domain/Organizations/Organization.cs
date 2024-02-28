using Domain.Abstractions;
using Domain.Organizations.Events;
using Domain.Organizations.ValueObjects;
using Domain.Shared;
using static Domain.Shared.Enums;

namespace Domain.Organizations;
public sealed class Organization : Entity<OrganizationId>
{
    private Organization() { }

    private Organization(
        OrganizationId id,
        OrganizationName name,
        OrganizationDescription description,
        Email email,
        OrganizationPhoneNumber phoneNumber,
        OrganizationAddress address,
        OrganizationImageUrl? imageUrl,
        int organizationTypeId,
        int statusId,
        DateTime createdOn,
        DateTime? lastModifiedOn)
        : base(id)
    {
        Name = name;
        Description = description;
        Email = email;
        PhoneNumber = phoneNumber;
        Address = address;
        ImageUrl = imageUrl;
        OrganizationTypeId = organizationTypeId;
        StatusId = statusId;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    public OrganizationName Name { get; private set; }
    public OrganizationDescription Description { get; private set; }
    public Email Email { get; private set; }
    public OrganizationPhoneNumber PhoneNumber { get; private set; }
    public OrganizationAddress Address { get; private set; }
    public OrganizationImageUrl? ImageUrl { get; private set; }
    public int OrganizationTypeId { get; private set; }
    public int StatusId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? LastModifiedOn { get; private set; }

    public static Organization Create(
        OrganizationId id,
        OrganizationName name,
        OrganizationDescription description,
        Email email,
        OrganizationPhoneNumber phoneNumber,
        OrganizationAddress address,
        OrganizationImageUrl? imageUrl,
        int organizationTypeId,
        int statusId)
    {
        var organization = new Organization(id, name, description, email, phoneNumber, address, imageUrl, organizationTypeId, statusId, DateTime.Now, null);

        organization.RaiseDomainEvent(new OrganizationCreatedDomainEvent(organization.Id));

        return organization;
    }

    public static void Update(
        Organization organization,
        OrganizationName name,
        OrganizationDescription description,
        Email email,
        OrganizationPhoneNumber phoneNumber,
        OrganizationAddress address,
        OrganizationImageUrl? imageUrl,
        int organizationTypeId,
        int statusId)
    {
        organization.Name = name;
        organization.Description = description;
        organization.Email = email;
        organization.PhoneNumber = phoneNumber;
        organization.Address = address;
        organization.LastModifiedOn = DateTime.Now;
        organization.StatusId = statusId;
        organization.OrganizationTypeId = organizationTypeId;
        organization.ImageUrl = imageUrl;

        organization.RaiseDomainEvent(new OrganizationUpdatedDomainEvent(organization.Id));
    }

    public static void Deactivate(Organization organization)
    {
        organization.StatusId = (int)Statuses.Deleted;
        organization.LastModifiedOn = DateTime.Now;

        organization.RaiseDomainEvent(new OrganizationDeletedDomainEvent(organization.Id));
    }
}
