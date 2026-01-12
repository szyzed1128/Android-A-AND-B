.class public Lmono/android/speech/RecognitionListenerImplementor;
.super Ljava/lang/Object;
.source "RecognitionListenerImplementor.java"

# interfaces
.implements Lmono/android/IGCUserPeer;
.implements Landroid/speech/RecognitionListener;


# static fields
.field public static final __md_methods:Ljava/lang/String;


# instance fields
.field private refList:Ljava/util/ArrayList;


# direct methods
.method static constructor <clinit>()V
    .locals 3

    .line 13
    const-string v0, "n_onBeginningOfSpeech:()V:GetOnBeginningOfSpeechHandler:Android.Speech.IRecognitionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onBufferReceived:([B)V:GetOnBufferReceived_arrayBHandler:Android.Speech.IRecognitionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onEndOfSpeech:()V:GetOnEndOfSpeechHandler:Android.Speech.IRecognitionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onError:(I)V:GetOnError_IHandler:Android.Speech.IRecognitionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onEvent:(ILandroid/os/Bundle;)V:GetOnEvent_ILandroid_os_Bundle_Handler:Android.Speech.IRecognitionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onPartialResults:(Landroid/os/Bundle;)V:GetOnPartialResults_Landroid_os_Bundle_Handler:Android.Speech.IRecognitionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onReadyForSpeech:(Landroid/os/Bundle;)V:GetOnReadyForSpeech_Landroid_os_Bundle_Handler:Android.Speech.IRecognitionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onResults:(Landroid/os/Bundle;)V:GetOnResults_Landroid_os_Bundle_Handler:Android.Speech.IRecognitionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onRmsChanged:(F)V:GetOnRmsChanged_FHandler:Android.Speech.IRecognitionListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onEndOfSegmentedSession:()V:GetOnEndOfSegmentedSessionHandler:Android.Speech.IRecognitionListener, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onSegmentResults:(Landroid/os/Bundle;)V:GetOnSegmentResults_Landroid_os_Bundle_Handler:Android.Speech.IRecognitionListener, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n"

    sput-object v0, Lmono/android/speech/RecognitionListenerImplementor;->__md_methods:Ljava/lang/String;

    .line 26
    const-class v1, Lmono/android/speech/RecognitionListenerImplementor;

    const-string v2, "Android.Speech.IRecognitionListenerImplementor, Mono.Android"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 27
    return-void
.end method

.method public constructor <init>()V
    .locals 3

    .line 32
    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    .line 33
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lmono/android/speech/RecognitionListenerImplementor;

    if-ne v0, v1, :cond_0

    .line 34
    const/4 v0, 0x0

    new-array v0, v0, [Ljava/lang/Object;

    const-string v1, "Android.Speech.IRecognitionListenerImplementor, Mono.Android"

    const-string v2, ""

    invoke-static {v1, v2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 36
    :cond_0
    return-void
.end method

.method private native n_onBeginningOfSpeech()V
.end method

.method private native n_onBufferReceived([B)V
.end method

.method private native n_onEndOfSegmentedSession()V
.end method

.method private native n_onEndOfSpeech()V
.end method

.method private native n_onError(I)V
.end method

.method private native n_onEvent(ILandroid/os/Bundle;)V
.end method

.method private native n_onPartialResults(Landroid/os/Bundle;)V
.end method

.method private native n_onReadyForSpeech(Landroid/os/Bundle;)V
.end method

.method private native n_onResults(Landroid/os/Bundle;)V
.end method

.method private native n_onRmsChanged(F)V
.end method

.method private native n_onSegmentResults(Landroid/os/Bundle;)V
.end method


# virtual methods
.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 129
    iget-object v0, p0, Lmono/android/speech/RecognitionListenerImplementor;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 130
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lmono/android/speech/RecognitionListenerImplementor;->refList:Ljava/util/ArrayList;

    .line 131
    :cond_0
    iget-object v0, p0, Lmono/android/speech/RecognitionListenerImplementor;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 132
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 136
    iget-object v0, p0, Lmono/android/speech/RecognitionListenerImplementor;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 137
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 138
    :cond_0
    return-void
.end method

.method public onBeginningOfSpeech()V
    .locals 0

    .line 41
    invoke-direct {p0}, Lmono/android/speech/RecognitionListenerImplementor;->n_onBeginningOfSpeech()V

    .line 42
    return-void
.end method

.method public onBufferReceived([B)V
    .locals 0

    .line 49
    invoke-direct {p0, p1}, Lmono/android/speech/RecognitionListenerImplementor;->n_onBufferReceived([B)V

    .line 50
    return-void
.end method

.method public onEndOfSegmentedSession()V
    .locals 0

    .line 113
    invoke-direct {p0}, Lmono/android/speech/RecognitionListenerImplementor;->n_onEndOfSegmentedSession()V

    .line 114
    return-void
.end method

.method public onEndOfSpeech()V
    .locals 0

    .line 57
    invoke-direct {p0}, Lmono/android/speech/RecognitionListenerImplementor;->n_onEndOfSpeech()V

    .line 58
    return-void
.end method

.method public onError(I)V
    .locals 0

    .line 65
    invoke-direct {p0, p1}, Lmono/android/speech/RecognitionListenerImplementor;->n_onError(I)V

    .line 66
    return-void
.end method

.method public onEvent(ILandroid/os/Bundle;)V
    .locals 0

    .line 73
    invoke-direct {p0, p1, p2}, Lmono/android/speech/RecognitionListenerImplementor;->n_onEvent(ILandroid/os/Bundle;)V

    .line 74
    return-void
.end method

.method public onPartialResults(Landroid/os/Bundle;)V
    .locals 0

    .line 81
    invoke-direct {p0, p1}, Lmono/android/speech/RecognitionListenerImplementor;->n_onPartialResults(Landroid/os/Bundle;)V

    .line 82
    return-void
.end method

.method public onReadyForSpeech(Landroid/os/Bundle;)V
    .locals 0

    .line 89
    invoke-direct {p0, p1}, Lmono/android/speech/RecognitionListenerImplementor;->n_onReadyForSpeech(Landroid/os/Bundle;)V

    .line 90
    return-void
.end method

.method public onResults(Landroid/os/Bundle;)V
    .locals 0

    .line 97
    invoke-direct {p0, p1}, Lmono/android/speech/RecognitionListenerImplementor;->n_onResults(Landroid/os/Bundle;)V

    .line 98
    return-void
.end method

.method public onRmsChanged(F)V
    .locals 0

    .line 105
    invoke-direct {p0, p1}, Lmono/android/speech/RecognitionListenerImplementor;->n_onRmsChanged(F)V

    .line 106
    return-void
.end method

.method public onSegmentResults(Landroid/os/Bundle;)V
    .locals 0

    .line 121
    invoke-direct {p0, p1}, Lmono/android/speech/RecognitionListenerImplementor;->n_onSegmentResults(Landroid/os/Bundle;)V

    .line 122
    return-void
.end method
