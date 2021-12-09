﻿using System;
using System.Collections.Generic;
using Autofac;
using TagsCloud.Visualization.ContainerVisitor;
using TagsCloud.Visualization.Drawers;
using TagsCloud.Visualization.FontFactory;
using TagsCloud.Visualization.ImagesSavior;
using TagsCloud.Visualization.LayoutContainer.ContainerBuilder;
using TagsCloud.Visualization.LayouterCores;
using TagsCloud.Visualization.PointGenerator;
using TagsCloud.Visualization.WordsFilter;
using TagsCloud.Visualization.WordsParser;
using TagsCloud.Visualization.WordsReaders;
using TagsCloud.Visualization.WordsReaders.FileReaders;
using TagsCloud.Visualization.WordsSizeService;

namespace TagsCloud.Visualization
{
    public class TagsCloudModule : Module
    {
        private readonly TagsCloudModuleSettings settings;

        public TagsCloudModule(TagsCloudModuleSettings settings)
            => this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TxtFileReader>().As<IFileReader>();
            builder.RegisterType<DocFileReader>().As<IFileReader>();
            builder.RegisterType<PdfFileReader>().As<IFileReader>();
            builder.Register(ctx => new FileReadService(settings.InputWordsFile,
                    ctx.Resolve<IEnumerable<IFileReader>>()))
                .As<IWordsReadService>();

            builder.RegisterType<WordsService>().As<IWordsService>();

            RegisterBoringWordsFilter(builder);
            builder.RegisterType<WordsParser.WordsParser>().As<IWordsParser>();

            builder.Register(_ => new FontFactory.FontFactory(settings.FontSettings)).As<IFontFactory>();
            builder.RegisterType<WordsSizeService.WordsSizeService>().As<IWordsSizeService>();
            builder.RegisterType<WordsContainerBuilder>().As<AbstractWordsContainerBuilder>();

            builder.Register(_ => new ArchimedesSpiralPointGenerator(settings.Center)).As<IPointGenerator>();
            builder.RegisterType(settings.LayouterType).As<ICloudLayouter>();

            builder.Register(_ => settings.LayoutVisitor).As<IContainerVisitor>();
            builder.RegisterType<Drawer>().As<IDrawer>();
            builder.Register(_ => new ImageSavior(settings.SaveSettings)).As<IImageSavior>();

            builder.RegisterType<LayouterCore>().As<ILayouterCore>();
        }

        private void RegisterBoringWordsFilter(ContainerBuilder builder)
        {
            if (settings.BoringWordsFile == null)
                builder.Register(_ => new BoringWordsFilter()).As<IWordsFilter>();
            else
                builder.Register(ctx => new BoringWordsFilter(new FileReadService(settings.BoringWordsFile,
                ctx.Resolve<IEnumerable<IFileReader>>()))).As<IWordsFilter>();
        }
    }
}