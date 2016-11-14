using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public static class NativeLocale
{
	const string Default = "en";

	#if !UNITY_EDITOR && UNITY_IOS
	[DllImport("__Internal")]
	static extern string _CNativeLocaleGetLocale();
	#endif

	public static string GetLocale()
	{
		#if UNITY_EDITOR
		return Default;
		#elif UNITY_ANDROID
		using (AndroidJavaClass cls = new AndroidJavaClass("java.util.Locale"))
		{
			using (AndroidJavaObject locale = cls.CallStatic<AndroidJavaObject>("getDefault"))
			{
				string lang = locale.Call<string>("getLanguage");
				
				if (string.IsNullOrEmpty(lang))
					return Default;

				return lang;
			}
		}
		#elif UNITY_IOS
		string lang = _CNativeLocaleGetLocale();

		if (string.IsNullOrEmpty(lang))
			return Default;

		return lang;
		#else
			return Default;
		#endif
	}
}