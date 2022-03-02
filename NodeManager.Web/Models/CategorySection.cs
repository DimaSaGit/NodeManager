﻿using System;
using System.Collections.Generic;
using NodeManager.Domain;
namespace NodeManager.Web.Models
{
    public class CategorySection
    {
        public CategorySection()
        {
            Menu = new Dictionary<Sections, IEnumerable<Categories>>();
        }
        public Dictionary<Sections, IEnumerable<Categories>> Menu { get; set; }

        public Nullable<int> SelectedSection { get; set; }
    }
}