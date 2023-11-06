using System;
using System.Collections.Generic;
using hr_developing.Models;

namespace hr_developing;

public partial class Company
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Owner { get; set; } = null!;

    public string FkClientId { get; set; } = null!;

    public virtual RegClientViewModel FkClient { get; set; } = null!;

    public virtual ICollection<Profession> Professions { get; set; } = new List<Profession>();
}
