using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Localpulse
{
    public class IssueComment : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null) {
				PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		string objectId = string.Empty;
		DateTime createdAt = new DateTime(0);
		DateTime updatedAt = new DateTime(0);
		string data = string.Empty;
		string target = string.Empty;

		#endregion

		public string ObjectId
		{
			get {
				return objectId;
			}
			set {
				if (value != objectId) {
					objectId = value;
					NotifyPropertyChanged();
				}
			}
		}
		public DateTime CreatedAt
		{
			get {
				return createdAt;
			}
			set {
				if (value != createdAt) {
					createdAt = value;
					NotifyPropertyChanged();
				}
			}
		}
		public DateTime UpdatedAt
		{
			get {
				return updatedAt;
			}
			set {
				if (value != updatedAt) {
					updatedAt = value;
					NotifyPropertyChanged();
				}
			}
		}
		public string Data
		{
			get {
				return data;
			}
			set {
				if (value != data) {
					data = value;
					NotifyPropertyChanged();
				}
			}
		}
		public string Target
		{
			get {
				return target;
			}
			set {
				if (value != target) {
					target = value;
					NotifyPropertyChanged();
				}
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType()) {
				return false;
			}

			return ObjectId == ((IssueComment)obj).ObjectId;
		}
		public override int GetHashCode()
		{
			return ((ObjectId[3] << 24) | (ObjectId[2] << 16) | (ObjectId[1] << 8) | ObjectId[0]);
		}

		public IssueComment()
		{
			;
		}
	}
}
