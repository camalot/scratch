using System;
using System.Collections.Generic;
using Growl.Destinations;

namespace Growl.Forwarder.Windows10 {
	public class NotificationCenterForwardHandler : IForwardDestinationHandler {
		public string Name {
			get {
				return "Windows 10 Notification Center";
			}
		}

		public List<DestinationListItem> GetListItems ( ) {
			ForwardDestinationListItem item = new ForwardDestinationListItem ( Name, GetIcon ( ), this );
			List<DestinationListItem> list = new List<DestinationListItem> ( );
			list.Add ( item );
			return list;
		}

		public DestinationSettingsPanel GetSettingsPanel ( DestinationListItem dbli ) {
			return new NotificationCenterSettingsPanel ( );
		}

		public DestinationSettingsPanel GetSettingsPanel ( DestinationBase db ) {
			return new NotificationCenterSettingsPanel();
		}

		public List<Type> Register ( ) {
			List<Type> list = new List<Type> ( );
			list.Add ( typeof ( NotificationCenterDestination ) );
			return list;
		}

		internal static System.Drawing.Image GetIcon ( ) {
			return new System.Drawing.Bitmap ( Properties.Resources.internet );
		}
	}
}
