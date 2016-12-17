#region File Description
//-----------------------------------------------------------------------------
// DrawableGameComponent.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VESSELS
{
    /// <summary>
    /// A base class for a component that can be updated.
    /// </summary>
    public abstract class DrawableGameComponent2 : GameComponent2, IDrawable
    {
        private bool visible = true;
        private int drawOrder = 0;

        /// <summary>
        /// Gets or sets whether the component should be drawn.
        /// </summary>
        public bool Visible
        {
            get 
            {
                VerifyNotDisposed();
                return visible; 
            }
            set
            {
                VerifyNotDisposed();
                if (visible != value)
                {
                    visible = value;
                    if (VisibleChanged != null)
                        VisibleChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the order in which the component should be drawn. 
        /// Components with smaller values are drawn first.
        /// </summary>
        public int DrawOrder
        {
            get
            {
                VerifyNotDisposed(); 
                return drawOrder;
            }
            set
            {
                VerifyNotDisposed();
                if (drawOrder != value)
                {
                    drawOrder = value;
                    if (DrawOrderChanged != null)
                        DrawOrderChanged(this, EventArgs.Empty);
                }
            }
        }

        ///// <summary>
        ///// Gets a GraphicsDevice that can be used for rendering.
        ///// </summary>
        //public GraphicsDevice GraphicsDevice
        //{
        //    get
        //    {
        //        VerifyNotDisposed();
        //        return this.GraphicsDevice;
        //       // return SharedGraphicsDeviceManager.Current.GraphicsDevice;
        //        //return GraphicsDevice;
                
        //    }
        //}

        /// <summary>
        /// Invoked when the Visible property changes.
        /// </summary>
        public event EventHandler<EventArgs> VisibleChanged;

        /// <summary>
        /// Invoked when the DrawOrder property changes.
        /// </summary>
        public event EventHandler<EventArgs> DrawOrderChanged;

        protected DrawableGameComponent2()
        {
            // Hook our own events to surface them as protected virtual methods
            VisibleChanged += OnVisibleChanged;
            DrawOrderChanged += OnDrawOrderChanged;
        }

        protected override void Dispose(bool disposing)
        {
            // Make sure to Unload content when being disposed
            if (disposing)
                UnloadContent();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        public override void Initialize()
        {
            // We pass down to the LoadContent method as part of initialization
            LoadContent();
        }

        /// <summary>
        /// Allows the component to load content.
        /// </summary>
        protected virtual void LoadContent() { }
        
        /// <summary>
        /// Allows the component to unload content.
        /// </summary>
        protected virtual void UnloadContent() { }

        /// <summary>
        /// Draws the component.
        /// </summary>
        public virtual void Draw(TimeSpan elapsedTime, TimeSpan totalTime) { }
        
        /// <summary>
        /// Called when the component's Visible property has changed.
        /// </summary>
        protected virtual void OnVisibleChanged(object sender, EventArgs args) { }

        /// <summary>
        /// Called when the component's DrawOrder property has changed.
        /// </summary>
        protected virtual void OnDrawOrderChanged(object sender, EventArgs args) { }
    }
}
