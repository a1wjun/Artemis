﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Artemis.Core.LayerBrushes;
using Artemis.Core.LayerEffects;
using Artemis.Storage.Entities.Profile;
using Artemis.Storage.Entities.Profile.Abstract;
using RGB.NET.Core;
using SkiaSharp;

namespace Artemis.Core
{
    /// <summary>
    ///     Represents a layer in a <see cref="Profile" />
    /// </summary>
    public sealed class Layer : RenderProfileElement
    {
        private LayerGeneralProperties _general;
        private BaseLayerBrush? _layerBrush;
        private LayerShape? _layerShape;
        private List<ArtemisLed> _leds;
        private LayerTransformProperties _transform;

        /// <summary>
        ///     Creates a new instance of the <see cref="Layer" /> class and adds itself to the child collection of the provided
        ///     <paramref name="parent" />
        /// </summary>
        /// <param name="parent">The parent of the layer</param>
        /// <param name="name">The name of the layer</param>
        public Layer(ProfileElement parent, string name) : base(parent, parent.Profile)
        {
            LayerEntity = new LayerEntity();
            EntityId = Guid.NewGuid();

            Profile = Parent.Profile;
            Name = name;
            Suspended = false;

            _general = new LayerGeneralProperties();
            _transform = new LayerTransformProperties();

            _leds = new List<ArtemisLed>();
            Leds = new ReadOnlyCollection<ArtemisLed>(_leds);

            Adapter = new LayerAdapter(this);
            Initialize();
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="Layer" /> class based on the provided layer entity
        /// </summary>
        /// <param name="profile">The profile the layer belongs to</param>
        /// <param name="parent">The parent of the layer</param>
        /// <param name="layerEntity">The entity of the layer</param>
        public Layer(Profile profile, ProfileElement parent, LayerEntity layerEntity) : base(parent, parent.Profile)
        {
            LayerEntity = layerEntity;
            EntityId = layerEntity.Id;

            Profile = profile;
            Parent = parent;

            _general = new LayerGeneralProperties();
            _transform = new LayerTransformProperties();

            _leds = new List<ArtemisLed>();
            Leds = new ReadOnlyCollection<ArtemisLed>(_leds);

            Adapter = new LayerAdapter(this);
            Load();
            Initialize();
        }

        /// <summary>
        ///     A collection of all the LEDs this layer is assigned to.
        /// </summary>
        public ReadOnlyCollection<ArtemisLed> Leds { get; private set; }

        /// <summary>
        ///     Defines the shape that is rendered by the <see cref="LayerBrush" />.
        /// </summary>
        public LayerShape? LayerShape
        {
            get => _layerShape;
            set
            {
                SetAndNotify(ref _layerShape, value);
                if (Path != null)
                    CalculateRenderProperties();
            }
        }

        /// <summary>
        ///     Gets the general properties of the layer
        /// </summary>
        [PropertyGroupDescription(Identifier = "General", Name = "General", Description = "A collection of general properties")]
        public LayerGeneralProperties General
        {
            get => _general;
            private set => SetAndNotify(ref _general, value);
        }

        /// <summary>
        ///     Gets the transform properties of the layer
        /// </summary>
        [PropertyGroupDescription(Identifier = "Transform", Name = "Transform", Description = "A collection of transformation properties")]
        public LayerTransformProperties Transform
        {
            get => _transform;
            private set => SetAndNotify(ref _transform, value);
        }

        /// <summary>
        ///     The brush that will fill the <see cref="LayerShape" />.
        /// </summary>
        public BaseLayerBrush? LayerBrush
        {
            get => _layerBrush;
            internal set => SetAndNotify(ref _layerBrush, value);
        }

        /// <summary>
        ///     Gets the layer entity this layer uses for persistent storage
        /// </summary>
        public LayerEntity LayerEntity { get; internal set; }

        /// <summary>
        ///     Gets the layer adapter that can be used to adapt this layer to a different set of devices
        /// </summary>
        public LayerAdapter Adapter { get; }

        /// <inheritdoc />
        public override bool ShouldBeEnabled => !Suspended && DisplayConditionMet;

        internal override RenderElementEntity RenderElementEntity => LayerEntity;

        /// <inheritdoc />
        public override List<ILayerProperty> GetAllLayerProperties()
        {
            List<ILayerProperty> result = new();
            result.AddRange(General.GetAllLayerProperties());
            result.AddRange(Transform.GetAllLayerProperties());
            if (LayerBrush?.BaseProperties != null)
                result.AddRange(LayerBrush.BaseProperties.GetAllLayerProperties());
            foreach (BaseLayerEffect layerEffect in LayerEffects)
                if (layerEffect.BaseProperties != null)
                    result.AddRange(layerEffect.BaseProperties.GetAllLayerProperties());

            return result;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Layer] {nameof(Name)}: {Name}, {nameof(Order)}: {Order}";
        }

        /// <summary>
        ///     Occurs when a property affecting the rendering properties of this layer has been updated
        /// </summary>
        public event EventHandler? RenderPropertiesUpdated;

        /// <summary>
        ///     Occurs when the layer brush of this layer has been updated
        /// </summary>
        public event EventHandler? LayerBrushUpdated;

        #region Overrides of BreakableModel

        /// <inheritdoc />
        public override IEnumerable<IBreakableModel> GetBrokenHierarchy()
        {
            if (LayerBrush?.BrokenState != null)
                yield return LayerBrush;
            foreach (BaseLayerEffect baseLayerEffect in LayerEffects.Where(e => e.BrokenState != null))
                yield return baseLayerEffect;
        }

        #endregion

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            Disable();

            Disposed = true;

            LayerBrushStore.LayerBrushAdded -= LayerBrushStoreOnLayerBrushAdded;
            LayerBrushStore.LayerBrushRemoved -= LayerBrushStoreOnLayerBrushRemoved;

            // Brush first in case it depends on any of the other disposables during it's own disposal
            _layerBrush?.Dispose();
            _general.Dispose();
            _transform.Dispose();

            base.Dispose(disposing);
        }

        internal void OnLayerBrushUpdated()
        {
            LayerBrushUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void Initialize()
        {
            LayerBrushStore.LayerBrushAdded += LayerBrushStoreOnLayerBrushAdded;
            LayerBrushStore.LayerBrushRemoved += LayerBrushStoreOnLayerBrushRemoved;

            // Layers have two hardcoded property groups, instantiate them
            PropertyGroupDescriptionAttribute generalAttribute = (PropertyGroupDescriptionAttribute) Attribute.GetCustomAttribute(
                GetType().GetProperty(nameof(General))!,
                typeof(PropertyGroupDescriptionAttribute)
            )!;
            PropertyGroupDescriptionAttribute transformAttribute = (PropertyGroupDescriptionAttribute) Attribute.GetCustomAttribute(
                GetType().GetProperty(nameof(Transform))!,
                typeof(PropertyGroupDescriptionAttribute)
            )!;

            LayerEntity.GeneralPropertyGroup ??= new PropertyGroupEntity {Identifier = generalAttribute.Identifier};
            LayerEntity.TransformPropertyGroup ??= new PropertyGroupEntity {Identifier = transformAttribute.Identifier};

            General.Initialize(this, null, generalAttribute, LayerEntity.GeneralPropertyGroup);
            Transform.Initialize(this, null, transformAttribute, LayerEntity.TransformPropertyGroup);

            General.ShapeType.CurrentValueSet += ShapeTypeOnCurrentValueSet;
            ApplyShapeType();
            ActivateLayerBrush();

            Reset();
        }

        private void LayerBrushStoreOnLayerBrushRemoved(object? sender, LayerBrushStoreEvent e)
        {
            if (LayerBrush?.Descriptor == e.Registration.LayerBrushDescriptor)
                DeactivateLayerBrush();
        }

        private void LayerBrushStoreOnLayerBrushAdded(object? sender, LayerBrushStoreEvent e)
        {
            if (LayerBrush != null || !General.PropertiesInitialized)
                return;

            LayerBrushReference? current = General.BrushReference.CurrentValue;
            if (e.Registration.PluginFeature.Id == current?.LayerBrushProviderId && e.Registration.LayerBrushDescriptor.LayerBrushType.Name == current.BrushType)
                ActivateLayerBrush();
        }

        private void OnRenderPropertiesUpdated()
        {
            RenderPropertiesUpdated?.Invoke(this, EventArgs.Empty);
        }

        #region Storage

        internal override void Load()
        {
            EntityId = LayerEntity.Id;
            Name = LayerEntity.Name;
            Suspended = LayerEntity.Suspended;
            Order = LayerEntity.Order;

            ExpandedPropertyGroups.AddRange(LayerEntity.ExpandedPropertyGroups);
            LoadRenderElement();
            Adapter.Load();
        }

        internal override void Save()
        {
            if (Disposed)
                throw new ObjectDisposedException("Layer");

            // Properties
            LayerEntity.Id = EntityId;
            LayerEntity.ParentId = Parent?.EntityId ?? new Guid();
            LayerEntity.Order = Order;
            LayerEntity.Suspended = Suspended;
            LayerEntity.Name = Name;
            LayerEntity.ProfileId = Profile.EntityId;
            LayerEntity.ExpandedPropertyGroups.Clear();
            LayerEntity.ExpandedPropertyGroups.AddRange(ExpandedPropertyGroups);

            General.ApplyToEntity();
            Transform.ApplyToEntity();

            // Don't override the old value of LayerBrush if the current value is null, this avoid losing settings of an unavailable brush
            if (LayerBrush != null)
            {
                LayerBrush.Save();
                LayerEntity.LayerBrush = LayerBrush.LayerBrushEntity;
            }

            // LEDs
            LayerEntity.Leds.Clear();
            foreach (ArtemisLed artemisLed in Leds)
            {
                LedEntity ledEntity = new()
                {
                    DeviceIdentifier = artemisLed.Device.Identifier,
                    LedName = artemisLed.RgbLed.Id.ToString(),
                    PhysicalLayout = artemisLed.Device.DeviceType == RGBDeviceType.Keyboard ? (int) artemisLed.Device.PhysicalLayout : null
                };
                LayerEntity.Leds.Add(ledEntity);
            }

            // Adaption hints
            Adapter.Save();

            SaveRenderElement();
        }

        #endregion

        #region Shape management

        private void ShapeTypeOnCurrentValueSet(object? sender, EventArgs e)
        {
            ApplyShapeType();
        }

        private void ApplyShapeType()
        {
            LayerShape = General.ShapeType.CurrentValue switch
            {
                LayerShapeType.Ellipse => new EllipseShape(this),
                LayerShapeType.Rectangle => new RectangleShape(this),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        #endregion

        #region Rendering

        /// <inheritdoc />
        public override void Update(double deltaTime)
        {
            if (Disposed)
                throw new ObjectDisposedException("Layer");

            if (Timeline.IsOverridden)
            {
                Timeline.ClearOverride();
                return;
            }

            UpdateDisplayCondition();
            UpdateTimeline(deltaTime);

            if (ShouldBeEnabled)
                Enable();
            else if (Timeline.IsFinished)
                Disable();

            if (Timeline.Delta == TimeSpan.Zero)
                return;

            General.Update(Timeline);
            Transform.Update(Timeline);
            LayerBrush?.InternalUpdate(Timeline);

            foreach (BaseLayerEffect baseLayerEffect in LayerEffects.Where(e => !e.Suspended))
                baseLayerEffect.InternalUpdate(Timeline);

            // Remove children that finished their timeline and update the rest
            for (int index = 0; index < Children.Count; index++)
            {
                ProfileElement profileElement = Children[index];
                if (((Layer) profileElement).Timeline.IsFinished)
                {
                    RemoveChild(profileElement);
                    profileElement.Dispose();
                    index--;
                }
                else
                    profileElement.Update(deltaTime);
            }
        }

        /// <inheritdoc />
        public override void Render(SKCanvas canvas, SKPointI basePosition)
        {
            if (Disposed)
                throw new ObjectDisposedException("Layer");

            // Ensure the layer is ready
            if (!Enabled || Path == null || LayerShape?.Path == null || !General.PropertiesInitialized || !Transform.PropertiesInitialized || !Leds.Any())
                return;

            // Render children first so they go below
            for (int i = Children.Count - 1; i >= 0; i--)
                Children[i].Render(canvas, basePosition);

            // Ensure the brush is ready
            if (LayerBrush == null || LayerBrush?.BaseProperties?.PropertiesInitialized == false)
                return;

            if (Timeline.IsFinished || LayerBrush?.BrushType != LayerBrushType.Regular)
                return;

            SKPaint layerPaint = new() {FilterQuality = SKFilterQuality.Low};
            try
            {
                canvas.Save();
                canvas.Translate(Bounds.Left - basePosition.X, Bounds.Top - basePosition.Y);
                using SKPath clipPath = new(Path);
                clipPath.Transform(SKMatrix.CreateTranslation(Bounds.Left * -1, Bounds.Top * -1));
                canvas.ClipPath(clipPath, SKClipOperation.Intersect, true);
                SKRectI layerBounds = SKRectI.Create(0, 0, Bounds.Width, Bounds.Height);

                // Apply blend mode and color
                layerPaint.BlendMode = General.BlendMode.CurrentValue;
                layerPaint.Color = new SKColor(0, 0, 0, (byte) (Transform.Opacity.CurrentValue * 2.55f));

                using SKPath renderPath = new();

                if (General.ShapeType.CurrentValue == LayerShapeType.Rectangle)
                    renderPath.AddRect(layerBounds);
                else
                    renderPath.AddOval(layerBounds);

                if (General.TransformMode.CurrentValue == LayerTransformMode.Normal)
                {
                    // Apply transformation except rotation to the render path
                    if (LayerBrush.SupportsTransformation)
                    {
                        SKMatrix renderPathMatrix = GetTransformMatrix(true, true, true, false);
                        renderPath.Transform(renderPathMatrix);
                    }

                    // Apply rotation to the canvas
                    if (LayerBrush.SupportsTransformation)
                    {
                        SKMatrix rotationMatrix = GetTransformMatrix(true, false, false, true);
                        canvas.SetMatrix(canvas.TotalMatrix.PreConcat(rotationMatrix));
                    }

                    DelegateRendering(canvas, renderPath, renderPath.Bounds, layerPaint);
                }
                else if (General.TransformMode.CurrentValue == LayerTransformMode.Clip)
                {
                    SKMatrix renderPathMatrix = GetTransformMatrix(true, true, true, true);
                    renderPath.Transform(renderPathMatrix);

                    DelegateRendering(canvas, renderPath, layerBounds, layerPaint);
                }
            }
            finally
            {
                canvas.Restore();
                layerPaint.DisposeSelfAndProperties();
            }

            Timeline.ClearDelta();
        }

        /// <inheritdoc />
        public override void Enable()
        {
            if (Enabled)
                return;

            bool tryOrBreak = TryOrBreak(() => LayerBrush?.InternalEnable(), "Failed to enable layer brush");
            if (!tryOrBreak)
                return;

            tryOrBreak = TryOrBreak(() =>
            {
                foreach (BaseLayerEffect baseLayerEffect in LayerEffects)
                    baseLayerEffect.InternalEnable();
            }, "Failed to enable one or more effects");
            if (!tryOrBreak)
                return;

            Enabled = true;
        }

        /// <inheritdoc />
        public override void OverrideTimelineAndApply(TimeSpan position, bool stickToMainSegment)
        {
            Timeline.Override(position, stickToMainSegment);

            General.Update(Timeline);
            Transform.Update(Timeline);
            LayerBrush?.InternalUpdate(Timeline);

            foreach (BaseLayerEffect baseLayerEffect in LayerEffects.Where(e => !e.Suspended))
                baseLayerEffect.InternalUpdate(Timeline);
        }

        /// <inheritdoc />
        public override void Activate()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Deactivate()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override void Disable()
        {
            if (!Enabled)
                return;

            LayerBrush?.InternalDisable();
            foreach (BaseLayerEffect baseLayerEffect in LayerEffects)
                baseLayerEffect.InternalDisable();

            Enabled = false;
        }

        /// <inheritdoc />
        public override void Reset()
        {
            UpdateDisplayCondition();

            if (DisplayConditionMet)
                Timeline.JumpToStart();
            else
                Timeline.JumpToEnd();

            while (Children.Any())
            {
                Children[0].Dispose();
                RemoveChild(Children[0]);
            }
        }

        /// <summary>
        ///     Creates a copy of this layer as a child and plays it once
        /// </summary>
        public void CreateCopyAsChild()
        {
            throw new NotImplementedException();

            // Create a copy of the layer and it's properties

            // Add to children
        }

        internal void CalculateRenderProperties()
        {
            if (Disposed)
                throw new ObjectDisposedException("Layer");

            if (!Leds.Any())
            {
                Path = new SKPath();
            }
            else
            {
                SKPath path = new() {FillType = SKPathFillType.Winding};
                foreach (ArtemisLed artemisLed in Leds)
                    path.AddRect(artemisLed.AbsoluteRectangle);

                Path = path;
            }

            // This is called here so that the shape's render properties are up to date when other code
            // responds to OnRenderPropertiesUpdated
            LayerShape?.CalculateRenderProperties();

            // Folder render properties are based on child paths and thus require an update
            if (Parent is Folder folder)
                folder.CalculateRenderProperties();

            OnRenderPropertiesUpdated();
        }

        internal SKPoint GetLayerAnchorPosition(bool applyTranslation, bool zeroBased, SKRect? customBounds = null)
        {
            if (Disposed)
                throw new ObjectDisposedException("Layer");

            SKRect bounds = customBounds ?? Bounds;
            SKPoint positionProperty = Transform.Position.CurrentValue;

            // Start at the center of the shape
            SKPoint position = zeroBased
                ? new SKPoint(bounds.MidX - bounds.Left, bounds.MidY - Bounds.Top)
                : new SKPoint(bounds.MidX, bounds.MidY);

            // Apply translation
            if (applyTranslation)
            {
                position.X += positionProperty.X * bounds.Width;
                position.Y += positionProperty.Y * bounds.Height;
            }

            return position;
        }

        private void DelegateRendering(SKCanvas canvas, SKPath renderPath, SKRect bounds, SKPaint layerPaint)
        {
            if (LayerBrush == null)
                throw new ArtemisCoreException("The layer is not yet ready for rendering");

            foreach (BaseLayerEffect baseLayerEffect in LayerEffects.Where(e => !e.Suspended))
                baseLayerEffect.InternalPreProcess(canvas, bounds, layerPaint);

            try
            {
                canvas.SaveLayer(layerPaint);
                canvas.ClipPath(renderPath);

                // Restore the blend mode before doing the actual render
                layerPaint.BlendMode = SKBlendMode.SrcOver;

                LayerBrush.InternalRender(canvas, bounds, layerPaint);

                foreach (BaseLayerEffect baseLayerEffect in LayerEffects.Where(e => !e.Suspended))
                    baseLayerEffect.InternalPostProcess(canvas, bounds, layerPaint);
            }

            finally
            {
                canvas.Restore();
            }
        }

        /// <summary>
        ///     Creates a transformation matrix that applies the current transformation settings
        /// </summary>
        /// <param name="zeroBased">
        ///     If true, treats the layer as if it is located at 0,0 instead of its actual position on the
        ///     surface
        /// </param>
        /// <param name="includeTranslation">Whether translation should be included</param>
        /// <param name="includeScale">Whether the scale should be included</param>
        /// <param name="includeRotation">Whether the rotation should be included</param>
        /// <param name="customBounds">Optional custom bounds to base the anchor on</param>
        /// <returns>The transformation matrix containing the current transformation settings</returns>
        public SKMatrix GetTransformMatrix(bool zeroBased, bool includeTranslation, bool includeScale, bool includeRotation, SKRect? customBounds = null)
        {
            if (Disposed)
                throw new ObjectDisposedException("Layer");

            if (Path == null)
                return SKMatrix.Empty;

            SKRect bounds = customBounds ?? Bounds;
            SKSize sizeProperty = Transform.Scale.CurrentValue;
            float rotationProperty = Transform.Rotation.CurrentValue;

            SKPoint anchorPosition = GetLayerAnchorPosition(true, zeroBased, bounds);
            SKPoint anchorProperty = Transform.AnchorPoint.CurrentValue;

            // Translation originates from the unscaled center of the shape and is tied to the anchor
            float x = anchorPosition.X - (zeroBased ? bounds.MidX - bounds.Left : bounds.MidX) - anchorProperty.X * bounds.Width;
            float y = anchorPosition.Y - (zeroBased ? bounds.MidY - bounds.Top : bounds.MidY) - anchorProperty.Y * bounds.Height;

            SKMatrix transform = SKMatrix.Empty;

            if (includeTranslation)
                // transform is always SKMatrix.Empty here...
                transform = SKMatrix.CreateTranslation(x, y);

            if (includeScale)
            {
                if (transform == SKMatrix.Empty)
                    transform = SKMatrix.CreateScale(sizeProperty.Width / 100f, sizeProperty.Height / 100f, anchorPosition.X, anchorPosition.Y);
                else
                    transform = transform.PostConcat(SKMatrix.CreateScale(sizeProperty.Width / 100f, sizeProperty.Height / 100f, anchorPosition.X, anchorPosition.Y));
            }

            if (includeRotation)
            {
                if (transform == SKMatrix.Empty)
                    transform = SKMatrix.CreateRotationDegrees(rotationProperty, anchorPosition.X, anchorPosition.Y);
                else
                    transform = transform.PostConcat(SKMatrix.CreateRotationDegrees(rotationProperty, anchorPosition.X, anchorPosition.Y));
            }

            return transform;
        }

        #endregion

        #region LED management

        /// <summary>
        ///     Adds a new <see cref="ArtemisLed" /> to the layer and updates the render properties.
        /// </summary>
        /// <param name="led">The LED to add</param>
        public void AddLed(ArtemisLed led)
        {
            if (Disposed)
                throw new ObjectDisposedException("Layer");

            _leds.Add(led);
            CalculateRenderProperties();
        }

        /// <summary>
        ///     Adds a collection of new <see cref="ArtemisLed" />s to the layer and updates the render properties.
        /// </summary>
        /// <param name="leds">The LEDs to add</param>
        public void AddLeds(IEnumerable<ArtemisLed> leds)
        {
            if (Disposed)
                throw new ObjectDisposedException("Layer");

            _leds.AddRange(leds.Except(_leds));
            CalculateRenderProperties();
        }

        /// <summary>
        ///     Removes a <see cref="ArtemisLed" /> from the layer and updates the render properties.
        /// </summary>
        /// <param name="led">The LED to remove</param>
        public void RemoveLed(ArtemisLed led)
        {
            if (Disposed)
                throw new ObjectDisposedException("Layer");

            _leds.Remove(led);
            CalculateRenderProperties();
        }

        /// <summary>
        ///     Removes all <see cref="ArtemisLed" />s from the layer and updates the render properties.
        /// </summary>
        public void ClearLeds()
        {
            if (Disposed)
                throw new ObjectDisposedException("Layer");

            _leds.Clear();
            CalculateRenderProperties();
        }

        internal void PopulateLeds(IEnumerable<ArtemisDevice> devices)
        {
            if (Disposed)
                throw new ObjectDisposedException("Layer");

            List<ArtemisLed> leds = new();

            // Get the surface LEDs for this layer
            List<ArtemisLed> availableLeds = devices.SelectMany(d => d.Leds).ToList();
            foreach (LedEntity ledEntity in LayerEntity.Leds)
            {
                ArtemisLed? match = availableLeds.FirstOrDefault(a => a.Device.Identifier == ledEntity.DeviceIdentifier &&
                                                                      a.RgbLed.Id.ToString() == ledEntity.LedName);
                if (match != null)
                    leds.Add(match);
            }

            _leds = leds;
            Leds = new ReadOnlyCollection<ArtemisLed>(_leds);
            CalculateRenderProperties();
        }

        #endregion

        #region Brush management

        /// <summary>
        ///     Changes the current layer brush to the provided layer brush and activates it
        /// </summary>
        public void ChangeLayerBrush(BaseLayerBrush? layerBrush)
        {
            General.BrushReference.SetCurrentValue(layerBrush != null ? new LayerBrushReference(layerBrush.Descriptor) : null, null);
            LayerBrush = layerBrush;

            if (LayerBrush != null)
                ActivateLayerBrush();
            else
                OnLayerBrushUpdated();
        }

        internal void ActivateLayerBrush()
        {
            try
            {
                if (LayerBrush == null)
                {
                    // If the brush is null, try to instantiate it
                    LayerBrushReference? brushReference = General.BrushReference.CurrentValue;
                    if (brushReference?.LayerBrushProviderId != null && brushReference.BrushType != null)
                        ChangeLayerBrush(LayerBrushStore.Get(brushReference.LayerBrushProviderId, brushReference.BrushType)?.LayerBrushDescriptor.CreateInstance(this, LayerEntity.LayerBrush));
                    // If that's not possible there's nothing to do
                    return;
                }

                General.ShapeType.IsHidden = LayerBrush != null && !LayerBrush.SupportsTransformation;
                General.BlendMode.IsHidden = LayerBrush != null && !LayerBrush.SupportsTransformation;
                Transform.IsHidden = LayerBrush != null && !LayerBrush.SupportsTransformation;

                OnLayerBrushUpdated();
                ClearBrokenState("Failed to initialize layer brush");
            }
            catch (Exception e)
            {
                SetBrokenState("Failed to initialize layer brush", e);
            }
        }

        internal void DeactivateLayerBrush()
        {
            if (LayerBrush == null)
                return;

            BaseLayerBrush? brush = LayerBrush;
            LayerBrush = null;
            brush?.Dispose();

            OnLayerBrushUpdated();
        }

        #endregion
    }

    /// <summary>
    ///     Represents a type of layer shape
    /// </summary>
    public enum LayerShapeType
    {
        /// <summary>
        ///     A circular layer shape
        /// </summary>
        Ellipse,

        /// <summary>
        ///     A rectangular layer shape
        /// </summary>
        Rectangle
    }

    /// <summary>
    ///     Represents a layer transform mode
    /// </summary>
    public enum LayerTransformMode
    {
        /// <summary>
        ///     Normal transformation
        /// </summary>
        Normal,

        /// <summary>
        ///     Transforms only a clip
        /// </summary>
        Clip
    }
}