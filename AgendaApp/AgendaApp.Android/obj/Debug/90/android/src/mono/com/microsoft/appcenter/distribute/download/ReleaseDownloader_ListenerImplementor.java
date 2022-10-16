package mono.com.microsoft.appcenter.distribute.download;


public class ReleaseDownloader_ListenerImplementor
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.microsoft.appcenter.distribute.download.ReleaseDownloader.Listener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onComplete:(JJ)V:GetOnComplete_JJHandler:Com.Microsoft.Appcenter.Distribute.Download.IReleaseDownloaderListenerInvoker, Microsoft.AppCenter.Distribute.Android.Bindings\n" +
			"n_onError:(Ljava/lang/String;)V:GetOnError_Ljava_lang_String_Handler:Com.Microsoft.Appcenter.Distribute.Download.IReleaseDownloaderListenerInvoker, Microsoft.AppCenter.Distribute.Android.Bindings\n" +
			"n_onProgress:(JJ)Z:GetOnProgress_JJHandler:Com.Microsoft.Appcenter.Distribute.Download.IReleaseDownloaderListenerInvoker, Microsoft.AppCenter.Distribute.Android.Bindings\n" +
			"n_onStart:(J)V:GetOnStart_JHandler:Com.Microsoft.Appcenter.Distribute.Download.IReleaseDownloaderListenerInvoker, Microsoft.AppCenter.Distribute.Android.Bindings\n" +
			"";
		mono.android.Runtime.register ("Com.Microsoft.Appcenter.Distribute.Download.IReleaseDownloaderListenerImplementor, Microsoft.AppCenter.Distribute.Android.Bindings", ReleaseDownloader_ListenerImplementor.class, __md_methods);
	}


	public ReleaseDownloader_ListenerImplementor ()
	{
		super ();
		if (getClass () == ReleaseDownloader_ListenerImplementor.class)
			mono.android.TypeManager.Activate ("Com.Microsoft.Appcenter.Distribute.Download.IReleaseDownloaderListenerImplementor, Microsoft.AppCenter.Distribute.Android.Bindings", "", this, new java.lang.Object[] {  });
	}


	public void onComplete (long p0, long p1)
	{
		n_onComplete (p0, p1);
	}

	private native void n_onComplete (long p0, long p1);


	public void onError (java.lang.String p0)
	{
		n_onError (p0);
	}

	private native void n_onError (java.lang.String p0);


	public boolean onProgress (long p0, long p1)
	{
		return n_onProgress (p0, p1);
	}

	private native boolean n_onProgress (long p0, long p1);


	public void onStart (long p0)
	{
		n_onStart (p0);
	}

	private native void n_onStart (long p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
