﻿using System;
using System.Collections.Generic;
using hr_developing.Models;

namespace hr_developing;

public partial class AddResumeModel
{
    public string Id { get; set; } = null!;

    public string Profession { get; set; } = null!;

    public double Salary { get; set; }

    public string Keyskills { get; set; } = null!;

    public string FkClientId { get; set; } = null!;

    public virtual Client FkClient { get; set; } = null!;
}
