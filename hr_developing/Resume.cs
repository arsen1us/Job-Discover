using System;
using System.Collections.Generic;
using hr_developing.Models;

namespace hr_developing;

public partial class Resume
{
    public string Id { get; set; } = null!;

    public string Profession { get; set; } = null!;

    public double Salary { get; set; }

    public string Keyskills { get; set; } = null!;

    public string FkClientId { get; set; } = null!;

    public virtual AuthClientModel FkClient { get; set; } = null!;
}
