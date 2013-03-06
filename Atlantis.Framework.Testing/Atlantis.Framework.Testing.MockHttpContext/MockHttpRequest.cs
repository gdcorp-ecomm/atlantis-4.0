﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Atlantis.Framework.Testing.MockHttpContext
{
  public class MockHttpRequest : HttpWorkerRequest
  {
    Uri _uri;
    TextWriter _outputWriter;
    List<KeyValuePair<string,string>> _mockHeaders = new List<KeyValuePair<string,string>>();
    IPAddress _remoteAddress = null;

    public void MockRemoteAddress(IPAddress address)
    {
      _remoteAddress = address;
    }

    public void MockHeaderValues(IEnumerable<KeyValuePair<string, string>> headerValues)
    {
      _mockHeaders = new List<KeyValuePair<string, string>>(headerValues);
    }

    public MockHttpRequest(string requestUrl)
    {
      if (!Uri.TryCreate(requestUrl, UriKind.Absolute, out _uri))
      {
        throw new ArgumentException("url must be a valid absolute url.");
      }
      _outputWriter = new StringWriter(new StringBuilder());
    }

    public MockHttpRequest(Uri requestUri)
    {
      if (requestUri == null)
      {
        throw new ArgumentNullException("Uri cannot be null.");
      }

      _uri = requestUri;
      _outputWriter = new StringWriter(new StringBuilder());
    }

    public override string GetServerName()
    {
      return _uri.Host;
    }

    public override string MapPath(string path)
    {
      return path;
    }

    public override void EndOfRequest()
    {
    }

    public override void FlushResponse(bool finalFlush)
    {
    }

    public override string GetHttpVerbName()
    {
      return "GET";
    }

    public override string GetHttpVersion()
    {
      return "HTTP/1.0";
    }

    public override string GetLocalAddress()
    {
      return IPAddress.Loopback.ToString();
    }

    public override int GetLocalPort()
    {
      return 80;
    }

    public override string GetQueryString()
    {
      return _uri.Query;
    }

    public override string GetRawUrl()
    {
      return _uri.AbsolutePath;
    }

    public override string GetRemoteAddress()
    {
      if (_remoteAddress != null)
      {
        return _remoteAddress.ToString();
      }
      else
      {
        return IPAddress.Loopback.ToString();
      }
    }

    public override int GetRemotePort()
    {
      return 0;
    }

    public override string GetUriPath()
    {
      return _uri.AbsolutePath;
    }

    public override void SendKnownResponseHeader(int index, string value)
    {
    }

    public override void SendResponseFromFile(IntPtr handle, long offset, long length)
    {
    }

    public override void SendResponseFromFile(string filename, long offset, long length)
    {
    }

    public override void SendResponseFromMemory(byte[] data, int length)
    {
      _outputWriter.Write(Encoding.Default.GetChars(data, 0, length));
    }

    public override void SendStatus(int statusCode, string statusDescription)
    {
    }

    public override void SendUnknownResponseHeader(string name, string value)
    {
    }

    public override string GetUnknownRequestHeader(string name)
    {
      foreach (var kvp in _mockHeaders)
      {
        if (kvp.Key.Equals(name, StringComparison.OrdinalIgnoreCase))
        {
          return kvp.Value;
        }
      }

      return null;
    }

    public override string[][] GetUnknownRequestHeaders()
    {
      if (_mockHeaders.Count == 0)
        return null;

      string[][] headersArray = new string[_mockHeaders.Count][];

      int index = 0;
      foreach (var kvp in _mockHeaders)
      {
        string[] kvarray = new string[2];
        kvarray[0] = kvp.Key;
        kvarray[1] = kvp.Value;
        headersArray[index] = kvarray;
        index++;
      }

      return headersArray;
    }

  }
}