.class public Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer_GestureListener;
.super Landroid/view/GestureDetector$SimpleOnGestureListener;
.source "InputLayoutToggleViewRenderer_GestureListener.java"

# interfaces
.implements Lmono/android/IGCUserPeer;


# static fields
.field public static final __md_methods:Ljava/lang/String;


# instance fields
.field private refList:Ljava/util/ArrayList;


# direct methods
.method static constructor <clinit>()V
    .locals 3

    .line 12
    const-string v0, "n_onDown:(Landroid/view/MotionEvent;)Z:GetOnDown_Landroid_view_MotionEvent_Handler\nn_onSingleTapUp:(Landroid/view/MotionEvent;)Z:GetOnSingleTapUp_Landroid_view_MotionEvent_Handler\n"

    sput-object v0, Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer_GestureListener;->__md_methods:Ljava/lang/String;

    .line 16
    const-class v1, Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer_GestureListener;

    const-string v2, "Syncfusion.XForms.Android.TextInputLayout.InputLayoutToggleViewRenderer+GestureListener, Syncfusion.Core.XForms.Android"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 17
    return-void
.end method

.method public constructor <init>()V
    .locals 3

    .line 22
    invoke-direct {p0}, Landroid/view/GestureDetector$SimpleOnGestureListener;-><init>()V

    .line 23
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer_GestureListener;

    if-ne v0, v1, :cond_0

    .line 24
    const/4 v0, 0x0

    new-array v0, v0, [Ljava/lang/Object;

    const-string v1, "Syncfusion.XForms.Android.TextInputLayout.InputLayoutToggleViewRenderer+GestureListener, Syncfusion.Core.XForms.Android"

    const-string v2, ""

    invoke-static {v1, v2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 26
    :cond_0
    return-void
.end method

.method public constructor <init>(Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer;)V
    .locals 2

    .line 30
    invoke-direct {p0}, Landroid/view/GestureDetector$SimpleOnGestureListener;-><init>()V

    .line 31
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer_GestureListener;

    if-ne v0, v1, :cond_0

    .line 32
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "Syncfusion.XForms.Android.TextInputLayout.InputLayoutToggleViewRenderer+GestureListener, Syncfusion.Core.XForms.Android"

    const-string v1, "Syncfusion.XForms.Android.TextInputLayout.InputLayoutToggleViewRenderer, Syncfusion.Core.XForms.Android"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 34
    :cond_0
    return-void
.end method

.method private native n_onDown(Landroid/view/MotionEvent;)Z
.end method

.method private native n_onSingleTapUp(Landroid/view/MotionEvent;)Z
.end method


# virtual methods
.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 55
    iget-object v0, p0, Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer_GestureListener;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 56
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer_GestureListener;->refList:Ljava/util/ArrayList;

    .line 57
    :cond_0
    iget-object v0, p0, Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer_GestureListener;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 58
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 62
    iget-object v0, p0, Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer_GestureListener;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 63
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 64
    :cond_0
    return-void
.end method

.method public onDown(Landroid/view/MotionEvent;)Z
    .locals 0

    .line 39
    invoke-direct {p0, p1}, Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer_GestureListener;->n_onDown(Landroid/view/MotionEvent;)Z

    move-result p1

    return p1
.end method

.method public onSingleTapUp(Landroid/view/MotionEvent;)Z
    .locals 0

    .line 47
    invoke-direct {p0, p1}, Lcrc644103bb497e895a1b/InputLayoutToggleViewRenderer_GestureListener;->n_onSingleTapUp(Landroid/view/MotionEvent;)Z

    move-result p1

    return p1
.end method
