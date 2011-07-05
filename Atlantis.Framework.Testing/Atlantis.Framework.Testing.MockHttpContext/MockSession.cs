using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.SessionState;

namespace Atlantis.Framework.Testing.MockHttpContext
{
	public class MockSession : NameObjectCollectionBase, IHttpSessionState
	{
		private string _sessionId = new Guid().ToString();

		internal MockSession()
		{
			this.Clear();
		}

		#region IHttpSessionState Members

		public void Abandon()
		{
			this.Clear();
		}

		public void Add(string name, object value)
		{
			BaseAdd(name, value);
		}

		public void Clear()
		{
			BaseClear();
		}

		public int CodePage
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public HttpCookieMode CookieMode
		{
			get { return HttpCookieMode.UseCookies; }
		}

		public void CopyTo(Array array, int index)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public bool IsCookieless
		{
			get { return false; }
		}

		public bool IsNewSession
		{
			get { return false; }
		}

		public bool IsSynchronized
		{
			get { return true; }
		}

		public int LCID
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public SessionStateMode Mode
		{
			get { return SessionStateMode.Custom; }
		}

		public void Remove(string name)
		{
			BaseRemove(name);
		}

		public void RemoveAll()
		{
			BaseClear();
		}

		public void RemoveAt(int index)
		{
			BaseRemoveAt(index);
		}

		public string SessionID
		{
			get { return _sessionId; }
		}

		public HttpStaticObjectsCollection StaticObjects
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public object SyncRoot
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public int Timeout
		{
			get
			{
				throw new Exception("The method or operation is not implemented.");
			}
			set
			{
				throw new Exception("The method or operation is not implemented.");
			}
		}

		public object this[int index]
		{
			get
			{
				return BaseGet(index);
			}
			set
			{
				BaseSet(index, value);
			}
		}

		public object this[string name]
		{
			get
			{
				return BaseGet(name);
			}
			set
			{
				BaseSet(name, value);
			}
		}

		#endregion

		public new bool IsReadOnly
		{
			get { return false; }
		}
	}
}
