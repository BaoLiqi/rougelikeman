﻿namespace GooglePlayGames.Android
{
    using Com.Google.Android.Gms.Common.Api;
    using GooglePlayGames;
    using GooglePlayGames.OurUtils;
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using UnityEngine;

    internal class AndroidTokenClient : TokenClient
    {
        private const string TokenFragmentClass = "com.google.games.bridge.TokenFragment";
        private const string FetchTokenSignature = "(Landroid/app/Activity;ZZZZLjava/lang/String;Z[Ljava/lang/String;ZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;";
        private const string FetchTokenMethod = "fetchToken";
        private const string GetAnotherAuthCodeMethod = "getAnotherAuthCode";
        private const string GetAnotherAuthCodeSignature = "(Landroid/app/Activity;ZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;";
        private bool requestEmail;
        private bool requestAuthCode;
        private bool requestIdToken;
        private List<string> oauthScopes;
        private string webClientId;
        private bool forceRefresh;
        private bool hidePopups;
        private string accountName;
        private string email;
        private string authCode;
        private string idToken;
        [CompilerGenerated]
        private static Action <>f__am$cache0;

        public void AddOauthScopes(params string[] scopes)
        {
            if (scopes != null)
            {
                if (this.oauthScopes == null)
                {
                    this.oauthScopes = new List<string>();
                }
                this.oauthScopes.AddRange(scopes);
            }
        }

        public static IntPtr CreateInvisibleView()
        {
            object[] args = new object[1];
            jvalue[] jvalueArray = AndroidJNIHelper.CreateJNIArgArray(args);
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("com.google.games.bridge.TokenFragment"))
                {
                    using (AndroidJavaObject obj2 = GetActivity())
                    {
                        IntPtr methodID = AndroidJNI.GetStaticMethodID(class2.GetRawClass(), "createInvisibleView", "(Landroid/app/Activity;)Landroid/view/View;");
                        jvalueArray[0].l = obj2.GetRawObject();
                        return AndroidJNI.CallStaticObjectMethod(class2.GetRawClass(), methodID, jvalueArray);
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.e("Exception creating invisible view: " + exception.Message);
                Logger.e(exception.ToString());
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
            return IntPtr.Zero;
        }

        internal void DoFetchToken(bool silent, Action<int> callback)
        {
            <DoFetchToken>c__AnonStorey1 storey = new <DoFetchToken>c__AnonStorey1 {
                callback = callback,
                $this = this
            };
            object[] args = new object[10];
            jvalue[] jvalueArray = AndroidJNIHelper.CreateJNIArgArray(args);
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("com.google.games.bridge.TokenFragment"))
                {
                    using (AndroidJavaObject obj2 = GetActivity())
                    {
                        IntPtr methodID = AndroidJNI.GetStaticMethodID(class2.GetRawClass(), "fetchToken", "(Landroid/app/Activity;ZZZZLjava/lang/String;Z[Ljava/lang/String;ZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;");
                        jvalueArray[0].l = obj2.GetRawObject();
                        jvalueArray[1].z = silent;
                        jvalueArray[2].z = this.requestAuthCode;
                        jvalueArray[3].z = this.requestEmail;
                        jvalueArray[4].z = this.requestIdToken;
                        jvalueArray[5].l = AndroidJNI.NewStringUTF(this.webClientId);
                        jvalueArray[6].z = this.forceRefresh;
                        jvalueArray[7].l = AndroidJNIHelper.ConvertToJNIArray(this.oauthScopes.ToArray());
                        jvalueArray[8].z = this.hidePopups;
                        jvalueArray[9].l = AndroidJNI.NewStringUTF(this.accountName);
                        new PendingResult<TokenResult>(AndroidJNI.CallStaticObjectMethod(class2.GetRawClass(), methodID, jvalueArray)).setResultCallback(new TokenResultCallback(new Action<int, string, string, string>(storey.<>m__0)));
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.e("Exception launching token request: " + exception.Message);
                Logger.e(exception.ToString());
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
        }

        public void FetchTokens(bool silent, Action<int> callback)
        {
            <FetchTokens>c__AnonStorey0 storey = new <FetchTokens>c__AnonStorey0 {
                silent = silent,
                callback = callback,
                $this = this
            };
            PlayGamesHelperObject.RunOnGameThread(new Action(storey.<>m__0));
        }

        public static AndroidJavaObject GetActivity()
        {
            using (AndroidJavaClass class2 = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                return class2.GetStatic<AndroidJavaObject>("currentActivity");
            }
        }

        public void GetAnotherServerAuthCode(bool reAuthenticateIfNeeded, Action<string> callback)
        {
            <GetAnotherServerAuthCode>c__AnonStorey2 storey = new <GetAnotherServerAuthCode>c__AnonStorey2 {
                callback = callback,
                $this = this
            };
            object[] args = new object[3];
            jvalue[] jvalueArray = AndroidJNIHelper.CreateJNIArgArray(args);
            try
            {
                using (AndroidJavaClass class2 = new AndroidJavaClass("com.google.games.bridge.TokenFragment"))
                {
                    using (AndroidJavaObject obj2 = GetActivity())
                    {
                        IntPtr methodID = AndroidJNI.GetStaticMethodID(class2.GetRawClass(), "getAnotherAuthCode", "(Landroid/app/Activity;ZLjava/lang/String;)Lcom/google/android/gms/common/api/PendingResult;");
                        jvalueArray[0].l = obj2.GetRawObject();
                        jvalueArray[1].z = reAuthenticateIfNeeded;
                        jvalueArray[2].l = AndroidJNI.NewStringUTF(this.webClientId);
                        new PendingResult<TokenResult>(AndroidJNI.CallStaticObjectMethod(class2.GetRawClass(), methodID, jvalueArray)).setResultCallback(new TokenResultCallback(new Action<int, string, string, string>(storey.<>m__0)));
                    }
                }
            }
            catch (Exception exception)
            {
                Logger.e("Exception launching auth code request: " + exception.Message);
                Logger.e(exception.ToString());
            }
            finally
            {
                AndroidJNIHelper.DeleteJNIArgArray(args, jvalueArray);
            }
        }

        public string GetAuthCode() => 
            this.authCode;

        public string GetEmail() => 
            this.email;

        public string GetIdToken() => 
            this.idToken;

        public void SetAccountName(string accountName)
        {
            this.accountName = accountName;
        }

        public void SetHidePopups(bool flag)
        {
            this.hidePopups = flag;
        }

        public void SetRequestAuthCode(bool flag, bool forceRefresh)
        {
            this.requestAuthCode = flag;
            this.forceRefresh = forceRefresh;
        }

        public void SetRequestEmail(bool flag)
        {
            this.requestEmail = flag;
        }

        public void SetRequestIdToken(bool flag)
        {
            this.requestIdToken = flag;
        }

        public void SetWebClientId(string webClientId)
        {
            this.webClientId = webClientId;
        }

        public void Signout()
        {
            this.authCode = null;
            this.email = null;
            this.idToken = null;
            if (<>f__am$cache0 == null)
            {
                <>f__am$cache0 = delegate {
                    Debug.Log("Calling Signout in token client");
                    AndroidJavaClass class2 = new AndroidJavaClass("com.google.games.bridge.TokenFragment");
                    object[] args = new object[] { GetActivity() };
                    class2.CallStatic("signOut", args);
                };
            }
            PlayGamesHelperObject.RunOnGameThread(<>f__am$cache0);
        }

        [CompilerGenerated]
        private sealed class <DoFetchToken>c__AnonStorey1
        {
            internal Action<int> callback;
            internal AndroidTokenClient $this;

            internal void <>m__0(int rc, string authCode, string email, string idToken)
            {
                this.$this.authCode = authCode;
                this.$this.email = email;
                this.$this.idToken = idToken;
                this.callback(rc);
            }
        }

        [CompilerGenerated]
        private sealed class <FetchTokens>c__AnonStorey0
        {
            internal bool silent;
            internal Action<int> callback;
            internal AndroidTokenClient $this;

            internal void <>m__0()
            {
                this.$this.DoFetchToken(this.silent, this.callback);
            }
        }

        [CompilerGenerated]
        private sealed class <GetAnotherServerAuthCode>c__AnonStorey2
        {
            internal Action<string> callback;
            internal AndroidTokenClient $this;

            internal void <>m__0(int rc, string authCode, string email, string idToken)
            {
                this.$this.authCode = authCode;
                this.callback(authCode);
            }
        }
    }
}

