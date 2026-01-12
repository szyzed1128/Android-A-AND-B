.class public Lcrc648aad9efe354a1d8f/MapRenderer;
.super Lcrc643f46942d9dd1fff9/ViewRenderer_2;
.source "MapRenderer.java"

# interfaces
.implements Lmono/android/IGCUserPeer;
.implements Lcom/google/android/gms/maps/GoogleMap$OnCameraMoveListener;
.implements Lcom/google/android/gms/maps/OnMapReadyCallback;


# static fields
.field public static final __md_methods:Ljava/lang/String;


# instance fields
.field private refList:Ljava/util/ArrayList;


# direct methods
.method static constructor <clinit>()V
    .locals 3

    .line 14
    const-string v0, "n_onLayout:(ZIIII)V:GetOnLayout_ZIIIIHandler\nn_onCameraMove:()V:GetOnCameraMoveHandler:Android.Gms.Maps.GoogleMap/IOnCameraMoveListenerInvoker, Xamarin.GooglePlayServices.Maps\nn_onMapReady:(Lcom/google/android/gms/maps/GoogleMap;)V:GetOnMapReady_Lcom_google_android_gms_maps_GoogleMap_Handler:Android.Gms.Maps.IOnMapReadyCallbackInvoker, Xamarin.GooglePlayServices.Maps\n"

    sput-object v0, Lcrc648aad9efe354a1d8f/MapRenderer;->__md_methods:Ljava/lang/String;

    .line 19
    const-class v1, Lcrc648aad9efe354a1d8f/MapRenderer;

    const-string v2, "Xamarin.Forms.Maps.Android.MapRenderer, Xamarin.Forms.Maps.Android"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 20
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;)V
    .locals 2

    .line 25
    invoke-direct {p0, p1}, Lcrc643f46942d9dd1fff9/ViewRenderer_2;-><init>(Landroid/content/Context;)V

    .line 26
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc648aad9efe354a1d8f/MapRenderer;

    if-ne v0, v1, :cond_0

    .line 27
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "Xamarin.Forms.Maps.Android.MapRenderer, Xamarin.Forms.Maps.Android"

    const-string v1, "Android.Content.Context, Mono.Android"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 29
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;)V
    .locals 2

    .line 34
    invoke-direct {p0, p1, p2}, Lcrc643f46942d9dd1fff9/ViewRenderer_2;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;)V

    .line 35
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc648aad9efe354a1d8f/MapRenderer;

    if-ne v0, v1, :cond_0

    .line 36
    const/4 v0, 0x2

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const-string p1, "Xamarin.Forms.Maps.Android.MapRenderer, Xamarin.Forms.Maps.Android"

    const-string p2, "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 38
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;I)V
    .locals 2

    .line 43
    invoke-direct {p0, p1, p2, p3}, Lcrc643f46942d9dd1fff9/ViewRenderer_2;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;I)V

    .line 44
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc648aad9efe354a1d8f/MapRenderer;

    if-ne v0, v1, :cond_0

    .line 45
    const/4 v0, 0x3

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const/4 p1, 0x2

    invoke-static {p3}, Ljava/lang/Integer;->valueOf(I)Ljava/lang/Integer;

    move-result-object p2

    aput-object p2, v0, p1

    const-string p1, "Xamarin.Forms.Maps.Android.MapRenderer, Xamarin.Forms.Maps.Android"

    const-string p2, "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 47
    :cond_0
    return-void
.end method

.method private native n_onCameraMove()V
.end method

.method private native n_onLayout(ZIIII)V
.end method

.method private native n_onMapReady(Lcom/google/android/gms/maps/GoogleMap;)V
.end method


# virtual methods
.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 76
    iget-object v0, p0, Lcrc648aad9efe354a1d8f/MapRenderer;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 77
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc648aad9efe354a1d8f/MapRenderer;->refList:Ljava/util/ArrayList;

    .line 78
    :cond_0
    iget-object v0, p0, Lcrc648aad9efe354a1d8f/MapRenderer;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 79
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 83
    iget-object v0, p0, Lcrc648aad9efe354a1d8f/MapRenderer;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 84
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 85
    :cond_0
    return-void
.end method

.method public onCameraMove()V
    .locals 0

    .line 60
    invoke-direct {p0}, Lcrc648aad9efe354a1d8f/MapRenderer;->n_onCameraMove()V

    .line 61
    return-void
.end method

.method public onLayout(ZIIII)V
    .locals 0

    .line 52
    invoke-direct/range {p0 .. p5}, Lcrc648aad9efe354a1d8f/MapRenderer;->n_onLayout(ZIIII)V

    .line 53
    return-void
.end method

.method public onMapReady(Lcom/google/android/gms/maps/GoogleMap;)V
    .locals 0

    .line 68
    invoke-direct {p0, p1}, Lcrc648aad9efe354a1d8f/MapRenderer;->n_onMapReady(Lcom/google/android/gms/maps/GoogleMap;)V

    .line 69
    return-void
.end method
