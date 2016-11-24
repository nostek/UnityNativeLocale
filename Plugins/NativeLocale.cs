using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class NativeLocale
{
	const string DefaultLanguage = "en";
	const string DefaultCountryCode = "en";

	#if !UNITY_EDITOR && UNITY_IOS
	[DllImport("__Internal")]
	static extern string _CNativeLocaleGetLanguage();

	[DllImport("__Internal")]
	static extern string _CNativeLocaleGetCountryCode();
	#endif

	public static string GetLanguage()
	{
		#if UNITY_EDITOR
		return DefaultLanguage;
		#elif UNITY_ANDROID
		using (AndroidJavaClass cls = new AndroidJavaClass("java.util.Locale"))
		{
			using (AndroidJavaObject locale = cls.CallStatic<AndroidJavaObject>("getDefault"))
			{
				string lang = locale.Call<string>("getLanguage");
				
				if (string.IsNullOrEmpty(lang))
					return DefaultLanguage;

				if (lang.Length > 2)
					lang = lang.Substring(0, 2);

				return lang.ToLower();
			}
		}
		#elif UNITY_IOS
		string lang = _CNativeLocaleGetLanguage();

		if (string.IsNullOrEmpty(lang))
			return DefaultLanguage;

		if (lang.Length > 2)
			lang = lang.Substring(0, 2);

		return lang.ToLower();
		#else
			return DefaultLanguage;
		#endif
	}

	public static string GetCountryCode()
	{
		#if UNITY_EDITOR
		return DefaultCountryCode;
		#elif UNITY_ANDROID
		using (AndroidJavaClass cls = new AndroidJavaClass("java.util.Locale"))
		{
			using (AndroidJavaObject locale = cls.CallStatic<AndroidJavaObject>("getDefault"))
			{
				string cc = locale.Call<string>("getCountry");

				if (string.IsNullOrEmpty(cc))
					return DefaultCountryCode;

				if (cc.Length > 2)
					cc = cc.Substring(0, 2);

				return cc.ToLower();
			}
		}
		#elif UNITY_IOS
		string cc = _CNativeLocaleGetCountryCode();

		if (string.IsNullOrEmpty(cc))
			return DefaultCountryCode;

		if (cc.Length > 2)
			cc = cc.Substring(0, 2);

		return cc.ToLower();
		#else
		return DefaultCountryCode;
		#endif
	}
}
