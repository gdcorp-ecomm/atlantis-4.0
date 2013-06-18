using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Util;
using System.Web.SessionState;
using System.ComponentModel;
using System.IO;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Reflection;
using System.Configuration;
using System.Diagnostics;

namespace BotDetect.Web
{
    /// <summary>
    /// A custom SessionIDManager used to workaround Captcha sound issues in 
    /// clients which don't propagate ASP.NET Session cookies properly. 
    /// Adds the SessionID (encrypted, to prevent Session hijacking) to 
    /// Captcha sound request querystrings.
    /// </summary>
    public class CustomSessionIdManager : SessionIDManager, ISessionIDManager
    {
        public CustomSessionIdManager()
        {
        }

        string ISessionIDManager.GetSessionID(HttpContext context)
        {
            // if this method is called, we can be sure the CustomSessionIdManager is running
            CustomSessionIdManager.ConfirmCustomSessionIdManagerInclusion();

            string id = base.GetSessionID(context);

            if (CaptchaControl.IsCaptchaRequest)
            {
                string encrypted = context.Request.QueryString["s"] as string;
                if (StringHelper.HasValue(encrypted))
                {
                    // to save time on decryption, we try to keep a cached plaintext value
                    string plaintextKey = GetCachedPlaintextKey(encrypted);
                    string decrypted = HttpRuntime.Cache[plaintextKey] as string;
                    if (!StringHelper.HasValue(decrypted))
                    {
                        // decryption
                        try
                        {
                            decrypted = CryptoHelper.Decrypt(encrypted, EncryptionPassword);
                            HttpRuntime.Cache[plaintextKey] = decrypted;
                        }
                        // since there is a number of different exception types that can
                        // happen here, we'll avoid handling them all individually and
                        // use the (usually troublesome) catch-all statement instead
                        //catch (System.Security.Cryptography.CryptographicException ex)
                        //catch (FormatException ex)
                        catch (Exception ex)
                        {
                            // remember that a particular cyphertext is meaningless
                            // (this avoids repeated decryption of invalid values, 
                            // and redundant throwing of this particular exception)
                            HttpRuntime.Cache[plaintextKey] = "invalid";

                            // ignore errors - invalid sid params are quietly dropped
                            Debug.Assert(false, ex.Message);
                        }
                    }

                    if (this.Validate(decrypted))
                    {
                        id = decrypted;
                    }
                }
            }

            return id;
        }


        /// <summary>
        /// The current SessionID, encrypted and ready to be added to Captcha request querystrings
        /// </summary>
        public static string EncryptedSessionId
        {
            get
            {
                if (!isCustomSessionIdManagerRegistered)
                {
                    // encrypting SessionIDs makes no sense if the CustomSessionIdManager
                    // is not registered
                    return null;
                }

                if (null != HttpContext.Current.Session)
                {
                    string sessionId = HttpContext.Current.Session.SessionID;

                    // to save time on encryption, we try to keep a cached cyphertext value
                    string cyphertextKey = GetCachedCyphertextKey(sessionId);
                    string encryptedSid = HttpRuntime.Cache[cyphertextKey] as string;
                    if (!StringHelper.HasValue(encryptedSid))
                    {
                        // encryption
                        encryptedSid = CryptoHelper.Encrypt(sessionId, EncryptionPassword);
                        encryptedSid = HttpContext.Current.Server.UrlEncode(encryptedSid);
                        HttpRuntime.Cache[cyphertextKey] = encryptedSid;
                    }

                    return encryptedSid;
                }

                return null;
            }
        }

        // check is the CustomSessionIdManager running in the current application
        private static bool isCustomSessionIdManagerRegistered = false;

        private static void ConfirmCustomSessionIdManagerInclusion()
        {
            if (!isCustomSessionIdManagerRegistered)
            {
                isCustomSessionIdManagerRegistered = true;
                SessionTroubleshooting.ConfirmSessionModuleInclusion();
                SessionTroubleshooting.ConfirmSessionIdManagerInclusion();
            }
        }


        // cache access
        private static readonly object cacheLock = new object();

        private static string GetCachedCyphertextKey(string sessionId)
        {
            return "LBD_EncryptedSessionID_" + sessionId;
        }

        private static string GetCachedPlaintextKey(string cyphertext)
        {
            return "LBD_PlaintextSessionID_" + cyphertext;
        }


        // configuration shorthand
        private static string EncryptionPassword
        {
            get
            {
                return CaptchaConfiguration.CaptchaEncryption.EncryptionPassword;
            }
        }
    }
}
