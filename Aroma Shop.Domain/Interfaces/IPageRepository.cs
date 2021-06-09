﻿using System;
using System.Collections.Generic;
using System.Text;
using Aroma_Shop.Domain.Models.PageModels;

namespace Aroma_Shop.Domain.Interfaces
{
    public interface IPageRepository : IGeneralRepository
    {
        void AddPage(Page page);
    }
}