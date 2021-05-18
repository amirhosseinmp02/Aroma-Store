﻿using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace Aroma_Shop.Domain.Models.CustomIdentityModels
{
    public class CustomIdentityUser : IdentityUser
    {

        //Navigations Properties

        public UserDetail UserDetail { get; set; }
    }
}
