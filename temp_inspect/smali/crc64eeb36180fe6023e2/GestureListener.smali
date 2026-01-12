.class public Lcrc64eeb36180fe6023e2/GestureListener;
.super Landroid/view/GestureDetector$SimpleOnGestureListener;
.source "GestureListener.java"

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
    const-string v0, "n_onDown:(Landroid/view/MotionEvent;)Z:GetOnDown_Landroid_view_MotionEvent_Handler\nn_onLongPress:(Landroid/view/MotionEvent;)V:GetOnLongPress_Landroid_view_MotionEvent_Handler\nn_onDoubleTap:(Landroid/view/MotionEvent;)Z:GetOnDoubleTap_Landroid_view_MotionEvent_Handler\nn_onDoubleTapEvent:(Landroid/view/MotionEvent;)Z:GetOnDoubleTapEvent_Landroid_view_MotionEvent_Handler\nn_onSingleTapUp:(Landroid/view/MotionEvent;)Z:GetOnSingleTapUp_Landroid_view_MotionEvent_Handler\n"

    sput-object v0, Lcrc64eeb36180fe6023e2/GestureListener;->__md_methods:Ljava/lang/String;

    .line 19
    const-class v1, Lcrc64eeb36180fe6023e2/GestureListener;

    const-string v2, "Syncfusion.XForms.Android.EffectsView.GestureListener, Syncfusion.Core.XForms.Android"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 20
    return-void
.end method

.method public constructor <init>()V
    .locals 3

    .line 25
    invoke-direct {p0}, Landroid/view/GestureDetector$SimpleOnGestureListener;-><init>()V

    .line 26
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc64eeb36180fe6023e2/GestureListener;

    if-ne v0, v1, :cond_0

    .line 27
    const/4 v0, 0x0

    new-array v0, v0, [Ljava/lang/Object;

    const-string v1, "Syncfusion.XForms.Android.EffectsView.GestureListener, Syncfusion.Core.XForms.Android"

    const-string v2, ""

    invoke-static {v1, v2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 29
    :cond_0
    return-void
.end method

.method public constructor <init>(Lcrc64eeb36180fe6023e2/SfEffectsViewRenderer;)V
    .locals 2

    .line 33
    invoke-direct {p0}, Landroid/view/GestureDetector$SimpleOnGestureListener;-><init>()V

    .line 34
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc64eeb36180fe6023e2/GestureListener;

    if-ne v0, v1, :cond_0

    .line 35
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "Syncfusion.XForms.Android.EffectsView.GestureListener, Syncfusion.Core.XForms.Android"

    const-string v1, "Syncfusion.XForms.Android.EffectsView.SfEffectsViewRenderer, Syncfusion.Core.XForms.Android"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 37
    :cond_0
    return-void
.end method

.method private native n_onDoubleTap(Landroid/view/MotionEvent;)Z
.end method

.method private native n_onDoubleTapEvent(Landroid/view/MotionEvent;)Z
.end method

.method private native n_onDown(Landroid/view/MotionEvent;)Z
.end method

.method private native n_onLongPress(Landroid/view/MotionEvent;)V
.end method

.method private native n_onSingleTapUp(Landroid/view/MotionEvent;)Z
.end method


# virtual methods
.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 82
    iget-object v0, p0, Lcrc64eeb36180fe6023e2/GestureListener;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 83
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc64eeb36180fe6023e2/GestureListener;->refList:Ljava/util/ArrayList;

    .line 84
    :cond_0
    iget-object v0, p0, Lcrc64eeb36180fe6023e2/GestureListener;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 85
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 89
    iget-object v0, p0, Lcrc64eeb36180fe6023e2/GestureListener;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 90
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 91
    :cond_0
    return-void
.end method

.method public onDoubleTap(Landroid/view/MotionEvent;)Z
    .locals 0

    .line 58
    invoke-direct {p0, p1}, Lcrc64eeb36180fe6023e2/GestureListener;->n_onDoubleTap(Landroid/view/MotionEvent;)Z

    move-result p1

    return p1
.end method

.method public onDoubleTapEvent(Landroid/view/MotionEvent;)Z
    .locals 0

    .line 66
    invoke-direct {p0, p1}, Lcrc64eeb36180fe6023e2/GestureListener;->n_onDoubleTapEvent(Landroid/view/MotionEvent;)Z

    move-result p1

    return p1
.end method

.method public onDown(Landroid/view/MotionEvent;)Z
    .locals 0

    .line 42
    invoke-direct {p0, p1}, Lcrc64eeb36180fe6023e2/GestureListener;->n_onDown(Landroid/view/MotionEvent;)Z

    move-result p1

    return p1
.end method

.method public onLongPress(Landroid/view/MotionEvent;)V
    .locals 0

    .line 50
    invoke-direct {p0, p1}, Lcrc64eeb36180fe6023e2/GestureListener;->n_onLongPress(Landroid/view/MotionEvent;)V

    .line 51
    return-void
.end method

.method public onSingleTapUp(Landroid/view/MotionEvent;)Z
    .locals 0

    .line 74
    invoke-direct {p0, p1}, Lcrc64eeb36180fe6023e2/GestureListener;->n_onSingleTapUp(Landroid/view/MotionEvent;)Z

    move-result p1

    return p1
.end method
