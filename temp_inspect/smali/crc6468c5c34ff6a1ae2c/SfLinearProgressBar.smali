.class public Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;
.super Lcrc6468c5c34ff6a1ae2c/ProgressBarBase;
.source "SfLinearProgressBar.java"

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
    const-string v0, "n_onAttachedToWindow:()V:GetOnAttachedToWindowHandler\nn_onMeasure:(II)V:GetOnMeasure_IIHandler\nn_onSizeChanged:(IIII)V:GetOnSizeChanged_IIIIHandler\nn_onFinishInflate:()V:GetOnFinishInflateHandler\n"

    sput-object v0, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;->__md_methods:Ljava/lang/String;

    .line 18
    const-class v1, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;

    const-string v2, "Syncfusion.Android.ProgressBar.SfLinearProgressBar, Syncfusion.SfProgressBar.XForms.Android"

    invoke-static {v2, v1, v0}, Lmono/android/Runtime;->register(Ljava/lang/String;Ljava/lang/Class;Ljava/lang/String;)V

    .line 19
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;)V
    .locals 2

    .line 24
    invoke-direct {p0, p1}, Lcrc6468c5c34ff6a1ae2c/ProgressBarBase;-><init>(Landroid/content/Context;)V

    .line 25
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;

    if-ne v0, v1, :cond_0

    .line 26
    const/4 v0, 0x1

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const-string p1, "Syncfusion.Android.ProgressBar.SfLinearProgressBar, Syncfusion.SfProgressBar.XForms.Android"

    const-string v1, "Android.Content.Context, Mono.Android"

    invoke-static {p1, v1, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 28
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;)V
    .locals 2

    .line 33
    invoke-direct {p0, p1, p2}, Lcrc6468c5c34ff6a1ae2c/ProgressBarBase;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;)V

    .line 34
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;

    if-ne v0, v1, :cond_0

    .line 35
    const/4 v0, 0x2

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const-string p1, "Syncfusion.Android.ProgressBar.SfLinearProgressBar, Syncfusion.SfProgressBar.XForms.Android"

    const-string p2, "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 37
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;I)V
    .locals 2

    .line 42
    invoke-direct {p0, p1, p2, p3}, Lcrc6468c5c34ff6a1ae2c/ProgressBarBase;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;I)V

    .line 43
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;

    if-ne v0, v1, :cond_0

    .line 44
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

    const-string p1, "Syncfusion.Android.ProgressBar.SfLinearProgressBar, Syncfusion.SfProgressBar.XForms.Android"

    const-string p2, "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 46
    :cond_0
    return-void
.end method

.method public constructor <init>(Landroid/content/Context;Landroid/util/AttributeSet;II)V
    .locals 2

    .line 51
    invoke-direct {p0, p1, p2, p3, p4}, Lcrc6468c5c34ff6a1ae2c/ProgressBarBase;-><init>(Landroid/content/Context;Landroid/util/AttributeSet;II)V

    .line 52
    invoke-virtual {p0}, Ljava/lang/Object;->getClass()Ljava/lang/Class;

    move-result-object v0

    const-class v1, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;

    if-ne v0, v1, :cond_0

    .line 53
    const/4 v0, 0x4

    new-array v0, v0, [Ljava/lang/Object;

    const/4 v1, 0x0

    aput-object p1, v0, v1

    const/4 p1, 0x1

    aput-object p2, v0, p1

    const/4 p1, 0x2

    invoke-static {p3}, Ljava/lang/Integer;->valueOf(I)Ljava/lang/Integer;

    move-result-object p2

    aput-object p2, v0, p1

    const/4 p1, 0x3

    invoke-static {p4}, Ljava/lang/Integer;->valueOf(I)Ljava/lang/Integer;

    move-result-object p2

    aput-object p2, v0, p1

    const-string p1, "Syncfusion.Android.ProgressBar.SfLinearProgressBar, Syncfusion.SfProgressBar.XForms.Android"

    const-string p2, "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib"

    invoke-static {p1, p2, p0, v0}, Lmono/android/TypeManager;->Activate(Ljava/lang/String;Ljava/lang/String;Ljava/lang/Object;[Ljava/lang/Object;)V

    .line 55
    :cond_0
    return-void
.end method

.method private native n_onAttachedToWindow()V
.end method

.method private native n_onFinishInflate()V
.end method

.method private native n_onMeasure(II)V
.end method

.method private native n_onSizeChanged(IIII)V
.end method


# virtual methods
.method public monodroidAddReference(Ljava/lang/Object;)V
    .locals 1

    .line 92
    iget-object v0, p0, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;->refList:Ljava/util/ArrayList;

    if-nez v0, :cond_0

    .line 93
    new-instance v0, Ljava/util/ArrayList;

    invoke-direct {v0}, Ljava/util/ArrayList;-><init>()V

    iput-object v0, p0, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;->refList:Ljava/util/ArrayList;

    .line 94
    :cond_0
    iget-object v0, p0, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;->refList:Ljava/util/ArrayList;

    invoke-virtual {v0, p1}, Ljava/util/ArrayList;->add(Ljava/lang/Object;)Z

    .line 95
    return-void
.end method

.method public monodroidClearReferences()V
    .locals 1

    .line 99
    iget-object v0, p0, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;->refList:Ljava/util/ArrayList;

    if-eqz v0, :cond_0

    .line 100
    invoke-virtual {v0}, Ljava/util/ArrayList;->clear()V

    .line 101
    :cond_0
    return-void
.end method

.method public onAttachedToWindow()V
    .locals 0

    .line 60
    invoke-direct {p0}, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;->n_onAttachedToWindow()V

    .line 61
    return-void
.end method

.method public onFinishInflate()V
    .locals 0

    .line 84
    invoke-direct {p0}, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;->n_onFinishInflate()V

    .line 85
    return-void
.end method

.method public onMeasure(II)V
    .locals 0

    .line 68
    invoke-direct {p0, p1, p2}, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;->n_onMeasure(II)V

    .line 69
    return-void
.end method

.method public onSizeChanged(IIII)V
    .locals 0

    .line 76
    invoke-direct {p0, p1, p2, p3, p4}, Lcrc6468c5c34ff6a1ae2c/SfLinearProgressBar;->n_onSizeChanged(IIII)V

    .line 77
    return-void
.end method
