using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class NativeLocale
{
	const string DefaultLanguageShort = "en";
	const string DefaultLanguageFull = "en-US";
	const string DefaultCountryCode = "en";

	#if !UNITY_EDITOR && UNITY_IOS
	[DllImport("__Internal")]
	static extern string _CNativeLocaleGetLanguage();

	[DllImport("__Internal")]
	static extern string _CNativeLocaleGetCountryCode();
	#endif

	static string DefaultLanguage(bool returnFull)
	{
		if (returnFull)
			return DefaultLanguageFull;
		return DefaultLanguageShort;
	}

	public static string GetLanguage()
	{
		return GetLanguage(false);
	}

	public static string GetLanguage(bool returnFullLanguage)
	{
		#if UNITY_EDITOR
		return DefaultLanguage(returnFullLanguage);
		#elif UNITY_ANDROID
		using (AndroidJavaClass cls = new AndroidJavaClass("java.util.Locale"))
		{
		using (AndroidJavaObject locale = cls.CallStatic<AndroidJavaObject>("getDefault"))
		{
		string lang = locale.Call<string>("getLanguage");

		if (string.IsNullOrEmpty(lang))
		return DefaultLanguage(returnFullLanguage);

		if (!returnFullLanguage && lang.Length > 2)
		lang = lang.Substring(0, 2);

		return lang.ToLower();
		}
		}
		#elif UNITY_IOS
		string lang = _CNativeLocaleGetLanguage();

		if (string.IsNullOrEmpty(lang))
		return DefaultLanguage(returnFullLanguage);

		if (!returnFullLanguage && lang.Length > 2)
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
