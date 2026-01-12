.class public Lcrc64515ee83f00766c60/PlatformTouchEffect_AccessibilityListener;
.super Ljava/lang/Object;
.source "PlatformTouchEffect_AccessibilityListener.java"

# interfaces
.implements Lmono/android/IGCUserPeer;
.implements Landroid/view/accessibility/AccessibilityManager$AccessibilityStateChangeListener;
.implements Landroid/view/accessibility/AccessibilityManager$TouchExplorationStateChangeListener;


# static fields
.field public static final __md_methods:Ljava/lang/String;


# instance fields
.field private refList:Ljava/util/ArrayList;


# direct methods
.method static constructor <clinit>()V
    .locals 3

    .line 14
    const-string v0, "n_onAccessibilityStateChanged:(Z)V:GetOnAccessibilityStateChanged_ZHandler:Android.Views.Accessibility.AccessibilityManager/IAccessibilityStateChangeListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\nn_onTouchExplorationStateChanged:(Z)V:GetOnTouchExplorationStateChanged_ZHandler:Android.Views.Accessibility.AccessibilityManager/ITouchExplorationStateChangeListenerInvoker, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null\n"

    sput-object v0, Lcrc64515ee83f00766c60/PlatformTouchEffect_AccessibilityListener;->__md_methods:Ljava/lang/String;

    .line 18
    const-class v1, Lcrc64515ee83f00766c60/PlatformTouchEffect_AccessibilityListener;

    const-string v2, "Xamarin.CommunityToolkit.Android.Effects.PlatformTouchEffect+AccessibilityListener, Xamarin.CommunityToolkit"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 19
    return-void
.end method

.method public constructor <init>()V
    .locals 3

    .line 24
    invoke-direct {p0}, Ljava/lang/Object;-><init>()V

    .line 25
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc64515ee83f00766c60/PlatformTouchEffect_AccessibilityListener;

    if-ne v0, v1, :cond_0

    .line 26
    const/4 v0, 0x0

    new-array v0, v0, [Ljava/lang/Object;

    const-string v1, "Xamarin.CommunityToolkit.Android.Effects.PlatformTouchEffect+AccessibilityListener, Xamarin.CommunityToolkit"

    const-string v2, ""

    invoke-static {v1, v2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 28
    :cond_0
    return-void
.end method

.method private native n_onAccessibilityStateChanged(Z)V
.end method

.method private native n_onTouchExplorationStateChanged(Z)V
.end method


# virtual methods
.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 49
    iget-object v0, p0, Lcrc64515ee83f00766c60/PlatformTouchEffect_AccessibilityListener;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 50
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc64515ee83f00766c60/PlatformTouchEffect_AccessibilityListener;->refList:Ljava/util/ArrayList;

    .line 51
    :cond_0
    iget-object v0, p0, Lcrc64515ee83f00766c60/PlatformTouchEffect_AccessibilityListener;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 52
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 56
    iget-object v0, p0, Lcrc64515ee83f00766c60/PlatformTouchEffect_AccessibilityListener;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 57
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 58
    :cond_0
    return-void
.end method

.method public onAccessibilityStateChanged(Z)V
    .locals 0

    .line 33
    invoke-direct {p0, p1}, Lcrc64515ee83f00766c60/PlatformTouchEffect_AccessibilityListener;->n_onAccessibilityStateChanged(Z)V

    .line 34
    return-void
.end method

.method public onTouchExplorationStateChanged(Z)V
    .locals 0

    .line 41
    invoke-direct {p0, p1}, Lcrc64515ee83f00766c60/PlatformTouchEffect_AccessibilityListener;->n_onTouchExplorationStateChanged(Z)V

    .line 42
    return-void
.end method
