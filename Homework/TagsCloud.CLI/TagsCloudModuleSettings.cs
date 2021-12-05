﻿using System;
using System.Drawing;
using TagsCloud.Visualization.ContainerVisitor;
using TagsCloud.Visualization.FontFactory;
using TagsCloud.Visualization.ImagesSaver;

namespace TagsCloud.Words
{
    public class TagsCloudModuleSettings
    {
        public Point Center { get; set; }
        public string InputWordsFile { get; set; }
        public Type LayouterType { get; set; }
        public IContainerVisitor LayoutVisitor { get; set; }
        public SaveSettings SaveSettings { get; set; }
        public FontSettings FontSettings { get; set; } = new();
    }
}