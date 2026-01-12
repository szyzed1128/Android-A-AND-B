.class public Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;
.super Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;
.source "FFAnimatedDrawable.java"

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
    const-string v0, ""

    sput-object v0, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;->__md_methods:Ljava/lang/String;

    .line 14
    const-class v1, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;

    const-string v2, "FFImageLoading.Drawables.FFAnimatedDrawable, FFImageLoading.Platform"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 15
    return-void
.end method

.method public constructor <init>()V
    .locals 3

    .line 20
    invoke-direct {p0}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>()V

    .line 21
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;

    if-ne v0, v1, :cond_0

    .line 22
    const/4 v0, 0x0

    new-array v0, v0, [Ljava/lang/Object;

    const-string v1, "FFImageLoading.Drawables.FFAnimatedDrawable, FFImageLoading.Platform"

    const-string v2, ""

    invoke-static {v1, v2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 24
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/res/Resources;)V
    .locals 2

    .line 29
    invoke-direct {p0, p1}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Landroid/content/res/Resources;)V

    .line 30
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;

    if-ne v0, v1, :cond_0

    .line 31
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "FFImageLoading.Drawables.FFAnimatedDrawable, FFImageLoading.Platform"

    const-string v1, "Android.Content.Res.Resources, Mono.Android"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 33
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/res/Resources;Landroid/graphics/Bitmap;)V
    .locals 2

    .line 38
    invoke-direct {p0, p1, p2}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Landroid/content/res/Resources;Landroid/graphics/Bitmap;)V

    .line 39
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;

    if-ne v0, v1, :cond_0

    .line 40
    const/4 v0, 0x2

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const-string p1, "FFImageLoading.Drawables.FFAnimatedDrawable, FFImageLoading.Platform"

    const-string p2, "Android.Content.Res.Resources, Mono.Android:Android.Graphics.Bitmap, Mono.Android"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 42
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/res/Resources;Ljava/io/InputStream;)V
    .locals 2

    .line 47
    invoke-direct {p0, p1, p2}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Landroid/content/res/Resources;Ljava/io/InputStream;)V

    .line 48
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;

    if-ne v0, v1, :cond_0

    .line 49
    const/4 v0, 0x2

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const-string p1, "FFImageLoading.Drawables.FFAnimatedDrawable, FFImageLoading.Platform"

    const-string p2, "Android.Content.Res.Resources, Mono.Android:System.IO.Stream, mscorlib"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 51
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/res/Resources;Ljava/lang/String;)V
    .locals 2

    .line 56
    invoke-direct {p0, p1, p2}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Landroid/content/res/Resources;Ljava/lang/String;)V

    .line 57
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;

    if-ne v0, v1, :cond_0

    .line 58
    const/4 v0, 0x2

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const-string p1, "FFImageLoading.Drawables.FFAnimatedDrawable, FFImageLoading.Platform"

    const-string p2, "Android.Content.Res.Resources, Mono.Android:System.String, mscorlib"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 60
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/graphics/Bitmap;)V
    .locals 2

    .line 65
    invoke-direct {p0, p1}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Landroid/graphics/Bitmap;)V

    .line 66
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;

    if-ne v0, v1, :cond_0

    .line 67
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "FFImageLoading.Drawables.FFAnimatedDrawable, FFImageLoading.Platform"

    const-string v1, "Android.Graphics.Bitmap, Mono.Android"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 69
    :cond_0
    return-void
.end method

.method public constructor <init>(Ljava/io/InputStream;)V
    .locals 2

    .line 74
    invoke-direct {p0, p1}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Ljava/io/InputStream;)V

    .line 75
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;

    if-ne v0, v1, :cond_0

    .line 76
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "FFImageLoading.Drawables.FFAnimatedDrawable, FFImageLoading.Platform"

    const-string v1, "System.IO.Stream, mscorlib"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 78
    :cond_0
    return-void
.end method

.method public constructor <init>(Ljava/lang/String;)V
    .locals 2

    .line 83
    invoke-direct {p0, p1}, Lcrc644bcdcf6d99873ace/SelfDisposingBitmapDrawable;-><init>(Ljava/lang/String;)V

    .line 84
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;

    if-ne v0, v1, :cond_0

    .line 85
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "FFImageLoading.Drawables.FFAnimatedDrawable, FFImageLoading.Platform"

    const-string v1, "System.String, mscorlib"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 87
    :cond_0
    return-void
.end method


# virtual methods
.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 92
    iget-object v0, p0, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 93
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;->refList:Ljava/util/ArrayList;

    .line 94
    :cond_0
    iget-object v0, p0, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 95
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 99
    iget-object v0, p0, Lcrc644bcdcf6d99873ace/FFAnimatedDrawable;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 100
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 101
    :cond_0
    return-void
.end method
