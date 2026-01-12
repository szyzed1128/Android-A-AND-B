.class public Lcom/google/android/gms/maps/MapView;
.super Landroid/widget/FrameLayout;


# annotations
.annotation system Ldalvik/annotation/MemberClasses;
    value = {
        Lcom/google/android/gms/maps/MapView$zzb;,
        Lcom/google/android/gms/maps/MapView$zza;
    }
.end annotation


# instance fields
.field private final zzbg:Lcom/google/android/gms/maps/MapView$zzb;


# direct methods
.method public constructor <init>(Landroid/content/Context;)V
    .locals 2

    .line 1
    invoke-direct {p0, p1}, Landroid/widget/FrameLayout;-><init>(Landroid/content/Context;)V

    .line 2
    new-instance v0, Lcom/google/android/gms/maps/MapView$zzb;

    const/4 v1, 0x0

    invoke-direct {v0, p0, p1, v1}, Lcom/google/android/gms/maps/MapView$zzb;-><init>(Landroid/view/ViewGroup;Landroid/content/Context;Lcom/google/android/gms/maps/GoogleMapOptions;)V

    iput-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    .line 3
    nop

    .line 4
    const/4 p1, 0x1

    invoke-virtual {p0, p1}, Lcom/google/android/gms/maps/MapView;->setClickable(Z)V

    .line 5
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;)V
    .locals 1

    .line 6
    invoke-direct {p0, p1, p2}, Landroid/widget/FrameLayout;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;)V

    .line 7
    new-instance v0, Lcom/google/android/gms/maps/MapView$zzb;

    .line 8
    invoke-static {p1, p2}, Lcom/google/android/gms/maps/GoogleMapOptions;->createFromAttributes(Landroid/content/Context;Landroid/util/AttributeSet;)Lcom/google/android/gms/maps/GoogleMapOptions;

    move-result-object p2

    invoke-direct {v0, p0, p1, p2}, Lcom/google/android/gms/maps/MapView$zzb;-><init>(Landroid/view/ViewGroup;Landroid/content/Context;Lcom/google/android/gms/maps/GoogleMapOptions;)V

    iput-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    .line 9
    nop

    .line 10
    const/4 p1, 0x1

    invoke-virtual {p0, p1}, Lcom/google/android/gms/maps/MapView;->setClickable(Z)V

    .line 11
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;I)V
    .locals 0

    .line 12
    invoke-direct {p0, p1, p2, p3}, Landroid/widget/FrameLayout;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;I)V

    .line 13
    new-instance p3, Lcom/google/android/gms/maps/MapView$zzb;

    .line 14
    invoke-static {p1, p2}, Lcom/google/android/gms/maps/GoogleMapOptions;->createFromAttributes(Landroid/content/Context;Landroid/util/AttributeSet;)Lcom/google/android/gms/maps/GoogleMapOptions;

    move-result-object p2

    invoke-direct {p3, p0, p1, p2}, Lcom/google/android/gms/maps/MapView$zzb;-><init>(Landroid/view/ViewGroup;Landroid/content/Context;Lcom/google/android/gms/maps/GoogleMapOptions;)V

    iput-object p3, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    .line 15
    nop

    .line 16
    const/4 p1, 0x1

    invoke-virtual {p0, p1}, Lcom/google/android/gms/maps/MapView;->setClickable(Z)V

    .line 17
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Lcom/google/android/gms/maps/GoogleMapOptions;)V
    .locals 1

    .line 18
    invoke-direct {p0, p1}, Landroid/widget/FrameLayout;-><init>(Landroid/content/Context;)V

    .line 19
    new-instance v0, Lcom/google/android/gms/maps/MapView$zzb;

    invoke-direct {v0, p0, p1, p2}, Lcom/google/android/gms/maps/MapView$zzb;-><init>(Landroid/view/ViewGroup;Landroid/content/Context;Lcom/google/android/gms/maps/GoogleMapOptions;)V

    iput-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    .line 20
    nop

    .line 21
    const/4 p1, 0x1

    invoke-virtual {p0, p1}, Lcom/google/android/gms/maps/MapView;->setClickable(Z)V

    .line 22
    return-void
.end method


# virtual methods
.method public getMapAsync(Lcom/google/android/gms/maps/OnMapReadyCallback;)V
    .locals 1

    .line 46
    const-string v0, "getMapAsync() must be called on the main thread"

    invoke-static {v0}, Lcom/google/android/gms/common/internal/Preconditions;->checkMainThread(Ljava/lang/String;)V

    .line 47
    iget-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    invoke-virtual {v0, p1}, Lcom/google/android/gms/maps/MapView$zzb;->getMapAsync(Lcom/google/android/gms/maps/OnMapReadyCallback;)V

    .line 48
    return-void
.end method

.method public final onCreate(Landroid/os/Bundle;)V
    .locals 2

    .line 23
    invoke-static {}, Landroid/os/StrictMode;->getThreadPolicy()Landroid/os/StrictMode$ThreadPolicy;

    move-result-object v0

    .line 24
    new-instance v1, Landroid/os/StrictMode$ThreadPolicy$Builder;

    invoke-direct {v1, v0}, Landroid/os/StrictMode$ThreadPolicy$Builder;-><init>(Landroid/os/StrictMode$ThreadPolicy;)V

    invoke-virtual {v1}, Landroid/os/StrictMode$ThreadPolicy$Builder;->permitAll()Landroid/os/StrictMode$ThreadPolicy$Builder;

    move-result-object v1

    invoke-virtual {v1}, Landroid/os/StrictMode$ThreadPolicy$Builder;->build()Landroid/os/StrictMode$ThreadPolicy;

    move-result-object v1

    .line 25
    invoke-static {v1}, Landroid/os/StrictMode;->setThreadPolicy(Landroid/os/StrictMode$ThreadPolicy;)V

    .line 26
    :try_start_0
    iget-object v1, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    invoke-virtual {v1, p1}, Lcom/google/android/gms/maps/MapView$zzb;->onCreate(Landroid/os/Bundle;)V

    .line 27
    iget-object p1, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    invoke-virtual {p1}, Lcom/google/android/gms/maps/MapView$zzb;->getDelegate()Lcom/google/android/gms/dynamic/LifecycleDelegate;

    move-result-object p1

    if-nez p1, :cond_0

    .line 28
    invoke-static {p0}, Lcom/google/android/gms/dynamic/DeferredLifecycleHelper;->showGooglePlayUnavailableMessage(Landroid/widget/FrameLayout;)V
    :try_end_0
    .catchall {:try_start_0 .. :try_end_0} :catchall_0

    .line 29
    :cond_0
    invoke-static {v0}, Landroid/os/StrictMode;->setThreadPolicy(Landroid/os/StrictMode$ThreadPolicy;)V

    .line 30
    return-void

    .line 31
    :catchall_0
    move-exception p1

    invoke-static {v0}, Landroid/os/StrictMode;->setThreadPolicy(Landroid/os/StrictMode$ThreadPolicy;)V

    throw p1
.end method

.method public final onDestroy()V
    .locals 1

    .line 40
    iget-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    invoke-virtual {v0}, Lcom/google/android/gms/maps/MapView$zzb;->onDestroy()V

    .line 41
    return-void
.end method

.method public final onEnterAmbient(Landroid/os/Bundle;)V
    .locals 2

    .line 49
    const-string v0, "onEnterAmbient() must be called on the main thread"

    invoke-static {v0}, Lcom/google/android/gms/common/internal/Preconditions;->checkMainThread(Ljava/lang/String;)V

    .line 50
    iget-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    .line 51
    invoke-virtual {v0}, Lcom/google/android/gms/maps/MapView$zzb;->getDelegate()Lcom/google/android/gms/dynamic/LifecycleDelegate;

    move-result-object v1

    if-eqz v1, :cond_0

    .line 52
    invoke-virtual {v0}, Lcom/google/android/gms/maps/MapView$zzb;->getDelegate()Lcom/google/android/gms/dynamic/LifecycleDelegate;

    move-result-object v0

    check-cast v0, Lcom/google/android/gms/maps/MapView$zza;

    invoke-virtual {v0, p1}, Lcom/google/android/gms/maps/MapView$zza;->onEnterAmbient(Landroid/os/Bundle;)V

    .line 53
    :cond_0
    return-void
.end method

.method public final onExitAmbient()V
    .locals 2

    .line 54
    const-string v0, "onExitAmbient() must be called on the main thread"

    invoke-static {v0}, Lcom/google/android/gms/common/internal/Preconditions;->checkMainThread(Ljava/lang/String;)V

    .line 55
    iget-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    .line 56
    invoke-virtual {v0}, Lcom/google/android/gms/maps/MapView$zzb;->getDelegate()Lcom/google/android/gms/dynamic/LifecycleDelegate;

    move-result-object v1

    if-eqz v1, :cond_0

    .line 57
    invoke-virtual {v0}, Lcom/google/android/gms/maps/MapView$zzb;->getDelegate()Lcom/google/android/gms/dynamic/LifecycleDelegate;

    move-result-object v0

    check-cast v0, Lcom/google/android/gms/maps/MapView$zza;

    invoke-virtual {v0}, Lcom/google/android/gms/maps/MapView$zza;->onExitAmbient()V

    .line 58
    :cond_0
    return-void
.end method

.method public final onLowMemory()V
    .locals 1

    .line 42
    iget-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    invoke-virtual {v0}, Lcom/google/android/gms/maps/MapView$zzb;->onLowMemory()V

    .line 43
    return-void
.end method

.method public final onPause()V
    .locals 1

    .line 34
    iget-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    invoke-virtual {v0}, Lcom/google/android/gms/maps/MapView$zzb;->onPause()V

    .line 35
    return-void
.end method

.method public final onResume()V
    .locals 1

    .line 32
    iget-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    invoke-virtual {v0}, Lcom/google/android/gms/maps/MapView$zzb;->onResume()V

    .line 33
    return-void
.end method

.method public final onSaveInstanceState(Landroid/os/Bundle;)V
    .locals 1

    .line 44
    iget-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    invoke-virtual {v0, p1}, Lcom/google/android/gms/maps/MapView$zzb;->onSaveInstanceState(Landroid/os/Bundle;)V

    .line 45
    return-void
.end method

.method public final onStart()V
    .locals 1

    .line 36
    iget-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    invoke-virtual {v0}, Lcom/google/android/gms/maps/MapView$zzb;->onStart()V

    .line 37
    return-void
.end method

.method public final onStop()V
    .locals 1

    .line 38
    iget-object v0, p0, Lcom/google/android/gms/maps/MapView;->zzbg:Lcom/google/android/gms/maps/MapView$zzb;

    invoke-virtual {v0}, Lcom/google/android/gms/maps/MapView$zzb;->onStop()V

    .line 39
    return-void
.end method
