.class public Lcrc643b2cfc53ba3be92f/MainApplication;
.super Landroid/app/Application;
.source "MainApplication.java"

# interfaces
.implements Lmono/android/IGCUserPeer;
.implements Landroid/app/Application$ActivityLifecycleCallbacks;


# static fields
.field public static final __md_methods:Ljava/lang/String;


# instance fields
.field private refList:Ljava/util/ArrayList;


# direct methods
.method static constructor <clinit>()V
    .locals 1

    .line 13
    const-string v0, "n_onCreate:()V:GetOnCreateHandler\nn_onTerminate:()V:GetOnTerminateHandler\nn_onActivityCreated:(Landroid/app/Activity;Landroid/os/Bundle;)V:GetOnActivityCreated_Landroid_app_Activity_Landroid_os_Bundle_Handler:Android.App.Application/IActivityLifecycleCallbacksInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityDestroyed:(Landroid/app/Activity;)V:GetOnActivityDestroyed_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacksInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPaused:(Landroid/app/Activity;)V:GetOnActivityPaused_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacksInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityResumed:(Landroid/app/Activity;)V:GetOnActivityResumed_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacksInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivitySaveInstanceState:(Landroid/app/Activity;Landroid/os/Bundle;)V:GetOnActivitySaveInstanceState_Landroid_app_Activity_Landroid_os_Bundle_Handler:Android.App.Application/IActivityLifecycleCallbacksInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityStarted:(Landroid/app/Activity;)V:GetOnActivityStarted_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacksInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityStopped:(Landroid/app/Activity;)V:GetOnActivityStopped_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacksInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPostCreated:(Landroid/app/Activity;Landroid/os/Bundle;)V:GetOnActivityPostCreated_Landroid_app_Activity_Landroid_os_Bundle_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPostDestroyed:(Landroid/app/Activity;)V:GetOnActivityPostDestroyed_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPostPaused:(Landroid/app/Activity;)V:GetOnActivityPostPaused_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPostResumed:(Landroid/app/Activity;)V:GetOnActivityPostResumed_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPostSaveInstanceState:(Landroid/app/Activity;Landroid/os/Bundle;)V:GetOnActivityPostSaveInstanceState_Landroid_app_Activity_Landroid_os_Bundle_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPostStarted:(Landroid/app/Activity;)V:GetOnActivityPostStarted_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPostStopped:(Landroid/app/Activity;)V:GetOnActivityPostStopped_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPreCreated:(Landroid/app/Activity;Landroid/os/Bundle;)V:GetOnActivityPreCreated_Landroid_app_Activity_Landroid_os_Bundle_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPreDestroyed:(Landroid/app/Activity;)V:GetOnActivityPreDestroyed_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPrePaused:(Landroid/app/Activity;)V:GetOnActivityPrePaused_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPreResumed:(Landroid/app/Activity;)V:GetOnActivityPreResumed_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPreSaveInstanceState:(Landroid/app/Activity;Landroid/os/Bundle;)V:GetOnActivityPreSaveInstanceState_Landroid_app_Activity_Landroid_os_Bundle_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPreStarted:(Landroid/app/Activity;)V:GetOnActivityPreStarted_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onActivityPreStopped:(Landroid/app/Activity;)V:GetOnActivityPreStopped_Landroid_app_Activity_Handler:Android.App.Application/IActivityLifecycleCallbacks, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n"

    sput-object v0, Lcrc643b2cfc53ba3be92f/MainApplication;->__md_methods:Ljava/lang/String;

    .line 38
    return-void
.end method

.method public constructor <init>()V
    .locals 0

    .line 41
    invoke-direct {p0}, Landroid/app/Application;-><init>()V

    .line 42
    invoke-static {p0}, Lmono/MonoPackageManager;->setContext(Landroid/content/Context;)V

    .line 43
    return-void
.end method

.method private native n_onActivityCreated(Landroid/app/Activity;Landroid/os/Bundle;)V
.end method

.method private native n_onActivityDestroyed(Landroid/app/Activity;)V
.end method

.method private native n_onActivityPaused(Landroid/app/Activity;)V
.end method

.method private native n_onActivityPostCreated(Landroid/app/Activity;Landroid/os/Bundle;)V
.end method

.method private native n_onActivityPostDestroyed(Landroid/app/Activity;)V
.end method

.method private native n_onActivityPostPaused(Landroid/app/Activity;)V
.end method

.method private native n_onActivityPostResumed(Landroid/app/Activity;)V
.end method

.method private native n_onActivityPostSaveInstanceState(Landroid/app/Activity;Landroid/os/Bundle;)V
.end method

.method private native n_onActivityPostStarted(Landroid/app/Activity;)V
.end method

.method private native n_onActivityPostStopped(Landroid/app/Activity;)V
.end method

.method private native n_onActivityPreCreated(Landroid/app/Activity;Landroid/os/Bundle;)V
.end method

.method private native n_onActivityPreDestroyed(Landroid/app/Activity;)V
.end method

.method private native n_onActivityPrePaused(Landroid/app/Activity;)V
.end method

.method private native n_onActivityPreResumed(Landroid/app/Activity;)V
.end method

.method private native n_onActivityPreSaveInstanceState(Landroid/app/Activity;Landroid/os/Bundle;)V
.end method

.method private native n_onActivityPreStarted(Landroid/app/Activity;)V
.end method

.method private native n_onActivityPreStopped(Landroid/app/Activity;)V
.end method

.method private native n_onActivityResumed(Landroid/app/Activity;)V
.end method

.method private native n_onActivitySaveInstanceState(Landroid/app/Activity;Landroid/os/Bundle;)V
.end method

.method private native n_onActivityStarted(Landroid/app/Activity;)V
.end method

.method private native n_onActivityStopped(Landroid/app/Activity;)V
.end method

.method private native n_onCreate()V
.end method

.method private native n_onTerminate()V
.end method


# virtual methods
.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 232
    iget-object v0, p0, Lcrc643b2cfc53ba3be92f/MainApplication;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 233
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc643b2cfc53ba3be92f/MainApplication;->refList:Ljava/util/ArrayList;

    .line 234
    :cond_0
    iget-object v0, p0, Lcrc643b2cfc53ba3be92f/MainApplication;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 235
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 239
    iget-object v0, p0, Lcrc643b2cfc53ba3be92f/MainApplication;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 240
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 241
    :cond_0
    return-void
.end method

.method public onActivityCreated(Landroid/app/Activity;Landroid/os/Bundle;)V
    .locals 0

    .line 64
    invoke-direct {p0, p1, p2}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityCreated(Landroid/app/Activity;Landroid/os/Bundle;)V

    .line 65
    return-void
.end method

.method public onActivityDestroyed(Landroid/app/Activity;)V
    .locals 0

    .line 72
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityDestroyed(Landroid/app/Activity;)V

    .line 73
    return-void
.end method

.method public onActivityPaused(Landroid/app/Activity;)V
    .locals 0

    .line 80
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPaused(Landroid/app/Activity;)V

    .line 81
    return-void
.end method

.method public onActivityPostCreated(Landroid/app/Activity;Landroid/os/Bundle;)V
    .locals 0

    .line 120
    invoke-direct {p0, p1, p2}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPostCreated(Landroid/app/Activity;Landroid/os/Bundle;)V

    .line 121
    return-void
.end method

.method public onActivityPostDestroyed(Landroid/app/Activity;)V
    .locals 0

    .line 128
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPostDestroyed(Landroid/app/Activity;)V

    .line 129
    return-void
.end method

.method public onActivityPostPaused(Landroid/app/Activity;)V
    .locals 0

    .line 136
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPostPaused(Landroid/app/Activity;)V

    .line 137
    return-void
.end method

.method public onActivityPostResumed(Landroid/app/Activity;)V
    .locals 0

    .line 144
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPostResumed(Landroid/app/Activity;)V

    .line 145
    return-void
.end method

.method public onActivityPostSaveInstanceState(Landroid/app/Activity;Landroid/os/Bundle;)V
    .locals 0

    .line 152
    invoke-direct {p0, p1, p2}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPostSaveInstanceState(Landroid/app/Activity;Landroid/os/Bundle;)V

    .line 153
    return-void
.end method

.method public onActivityPostStarted(Landroid/app/Activity;)V
    .locals 0

    .line 160
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPostStarted(Landroid/app/Activity;)V

    .line 161
    return-void
.end method

.method public onActivityPostStopped(Landroid/app/Activity;)V
    .locals 0

    .line 168
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPostStopped(Landroid/app/Activity;)V

    .line 169
    return-void
.end method

.method public onActivityPreCreated(Landroid/app/Activity;Landroid/os/Bundle;)V
    .locals 0

    .line 176
    invoke-direct {p0, p1, p2}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPreCreated(Landroid/app/Activity;Landroid/os/Bundle;)V

    .line 177
    return-void
.end method

.method public onActivityPreDestroyed(Landroid/app/Activity;)V
    .locals 0

    .line 184
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPreDestroyed(Landroid/app/Activity;)V

    .line 185
    return-void
.end method

.method public onActivityPrePaused(Landroid/app/Activity;)V
    .locals 0

    .line 192
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPrePaused(Landroid/app/Activity;)V

    .line 193
    return-void
.end method

.method public onActivityPreResumed(Landroid/app/Activity;)V
    .locals 0

    .line 200
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPreResumed(Landroid/app/Activity;)V

    .line 201
    return-void
.end method

.method public onActivityPreSaveInstanceState(Landroid/app/Activity;Landroid/os/Bundle;)V
    .locals 0

    .line 208
    invoke-direct {p0, p1, p2}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPreSaveInstanceState(Landroid/app/Activity;Landroid/os/Bundle;)V

    .line 209
    return-void
.end method

.method public onActivityPreStarted(Landroid/app/Activity;)V
    .locals 0

    .line 216
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPreStarted(Landroid/app/Activity;)V

    .line 217
    return-void
.end method

.method public onActivityPreStopped(Landroid/app/Activity;)V
    .locals 0

    .line 224
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityPreStopped(Landroid/app/Activity;)V

    .line 225
    return-void
.end method

.method public onActivityResumed(Landroid/app/Activity;)V
    .locals 0

    .line 88
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityResumed(Landroid/app/Activity;)V

    .line 89
    return-void
.end method

.method public onActivitySaveInstanceState(Landroid/app/Activity;Landroid/os/Bundle;)V
    .locals 0

    .line 96
    invoke-direct {p0, p1, p2}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivitySaveInstanceState(Landroid/app/Activity;Landroid/os/Bundle;)V

    .line 97
    return-void
.end method

.method public onActivityStarted(Landroid/app/Activity;)V
    .locals 0

    .line 104
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityStarted(Landroid/app/Activity;)V

    .line 105
    return-void
.end method

.method public onActivityStopped(Landroid/app/Activity;)V
    .locals 0

    .line 112
    invoke-direct {p0, p1}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onActivityStopped(Landroid/app/Activity;)V

    .line 113
    return-void
.end method

.method public onCreate()V
    .locals 0

    .line 48
    invoke-direct {p0}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onCreate()V

    .line 49
    return-void
.end method

.method public onTerminate()V
    .locals 0

    .line 56
    invoke-direct {p0}, Lcrc643b2cfc53ba3be92f/MainApplication;->n_onTerminate()V

    .line 57
    return-void
.end method
