using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Growl.DisplayStyle;

namespace Growl.Display.Windows10 {
	public partial class WindowsGrowlNotificationWindow : NotificationWindow {
		private Screen preferredDeviceName;
		public WindowsGrowlNotificationWindow ( WindowsGrowlVisualDisplayLocation displayLocation ) {
			InitializeComponent ( );
			this.AfterLoad += WindowsGrowlNotificationWindow_AfterLoad;
			this.AutoClosing += WindowsGrowlNotificationWindow_AutoClosing;
			this.BeforeShown += WindowsGrowlNotificationWindow_BeforeShown;
			this.DisplayLocation = displayLocation;
			var dIn = this.DisplayLocation == WindowsGrowlVisualDisplayLocation.TopLeft || this.DisplayLocation == WindowsGrowlVisualDisplayLocation.BottomLeft ? Win32AnimatorEx.AnimationDirection.Right : Win32AnimatorEx.AnimationDirection.Left;
			var dOut = this.DisplayLocation == WindowsGrowlVisualDisplayLocation.TopLeft || this.DisplayLocation == WindowsGrowlVisualDisplayLocation.BottomLeft ? Win32AnimatorEx.AnimationDirection.Left : Win32AnimatorEx.AnimationDirection.Right;

			Animator = new Win32AnimatorEx ( this, Win32AnimatorEx.AnimationMethod.Slide, dIn, dOut, 500 );
			HookUpClickEvents ( this );
			base.SetAutoCloseInterval ( 4000 );
		}

		private void WindowsGrowlNotificationWindow_BeforeShown ( object sender, EventArgs e ) {

		}

		private void WindowsGrowlNotificationWindow_AutoClosing ( object sender, FormClosingEventArgs e ) {

		}

		private void WindowsGrowlNotificationWindow_AfterLoad ( object sender, EventArgs e ) {
			int width = this.PreferredDevice.WorkingArea.Width - base.Width;
			int height = this.PreferredDevice.WorkingArea.Height;
			var leftXLocation = this.PreferredDevice.WorkingArea.Left;
			Rectangle workingArea = this.PreferredDevice.WorkingArea;
			var rightXLocation = workingArea.Right - base.Width;
			var topYLocation = this.PreferredDevice.WorkingArea.Top;
			var bottomYLocation = this.PreferredDevice.WorkingArea.Bottom;
			base.Location = new Point ( width, height );
			switch ( this.DisplayLocation ) {
				case WindowsGrowlVisualDisplayLocation.TopLeft:
					base.Location = new Point ( leftXLocation, topYLocation );
					return;
				case WindowsGrowlVisualDisplayLocation.TopRight:
					base.Location = new Point ( rightXLocation, topYLocation );
					return;
				case WindowsGrowlVisualDisplayLocation.BottomLeft:
					base.Location = new Point ( leftXLocation, bottomYLocation - base.Height - 30 );
					return;
			}
			base.Location = new Point ( rightXLocation, bottomYLocation - base.Height - 30 );
		}

		public override void SetNotification ( Notification n ) {
			base.SetNotification ( n );

			// handle the image. if the image is not set, move the other controls over to compensate
			Image image = n.Image;
			if ( image != null ) {
				this.icon.Image = image;
				this.icon.Visible = true;
			} else {
				//int offset = this.icon.Width - 6;
				//this.title.Left = this.title.Left + offset;
				//this.title.Width = this.title.Width - offset;
				//this.description.Left = this.description.Left + offset;
				//this.description.Width = this.description.Width - offset;
				this.icon.Visible = true;
			}

			this.title.Text = n.Title;
			this.description.Text = n.Description.Replace ( "\n", "\r\n" );
			this.Sticky = n.Sticky;


		}

		public WindowsGrowlVisualDisplayLocation DisplayLocation { get; private set; } = WindowsGrowlVisualDisplayLocation.BottomRight;

		public void Replace ( Notification n, WindowsGrowlVisualDisplayLocation location ) {
			this.Replaced = true;
			this.DisplayLocation = location;
			this.OnAfterLoad ( this, EventArgs.Empty );

			var dIn = this.DisplayLocation == WindowsGrowlVisualDisplayLocation.TopLeft || this.DisplayLocation == WindowsGrowlVisualDisplayLocation.BottomLeft ? Win32AnimatorEx.AnimationDirection.Right : Win32AnimatorEx.AnimationDirection.Left;
			var dOut = this.DisplayLocation == WindowsGrowlVisualDisplayLocation.TopLeft || this.DisplayLocation == WindowsGrowlVisualDisplayLocation.BottomLeft ? Win32AnimatorEx.AnimationDirection.Left : Win32AnimatorEx.AnimationDirection.Right;
			Animator = new Win32AnimatorEx ( this, Win32AnimatorEx.AnimationMethod.Slide, dIn, dOut, 500 );
			
			base.StopAutoCloseTimer ( );
			this.SetNotification ( n );
			this.Show ( );
			this.OnShown ( EventArgs.Empty );


			base.StartAutoCloseTimer ( );
		}

		protected override void OnShown ( EventArgs e ) {
			base.OnShown ( e );
		}
		private void title_LabelHeightChanged ( ExpandingLabel.LabelHeightChangedEventArgs args ) {
			this.icon.Top += args.HeightChange;
			this.description.Top += args.HeightChange;
			description_LabelHeightChanged ( args );
		}

		private void description_LabelHeightChanged ( ExpandingLabel.LabelHeightChangedEventArgs args ) {
		}

		private bool Replaced { get; set; } = false;

		public Screen PreferredDevice {
			get {
				if ( this.preferredDeviceName == null ) {
					return Screen.FromControl ( this );
				}
				return this.preferredDeviceName;
			}
			set {
				this.preferredDeviceName = value;
			}
		}
	}
}
