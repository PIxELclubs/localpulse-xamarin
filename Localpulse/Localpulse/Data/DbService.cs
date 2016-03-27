using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Localpulse
{
    public class DbService
    {
		// ObjectId => data
		public static Dictionary<string, IssueDetail> Issues = new Dictionary<string, IssueDetail>();
		public static Dictionary<string, ObservableCollection<IssueComment>> IssueComments = new Dictionary<string, ObservableCollection<IssueComment>>();

		public static void MergeCollection<T>(Collection<T> dst, Collection<T> src)
		{
			var deleted = Enumerable.Except(dst, src);
			var added = Enumerable.Except(src, dst);
			foreach (var item in deleted) {
				dst.Remove(item);
			}
			foreach (var item in added) {
				dst.Add(item);
			}
		}
	}
}
