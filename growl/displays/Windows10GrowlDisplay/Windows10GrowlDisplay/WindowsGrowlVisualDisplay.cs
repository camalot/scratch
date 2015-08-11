using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Growl.DisplayStyle;

namespace Growl.Display.Windows10 {

	public class WindowsGrowlVisualDisplay : MultiMonitorVisualDisplay {
		private LayoutManager tllm = new LayoutManager ( LayoutManager.AutoPositionDirection.DownRight, 5, 5 );
		private LayoutManager bllm = new LayoutManager ( LayoutManager.AutoPositionDirection.UpRight, 5, 5 );
		private LayoutManager trlm = new LayoutManager ( LayoutManager.AutoPositionDirection.DownLeft, 5, 5 );
		private LayoutManager brlm = new LayoutManager ( LayoutManager.AutoPositionDirection.UpLeft, 5, 5 );
		private LayoutManager lm = new LayoutManager ( LayoutManager.AutoPositionDirection.UpLeft, 10, 10 );

		public static Color BGCOLOR = Color.FromArgb ( 31, 31, 31 );

		public WindowsGrowlVisualDisplay ( ) {
			base.SettingsPanel = new WindowsGrowlDisplaySettings ( );
		}

		public override string Author { get { return "Ryan Conrad"; } }

		public override string Description { get { return "A simple display that matches the Windows 10 look and feel."; } }

		public override string Name { get { return "Win10"; } }

		public override string Version {
			get {
				System.Reflection.Assembly a = System.Reflection.Assembly.GetExecutingAssembly ( );
				System.Diagnostics.FileVersionInfo f = System.Diagnostics.FileVersionInfo.GetVersionInfo ( a.Location );
				return f.FileVersion;
			}
		}

		public override string Website { get { return "http://bit13.com"; } }

		protected override void HandleNotification ( Notification notification, string displayName ) {

			bool flag = false;
			if ( !string.IsNullOrEmpty ( notification.CoalescingGroup ) ) {
				foreach ( NotificationWindow activeWindow in base.ActiveWindows ) {
					if ( activeWindow.CoalescingGroup != notification.CoalescingGroup ) {
						continue;
					}
						( (WindowsGrowlNotificationWindow)activeWindow ).Replace ( notification );
					flag = true;
					break;
				}
			}
			if ( !flag ) {
				WindowsGrowlNotificationWindow visualWindow = new WindowsGrowlNotificationWindow ( );
				visualWindow.SetNotification ( notification );
				visualWindow.BackColor = BGCOLOR;
				visualWindow.SetDisplayLocation ( this.GetLocationFromSetting ( ) );
				visualWindow.PreferredDevice = base.GetPreferredDisplay ( );
				base.Show ( visualWindow );
			}
		}

		protected override LayoutManager GetLayoutManager ( NotificationWindow win ) {
			switch ( ( (WindowsGrowlNotificationWindow)win ).DisplayLocation ) {
				case WindowsGrowlVisualDisplayLocation.TopLeft:
					return this.tllm;
				case WindowsGrowlVisualDisplayLocation.TopRight:
					return this.trlm;
				case WindowsGrowlVisualDisplayLocation.BottomLeft:
					return this.bllm;
			}
			return this.brlm;
		}

		private WindowsGrowlVisualDisplayLocation GetLocationFromSetting ( ) {
			var location = WindowsGrowlVisualDisplayLocation.BottomRight;
			var settings = base.SettingsPanel.GetSettings ( );
      if ( settings != null && settings.ContainsKey ( WindowsGrowlDisplaySettings.SETTING_DISPLAYLOCATION ) ) {
				try {
					object item = settings[WindowsGrowlDisplaySettings.SETTING_DISPLAYLOCATION];
					if ( item != null ) {
						location = (WindowsGrowlVisualDisplayLocation)item;
					}
				} catch {
				}
			}
			return location;
		}
	}
}
