using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Growl.DisplayStyle;
using Microsoft.Win32;

namespace Growl.Display.Windows10 {
	class SettingsHelper {
		public const string SETTING_DISPLAYLOCATION = "DisplayLocation";
		public const string SETTING_BGCOLOR = "DisplayBGColor";
		public const string SETTING_TITLECOLOR = "DisplayTitleColor";
		public const string SETTING_DESCCOLOR = "DisplayDescColor";
		public const string SETTING_SPEED = "DisplaySpeed";

		public static Color DEFAULT_BGCOLOR = Color.FromArgb ( 31, 31, 31 );
		public static Color DEFAULT_TITLECOLOR = Color.FromArgb ( 255, 255, 255 );
		public static Color DEFAULT_DESCCOLOR = Color.FromArgb ( 165, 165, 165 );
		public static int DEFAULT_SPEED = 250;

		public static Color GetBackColorFromSetting ( SettingsPanelBase settingsPanel ) {
			return GetColorFromSetting ( settingsPanel, SETTING_BGCOLOR, DEFAULT_BGCOLOR );
		}

		public static Color GetTitleColorFromSetting ( SettingsPanelBase settingsPanel ) {
			return GetColorFromSetting ( settingsPanel, SETTING_TITLECOLOR, DEFAULT_TITLECOLOR );
		}

		public static Color GetDescColorFromSetting ( SettingsPanelBase settingsPanel ) {
			return GetColorFromSetting ( settingsPanel, SETTING_DESCCOLOR, DEFAULT_DESCCOLOR );
		}

		public static int GetSpeedFromSetting( SettingsPanelBase panel ) {
			return GetFromSetting ( panel, SETTING_SPEED, DEFAULT_SPEED );
		}

		private static Color GetColorFromSetting ( SettingsPanelBase settingsPanel, string key, Color defaultColor ) {
			return GetFromSetting ( settingsPanel, key, defaultColor );
		}

		private static T GetFromSetting<T>(SettingsPanelBase panel, string key, T defaultValue ) {
			var d = defaultValue;
			var settings = panel.GetSettings ( );
			if ( settings != null && settings.ContainsKey ( key ) ) {
				try {
					object item = settings[key];
					if ( item != null ) {
						d = (T)item;
					}
				} catch {
				}
			}
			return d;
		}


		public static WindowsGrowlVisualDisplayLocation GetLocationFromSetting ( SettingsPanelBase settingsPanel ) {
			var location = WindowsGrowlVisualDisplayLocation.BottomRight;
			var settings = settingsPanel.GetSettings ( );
			if ( settings != null && settings.ContainsKey ( SETTING_DISPLAYLOCATION ) ) {
				try {
					object item = settings[SETTING_DISPLAYLOCATION];
					if ( item != null ) {
						location = (WindowsGrowlVisualDisplayLocation)item;
					}
				} catch {
				}
			}
			return location;
		}

		private static Color ConvertFromWin32Color ( int color ) {
			if ( color < 0 ) {
				return DEFAULT_BGCOLOR;
			}
			return ColorTranslator.FromWin32 ( color );
		}

		private static int RegistryReadAccentColor ( ) {
			using ( var hive = Registry.CurrentUser.OpenSubKey ( @"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Accent" ) ) {
				var val = hive.GetValue ( "AccentColor" );
				if ( val != null ) {
					return (int)val;
				} else {
					return -1;
				}
			}
		}
	}
}
