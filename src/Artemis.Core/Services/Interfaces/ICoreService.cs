﻿using System;
using System.Collections.Generic;

namespace Artemis.Core.Services
{
    /// <summary>
    ///     A service that initializes the Core and manages the render loop
    /// </summary>
    public interface ICoreService : IArtemisService, IDisposable
    {
        /// <summary>
        ///     Gets whether the or not the core has been initialized
        /// </summary>
        bool IsInitialized { get; }

        /// <summary>
        ///     The time the last frame took to render
        /// </summary>
        TimeSpan FrameTime { get; }

        /// <summary>
        ///     Gets or sets whether modules are rendered each frame by calling their Render method
        /// </summary>
        bool ModuleRenderingDisabled { get; set; }

        /// <summary>
        ///     Gets or sets a list of startup arguments
        /// </summary>
        List<string> StartupArguments { get; set; }

        /// <summary>
        ///     Gets a boolean indicating whether Artemis is running in an elevated environment (admin permissions)
        /// </summary>
        bool IsElevated { get; set; }

        /// <summary>
        ///     Initializes the core, only call once
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Plays the into animation profile defined in <c>Resources/intro-profile.json</c>
        /// </summary>
        void PlayIntroAnimation();

        /// <summary>
        ///     Occurs the core has finished initializing
        /// </summary>
        event EventHandler Initialized;

        /// <summary>
        ///     Occurs whenever a frame is rendering, after modules have rendered
        /// </summary>
        event EventHandler<FrameRenderingEventArgs> FrameRendering;

        /// <summary>
        ///     Occurs whenever a frame is finished rendering and the render pipeline is closed
        /// </summary>
        event EventHandler<FrameRenderedEventArgs> FrameRendered;
    }
}