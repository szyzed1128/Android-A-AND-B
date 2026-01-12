.class public Lcrc644bcdcf6d99873ace/FFBitmapDrawable;
.super Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;
.source "FFBitmapDrawable.java"

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
    const-string v0, "n_onBoundsChange:(Landroid/graphics/Rect;)V:GetOnBoundsChange_Landroid_graphics_Rect_Handler\nn_draw:(Landroid/graphics/Canvas;)V:GetDraw_Landroid_graphics_Canvas_Handler\nn_setAlpha:(I)V:GetSetAlpha_IHandler\nn_setColorFilter:(ILandroid/graphics/PorterDuff$Mode;)V:GetSetColorFilter_ILandroid_graphics_PorterDuff_Mode_Handler\n"

    sput-object v0, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;->__md_methods:Ljava/lang/String;

    .line 18
    const-class v1, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;

    const-string v2, "FFImageLoading.Drawables.FFBitmapDrawable, FFImageLoading.Platform"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 19
    return-void
.end method

.method public constructor <init>()V
    .locals 3

    .line 24
    invoke-direct {p0}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>()V

    .line 25
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;

    if-ne v0, v1, :cond_0

    .line 26
    const/4 v0, 0x0

    new-array v0, v0, [Ljava/lang/Object;

    const-string v1, "FFImageLoading.Drawables.FFBitmapDrawable, FFImageLoading.Platform"

    const-string v2, ""

    invoke-static {v1, v2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 28
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/res/Resources;)V
    .locals 2

    .line 33
    invoke-direct {p0, p1}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Landroid/content/res/Resources;)V

    .line 34
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;

    if-ne v0, v1, :cond_0

    .line 35
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "FFImageLoading.Drawables.FFBitmapDrawable, FFImageLoading.Platform"

    const-string v1, "Android.Content.Res.Resources, Mono.Android"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 37
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/res/Resources;Landroid/graphics/Bitmap;)V
    .locals 2

    .line 42
    invoke-direct {p0, p1, p2}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Landroid/content/res/Resources;Landroid/graphics/Bitmap;)V

    .line 43
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;

    if-ne v0, v1, :cond_0

    .line 44
    const/4 v0, 0x2

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const-string p1, "FFImageLoading.Drawables.FFBitmapDrawable, FFImageLoading.Platform"

    const-string p2, "Android.Content.Res.Resources, Mono.Android:Android.Graphics.Bitmap, Mono.Android"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 46
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/res/Resources;Ljava/io/InputStream;)V
    .locals 2

    .line 51
    invoke-direct {p0, p1, p2}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Landroid/content/res/Resources;Ljava/io/InputStream;)V

    .line 52
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;

    if-ne v0, v1, :cond_0

    .line 53
    const/4 v0, 0x2

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const-string p1, "FFImageLoading.Drawables.FFBitmapDrawable, FFImageLoading.Platform"

    const-string p2, "Android.Content.Res.Resources, Mono.Android:System.IO.Stream, mscorlib"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 55
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/res/Resources;Ljava/lang/String;)V
    .locals 2

    .line 60
    invoke-direct {p0, p1, p2}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Landroid/content/res/Resources;Ljava/lang/String;)V

    .line 61
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;

    if-ne v0, v1, :cond_0

    .line 62
    const/4 v0, 0x2

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const-string p1, "FFImageLoading.Drawables.FFBitmapDrawable, FFImageLoading.Platform"

    const-string p2, "Android.Content.Res.Resources, Mono.Android:System.String, mscorlib"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 64
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/graphics/Bitmap;)V
    .locals 2

    .line 69
    invoke-direct {p0, p1}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Landroid/graphics/Bitmap;)V

    .line 70
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;

    if-ne v0, v1, :cond_0

    .line 71
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "FFImageLoading.Drawables.FFBitmapDrawable, FFImageLoading.Platform"

    const-string v1, "Android.Graphics.Bitmap, Mono.Android"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 73
    :cond_0
    return-void
.end method

.method public constructor <init>(Ljava/io/InputStream;)V
    .locals 2

    .line 78
    invoke-direct {p0, p1}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Ljava/io/InputStream;)V

    .line 79
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;

    if-ne v0, v1, :cond_0

    .line 80
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "FFImageLoading.Drawables.FFBitmapDrawable, FFImageLoading.Platform"

    const-string v1, "System.IO.Stream, mscorlib"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 82
    :cond_0
    return-void
.end method

.method public constructor <init>(Ljava/lang/String;)V
    .locals 2

    .line 87
    invoke-direct {p0, p1}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Ljava/lang/String;)V

    .line 88
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;

    if-ne v0, v1, :cond_0

    .line 89
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "FFImageLoading.Drawables.FFBitmapDrawable, FFImageLoading.Platform"

    const-string v1, "System.String, mscorlib"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 91
    :cond_0
    return-void
.end method

.method private native n_draw(Landroid/graphics/Canvas;)V
.end method

.method private native n_onBoundsChange(Landroid/graphics/Rect;)V
.end method

.method private native n_setAlpha(I)V
.end method

.method private native n_setColorFilter(ILandroid/graphics/PorterDuff$Mode;)V
.end method


# virtual methods
.method public draw(Landroid/graphics/Canvas;)V
    .locals 0

    .line 104
    invoke-direct {p0, p1}, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;->n_draw(Landroid/graphics/Canvas;)V

    .line 105
    return-void
.end method

.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 128
    iget-object v0, p0, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 129
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;->refList:Ljava/util/ArrayList;

    .line 130
    :cond_0
    iget-object v0, p0, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 131
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 135
    iget-object v0, p0, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 136
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 137
    :cond_0
    return-void
.end method

.method public onBoundsChange(Landroid/graphics/Rect;)V
    .locals 0

    .line 96
    invoke-direct {p0, p1}, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;->n_onBoundsChange(Landroid/graphics/Rect;)V

    .line 97
    return-void
.end method

.method public setAlpha(I)V
    .locals 0

    .line 112
    invoke-direct {p0, p1}, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;->n_setAlpha(I)V

    .line 113
    return-void
.end method

.method public setColorFilter(ILandroid/graphics/PorterDuff$Mode;)V
    .locals 0

    .line 120
    invoke-direct {p0, p1, p2}, Lcrc644bcdcf6d99873ace/FFBitmapDrawable;->n_setColorFilter(ILandroid/graphics/PorterDuff$Mode;)V

    .line 121
    return-void
.end method
